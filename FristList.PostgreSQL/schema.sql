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
    "TwoFactorEnable"       BOOLEAN NOT NULL
);

CREATE TABLE category (
    "Id"        SERIAL PRIMARY KEY,
    "Name"      VARCHAR(256),
    "UserId"    INTEGER NOT NULL,
    
    FOREIGN KEY ("UserId") REFERENCES app_user("Id")
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

CREATE TABLE project (
    "Id"            SERIAL PRIMARY KEY,
    "Name"          TEXT NOT NULL,
    "Description"   TEXT,
    "UserId"        INTEGER NOT NULL,

    FOREIGN KEY ("UserId") REFERENCES app_user("Id")
);

CREATE TABLE task (
    "Id"        SERIAL PRIMARY KEY,
    "Name"      TEXT,
    "UserId"    INTEGER NOT NULL,
    
    FOREIGN KEY ("UserId") REFERENCES app_user("Id")
);

CREATE TABLE task_categories (
    "TaskId"        INTEGER NOT NULL,
    "CategoryId"    INTEGER NOT NULL,
    
    PRIMARY KEY ("TaskId", "CategoryId"),
    FOREIGN KEY ("TaskId") REFERENCES task("Id"),
    FOREIGN KEY ("CategoryId") REFERENCES category("Id")
);
