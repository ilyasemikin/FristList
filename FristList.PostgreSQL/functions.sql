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

CREATE OR REPLACE FUNCTION get_user_categories_time(user_id INTEGER)
RETURNS TABLE (
    "CategoryId"        INTEGER,
    "CategoryName"      TEXT,
    "CategoryUserId"    INTEGER,
    "IntervalTime"      INTERVAL
)
AS $$
BEGIN
    RETURN QUERY
        SELECT c."Id", c."Name"::TEXT, a."UserId",
               SUM(upper(a."During") - lower(a."During"))
          FROM action a 
              LEFT JOIN action_categories ac ON a."Id"=ac."ActionId" 
              LEFT JOIN category c ON ac."CategoryId"=c."Id" 
         WHERE a."UserId"=user_id
        GROUP BY c."Id", c."Name", a."UserId";
END
$$ LANGUAGE plpgsql;

-- End category function

-- Action functions

CREATE OR REPLACE FUNCTION get_action(action_id INTEGER)
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

CREATE OR REPLACE FUNCTION get_user_actions(user_id INTEGER, skip INTEGER, count INTEGER) 
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

CREATE OR REPLACE FUNCTION get_user_actions_without_categories(user_id INTEGER, skip INTEGER, count INTEGER)
RETURNS TABLE (
    "ActionId"          INTEGER,
    "ActionStartTime"   TIMESTAMP WITHOUT TIME ZONE,
    "ActionEndTime"     TIMESTAMP WITHOUT TIME ZONE,
    "ActionUserId"      INTEGER
)
AS $$
BEGIN
    RETURN QUERY
        SELECT a."Id", lower(a."During"), upper(a."During"), a."UserId"
          FROM action a 
              LEFT JOIN action_categories ac ON a."Id" = ac."ActionId"
        WHERE a."UserId"=user_id AND ac."CategoryId" IS NULL
     ORDER BY a."Id"
       OFFSET skip
        LIMIT count;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_actions_by_time(user_id INTEGER, from_time TIMESTAMP, to_time TIMESTAMP)
RETURNS TABLE (
    "ActionId"          INTEGER,
    "ActionStartTime"   TIMESTAMP WITHOUT TIME ZONE,
    "ActionEndTime"     TIMESTAMP WITHOUT TIME ZONE,
    "ActionUserId"      INTEGER,
    "CategoryId"        INTEGER,
    "CategoryName"      TEXT,
    "CategoryActionId"  INTEGER
)
AS $$
BEGIN
    RETURN QUERY
        SELECT a."Id",
               lower(a."During"),
               upper(a."During"),
               a."UserId",
               c."Id",
               c."Name",
               a."UserId"
          FROM action a 
              LEFT JOIN action_categories ac ON a."Id"=ac."ActionId"
              LEFT JOIN category c ON ac."CategoryId"=c."Id"
         WHERE a."UserId"=user_id AND a."During" && TSRANGE(from_time, to_time);
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_time(user_id INTEGER, from_time TIMESTAMP, to_time TIMESTAMP)
RETURNS TABLE (
    "Time"          INTERVAL,
    "TotalTime"     INTERVAL,
    "ActionCount"   INTEGER
)
AS $$
BEGIN
    RETURN QUERY
        SELECT COALESCE(SUM(upper(a."During") - lower(a."During")), INTERVAL '0'),
               to_time - from_time,
               COUNT(*)::integer
          FROM action a
         WHERE "UserId"=user_id AND a."During" && TSRANGE(from_time, to_time);
END
$$ LANGUAGE plpgsql;

-- End action functions

-- Running action functions

CREATE OR REPLACE FUNCTION get_current_action(user_id INTEGER)
RETURNS TABLE (
    "RunningActionStartTime"    TIMESTAMP WITHOUT TIME ZONE,
    "RunningActionUserId"       INTEGER,
    "CategoryId"                INTEGER,
    "CategoryName"              TEXT,
    "CategoryUserId"            INTEGER
)
AS $$
BEGIN
    RETURN QUERY
        SELECT ra."StartTime", 
               ra."UserId", 
               rac."CategoryId",
               c."Name"::TEXT,
               c."UserId"
          FROM running_action ra 
              LEFT JOIN running_action_categories rac ON ra."UserId"=rac."UserId" 
              LEFT JOIN category c ON rac."CategoryId"=c."Id"
        WHERE ra."UserId"=user_id;
END
$$ LANGUAGE plpgsql;

-- End running action functions

-- Task functions

CREATE OR REPLACE FUNCTION get_task(task_id INTEGER) 
RETURNS TABLE (
    "TaskId"            INTEGER,
    "TaskName"          TEXT,
    "TaskUserId"        INTEGER,
    "TaskProjectId"     INTEGER,
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

CREATE OR REPLACE FUNCTION get_user_tasks(user_id INTEGER, skip INTEGER, count INTEGER)
RETURNS TABLE (
    "TaskId"            INTEGER,
    "TaskName"          TEXT,
    "TaskUserId"        INTEGER,
    "TaskProjectId"     INTEGER,
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
               c."Id",
               c."Name"::TEXT,
               c."UserId"
          FROM (SELECT * FROM task WHERE "UserId"=user_id ORDER BY "Id" OFFSET skip LIMIT count) t 
              LEFT JOIN task_categories tc ON t."Id"=tc."TaskId" 
              LEFT JOIN category c ON tc."CategoryId"=c."Id" 
              LEFT JOIN project_tasks pt ON t."Id"=pt."TaskId";
END
$$ LANGUAGE plpgsql;

-- End task functions

-- Project functions

CREATE OR REPLACE FUNCTION get_project(project_id INTEGER)
RETURNS TABLE (
    "ProjectId"             INTEGER,
    "ProjectName"           TEXT,
    "ProjectDescription"    TEXT,
    "ProjectUserId"         INTEGER
)
AS $$
BEGIN
    RETURN QUERY 
        SELECT p."Id", p."Name"::TEXT, p."Description"::TEXT, p."UserId" 
          FROM project p 
         WHERE "Id"=project_id;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_projects(user_id INTEGER, skip INTEGER, count INTEGER)
RETURNS TABLE (
    "ProjectId"             INTEGER,
    "ProjectName"           TEXT,
    "ProjectDescription"    TEXT,
    "ProjectUserId"         INTEGER
)
AS $$
BEGIN
    RETURN QUERY
        SELECT p."Id", p."Name"::TEXT, p."Description"::TEXT, p."UserId"
          FROM project p WHERE "UserId"=user_id 
      ORDER BY "Id" 
        OFFSET skip 
         LIMIT count;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_project_tasks(project_id INTEGER, skip INTEGER, count INTEGER)
RETURNS TABLE (
    "TaskId"            INTEGER,
    "TaskName"          TEXT,
    "TaskUserId"        INTEGER,
    "TaskProjectId"     INTEGER,
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
               c."Id",
               c."Name"::TEXT,
               c."UserId"
          FROM (SELECT * FROM project_tasks pt WHERE pt."ProjectId"=project_id ORDER BY pt."TaskId" OFFSET skip LIMIT count) pt 
              LEFT JOIN task t ON pt."TaskId"=t."Id" 
              LEFT JOIN task_categories tc ON t."Id" = tc."TaskId" 
              LEFT JOIN category c ON tc."CategoryId" = c."Id";
END
$$ LANGUAGE plpgsql;

-- End project functions