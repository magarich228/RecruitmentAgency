-- 1. Заполнение базовых таблиц (Activity, Profession, Qualification)
INSERT INTO "Activities" ("Id", "Name") VALUES
                                            (gen_random_uuid(), 'IT'),
                                            (gen_random_uuid(), 'Маркетинг'),
                                            (gen_random_uuid(), 'Финансы'),
                                            (gen_random_uuid(), 'Логистика'),
                                            (gen_random_uuid(), 'Образование');

INSERT INTO "Professions" ("Id", "Name") VALUES
                                             (gen_random_uuid(), 'Программист'),
                                             (gen_random_uuid(), 'Маркетолог'),
                                             (gen_random_uuid(), 'Бухгалтер'),
                                             (gen_random_uuid(), 'Логист'),
                                             (gen_random_uuid(), 'Преподаватель');

INSERT INTO "Qualifications" ("Id", "Name", "ProfessionId")
SELECT
    gen_random_uuid(),
    qual_data."Name",
    p."Id"
FROM
    (VALUES
         ('C#', 'Программист'),
         ('Python', 'Программист'),
         ('SMM', 'Маркетолог'),
         ('SEO', 'Маркетолог'),
         ('Налоговый учет', 'Бухгалтер'),
         ('1C:Логистика', 'Логист'),
         ('WMS', 'Логист'),
         ('Педагогика', 'Преподаватель'),
         ('Методика обучения', 'Преподаватель')
    ) AS qual_data("Name", "ProfessionName")
        JOIN "Professions" p ON p."Name" = qual_data."ProfessionName";

-- 2. Заполнение пользователей (User, Admin, Employee, Employer)
-- Администратор
WITH admin_user AS (
INSERT INTO "Users" ("Id", "PhoneNumber", "PasswordHash")
VALUES ('admin1', '+79991112233', 'hash_admin')
    RETURNING "Id"
    )
INSERT INTO "Admin" ("Id") SELECT "Id" FROM admin_user;

-- Работники
INSERT INTO "Users" ("Id", "PhoneNumber", "PasswordHash") VALUES
                                                              ('emp1', '+79994445566', 'hash_emp1'),
                                                              ('emp2', '+79997778899', 'hash_emp2'),
                                                              ('emp3', '+79991234567', 'hash_emp3'),
                                                              ('emp4', '+79995556677', 'hash_emp4'),
                                                              ('emp5', '+79996667788', 'hash_emp5');

INSERT INTO "Employee" ("Id", "FullName", "Resume") VALUES
                                                        ('emp1', 'Иванов Иван Иванович', 'Опыт работы: 5 лет в backend-разработке'),
                                                        ('emp2', 'Петрова Анна Сергеевна', 'Специалист по SMM'),
                                                        ('emp3', 'Сидоров Алексей Викторович', 'Бухгалтер 1С'),
                                                        ('emp4', 'Козлова Мария Дмитриевна', 'Логист с опытом работы в 1С'),
                                                        ('emp5', 'Николаев Денис Олегович', 'Преподаватель математики');

-- Работодатели
INSERT INTO "Users" ("Id", "PhoneNumber", "PasswordHash") VALUES
                                                              ('emplr1', '+79031112233', 'hash_emplr1'),
                                                              ('emplr2', '+79034445566', 'hash_emplr2'),
                                                              ('emplr3', '+79037778899', 'hash_emplr3');

INSERT INTO "Employer" ("Id", "Name", "MainAddress", "Description") VALUES
                                                                        ('emplr1', 'ООО ТехноСофт', 'Москва, ул. Ленина 1', 'IT-компания'),
                                                                        ('emplr2', 'Агентство МаркетПро', 'Санкт-Петербург, Невский пр. 50', 'Маркетинговое агентство'),
                                                                        ('emplr3', 'ООО ЛогикТранс', 'Казань, ул. Гагарина 15', 'Логистическая компания');

-- 3. Связь работодателей с направлениями деятельности (ActivityEmployer)
INSERT INTO "ActivityEmployer" ("ActivitiesId", "EmployersId")
SELECT
    a."Id",
    e."Id"
FROM
    "Employer" e
        CROSS JOIN
    (SELECT * FROM "Activities" WHERE "Name" IN ('IT', 'Маркетинг', 'Логистика')) a
WHERE
    (e."Name" = 'ООО ТехноСофт' AND a."Name" = 'IT') OR
    (e."Name" = 'Агентство МаркетПро' AND a."Name" = 'Маркетинг') OR
    (e."Name" = 'ООО ЛогикТранс' AND a."Name" = 'Логистика');

-- 4. Вакансии
INSERT INTO "Vacancies" ("Id", "Title", "Description", "MinSalary", "MaxSalary", "Commission", "EmployerId")
SELECT
    gen_random_uuid(),
    vac_data."Title",
    vac_data."Description",
    vac_data."MinSalary",
    vac_data."MaxSalary",
    vac_data."Commission",
    e."Id"
FROM
    (VALUES
         ('Backend-разработчик', 'Ищем C# разработчика', 120000, 200000, 15000, 'ООО ТехноСофт'),
         ('SMM-менеджер', 'Управление соцсетями', 60000, 90000, 8000, 'Агентство МаркетПро'),
         ('Логист', 'Работа с WMS-системами', 70000, 100000, 10000, 'ООО ЛогикТранс'),
         ('Финансовый аналитик', 'Анализ отчетности', 90000, 150000, 12000, 'ООО ТехноСофт'),
         ('Преподаватель программирования', 'Обучение студентов', 80000, 120000, 10000, 'Агентство МаркетПро')
    ) AS vac_data("Title", "Description", "MinSalary", "MaxSalary", "Commission", "EmployerName")
        JOIN "Employer" e ON e."Name" = vac_data."EmployerName";

-- 5. Связь квалификаций с вакансиями (QualificationVacancy)
INSERT INTO "QualificationVacancy" ("QualificationsId", "VacanciesId")
SELECT
    q."Id",
    v."Id"
FROM
    "Vacancies" v
        JOIN
    "Qualifications" q ON
        (v."Title" = 'Backend-разработчик' AND q."Name" = 'C#') OR
        (v."Title" = 'SMM-менеджер' AND q."Name" = 'SMM') OR
        (v."Title" = 'Логист' AND q."Name" = 'WMS') OR
        (v."Title" = 'Финансовый аналитик' AND q."Name" = 'Python') OR
        (v."Title" = 'Преподаватель программирования' AND q."Name" = 'Педагогика');

-- 6. Связь работников с квалификациями (EmployeeQualification)
INSERT INTO "EmployeeQualification" ("EmployeesId", "QualificationsId")
SELECT
    e."Id",
    q."Id"
FROM
    "Employee" e
        JOIN
    "Qualifications" q ON
        (e."FullName" = 'Иванов Иван Иванович' AND q."Name" = 'C#') OR
        (e."FullName" = 'Петрова Анна Сергеевна' AND q."Name" = 'SMM') OR
        (e."FullName" = 'Козлова Мария Дмитриевна' AND q."Name" = '1C:Логистика') OR
        (e."FullName" = 'Николаев Денис Олегович' AND q."Name" = 'Педагогика');

-- 7. Отклики и офферы
WITH applications AS (
INSERT INTO "JobApplications" ("Id", "EmployeeId", "VacancyId")
SELECT
    gen_random_uuid(),
    e."Id",
    v."Id"
FROM
    "Employee" e
        JOIN
    "Vacancies" v ON
        (e."FullName" = 'Иванов Иван Иванович' AND v."Title" = 'Backend-разработчик') OR
        (e."FullName" = 'Петрова Анна Сергеевна' AND v."Title" = 'SMM-менеджер') OR
        (e."FullName" = 'Козлова Мария Дмитриевна' AND v."Title" = 'Логист') OR
        (e."FullName" = 'Николаев Денис Олегович' AND v."Title" = 'Преподаватель программирования')
    RETURNING "Id", "VacancyId", "EmployeeId"
)
INSERT INTO "JobOffers" ("Id", "Verdict", "VacancyId", "EmployeeId", "JobApplicationId")
SELECT
    gen_random_uuid(),
    CASE
        WHEN RANDOM() < 0.5 THEN 'Приглашение'
        ELSE 'Отказ'
        END,
    a."VacancyId",
    a."EmployeeId",
    a."Id"
FROM applications a;