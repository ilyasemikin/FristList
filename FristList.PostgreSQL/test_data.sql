CREATE EXTENSION btree_gist;

BEGIN;

CREATE TEMPORARY TABLE test_action (
    "Id"                SERIAL PRIMARY KEY,
    "During"            TSRANGE NOT NULL,
    "Description"       TEXT DEFAULT ''
)
ON COMMIT DELETE ROWS;

CREATE INDEX ON test_action USING GIST ("During");

CREATE TEMPORARY TABLE test_category (
    "Id"        SERIAL PRIMARY KEY,
    "Name"      TEXT
)
ON COMMIT DELETE ROWS;

CREATE TEMPORARY TABLE test_action_categories (
    "ActionId"          INTEGER NOT NULL,
    "CategoryId"        INTEGER NOT NULL,
    
    FOREIGN KEY ("ActionId") REFERENCES test_action("Id"),
    FOREIGN KEY ("CategoryId") REFERENCES test_category("Id"),
    PRIMARY KEY ("ActionId", "CategoryId")
)
ON COMMIT DELETE ROWS;

INSERT INTO test_action ("During", "Description")
     VALUES (TSRANGE('2020-01-01 00:00', '2020-01-01 01:00', '[)'), DEFAULT),
            (TSRANGE('2020-01-01 00:30', '2020-01-01 01:30', '[)'), DEFAULT),
	        (TSRANGE('2020-01-01 00:30', '2020-01-01 01:30', '[)'), DEFAULT),
	        (TSRANGE('2020-01-01 01:00', '2020-01-01 02:00', '[)'), DEFAULT),
	        (TSRANGE('2020-01-01 03:00', '2020-01-01 03:30', '[)'), DEFAULT),
            (TSRANGE('2020-01-01 03:15', '2020-01-01 04:00', '[)'), DEFAULT),
            (TSRANGE('2020-01-01 05:00', '2020-01-01 05:30', '[)'), DEFAULT),
            (TSRANGE('2020-01-01 05:30', '2020-01-01 06:00', '[)'), DEFAULT);

INSERT INTO test_category ("Name")
     VALUES ('Category1'),
            ('Category2'),
            ('Category3');

INSERT INTO test_action_categories ("ActionId", "CategoryId")
     VALUES (1, 1),
            (1, 2),
            (2, 1),
            (3, 1),
            (4, 1),
            (5, 1),
            (6, 1),
            (7, 1),
            (8, 1);

SELECT a."Id", a."During", tac."CategoryId" 
  FROM test_action a JOIN test_action_categories tac on a."Id" = tac."ActionId";

WITH t1 AS (
	SELECT a."Id", LOWER(a."During") AS start, UPPER(a."During") AS end, tac."CategoryId", COALESCE(LOWER(a."During") > MAX(UPPER(a."During")) OVER(PARTITION BY tac."CategoryId" ORDER BY LOWER(a."During"), UPPER(a."During") ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING), TRUE) ns 
	  FROM test_action a LEFT JOIN test_action_categories tac on a."Id" = tac."ActionId"
),
t2 AS (
	SELECT a."CategoryId", a.start, a.end, SUM(ns::INTEGER) OVER(ORDER BY a.start, a.end) AS grp FROM t1 a
)
--SELECT * FROM t1;
SELECT MIN(a.start) AS "StartTime", MAX(a.end) AS "EndTime", MAX(a.end) - MIN(a.start) AS "Interval", SUM(MAX(a.end) - MIN(a.start)) OVER(PARTITION BY a."CategoryId"), a."CategoryId" FROM t2 a GROUP BY grp, a."CategoryId" ORDER BY a."CategoryId", 1;

COMMIT;
