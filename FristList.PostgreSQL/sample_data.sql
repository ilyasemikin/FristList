-- Fill empty database

-- user1: password 123isis123
-- user2: password 123isis123
-- user3: password 123isis123
INSERT INTO app_user ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "PasswordHash") 
VALUES (1, 'user1', 'USER1', 'user1@gmail.com', 'USER1@GMAIL.COM', 'AQAAAAEAACcQAAAAEOTcPk6lm3RpBYFY9zq+yIBzVGo9LSEmh9CoVIxnv0Edfbq67q6XnFfcT7+QQleIcw=='),
       (2, 'user2', 'USER2', 'user2@gmail.com', 'USER2@GMAIL.COM', 'AQAAAAEAACcQAAAAEAXSSCbEb/zUnMBh5vKS/sd7x6L0fupRu9lazAHgeJTMy3Sd+XAe8pS5ZwJIT8CwIQ=='),
       (3, 'user3', 'USER3', 'user3@gmail.com', 'USER3@GMAIL.COM', 'AQAAAAEAACcQAAAAENpcduOGNo/+rMSc7YztlWtN6EjYdgvMJH8pWRnv9uuH/VFG2CyPL0hTQT2qKOpRjw==');

INSERT INTO category ("Id", "Name", "UserId") 
VALUES (1, 'Work', 1),
       (2, 'Sport', 1),
       (3, 'Reading', 1),
       (4, 'Relax', 1),
       (5, 'Gaming', 1),
       (6, 'Drawing', 1),
       (7, 'Singing', 2),
       (8, 'Work', 2),
       (9, 'Drawing', 2),
       (10, 'Bicycle ride', 2),
       (11, 'Language learning', 2),
       (12, 'Practice', 2),
       (13, 'Programming', 3),
       (14, 'Reading', 3),
       (15, 'Hardware engineering', 3),
       (16, 'Sport', 3),
       (17, 'Pet projects', 3);

INSERT INTO project ("Id", "Name", "UserId")
VALUES (1, 'Flower picture', 1),
       (2, 'Finish Disco Elysium', 1),
       (3, 'Gym', 1),
       (4, 'English learning', 2),
       (5, 'German learning', 2),
       (6, 'Finish console program', 3),
       (7, 'Trash game', 3);

INSERT INTO task ("Id", "Name", "UserId")
VALUES (1,  'dignissim.', 1), 
       (2,  'ac', 1),
       (3,  'euismod', 1),
       (4,  'sit', 1),
       (5,  'metus.', 1),
       (6,  'sit', 1),
       (7,  'consequat', 1),
       (8,  'sit', 1),
       (9,  'natoque', 1),
       (10, 'lacus.', 1),
       (11, 'tellus.', 1),
       (12, 'magna.', 2),
       (13, 'arcu', 2),
       (14, 'purus', 2),
       (15, 'Aliquam', 2),
       (16, 'convallis', 2),
       (17, 'pede,', 2),
       (18, 'risus', 3),
       (19, 'luctus', 3),
       (20, 'erat', 3),
       (21, 'orci', 3),
       (22, 'nec', 3),
       (23, 'felis.', 3),
       (24, 'Aliquam', 3),
       (25, 'ipsum', 3),
       (26, 'Aliquam', 3),
       (27, 'nibh', 3),
       (28, 'lobortis', 3),
       (29, 'hendrerit', 3),
       (30, 'aliquet', 3);

INSERT INTO task_categories ("TaskId", "CategoryId")
VALUES (1, 1),
       (1, 2),
       (1, 3),
       (2, 3),
       (3, 2),
       (3, 5),
       (3, 6),
       (5, 1),
       (5, 5),
       (5, 6),
       (7, 2),
       (7, 3),
       (10, 4),
       (10, 5),
       (10, 6),
       (11, 1),
       (11, 3),
       (11, 5),
       (11, 6);

INSERT INTO project_tasks ("TaskId", "ProjectId", "IndexInProject")
VALUES (1,  1, 0),
       (2,  1, 1),
       (3,  1, 2),
       (4,  1, 3),
       (15, 4, 0),
       (17, 4, 1),
       (24, 6, 0),
       (28, 6, 1),
       (30, 6, 2);

ALTER SEQUENCE "app_user_Id_seq" RESTART WITH 4;
ALTER SEQUENCE "category_Id_seq" RESTART WITH 18;
ALTER SEQUENCE "project_Id_seq" RESTART WITH 8;
ALTER SEQUENCE "task_Id_seq" RESTART WITH 31;