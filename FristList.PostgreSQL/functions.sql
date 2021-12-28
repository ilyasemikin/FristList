-- Functions

-- Refresh token functions

CREATE OR REPLACE FUNCTION get_refresh_token(token TEXT)
    RETURNS TABLE (
        "RefreshTokenId"        INTEGER,
        "RefreshTokenValue"     TEXT,
        "RefreshTokenExpires"   TIMESTAMP WITHOUT TIME ZONE,
        "RefreshTokenUserId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT "Id", "Token"::TEXT, "Expires", "UserId"
          FROM user_refresh_token
         WHERE "Token"=token;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION update_refresh_token(old_token TEXT, new_token TEXT, new_token_expires TIMESTAMP WITHOUT TIME ZONE)
    RETURNS TABLE (
        "RefreshTokenId"        INTEGER,
        "RefreshTokenValue"     TEXT,
        "RefreshTokenExpires"   TIMESTAMP WITHOUT TIME ZONE,
        "RefreshTokenUserId"    INTEGER
    )
AS $$
DECLARE
    expires_time    TIMESTAMP WITHOUT TIME ZONE;
BEGIN
    IF NOT EXISTS(SELECT "Id" FROM user_refresh_token WHERE "Token"=old_token) THEN
        RETURN;
    END IF;
    
    SELECT "Expires" FROM user_refresh_token WHERE "Token"=old_token INTO expires_time;
    IF NOW() AT TIME ZONE 'UTC' >= expires_time THEN
        DELETE 
          FROM user_refresh_token 
         WHERE "Token"=old_token;
        RETURN;
    END IF;

    UPDATE user_refresh_token 
       SET "Token"=new_token, 
           "Expires"=new_token_expires 
     WHERE "Token"=old_token;
    
    RETURN QUERY
        SELECT "Id", "Token"::TEXT, "Expires", "UserId"
          FROM user_refresh_token 
         WHERE "Token"=new_token;
END
$$ LANGUAGE plpgsql;

-- End refresh token functions

-- Category functions

CREATE OR REPLACE FUNCTION get_category(category_id INTEGER)
    RETURNS TABLE (
        "CategoryId"        INTEGER,
        "CategoryName"      TEXT,
        "CategoryUserId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT "Id", "Name"::TEXT, "UserId" 
          FROM category 
         WHERE "Id"=category_id;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_category(category_name TEXT, user_id INTEGER)
    RETURNS TABLE (
        "CategoryId"        INTEGER,
        "CategoryName"      TEXT,
        "CategoryUserId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT "Id", "Name"::TEXT, "UserId"
        FROM category
        WHERE "Name"=category_name AND "UserId"=user_id;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_categories(category_ids INTEGER[])
    RETURNS TABLE (
        "CategoryId"        INTEGER,
        "CategoryName"      TEXT,
        "CategoryUserId"    INTEGER
    )
AS $$
BEGIN 
    RETURN QUERY
        SELECT "Id", "Name"::TEXT, "UserId"
          FROM unnest(category_ids) id JOIN category c ON id=c."Id";
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_categories(user_id INTEGER, skip INTEGER, count INTEGER)
    RETURNS TABLE (
        "CategoryId"        INTEGER,
        "CategoryName"      TEXT,
        "CategoryUserId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT "Id", "Name"::TEXT, "UserId" 
          FROM category 
         WHERE "UserId"=user_id 
      ORDER BY "Id" 
        OFFSET skip 
         LIMIT count;
END
$$ LANGUAGE plpgsql;

-- End category function

-- Action functions

CREATE OR REPLACE FUNCTION add_action(
    start_time TIMESTAMP WITHOUT TIME ZONE, 
    end_time TIMESTAMP WITHOUT TIME ZONE, 
    description TEXT, 
    user_id INTEGER, 
    categories INTEGER[]
)
    RETURNS INTEGER
AS $$
DECLARE
    action_id   INTEGER;
BEGIN
    INSERT INTO action ("During", "Description", "UserId") 
         VALUES (TSRANGE(start_time, end_time, '[)'), description, user_id) 
      RETURNING "Id" INTO action_id;
    
    INSERT INTO action_categories ("ActionId", "CategoryId") 
         SELECT action_id, unnest
           FROM unnest(categories);
    
    RETURN action_id;
END
$$ LANGUAGE plpgsql;

-- End action functions

-- Start task functions

CREATE OR REPLACE FUNCTION add_task(name TEXT, user_id INTEGER, category_ids INTEGER[], project_id INTEGER DEFAULT NULL)
    RETURNS INTEGER
AS $$
DECLARE
    task_id     INTEGER;
BEGIN
    IF NOT EXISTS(SELECT "Id" FROM app_user WHERE "Id"=user_id) THEN
        RAISE EXCEPTION 'User not exists';
    END IF;
    
    IF project_id IS NOT NULL AND NOT EXISTS(SELECT "Id" FROM project WHERE "Id"=project_id AND "UserId"=user_id) THEN
        RAISE exception 'Project not exists';
    END IF;
    
    IF (SELECT COUNT("Id") FROM unnest(category_ids) id JOIN category c ON c."Id"=id WHERE "UserId"=user_id) != array_length(category_ids, 1) THEN
        RETURN FALSE;
    END IF;
    
    INSERT INTO task ("Name", "UserId") VALUES (name, user_id) RETURNING "Id" INTO task_id;
    INSERT INTO task_categories ("TaskId", "CategoryId")
         SELECT task_id, id FROM unnest(category_ids) id;
    
    IF project_id IS NOT NULL THEN
        DECLARE
            parent_task_id  INTEGER DEFAULT NULL;
        BEGIN
            SELECT "TaskId" 
              FROM project_tasks 
             WHERE "ProjectId"=project_id AND "NextTaskId" IS NULL INTO parent_task_id;

            IF (SELECT * FROM append_task_to_project(project_id, task_id, parent_task_id)) THEN
                RAISE EXCEPTION 'Error while attempt add task to project';
            END IF;
        END;
    END IF;
    
    RETURN task_id;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION delete_task(task_id INTEGER)
    RETURNS BOOLEAN
AS $$
BEGIN
    PERFORM delete_task_from_project(task_id);

    DELETE FROM task_categories WHERE "TaskId"=task_id;
    DELETE FROM task WHERE "Id"=task_id;
    
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION complete_task(task_id INTEGER)
    RETURNS BOOLEAN
AS $$
BEGIN
    UPDATE task SET "IsCompleted"=TRUE WHERE "Id"=task_id;
    
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION uncomplete_task(task_id INTEGER)
    RETURNS BOOLEAN
AS $$
BEGIN
    UPDATE task SET "IsCompleted"=FALSE WHERE "Id"=task_id;
    
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

-- End task functions

-- Running action functions

CREATE OR REPLACE FUNCTION start_action(user_id INTEGER, category_ids INTEGER[])
    RETURNS BOOLEAN
AS $$
DECLARE
    r       category%ROWTYPE;
BEGIN
    FOR r IN
        SELECT "Id", "Name", "UserId"
          FROM unnest(category_ids) c_id 
              LEFT JOIN category c ON c_id=c."Id"
    LOOP
        IF r."Id" IS NULL OR r."UserId" != user_id THEN
            RETURN FALSE;
        END IF;
    END LOOP;
    
    INSERT INTO running_action ("UserId", "StartTime", "TaskId") 
         VALUES (user_id, NOW() AT TIME ZONE 'UTC', NULL);
    
    INSERT INTO running_action_categories ("UserId", "CategoryId")
         SELECT user_id, c_id FROM unnest(category_ids) c_id;
    
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

CREATE OR REPlACE FUNCTION start_task_action(user_id INTEGER, task_id INTEGER)
    RETURNS BOOLEAN
AS $$
BEGIN
    IF (SELECT t."UserId" FROM task t WHERE t."Id"=task_id) != user_id THEN
        RETURN FALSE;
    END IF;
    
    INSERT INTO running_action ("UserId", "StartTime", "TaskId") 
         VALUES (user_id, NOW() AT TIME ZONE 'UTC', task_id);
    
    INSERT INTO running_action_categories ("UserId", "CategoryId") 
         SELECT user_id, tc."CategoryId" 
           FROM task_categories tc 
          WHERE tc."TaskId"=task_id;
    
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION delete_running_action(user_id INTEGER)
    RETURNS BOOLEAN
AS $$
BEGIN
    IF NOT EXISTS(SELECT "StartTime" FROM running_action) THEN
        RETURN FALSE;
    END IF;
    
    DELETE FROM running_action WHERE "UserId"=user_id;
    
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

-- End running action functions

-- Project functions

CREATE OR REPLACE FUNCTION add_project(name TEXT, description TEXT, user_id INTEGER, is_completed BOOLEAN DEFAULT FALSE)
    RETURNS INTEGER
AS $$
DECLARE
    project_id  INTEGER DEFAULT NULL;
BEGIN
    INSERT INTO project ("Name", "Description", "IsCompleted", "UserId") 
         VALUES (name, description, is_completed, user_id)
      RETURNING "Id"
           INTO project_id;

    RETURN project_id;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION delete_project(project_id INTEGER)
    RETURNS BOOLEAN
AS $$
BEGIN
    DELETE FROM project WHERE "Id"=project_id;
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_project(project_id INTEGER)
    RETURNS TABLE (
        "ProjectId"             INTEGER,
        "ProjectName"           TEXT,
        "ProjectDescription"    TEXT,
        "ProjectIsCompleted"    BOOLEAN,
        "ProjectUserId"         INTEGER
    )
AS $$
BEGIN
    RETURN QUERY 
        SELECT p."Id", p."Name"::TEXT, p."Description"::TEXT, p."IsCompleted", p."UserId" 
          FROM project p 
         WHERE "Id"=project_id;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_projects(user_id INTEGER, skip INTEGER, count INTEGER)
    RETURNS TABLE (
        "ProjectId"             INTEGER,
        "ProjectName"           TEXT,
        "ProjectDescription"    TEXT,
        "ProjectIsCompleted"    BOOLEAN,
        "ProjectUserId"         INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT p."Id", p."Name"::TEXT, p."Description"::TEXT, p."IsCompleted", p."UserId"
          FROM project p WHERE "UserId"=user_id 
      ORDER BY "Id" 
        OFFSET skip 
         LIMIT count;
END
$$ LANGUAGE plpgsql;

-- End project functions

-- Start project tasks functions

CREATE OR REPLACE FUNCTION append_task_to_project(project_id INTEGER, task_id INTEGER)
    RETURNS BOOLEAN
AS $$
DECLARE
    parent_id   INTEGER;
BEGIN
    IF EXISTS(SELECT "ProjectId" FROM project_tasks WHERE "TaskId"=task_id) THEN
        RETURN FALSE;
    END IF;
    
    SELECT "TaskId" FROM project_tasks WHERE "ProjectId"=project_id AND "NextTaskId" IS NULL INTO parent_id;
    
    INSERT INTO project_tasks ("TaskId", "ProjectId") VALUES (task_id, project_id);
    
    IF parent_id IS NOT NULL THEN
        UPDATE project_tasks SET "NextTaskId"=task_id WHERE "TaskId"=parent_id;
    END IF;
    
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION insert_task_to_project(project_id INTEGER, task_id INTEGER, index INTEGER)
    RETURNS BOOLEAN
AS $$
DECLARE
    child_id    INTEGER;
    parent_id   INTEGER;
    task_count  INTEGER;
BEGIN
    IF index = 0 THEN
        SELECT "TaskId" FROM get_project_task_parents(project_id) WHERE "ParentTaskId" IS NULL INTO child_id;
        INSERT INTO project_tasks ("TaskId", "ProjectId", "NextTaskId") VALUES (task_id, project_id, child_id);
        RETURN TRUE;
    END IF;
    
    SELECT "Count" FROM project_task_count WHERE "ProjectId"=project_id INTO task_count;
    
    IF index >= task_count THEN
        RETURN append_task_to_project(project_id, task_id);
    END IF;
    
    SELECT t."TaskId", "NextTaskId" 
      FROM get_project_tasks_indexes(project_id) t 
          LEFT JOIN project_tasks pt ON t."TaskId"=pt."TaskId" 
     WHERE "TaskProjectIndex"=index - 1
      INTO parent_id, child_id;
    
    INSERT INTO project_tasks ("TaskId", "ProjectId", "NextTaskId") VALUES (task_id, project_id, child_id);
    UPDATE project_tasks SET "NextTaskId"=task_id WHERE "TaskId"=parent_id;
    
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION update_project_task_parent(task_id INTEGER, parent_id INTEGER)
    RETURNS BOOLEAN
AS $$
DECLARE
    project_id  INTEGER;
    index       INTEGER;
BEGIN
    SELECT "ProjectId" FROM project_tasks WHERE "TaskId"=task_id INTO project_id;
    PERFORM delete_task_from_project(task_id);
    
    IF parent_id IS NULL THEN
        PERFORM insert_task_to_project(project_id, task_id, 0);
    ELSE
        SELECT "TaskProjectIndex" FROM get_project_tasks_indexes(project_id) WHERE "TaskId"=parent_id INTO index;
        PERFORM insert_task_to_project(project_id, task_id, index + 1);
    END IF;
    
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION delete_task_from_project(task_id INTEGER)
    RETURNS BOOLEAN
AS $$
DECLARE
    project_id  INTEGER;
    parent_id   INTEGER;
    child_id    INTEGER;
BEGIN
    SELECT "NextTaskId", "ProjectId" FROM project_tasks WHERE "TaskId"=task_id INTO child_id, project_id;
    IF project_id IS NULL THEN
        RETURN FALSE;
    END IF;
    
    SELECT "ParentTaskId" FROM get_project_task_parents(project_id) WHERE "TaskId"=task_id INTO parent_id;
    
    DELETE FROM project_tasks WHERE "TaskId"=task_id;
    
    IF parent_id IS NOT NULL THEN
        UPDATE project_tasks SET "NextTaskId"=child_id WHERE "TaskId"=parent_id;
    END IF;
    
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_project_tasks(project_id INTEGER, skip INTEGER, count INTEGER)
    RETURNS TABLE (
        "TaskId"            INTEGER,
        "TaskName"          TEXT,
        "TaskUserId"        INTEGER,
        "TaskProjectId"     INTEGER,
        "NextTaskId"        INTEGER,
        "IsHead"            BOOLEAN
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT t."Id",
               t."Name"::TEXT,
               t."UserId",
               pt."ProjectId",
               pt."NextTaskId",
               prt."ParentTaskId" IS NULL
        FROM (SELECT * FROM project_tasks pt WHERE pt."ProjectId"=project_id ORDER BY pt."TaskId" OFFSET skip LIMIT count) pt
                 LEFT JOIN task t ON pt."TaskId"=t."Id"
                 LEFT JOIN get_project_task_parents(project_id) prt ON prt."TaskId"=t."Id";
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_project_task_parents(project_id INTEGER)
    RETURNS TABLE (
        "TaskId"        INTEGER,
        "ParentTaskId"  INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT p1."TaskId", p2."TaskId"
        FROM project_tasks p1
                 LEFT JOIN project_tasks p2 ON p1."TaskId"=p2."NextTaskId"
        WHERE p1."ProjectId"=project_id ORDER BY p1."TaskId";
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_project_tasks_indexes(project_id INTEGER)
    RETURNS TABLE (
        "TaskId"            INTEGER,
        "TaskProjectIndex"  INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        WITH RECURSIVE project_tasks_ordered AS (
            SELECT p."TaskId", "NextTaskId"
            FROM get_project_task_parents(project_id) p
                     LEFT JOIN project_tasks pt ON p."TaskId" = pt."TaskId"
            WHERE "ParentTaskId" IS NULL

            UNION

            SELECT pt."TaskId", pt."NextTaskId"
            FROM project_tasks_ordered pto
                     JOIN project_tasks pt ON pto."NextTaskId" = pt."TaskId"
            WHERE pto."NextTaskId" IS NOT NULL
        )
        SELECT pto."TaskId", (ROW_NUMBER() OVER () - 1)::INTEGER
          FROM project_tasks_ordered pto;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION complete_project(project_id INTEGER)
    RETURNS BOOLEAN
AS $$
BEGIN
    UPDATE project SET "IsCompleted"=TRUE WHERE "Id"=project_id;
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION uncomplete_project(project_id INTEGER)
    RETURNS BOOLEAN
AS $$
BEGIN
    UPDATE project SET "IsCompleted"=FALSE WHERE "Id"=project_id;
    RETURN TRUE;
END
$$ LANGUAGE plpgsql;

-- End project tasks functions

-- Start time statistics

CREATE OR REPLACE FUNCTION timestamp_to_str(t TIMESTAMP WITHOUT TIME ZONE)
    RETURNS TEXT
AS $$
BEGIN
    RETURN FORMAT('(TIMESTAMP ''%s'')', t);
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_intervals(intervals TSRANGE[])
    RETURNS TABLE (
        "StartTime"         TIMESTAMP WITHOUT TIME ZONE,
        "EndTime"           TIMESTAMP WITHOUT TIME ZONE
    )
AS $$
BEGIN
    RETURN QUERY
        WITH t1 AS (
            SELECT LOWER("During") AS s, UPPER("During") AS e,
                   COALESCE(
                       LOWER("During") > MAX(UPPER("During"))
                            OVER(ORDER BY LOWER("During"), UPPER("During")
                                ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING), TRUE
                   ) AS ns
              FROM unnest(intervals) "During"
        ), t2 AS (
            SELECT t1.s, t1.e, SUM(ns::INTEGER) OVER(ORDER BY t1.s, t1.e) AS grp
            FROM t1
        )
        SELECT MIN(a.s), MAX(a.e)
          FROM t2 a
      GROUP BY a.grp
      ORDER BY MIN(a.s);
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_summary_time(query_text TEXT)
    RETURNS INTERVAL
AS $$
DECLARE 
    durings         TSRANGE[];
    summary_time    INTERVAL;
BEGIN
    EXECUTE FORMAT('SELECT ARRAY(%s)', query_text) INTO durings;
    SELECT SUM("EndTime" - "StartTime") FROM get_intervals(durings) INTO summary_time;
    RETURN JUSTIFY_INTERVAL(COALESCE(summary_time, INTERVAL '0'));
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_summary_time(user_id INTEGER, from_time TIMESTAMP WITHOUT TIME ZONE, to_time TIMESTAMP WITHOUT TIME ZONE)
    RETURNS INTERVAL
AS $$
BEGIN
    RETURN get_summary_time(
            FORMAT('SELECT a."During" FROM action a WHERE a."UserId"=%s AND LOWER(a."During") > %L AND UPPER(a."During") < %L',
                   user_id, from_time, to_time));
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_category_summary_time(category_id INTEGER, from_time TIMESTAMP WITHOUT TIME ZONE, to_time TIMESTAMP WITHOUT TIME ZONE)
    RETURNS INTERVAL
AS $$
BEGIN
    RETURN get_summary_time(
        FORMAT('SELECT a."During" FROM action a JOIN action_categories ac ON a."Id"=ac."CategoryId" WHERE ac."CategoryId"=%s AND LOWER(a."During") > %L AND UPPER(a."During") < %L', 
            category_id, from_time, to_time));
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_task_summary_time(task_id INTEGER, from_time TIMESTAMP WITHOUT TIME ZONE, to_time TIMESTAMP WITHOUT TIME ZONE)
    RETURNS INTERVAL
AS $$
BEGIN
    RETURN get_summary_time(
            FORMAT('SELECT a."During" FROM action a JOIN task_actions tc ON a."Id"=tc."ActionId" WHERE tc."TaskId"=%s AND LOWER(a."During") > %L AND UPPER(a."During") < %L',
                   task_id, from_time, to_time));
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_task_summary_time(user_id INTEGER, task_id INTEGER, from_time TIMESTAMP WITHOUT TIME ZONE, to_time TIMESTAMP WITHOUT TIME ZONE)
    RETURNS INTERVAL
AS $$
BEGIN
    RAISE EXCEPTION 'Not implemented yet';
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_project_summary_time(project_id INTEGER, from_time TIMESTAMP WITHOUT TIME ZONE, to_time TIMESTAMP WITHOUT TIME ZONE)
    RETURNS INTERVAL
AS $$
BEGIN
    RETURN get_summary_time(
            FORMAT('SELECT a."During" FROM action a JOIN task_actions tc ON a."Id"=tc."ActionId" LEFT JOIN project_tasks pt ON tc."TaskId"=pt."TaskId" WHERE pt."ProjectId"=%s AND LOWER(a."During") > %L AND UPPER(a."During") < %L',
                   project_id, from_time, to_time));
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_project_summary_time(user_id INTEGER, project_id INTEGER, from_time TIMESTAMP WITHOUT TIME ZONE, to_time TIMESTAMP WITHOUT TIME ZONE)
    RETURNS INTERVAL
AS $$
BEGIN
    RAISE EXCEPTION 'Not implemented yet';
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_action_intervals(
    user_id         INTEGER,
    from_time       TIMESTAMP WITHOUT TIME ZONE,
    to_time         TIMESTAMP WITHOUT TIME ZONE
)
    RETURNS TABLE (
        "StartTime"         TIMESTAMP WITHOUT TIME ZONE,
        "EndTime"           TIMESTAMP WITHOUT TIME ZONE
    )
AS $$
BEGIN
    RETURN QUERY
        WITH t1 AS (
            SELECT LOWER(a."During") AS s, UPPER(a."During") AS e,
                   COALESCE(
                       LOWER(a."During") > MAX(UPPER(a."During")) 
                           OVER(ORDER BY LOWER(a."During"), UPPER(a."During") 
                               ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING), TRUE
                       ) AS ns
            FROM action a
            WHERE a."UserId"=user_id AND LOWER(a."During") > from_time AND UPPER(a."During") < to_time
        ), t2 AS (
            SELECT t1.s, t1.e, SUM(ns::INTEGER) OVER(ORDER BY t1.s, t1.e) AS grp
              FROM t1
        )
          SELECT MIN(a.s), MAX(a.e) 
            FROM t2 a
        GROUP BY a.grp
        ORDER BY MIN(a.s);
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_action_intervals_by_categories(
    user_id     INTEGER,
    from_time   TIMESTAMP WITHOUT TIME ZONE,
    to_time     TIMESTAMP WITHOUT TIME ZONE
)
    RETURNS TABLE (
        "StartTime"     TIMESTAMP WITHOUT TIME ZONE,
        "EndTime"       TIMESTAMP WITHOUT TIME ZONE,
        "CategoryId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        WITH t1 AS (
            SELECT ac."CategoryId", LOWER(a."During") AS s, UPPER(a."During") AS e,
                   COALESCE(
                       LOWER(a."During") > MAX(UPPER(a."During"))
                           OVER(PARTITION BY ac."CategoryId" ORDER BY LOWER(a."During"), UPPER(a."During")
                               ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING), TRUE
                       ) AS ns
              FROM action a LEFT JOIN action_categories ac on a."Id"=ac."ActionId"
             WHERE a."UserId"=user_id AND LOWER(a."During") > from_time AND UPPER(a."During") < to_time
        ), t2 AS (
            SELECT t1."CategoryId", t1.s, t1.e, SUM(ns::INTEGER) OVER(ORDER BY t1.s, t1.e) AS grp
              FROM t1
        )
          SELECT MIN(a.s), MAX(a.e), a."CategoryId"
            FROM t2 a
        GROUP BY a.grp, a."CategoryId"
        ORDER BY MIN(a.s);
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_action_time(
    user_id     INTEGER, 
    from_time   TIMESTAMP WITHOUT TIME ZONE DEFAULT (TO_TIMESTAMP(0)),
    to_time     TIMESTAMP WITHOUT TIME ZONE DEFAULT (NOW() AT TIME ZONE 'UTC')
)
    RETURNS TABLE (
        "StartTime"         TIMESTAMP WITHOUT TIME ZONE,
        "EndTime"           TIMESTAMP WITHOUT TIME ZONE,
        "Duration"          INTERVAL
    )
AS $$
BEGIN
    RETURN QUERY
          SELECT a."StartTime", a."EndTime", a."EndTime" - a."StartTime"
            FROM get_user_action_intervals(user_id, from_time, to_time) AS a
        ORDER BY a."StartTime";
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_total_action_time(
    user_id     INTEGER,
    from_time   TIMESTAMP WITHOUT TIME ZONE DEFAULT (TO_TIMESTAMP(0)),
    to_time     TIMESTAMP WITHOUT TIME ZONE DEFAULT (NOW() AT TIME ZONE 'UTC')
)
    RETURNS TABLE (
        "TotalTime"         INTERVAL,
        "MaxTime"           INTERVAL
    )
AS $$
DECLARE 
    total_time  INTERVAL;
BEGIN
    SELECT JUSTIFY_INTERVAL(SUM(a."EndTime" - a."StartTime"))
      FROM get_user_action_intervals(user_id, from_time, to_time) AS a
      INTO total_time;
    
    IF total_time IS NULL THEN
        total_time := INTERVAL '0';
    END IF;
    
    RETURN QUERY SELECT total_time, JUSTIFY_INTERVAL(to_time - from_time);
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_total_action_time_by_categories(
    user_id     INTEGER,
    from_time   TIMESTAMP WITHOUT TIME ZONE DEFAULT (TO_TIMESTAMP(0)),
    to_time     TIMESTAMP WITHOUT TIME ZONE DEFAULT (NOW() AT TIME ZONE 'UTC')
)
    RETURNS TABLE (
        "CategoryId"    INTEGER,
        "TotalTime"     INTERVAL
    )
AS $$
BEGIN
    RETURN QUERY
          SELECT c."Id", COALESCE(JUSTIFY_INTERVAL(SUM(a."EndTime" - a."StartTime")), INTERVAL '0')
            FROM category c LEFT JOIN get_user_action_intervals_by_categories(user_id, from_time, to_time) AS a 
                ON c."Id" = a."CategoryId"
           WHERE c."UserId"=user_id
        GROUP BY c."Id"
        ORDER BY c."Id";
END
$$ LANGUAGE plpgsql;

-- End time statistics

-- Start running action with categories

CREATE OR REPLACE FUNCTION get_running_action_with_categories(user_id INTEGER)
    RETURNS TABLE (
        "RunningActionStartTime"    TIMESTAMP WITHOUT TIME ZONE,
        "RunningActionUserId"       INTEGER,
        "RunningTaskId"             INTEGER,
        "CategoryId"                INTEGER,
        "CategoryName"              TEXT,
        "CategoryUserId"            INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT ra."StartTime",
               ra."UserId",
               ra."TaskId",
               rac."CategoryId",
               c."Name"::TEXT,
               c."UserId"
        FROM running_action ra
                 LEFT JOIN running_action_categories rac ON ra."UserId"=rac."UserId"
                 LEFT JOIN category c ON rac."CategoryId"=c."Id"
        WHERE ra."UserId"=user_id;
END
$$ LANGUAGE plpgsql;

-- End running action with categories

-- Start action with categories

CREATE OR REPLACE FUNCTION get_action_with_categories(action_id INTEGER)
    RETURNS TABLE (
        "ActionId"          INTEGER,
        "ActionStartTime"   TIMESTAMP WITHOUT TIME ZONE,
        "ActionEndTime"     TIMESTAMP WITHOUT TIME ZONE,
        "ActionUserId"      INTEGER,
        "CategoryId"        INTEGER,
        "CategoryName"      TEXT,
        "CategoryUserId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT a."Id" AS "ActionId",
               lower(a."During"),
               upper(a."During"),
               a."UserId",
               c."Id",
               c."Name"::TEXT,
               c."UserId"
        FROM action a
                 LEFT JOIN action_categories ac on a."Id"=ac."ActionId"
                 LEFT JOIN category c on ac."CategoryId"=c."Id"
        WHERE a."Id"=action_id;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_actions_with_categories(user_id INTEGER, skip INTEGER, count INTEGER)
    RETURNS TABLE (
        "ActionId"          INTEGER,
        "ActionStartTime"   TIMESTAMP WITHOUT TIME ZONE,
        "ActionEndTime"     TIMESTAMP WITHOUT TIME ZONE,
        "ActionUserId"      INTEGER,
        "CategoryId"        INTEGER,
        "CategoryName"      TEXT,
        "CategoryUserId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT a."Id",
               lower(a."During"),
               upper(a."During"),
               user_id,
               c."Id",
               c."Name"::TEXT,
               user_id
        FROM (SELECT * FROM action WHERE "UserId"=user_id ORDER BY "Id" OFFSET skip LIMIT count) a
                 LEFT JOIN action_categories ac ON a."Id"=ac."ActionId"
                 LEFT JOIN category c ON ac."CategoryId"=c."Id";
END
$$ LANGUAGE plpgsql;

-- End action with categories

-- Start task with categories

CREATE OR REPLACE FUNCTION get_task_with_categories(task_id INTEGER)
    RETURNS TABLE (
        "TaskId"            INTEGER,
        "TaskName"          TEXT,
        "TaskUserId"        INTEGER,
        "TaskProjectId"     INTEGER,
        "TaskIsCompleted"   BOOLEAN,
        "CategoryId"        INTEGER,
        "CategoryName"      TEXT,
        "CategoryUserId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT t."Id",
               t."Name"::TEXT,
               t."UserId",
               pt."ProjectId",
               t."IsCompleted",
               c."Id",
               c."Name"::TEXT,
               c."UserId"
        FROM task t
                 LEFT JOIN task_categories tc ON t."Id" = tc."TaskId"
                 LEFT JOIN category c ON tc."CategoryId" = c."Id"
                 LEFT JOIN project_tasks pt ON t."Id" = pt."TaskId"
        WHERE t."Id"=task_id;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_tasks_with_categories(user_id INTEGER, skip INTEGER, count INTEGER)
    RETURNS TABLE (
        "TaskId"            INTEGER,
        "TaskName"          TEXT,
        "TaskUserId"        INTEGER,
        "TaskProjectId"     INTEGER,
        "TaskIsCompleted"   BOOLEAN,
        "CategoryId"        INTEGER,
        "CategoryName"      TEXT,
        "CategoryUserId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT t."Id",
               t."Name"::TEXT,
               t."UserId",
               pt."ProjectId",
               t."IsCompleted",
               c."Id",
               c."Name"::TEXT,
               c."UserId"
        FROM (SELECT * FROM task WHERE "UserId"=user_id ORDER BY "Id" OFFSET skip LIMIT count) t
                 LEFT JOIN task_categories tc ON t."Id"=tc."TaskId"
                 LEFT JOIN category c ON tc."CategoryId"=c."Id"
                 LEFT JOIN project_tasks pt ON t."Id"=pt."TaskId";
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_uncompleted_tasks_with_categories(user_id INTEGER, skip INTEGER, count INTEGER)
    RETURNS TABLE (
        "TaskId"            INTEGER,
        "TaskName"          TEXT,
        "TaskUserId"        INTEGER,
        "TaskProjectId"     INTEGER,
        "TaskIsCompleted"   BOOLEAN,
        "CategoryId"        INTEGER,
        "CategoryName"      TEXT,
        "CategoryUserId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
        SELECT t."Id",
               t."Name"::TEXT,
               t."UserId",
               pt."ProjectId",
               t."IsCompleted",
               c."Id",
               c."Name"::TEXT,
               c."UserId"
        FROM (SELECT * FROM task WHERE "UserId"=user_id AND NOT "IsCompleted" ORDER BY "Id" OFFSET skip LIMIT count) t
                 LEFT JOIN task_categories tc ON t."Id"=tc."TaskId"
                 LEFT JOIN category c ON tc."CategoryId"=c."Id"
                 LEFT JOIN project_tasks pt ON t."Id"=pt."TaskId";
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_non_project_tasks_with_categories(user_id INTEGER, skip INTEGER, count INTEGER)
    RETURNS TABLE (
        "TaskId"            INTEGER,
        "TaskName"          TEXT,
        "TaskUserId"        INTEGER,
        "TaskIsCompleted"   BOOLEAN,
        "CategoryId"        INTEGER,
        "CategoryName"      TEXT,
        "CategoryUserId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
          SELECT t."Id", t."Name"::TEXT, t."UserId", t."IsCompleted", c."Id", c."Name"::TEXT, c."UserId"
            FROM (SELECT * 
                    FROM task t 
                        LEFT JOIN project_tasks pt ON t."Id"=pt."TaskId" 
                   WHERE t."UserId"=user_id AND pt."ProjectId" IS NULL
                ORDER BY t."Id" OFFSET skip LIMIT count) t
                LEFT JOIN task_categories tc ON t."Id"=tc."TaskId"
                LEFT JOIN category c ON tc."CategoryId" = c."Id";
END
$$ LANGUAGE plpgsql;

-- End task with categories

-- Start project task with categories

CREATE OR REPLACE FUNCTION get_project_tasks_with_categories(project_id INTEGER)
    RETURNS TABLE (
        "TaskId"            INTEGER,
        "TaskName"          TEXT,
        "TaskUserId"        INTEGER,
        "TaskProjectId"     INTEGER,
        "TaskProjectIndex"  INTEGER,
        "TaskIsCompleted"   BOOLEAN,
        "CategoryId"        INTEGER,
        "CategoryName"      TEXT,
        "CategoryUserId"    INTEGER
    )
AS $$
BEGIN
    RETURN QUERY
          SELECT t."Id", 
                 t."Name"::TEXT, 
                 t."UserId", 
                 project_id, 
                 pti."TaskProjectIndex",
                 t."IsCompleted",
                 c."Id", 
                 c."Name"::TEXT, 
                 c."UserId"
           FROM get_project_tasks_indexes(project_id) pti 
               LEFT JOIN task t ON pti."TaskId"=t."Id"
               LEFT JOIN task_categories tc ON t."Id"=tc."TaskId"
               LEFT JOIN category c on tc."CategoryId"=c."Id"
       ORDER BY pti."TaskProjectIndex";
END
$$ LANGUAGE plpgsql;

-- End project task with categories