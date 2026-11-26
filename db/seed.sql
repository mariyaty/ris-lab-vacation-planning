-- Учебные данные для ЛР9. Все учётные записи и сведения вымышлены.
-- password_hash содержит демонстрационные строки, а не реальные пароли.

INSERT INTO sites (id, name) VALUES
    (1, 'Участок связи'),
    (2, 'Локомотивное депо')
ON CONFLICT (id) DO UPDATE SET name = EXCLUDED.name;

INSERT INTO users (id, full_name, login, password_hash, role) VALUES
    (1, 'Иванова Анна Сергеевна', 'hr.ivanova',
     '$2b$12$demo_hash_for_course_only_000000000000000000000000000000', 'HRSpecialist'),
    (2, 'Петров Алексей Дмитриевич', 'planner.petrov',
     '$2b$12$demo_hash_for_course_only_111111111111111111111111111111', 'Planner'),
    (3, 'Сидорова Елена Павловна', 'manager.sidorova',
     '$2b$12$demo_hash_for_course_only_222222222222222222222222222222', 'Manager')
ON CONFLICT (id) DO UPDATE SET
    full_name = EXCLUDED.full_name,
    login = EXCLUDED.login,
    password_hash = EXCLUDED.password_hash,
    role = EXCLUDED.role;

INSERT INTO vacation_requests
    (id, number, title, description, site_id, status,
     created_at, created_by_user_id, assignee_user_id)
VALUES
    (1, 'VR-00001', 'График отпусков участка связи',
     'Подготовить проект графика отпусков работников участка связи.',
     1, 'New', CURRENT_TIMESTAMP - INTERVAL '4 days', 1, NULL),
    (2, 'VR-00002', 'План отпусков локомотивного депо',
     'Согласовать периоды отпусков сотрудников локомотивного депо.',
     2, 'InProgress', CURRENT_TIMESTAMP - INTERVAL '3 days', 1, 2),
    (3, 'VR-00003', 'Корректировка графика отпусков',
     'Проверить и внести согласованные изменения в график отпусков.',
     1, 'Closed', CURRENT_TIMESTAMP - INTERVAL '2 days', 1, 2),
    (4, 'VR-00004', 'Перенос периода отпуска',
     'Рассмотреть запрос на перенос ранее запланированного периода отпуска.',
     2, 'Cancelled', CURRENT_TIMESTAMP - INTERVAL '1 day', 1, NULL)
ON CONFLICT (id) DO UPDATE SET
    number = EXCLUDED.number,
    title = EXCLUDED.title,
    description = EXCLUDED.description,
    site_id = EXCLUDED.site_id,
    status = EXCLUDED.status,
    created_at = EXCLUDED.created_at,
    created_by_user_id = EXCLUDED.created_by_user_id,
    assignee_user_id = EXCLUDED.assignee_user_id;

-- Синхронизация счётчиков IDENTITY после вставки явных идентификаторов.
SELECT setval(pg_get_serial_sequence('sites', 'id'),
              GREATEST((SELECT COALESCE(MAX(id), 1) FROM sites), 1), true);
SELECT setval(pg_get_serial_sequence('users', 'id'),
              GREATEST((SELECT COALESCE(MAX(id), 1) FROM users), 1), true);
SELECT setval(pg_get_serial_sequence('vacation_requests', 'id'),
              GREATEST((SELECT COALESCE(MAX(id), 1) FROM vacation_requests), 1), true);
