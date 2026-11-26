-- ЛР9. Обязательные SQL-запросы.

-- 1. Фильтр по статусу и сортировка по дате создания (сначала новые).
SELECT number, title, status, created_at
FROM vacation_requests
WHERE status = 'New'
ORDER BY created_at DESC;

-- 2. JOIN: выводим название участка из справочника sites,
-- а не только внешний ключ site_id.
SELECT
    vr.number,
    vr.title,
    s.name AS site_name,
    vr.status
FROM vacation_requests AS vr
JOIN sites AS s ON s.id = vr.site_id
ORDER BY vr.created_at DESC;

-- 3. GROUP BY: количество заявок по каждому статусу.
-- В seed.sql есть записи для всех четырёх статусов.
SELECT status, COUNT(*) AS request_count
FROM vacation_requests
GROUP BY status
ORDER BY status;

-- 4. Дополнительный JOIN: автор и назначенный ответственный.
SELECT
    vr.number,
    creator.full_name AS created_by,
    assignee.full_name AS assignee,
    vr.status
FROM vacation_requests AS vr
JOIN users AS creator ON creator.id = vr.created_by_user_id
LEFT JOIN users AS assignee ON assignee.id = vr.assignee_user_id
ORDER BY vr.created_at DESC;

-- 5. Проверка ограничения CHECK.
-- Раскомментируйте и выполните отдельно: PostgreSQL должен отклонить
-- вставку со статусом Done. Ошибочная строка не попадёт в таблицу.
--
-- INSERT INTO vacation_requests
--     (number, title, description, site_id, status, created_by_user_id)
-- VALUES
--     ('VR-INVALID', 'Тест неверного статуса',
--      'Проверка ограничения допустимых статусов.', 1, 'Done', 1);
