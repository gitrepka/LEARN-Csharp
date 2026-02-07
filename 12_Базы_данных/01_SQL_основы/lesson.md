# Тема 12.1: Основы SQL

## Что ты узнаешь
- SELECT, INSERT, UPDATE, DELETE
- WHERE, ORDER BY, GROUP BY, JOIN
- Создание таблиц, PRIMARY KEY, FOREIGN KEY
- Индексы

---

## Объяснение

### ЗАЧЕМ?
Данные приложений (пользователи, заказы, настройки) хранятся в **базах данных**. SQL — язык для работы с ними. Даже если используешь ORM (EF Core), знание SQL обязательно.

```sql
-- Создание таблицы
CREATE TABLE Players (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Level INTEGER DEFAULT 1,
    Score INTEGER DEFAULT 0,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Вставка
INSERT INTO Players (Name, Level, Score) VALUES ('Алиса', 15, 1200);
INSERT INTO Players (Name, Level, Score) VALUES ('Боб', 10, 800);

-- Выборка
SELECT * FROM Players;
SELECT Name, Level FROM Players WHERE Level > 10 ORDER BY Score DESC;
SELECT Level, COUNT(*) as Count, AVG(Score) as AvgScore
FROM Players GROUP BY Level HAVING COUNT(*) > 1;

-- Обновление
UPDATE Players SET Level = 16, Score = 1300 WHERE Name = 'Алиса';

-- Удаление
DELETE FROM Players WHERE Score < 100;

-- JOIN — объединение таблиц
SELECT p.Name, i.ItemName FROM Players p
INNER JOIN Inventory i ON p.Id = i.PlayerId;
```

---

## Мини-упражнения
1. **⭐** Создай таблицу, вставь 5 записей, выбери с WHERE.
2. **⭐** GROUP BY + COUNT + AVG.
3. **⭐⭐** JOIN двух таблиц.

## Что дальше
Дальше — **Entity Framework Core** (12.2).
