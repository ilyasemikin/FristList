-- Functions

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
        SELECT "Id" AS "CategoryId", 
               "Name"::text AS "CategoryName", 
               "UserId" 
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
        SELECT "Id" AS "CategoryId", 
               "Name"::text AS "CategoryName", 
               "UserId" 
          FROM category 
         WHERE "UserId"=user_id 
      ORDER BY "Id" 
        OFFSET skip 
         LIMIT count;
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
               lower(a."During") AS "ActionStartTime",
               upper(a."During") AS "ActionEndTime",
               a."UserId" AS "ActionUserId",
               c."Id" AS "CategoryId", 
               c."Name"::text AS "CategoryName",
               c."UserId" AS "CategoryUserId"
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
        SELECT a."Id" AS "ActionId",
               lower(a."During") AS "ActionStartTime",
               upper(a."During") AS "ActionEndTime",
               user_id AS "ActionUserId",
               c."Id" AS "CategoryId",
               c."Name"::text AS "CategoryName",
               user_id AS "CategoryUserId"
        FROM (SELECT * FROM action WHERE "UserId"=user_id ORDER BY "Id" OFFSET skip LIMIT count) a
                 LEFT JOIN action_categories ac ON a."Id"=ac."ActionId"
                 LEFT JOIN category c ON ac."CategoryId"=c."Id";
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
        SELECT ra."StartTime" AS "RunningActionStartTime", 
               ra."UserId" AS "RunningActionUserId", 
               rac."CategoryId",
               c."Name"::text AS "CategoryName",
               c."UserId" AS "CategoryUserId"
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
        SELECT t."Id" AS "TaskId", 
               t."Name"::text AS "TaskName",
               t."UserId" AS "TaskUserId",
               pt."ProjectId" AS "TaskProjectId", 
               c."Id" AS "CategoryId", 
               c."Name"::text AS "CategoryName",
               c."UserId" AS "CategoryUserId"
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
        SELECT t."Id" AS "TaskId", 
               t."Name"::text AS "TaskName",
               t."UserId" AS "TaskUserId",
               pt."ProjectId" AS "TaskProjectId",
               c."Id" AS "CategoryId",
               c."Name"::text AS "CategoryName",
               c."UserId" AS "CategoryUserId"
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
        SELECT p."Id" AS "ProjectId", 
               p."Name"::text AS "ProjectName", 
               p."Description"::text AS "ProjectDescription", 
               p."UserId" AS "ProjectUserId" 
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
        SELECT p."Id" AS "ProjectId", 
               p."Name"::text AS "ProjectName", 
               p."Description"::text AS "ProjectDescription", 
               p."UserId" AS "ProjectUserId" 
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
        SELECT t."Id" AS "TaskId", 
               t."Name"::text AS "TaskName", 
               t."UserId" AS "TaskUserId", 
               pt."ProjectId" AS "TaskProjectId", 
               c."Id" AS "CategoryId", 
               c."Name"::text AS "CategoryName",
               c."UserId" AS "CategoryUserId"
          FROM (SELECT * FROM project_tasks pt WHERE pt."ProjectId"=project_id ORDER BY pt."TaskId" OFFSET skip LIMIT count) pt 
              LEFT JOIN task t ON pt."TaskId"=t."Id" 
              LEFT JOIN task_categories tc ON t."Id" = tc."TaskId" 
              LEFT JOIN category c ON tc."CategoryId" = c."Id";
END
$$ LANGUAGE plpgsql;

-- End project functions