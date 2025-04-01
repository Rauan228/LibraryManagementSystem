# LibraryManagementSystem — Система управления библиотекой на WPF (C#)

## Описание
**LibraryManagementSystem** — это десктопное приложение, разработанное на WPF (C#), предназначенное для учета книг, читателей и выданных экземпляров в библиотеке. Проект включает две основные части: административную панель для управления данными и пользовательский интерфейс для взаимодействия читателей с библиотекой. Система позволяет отслеживать книги на руках у читателей, управлять заявками на выдачу книг, а также выполнять аналитические запросы, такие как подсчет выданных книг по жанрам или выявление должников.

Приложение взаимодействует с базой данных Microsoft SQL Server, где хранятся данные о книгах, читателях и выдачах.

---

## Основные возможности
### Административная часть
- Управление таблицами:
  - **Книги** (`books`): добавление, редактирование, удаление книг.
  - **Читатели** (`borrowers`): управление списком пользователей.
  - **Выдачи** (`loans`): учет выданных и возвращенных книг.
- Аналитика:
  - Список книг, находящихся на руках у читателей.
  - Количество книг, выданных каждому читателю.
  - Поиск читателей, не вернувших книги в срок.
  - Подсчет выданных книг по жанрам.
- Хранимая процедура для автоматического продления срока возврата книги.

### Пользовательская часть
- Просмотр списка доступных книг.
- Отправка заявок на выдачу книги.
- Отслеживание статуса заявок и выданных книг.

---

## Технологии
- **WPF (C#)**: Интерфейс приложения.
- **Microsoft SQL Server**: База данных для хранения информации.
- **ADO.NET**: Подключение и работа с базой данных.
- **.NET Framework**: Основа приложения.


---

## Структура проекта
Предполагаемая структура репозитория (основана на типичной организации WPF-проекта):
```
LibraryManagementSystem/
├── Models/                   # Модели данных (Book, Borrower, Loan)
├── Views/                    # XAML-файлы для интерфейса
│   ├── Admin/                # Интерфейсы админской части
│   │   ├── BooksView.xaml    # Управление книгами
│   │   ├── BorrowersView.xaml# Управление читателями
│   │   └── LoansView.xaml    # Управление выдачами
│   └── User/                 # Интерфейсы пользовательской части
│       ├── BooksView.xaml    # Просмотр книг
│       ├── RequestsView.xaml # Отслеживание заявок
├── ViewModels/               # Логика взаимодействия с интерфейсом
│   ├── AdminViewModel.cs     # ViewModel для админской части
│   └── UserViewModel.cs      # ViewModel для пользовательской части
├── Data/                     # Работа с базой данных
│   └── DatabaseContext.cs    # Подключение к БД
├── App.xaml                  # Точка входа приложения
├── MainWindow.xaml           # Главное окно с выбором роли (админ/пользователь)
├── .gitignore                # Игнорируемые файлы
├── LibraryManagementSystem.csproj # Файл проекта
├── README.md                 # Документация
└── SQL/                      # SQL-скрипты
    └── create_database.sql    # Создание и заполнение БД
```

---

## Требования
- **ОС**: Windows (WPF работает только на Windows).
- **.NET Framework**: Версия 4.7.2 или выше.
- **Microsoft SQL Server**: Установленный сервер (например, MSSQL16).
- **Visual Studio**: Для сборки и запуска проекта (рекомендуется 2022).

---

## Установка и запуск

### 1. Клонирование репозитория
Склонируйте проект с GitHub:
```bash
git clone https://github.com/Rauan228/LibraryManagementSystem.git
cd LibraryManagementSystem
```

### 2. Настройка базы данных
1. Убедитесь, что Microsoft SQL Server установлен и запущен.
2. Выполните SQL-скрипт для создания базы данных и заполнения данными (см. раздел "Создание и заполнение базы данных").
3. Обновите строку подключения в `Data/DatabaseContext.cs`:
   ```csharp
   string connectionString = "Server=localhost;Database=LibraryMS;Trusted_Connection=True;";
   ```

### 3. Открытие проекта
1. Откройте файл `LibraryManagementSystem.csproj` в Visual Studio.
2. Убедитесь, что все зависимости восстановлены (NuGet автоматически загрузит пакеты).

### 4. Сборка и запуск
- Нажмите `F5` в Visual Studio для сборки и запуска приложения.
- Выберите роль: администратор или пользователь.

---

## Создание и заполнение базы данных
Ниже приведен SQL-скрипт для создания базы данных `LibraryMS` и заполнения её тестовыми данными (20 записей для каждой таблицы, кроме администраторов).

```sql
USE [master]
GO

-- Создание базы данных
CREATE DATABASE [LibraryMS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LibraryMS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.TDG2022\MSSQL\DATA\LibraryMS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LibraryMS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.TDG2022\MSSQL\DATA\LibraryMS_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [LibraryMS] SET COMPATIBILITY_LEVEL = 160
GO

USE [LibraryMS]
GO

-- Таблица администраторов
CREATE TABLE [dbo].[tblAdmins](
    [AdminId] [int] IDENTITY(1,1) NOT NULL,
    [AdminName] [nvarchar](100) NOT NULL,
    [AdminEmail] [nvarchar](255) NOT NULL,
    [AdminPass] [nvarchar](255) NOT NULL,
    PRIMARY KEY CLUSTERED ([AdminId] ASC),
    UNIQUE NONCLUSTERED ([AdminEmail] ASC)
) ON [PRIMARY]
GO

-- Таблица книг
CREATE TABLE [dbo].[tblBooks](
    [BookId] [int] IDENTITY(1,1) NOT NULL,
    [BookTitle] [nvarchar](255) NOT NULL,
    [BookAuthor] [nvarchar](255) NOT NULL,
    [BookGenre] [nvarchar](50) NOT NULL,
    [BookCopies] [int] NOT NULL,
    PRIMARY KEY CLUSTERED ([BookId] ASC)
) ON [PRIMARY]
GO

-- Таблица читателей
CREATE TABLE [dbo].[tblBorrowers](
    [BorrowerId] [int] IDENTITY(1,1) NOT NULL,
    [BorrowerName] [nvarchar](100) NOT NULL,
    [BorrowerEmail] [nvarchar](255) NOT NULL,
    [BorrowerPass] [nvarchar](255) NOT NULL,
    PRIMARY KEY CLUSTERED ([BorrowerId] ASC),
    UNIQUE NONCLUSTERED ([BorrowerEmail] ASC)
) ON [PRIMARY]
GO

-- Таблица выдач
CREATE TABLE [dbo].[tblLoans](
    [LoanId] [int] IDENTITY(1,1) NOT NULL,
    [BookId] [int] NOT NULL,
    [BorrowerId] [int] NOT NULL,
    [DateIssued] [date] NOT NULL,
    [DateDue] [date] NOT NULL,
    [DateReturned] [date] NULL,
    PRIMARY KEY CLUSTERED ([LoanId] ASC),
    FOREIGN KEY ([BookId]) REFERENCES [dbo].[tblBooks] ([BookId]),
    FOREIGN KEY ([BorrowerId]) REFERENCES [dbo].[tblBorrowers] ([BorrowerId])
) ON [PRIMARY]
GO

-- Заполнение tblAdmins (1 запись)
INSERT INTO [dbo].[tblAdmins] (AdminName, AdminEmail, AdminPass)
VALUES ('Admin', 'admin@example.com', 'admin123');
GO

-- Заполнение tblBooks (20 записей)
INSERT INTO [dbo].[tblBooks] (BookTitle, BookAuthor, BookGenre, BookCopies)
VALUES 
    ('The Great Gatsby', 'F. Scott Fitzgerald', 'Fiction', 5),
    ('1984', 'George Orwell', 'Dystopia', 3),
    ('To Kill a Mockingbird', 'Harper Lee', 'Fiction', 4),
    ('Pride and Prejudice', 'Jane Austen', 'Romance', 6),
    ('The Catcher in the Rye', 'J.D. Salinger', 'Fiction', 2),
    ('Brave New World', 'Aldous Huxley', 'Dystopia', 3),
    ('Moby Dick', 'Herman Melville', 'Adventure', 4),
    ('War and Peace', 'Leo Tolstoy', 'Historical', 5),
    ('The Hobbit', 'J.R.R. Tolkien', 'Fantasy', 7),
    ('Fahrenheit 451', 'Ray Bradbury', 'Dystopia', 3),
    ('Jane Eyre', 'Charlotte Brontë', 'Romance', 4),
    ('The Odyssey', 'Homer', 'Epic', 2),
    ('Crime and Punishment', 'Fyodor Dostoevsky', 'Fiction', 5),
    ('The Lord of the Rings', 'J.R.R. Tolkien', 'Fantasy', 6),
    ('Animal Farm', 'George Orwell', 'Satire', 3),
    ('Wuthering Heights', 'Emily Brontë', 'Romance', 4),
    ('The Iliad', 'Homer', 'Epic', 2),
    ('Great Expectations', 'Charles Dickens', 'Fiction', 5),
    ('The Alchemist', 'Paulo Coelho', 'Adventure', 3),
    ('Dracula', 'Bram Stoker', 'Horror', 4);
GO

-- Заполнение tblBorrowers (20 записей)
INSERT INTO [dbo].[tblBorrowers] (BorrowerName, BorrowerEmail, BorrowerPass)
VALUES 
    ('John Doe', 'john.doe@example.com', 'pass123'),
    ('Jane Smith', 'jane.smith@example.com', 'pass456'),
    ('Alice Johnson', 'alice.j@example.com', 'pass789'),
    ('Bob Brown', 'bob.brown@example.com', 'pass101'),
    ('Emma Davis', 'emma.davis@example.com', 'pass202'),
    ('Michael Wilson', 'michael.w@example.com', 'pass303'),
    ('Sarah Taylor', 'sarah.t@example.com', 'pass404'),
    ('David Lee', 'david.lee@example.com', 'pass505'),
    ('Laura Adams', 'laura.a@example.com', 'pass606'),
    ('James Clark', 'james.c@example.com', 'pass707'),
    ('Emily White', 'emily.w@example.com', 'pass808'),
    ('Tom Harris', 'tom.h@example.com', 'pass909'),
    ('Olivia King', 'olivia.k@example.com', 'pass010'),
    ('William Green', 'william.g@example.com', 'pass111'),
    ('Sophia Turner', 'sophia.t@example.com', 'pass222'),
    ('Henry Moore', 'henry.m@example.com', 'pass333'),
    ('Chloe Evans', 'chloe.e@example.com', 'pass444'),
    ('Liam Parker', 'liam.p@example.com', 'pass555'),
    ('Mia Scott', 'mia.s@example.com', 'pass666'),
    ('Noah Hill', 'noah.h@example.com', 'pass777');
GO

-- Заполнение tblLoans (20 записей)
INSERT INTO [dbo].[tblLoans] (BookId, BorrowerId, DateIssued, DateDue, DateReturned)
VALUES 
    (1, 1, '2025-03-01', '2025-03-15', NULL),
    (2, 2, '2025-03-02', '2025-03-16', '2025-03-10'),
    (3, 3, '2025-03-03', '2025-03-17', NULL),
    (4, 4, '2025-03-04', '2025-03-18', NULL),
    (5, 5, '2025-03-05', '2025-03-19', '2025-03-12'),
    (6, 6, '2025-03-06', '2025-03-20', NULL),
    (7, 7, '2025-03-07', '2025-03-21', NULL),
    (8, 8, '2025-03-08', '2025-03-22', '2025-03-15'),
    (9, 9, '2025-03-09', '2025-03-23', NULL),
    (10, 10, '2025-03-10', '2025-03-24', NULL),
    (11, 11, '2025-03-11', '2025-03-25', '2025-03-18'),
    (12, 12, '2025-03-12', '2025-03-26', NULL),
    (13, 13, '2025-03-13', '2025-03-27', NULL),
    (14, 14, '2025-03-14', '2025-03-28', NULL),
    (15, 15, '2025-03-15', '2025-03-29', '2025-03-20'),
    (16, 16, '2025-03-16', '2025-03-30', NULL),
    (17, 17, '2025-03-17', '2025-03-31', NULL),
    (18, 18, '2025-03-18', '2025-04-01', NULL),
    (19, 19, '2025-03-19', '2025-04-02', '2025-03-25'),
    (20, 20, '2025-03-20', '2025-04-03', NULL);
GO

-- Хранимая процедура для продления срока возврата
CREATE PROCEDURE [dbo].[ExtendLoanDueDate]
    @LoanId INT,
    @DaysToExtend INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[tblLoans]
    SET DateDue = DATEADD(day, @DaysToExtend, DateDue)
    WHERE LoanId = @LoanId AND DateReturned IS NULL;
END;
GO
```

Сохраните этот скрипт в `SQL/create_database.sql` и выполните его в SQL Server Management Studio (SSMS).

---

## Использование
### Администратор
1. Войдите как администратор: `admin@example.com` / `admin123`.
2. Управляйте книгами, читателями и выдачами через соответствующие вкладки.
3. Используйте аналитику для просмотра отчетов.

### Пользователь
1. Войдите с учетной записью (например, `john.doe@example.com` / `pass123`).
2. Просматривайте список книг и отправляйте заявки на выдачу.
3. Отслеживайте статус своих заявок и выданных книг.

---

## Устранение ошибок
### Ошибка подключения к базе данных
- Убедитесь, что SQL Server запущен.
- Проверьте строку подключения в `DatabaseContext.cs`.

### Ошибка: `Missing dependencies`
- Восстановите NuGet-пакеты в Visual Studio: `Tools > NuGet Package Manager > Restore NuGet Packages`.

---

## Контакты
Автор: **Ахметов Рауан**  
Email: [rauan.az.2006@gmail.com](mailto:rauan.az.2006@gmail.com)  
GitHub: [Rauan228](https://github.com/Rauan228)

---

## Лицензия
Проект распространяется под лицензией MIT.
```

---

### Пояснения
1. **Описание**: Указано, что это система управления библиотекой с разделением на админскую и пользовательскую части.
2. **Структура**: Предложена структура, основанная на типичной организации WPF-проекта.
3. **База данных**: Созданы таблицы `tblAdmins`, `tblBooks`, `tblBorrowers`, `tblLoans` с 20 записями для книг, читателей и выдач, и 1 запись для админа. Добавлена процедура `ExtendLoanDueDate`.
4. **Инструкции**: Подробно описаны установка, запуск и использование.

Сохраните этот текст в `README.md` в корне репозитория. Если у вас есть конкретные файлы проекта или дополнительные детали, которые нужно включить, дайте знать!
