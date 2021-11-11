-- PostgreSQL - FristList schema

CREATE TABLE app_user (
    "Id"                    SERIAL PRIMARY KEY,
    "UserName"              VARCHAR(256) NOT NULL UNIQUE,
    "NormalizedUserName"    VARCHAR(256),
    "Email"                 VARCHAR(256) NOT NULL UNIQUE,
    "NormalizedEmail"       VARCHAR(256),
    "EmailConfirmed"        BOOLEAN DEFAULT false NOT NULL,
    "PhoneNumber"           VARCHAR(16),
    "PhoneNumberConfirmed"  BOOLEAN DEFAULT false NOT NULL,
    "PasswordHash"          VARCHAR(256) NOT NULL,
    "TwoFactorEnable"       BOOLEAN DEFAULT false NOT NULL
);

CREATE TABLE running_action (
    "UserId"    INTEGER NOT NULL,
    "StartTime" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    
    PRIMARY KEY ("UserId"),
    FOREIGN KEY ("UserId") REFERENCES app_user("Id")
);

CREATE TABLE category (
    "Id"        SERIAL PRIMARY KEY,
    "Name"      VARCHAR(256),
    "UserId"    INTEGER NOT NULL,

    FOREIGN KEY ("UserId") REFERENCES app_user("Id"),
    UNIQUE ("Name", "UserId")
);

CREATE TABLE running_action_categories (
    "UserId"        INTEGER NOT NULL,
    "CategoryId"    INTEGER NOT NULL,
    
    PRIMARY KEY ("UserId", "CategoryId"),
    FOREIGN KEY ("UserId") REFERENCES app_user("Id"),
    FOREIGN KEY ("CategoryId") REFERENCES category("Id")
);

CREATE TABLE action (
    "Id"            SERIAL PRIMARY KEY,
    "StartTime"     TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "EndTime"       TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "UserId"        INTEGER NOT NULL,

    CHECK ("StartTime" <= "EndTime"),
    FOREIGN KEY ("UserId") REFERENCES app_user("Id")
);

CREATE TABLE action_categories (
    "ActionId"      INTEGER NOT NULL,
    "CategoryId"    INTEGER NOT NULL,
    
    PRIMARY KEY ("ActionId", "CategoryId"),
    FOREIGN KEY ("ActionId") REFERENCES action("Id") ON DELETE CASCADE,
    FOREIGN KEY ("CategoryId") REFERENCES category("Id") ON DELETE CASCADE
);

CREATE TABLE task (
    "Id"        SERIAL PRIMARY KEY,
    "Name"      TEXT,
    "UserId"    INTEGER NOT NULL,
    
    FOREIGN KEY ("UserId") REFERENCES app_user("Id")
);

CREATE TABLE task_actions (
    "Id"            SERIAL PRIMARY KEY,
    "TaskId"        INTEGER NOT NULL,
    "ActionId"      INTEGER NOT NULL,
    "Description"   TEXT,
    
    FOREIGN KEY ("TaskId") REFERENCES task("Id"),
    FOREIGN KEY ("ActionId") REFERENCES action("Id")
);

CREATE TABLE task_categories (
    "TaskId"        INTEGER NOT NULL,
    "CategoryId"    INTEGER NOT NULL,
    
    PRIMARY KEY ("TaskId", "CategoryId"),
    FOREIGN KEY ("TaskId") REFERENCES task("Id"),
    FOREIGN KEY ("CategoryId") REFERENCES category("Id")
);

CREATE TABLE project (
    "Id"            SERIAL PRIMARY KEY,
    "Name"          TEXT NOT NULL,
    "Description"   TEXT,
    "UserId"        INTEGER NOT NULL,

    FOREIGN KEY ("UserId") REFERENCES app_user("Id")
);

CREATE TABLE project_tasks (
    "TaskId"            INTEGER NOT NULL,
    "ProjectId"         INTEGER NOT NULL,
    "IndexInProject"    INTEGER DEFAULT 0 NOT NULL,
    
    PRIMARY KEY ("TaskId"),
    FOREIGN KEY ("TaskId") REFERENCES task("Id"),
    FOREIGN KEY ("ProjectId") REFERENCES project("Id")
);

CREATE FUNCTION save_running_action(user_id INTEGER) RETURNS VOID
AS $$
DECLARE
    action_id INTEGER;
BEGIN
    IF NOT EXISTS(SELECT "UserId" FROM "running_action" WHERE "UserId" = user_id) THEN
        RETURN;
    END IF;

    INSERT INTO action ("StartTime", "EndTime", "UserId")
        (SELECT "StartTime", NOW() AT TIME ZONE 'UTC', user_id FROM running_action WHERE "UserId" = user_id)
        RETURNING "Id" INTO action_id;
    
    INSERT INTO action_categories ("ActionId", "CategoryId")
    SELECT action_id, "CategoryId" FROM running_action_categories WHERE "UserId" = user_id;

    DELETE FROM running_action_categories WHERE "UserId" = user_id;
    DELETE FROM running_action WHERE "UserId" = user_id;
END
$$ LANGUAGE plpgsql;

CREATE FUNCTION running_action_trigger_handler() RETURNS TRIGGER
AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        IF NEW."UserId" IS NULL OR EXISTS(SELECT "Id" FROM app_user WHERE "Id" = NEW."UserId") IS FALSE THEN
            RAISE EXCEPTION '"UserId" must be valid user';
        END IF;

        IF NEW."StartTime" IS NULL THEN
            NEW."StartTime" := NOW() AT TIME ZONE 'UTC';
        END IF;

        IF EXISTS(SELECT "UserId" FROM running_action WHERE "UserId"=NEW."UserId") THEN
            PERFORM save_running_action(NEW."UserId");
        END IF;

        RETURN NEW;
    ELSIF TG_OP = 'DELETE' THEN
        IF EXISTS(SELECT "UserId" FROM running_action WHERE "UserId"=OLD."UserId") THEN
            DELETE FROM running_action_categories WHERE "UserId" = OLD."UserId";
        END IF;

        RETURN OLD;
    END IF;

    RETURN NULL;
END
$$ LANGUAGE plpgsql;

CREATE FUNCTION check_running_action_categories() RETURNS TRIGGER
AS $$
BEGIN
    IF (SELECT "UserId" FROM category WHERE "Id" = NEW."CategoryId") != NEW."UserId" THEN
        RAISE EXCEPTION 'Category cannot belong to another user';
    END IF;

    RETURN NEW;
END
$$ LANGUAGE plpgsql;

CREATE FUNCTION check_project_task() RETURNS TRIGGER
AS $$
DECLARE 
    task_user_id        INTEGER;
    project_user_id     INTEGER;
BEGIN
    SELECT "UserId" FROM project WHERE "Id" = NEW."ProjectId" INTO project_user_id;
    SELECT "UserId" FROM task WHERE "Id" = NEW."TaskId" INTO task_user_id;

    IF project_user_id != task_user_id THEN
        RAISE EXCEPTION 'Incorrect project task';
    END IF;

    RETURN NEW;
END
$$ LANGUAGE plpgsql;

CREATE TRIGGER running_action_save_trigger BEFORE INSERT ON running_action
    FOR EACH ROW
    EXECUTE PROCEDURE running_action_trigger_handler();

CREATE TRIGGER running_action_delete_trigger BEFORE DELETE ON running_action
    FOR EACH ROW
    EXECUTE PROCEDURE running_action_trigger_handler();

CREATE CONSTRAINT TRIGGER check_running_action_categories AFTER INSERT OR UPDATE ON running_action_categories
    FOR EACH ROW
    EXECUTE PROCEDURE check_running_action_categories();

CREATE CONSTRAINT TRIGGER check_project_task_owner AFTER INSERT OR UPDATE ON project_tasks
    FOR EACH ROW
    EXECUTE PROCEDURE check_project_task();