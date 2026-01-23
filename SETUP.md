# Инструкция по запуску Teacher Dashboard

## Исправленные ошибки и добавленный функционал

### ✅ Исправленные критические ошибки:

1. **Аутентификация и авторизация**
   - Добавлена JWT аутентификация
   - Реализована регистрация пользователей
   - Добавлены guards для защиты маршрутов
   - Хеширование паролей с помощью BCrypt

2. **Не реализованные методы API**
   - Исправлены методы `toggle()` и `massCreate()` в CompletedAssignmentController
   - Добавлены полные CRUD операции для всех сущностей
   - Добавлена валидация данных

3. **Безопасность**
   - JWT токены для аутентификации
   - Валидация входных данных
   - Защита API endpoints атрибутом [Authorize]

### ✅ Добавленный функционал:

1. **Регистрация пользователей**
   - Endpoint: `POST /api/Auth/register`
   - Валидация данных
   - Проверка уникальности имени пользователя

2. **CRUD операции**
   - **Студенты**: GET, POST, PUT, DELETE + поиск
   - **Группы**: GET, POST, PUT, DELETE
   - **Задания**: GET, POST, PUT, DELETE
   - **Выполненные задания**: GET, POST, DELETE

3. **SQL запросы**
   - Статистика по группам через SQL запрос
   - Поиск студентов через SQL запрос

4. **Админ-панель**
   - Статистика по группам
   - Быстрые действия (заготовки)

## Предварительные требования

1. **PostgreSQL** (версия 12+)
2. **.NET 10.0 SDK**
3. **Node.js** (версия 18+)
4. **Angular CLI** (версия 21+)

## Настройка базы данных

1. Установите PostgreSQL
2. Создайте базу данных:
   ```sql
   CREATE DATABASE teacher_dashboard_db;
   ```
3. Обновите строку подключения в `Teacher_Dashboard_Backend/appsettings.json` если необходимо

## Запуск Backend

1. Перейдите в папку backend:
   ```bash
   cd Teacher_Dashboard_Backend
   ```

2. Восстановите пакеты:
   ```bash
   dotnet restore
   ```

3. Запустите приложение:
   ```bash
   dotnet run
   ```

Backend будет доступен по адресу: `http://localhost:5050`
Swagger UI: `http://localhost:5050/swagger`

## Запуск Frontend

1. Перейдите в папку frontend:
   ```bash
   cd Teacher_Dashboard_Frontend
   ```

2. Установите зависимости:
   ```bash
   npm install
   ```

3. Запустите приложение:
   ```bash
   ng serve
   ```

Frontend будет доступен по адресу: `http://localhost:4200`

## Тестовые пользователи

При первом запуске автоматически создаются тестовые пользователи:

| Логин   | Пароль     | Роль           |
|---------|------------|----------------|
| admin   | admin123   | Администратор  |
| teacher | teacher123 | Преподаватель  |
| demo    | demo123    | Преподаватель  |

## API Endpoints

### Аутентификация
- `POST /api/Auth/login` - Вход в систему
- `POST /api/Auth/register` - Регистрация

### Дашборд
- `GET /api/Dashboard` - Получить данные дашборда

### Студенты
- `GET /api/Students` - Получить всех студентов
- `GET /api/Students/{id}` - Получить студента по ID
- `POST /api/Students` - Создать студента
- `PUT /api/Students/{id}` - Обновить студента
- `DELETE /api/Students/{id}` - Удалить студента
- `GET /api/Students/search?query={query}` - Поиск студентов (SQL)

### Группы
- `GET /api/Groups` - Получить все группы
- `GET /api/Groups/{id}` - Получить группу по ID
- `POST /api/Groups` - Создать группу
- `PUT /api/Groups/{id}` - Обновить группу
- `DELETE /api/Groups/{id}` - Удалить группу

### Задания
- `GET /api/Assignments` - Получить все задания
- `GET /api/Assignments/{id}` - Получить задание по ID
- `GET /api/Assignments/by-group/{groupId}` - Получить задания группы
- `POST /api/Assignments` - Создать задание
- `PUT /api/Assignments/{id}` - Обновить задание
- `DELETE /api/Assignments/{id}` - Удалить задание

### Выполненные задания
- `GET /api/CompletedAssignment/by-assignment/{assignmentId}` - По заданию
- `GET /api/CompletedAssignment/student-progress/{studentId}` - Прогресс студента
- `POST /api/CompletedAssignment/toggle` - Переключить статус
- `POST /api/CompletedAssignment/massCreate` - Массовое создание
- `DELETE /api/CompletedAssignment/{recordId}` - Удалить запись
- `GET /api/CompletedAssignment/group-statistics` - Статистика по группам (SQL)

## Структура проекта

```
Teacher_Dashboard_Backend/
├── Controllers/          # API контроллеры
├── Models/              # Модели данных
├── DTOs/                # Data Transfer Objects
├── Services/            # Бизнес-логика
├── Data/                # Контекст БД и инициализация
└── Program.cs           # Точка входа

Teacher_Dashboard_Frontend/
├── src/app/
│   ├── pages/           # Страницы приложения
│   ├── services/        # Сервисы для API
│   ├── models/          # TypeScript модели
│   ├── guards/          # Route guards
│   ├── interceptors/    # HTTP interceptors
│   └── modal-forms/     # Модальные окна
```

## Функциональность

### Для преподавателей:
- Просмотр дашборда со студентами
- Фильтрация и поиск студентов
- Добавление заданий студентам
- Массовое добавление заданий
- Просмотр истории выполнения

### Для администраторов:
- Все функции преподавателя
- Статистика по группам
- Доступ к админ-панели

## Технологии

**Backend:**
- ASP.NET Core 10.0
- Entity Framework Core 10.0
- PostgreSQL
- JWT Authentication
- BCrypt для хеширования паролей
- Swagger для документации API

**Frontend:**
- Angular 21
- TypeScript
- RxJS
- Standalone Components
- Signals для реактивности

## Безопасность

- JWT токены с истечением срока действия
- Хеширование паролей
- Валидация входных данных
- CORS настройки
- Route guards для защиты страниц
- HTTP interceptors для автоматического добавления токенов

## Дополнительные возможности

- Автоматическая инициализация БД с тестовыми данными
- Responsive дизайн
- Обработка ошибок
- Loading индикаторы
- Soft delete для записей
- SQL запросы для аналитики