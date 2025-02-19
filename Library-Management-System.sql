USE [master]
GO
/****** Object:  Database [LibraryMSWF]    Script Date: 13.02.2025 1:35:55 ******/
CREATE DATABASE [LibraryMSWF]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LibraryMSWF', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.TDG2022\MSSQL\DATA\LibraryMSWF.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LibraryMSWF_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.TDG2022\MSSQL\DATA\LibraryMSWF_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [LibraryMSWF] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LibraryMSWF].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LibraryMSWF] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LibraryMSWF] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LibraryMSWF] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LibraryMSWF] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LibraryMSWF] SET ARITHABORT OFF 
GO
ALTER DATABASE [LibraryMSWF] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [LibraryMSWF] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LibraryMSWF] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LibraryMSWF] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LibraryMSWF] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LibraryMSWF] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LibraryMSWF] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LibraryMSWF] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LibraryMSWF] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LibraryMSWF] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LibraryMSWF] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LibraryMSWF] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LibraryMSWF] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LibraryMSWF] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LibraryMSWF] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LibraryMSWF] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LibraryMSWF] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LibraryMSWF] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LibraryMSWF] SET  MULTI_USER 
GO
ALTER DATABASE [LibraryMSWF] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LibraryMSWF] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LibraryMSWF] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LibraryMSWF] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LibraryMSWF] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LibraryMSWF] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [LibraryMSWF] SET QUERY_STORE = ON
GO
ALTER DATABASE [LibraryMSWF] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200)
GO
USE [LibraryMSWF]
GO
/****** Object:  Table [dbo].[tblAdmins]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAdmins](
	[AdminId] [int] IDENTITY(1,1) NOT NULL,
	[AdminName] [nvarchar](100) NOT NULL,
	[AdminEmail] [nvarchar](255) NOT NULL,
	[AdminPass] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[AdminEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblBooks]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBooks](
	[BookId] [int] IDENTITY(1,1) NOT NULL,
	[BookName] [nvarchar](255) NOT NULL,
	[BookAuthor] [nvarchar](255) NOT NULL,
	[BookISBN] [nvarchar](50) NOT NULL,
	[BookPrice] [decimal](10, 2) NOT NULL,
	[BookCopies] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[BookISBN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRecievedUsers]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRecievedUsers](
	[BookId] [int] NOT NULL,
	[BookName] [nvarchar](255) NOT NULL,
	[DateRecieved] [date] NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRequestedUsers]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRequestedUsers](
	[BookId] [int] NOT NULL,
	[BookName] [nvarchar](255) NOT NULL,
	[DateRequested] [date] NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblReturnedUsers]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblReturnedUsers](
	[BookId] [int] NOT NULL,
	[BookName] [nvarchar](255) NOT NULL,
	[DateReturned] [date] NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUsers]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUsers](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[UserAdNo] [nvarchar](50) NOT NULL,
	[UserEmail] [nvarchar](255) NOT NULL,
	[UserPass] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[UserEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblRecievedUsers]  WITH CHECK ADD FOREIGN KEY([BookId])
REFERENCES [dbo].[tblBooks] ([BookId])
GO
ALTER TABLE [dbo].[tblRecievedUsers]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[tblUsers] ([UserId])
GO
ALTER TABLE [dbo].[tblRequestedUsers]  WITH CHECK ADD FOREIGN KEY([BookId])
REFERENCES [dbo].[tblBooks] ([BookId])
GO
ALTER TABLE [dbo].[tblRequestedUsers]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[tblUsers] ([UserId])
GO
ALTER TABLE [dbo].[tblReturnedUsers]  WITH CHECK ADD FOREIGN KEY([BookId])
REFERENCES [dbo].[tblBooks] ([BookId])
GO
ALTER TABLE [dbo].[tblReturnedUsers]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[tblUsers] ([UserId])
GO
ALTER TABLE [dbo].[tblBooks]  WITH CHECK ADD CHECK  (([BookCopies]>=(0)))
GO
/****** Object:  StoredProcedure [dbo].[AddBook]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddBook]  
    @BookName NVARCHAR(255),  
    @BookAuthor NVARCHAR(255),  
    @BookISBN NVARCHAR(50),  
    @BookPrice DECIMAL(10,2),  
    @BookCopies INT,  
    @Result INT OUTPUT  -- Добавили OUTPUT параметр  
AS  
BEGIN  
    SET NOCOUNT ON;  

    INSERT INTO dbo.tblBooks (BookName, BookAuthor, BookISBN, BookPrice, BookCopies)  
    VALUES (@BookName, @BookAuthor, @BookISBN, @BookPrice, @BookCopies);  

    SET @Result = @@ROWCOUNT;  -- Устанавливаем количество вставленных строк  
END
GO
/****** Object:  StoredProcedure [dbo].[AddRecieve]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddRecieve]
    @bId INT,
    @bName NVARCHAR(255),
    @date DATE,
    @uId INT,
    @uName NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- Проверяем, существует ли книга и пользователь
    IF EXISTS (SELECT 1 FROM tblBooks WHERE BookId = @bId)
       AND EXISTS (SELECT 1 FROM tblUsers WHERE UserId = @uId)
    BEGIN
        INSERT INTO tblRecievedUsers (BookId, BookName, DateRecieved, UserId, UserName)
        VALUES (@bId, @bName, @date, @uId, @uName);

        -- Проверяем, действительно ли запись добавилась
        IF @@ROWCOUNT > 0
            SELECT 1 AS Inserted; -- Успешное добавление
        ELSE
            SELECT 0 AS Inserted; -- Ошибка вставки
    END
    ELSE
    BEGIN
        SELECT -1 AS Inserted; -- Ошибка: нет книги или пользователя
    END
END;
GO
/****** Object:  StoredProcedure [dbo].[AddRequest]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddRequest]
    @bId INT,
    @bName NVARCHAR(255),
    @date DATE,
    @uId INT,
    @uName NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Requests (BookId, BookName, RequestDate, UserId, UserName)
    VALUES (@bId, @bName, @date, @uId, @uName);
END;
GO
/****** Object:  StoredProcedure [dbo].[AddReturn]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Создаем процедуру для добавления возврата книги  
CREATE PROCEDURE [dbo].[AddReturn]  
    @bId INT,  
    @bName NVARCHAR(255),  
    @date DATE,  
    @uId INT,  
    @uName NVARCHAR(255)  
AS  
BEGIN  
    SET NOCOUNT ON;  
      
    INSERT INTO tblReturnedUsers (BookId, BookName, DateReturned, UserId, UserName)  
    VALUES (@bId, @bName, @date, @uId, @uName);  
END;  
GO
/****** Object:  StoredProcedure [dbo].[AddUser]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddUser]
    @name NVARCHAR(100),
    @adno INT,
    @email NVARCHAR(100),
    @pass NVARCHAR(100)
AS
BEGIN
    INSERT INTO tblUsers (UserName, UserAdNo, UserEmail, UserPass)
    VALUES (@name, @adno, @email, @pass);

    -- Обязательно возвращаем 'true'
    SELECT 'true';
END;
GO
/****** Object:  StoredProcedure [dbo].[AdminLogin]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AdminLogin]
    @adminEmail VARCHAR(50),
    @adminPass VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) 
    FROM tblAdmins 
    WHERE AdminEmail = @adminEmail AND AdminPass = @adminPass;
END;
GO
/****** Object:  StoredProcedure [dbo].[DeleteBook]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteBook]  
    @BookId INT  
AS  
BEGIN  
    SET NOCOUNT ON;  

    -- Проверяем, существует ли книга  
    IF EXISTS (SELECT 1 FROM tblBooks WHERE BookId = @BookId)  
    BEGIN  
        -- Удаляем зависимые записи во всех связанных таблицах  
        DELETE FROM tblRequestedUsers WHERE BookId = @BookId;  
        DELETE FROM tblRecievedUsers WHERE BookId = @BookId;  
        DELETE FROM tblReturnedUsers WHERE BookId = @BookId;  

        -- Теперь удаляем саму книгу  
        DELETE FROM tblBooks WHERE BookId = @BookId;  

        SELECT 1 AS Deleted; -- Успешное удаление  
    END  
    ELSE  
    BEGIN  
        SELECT 0 AS Deleted; -- Книга не найдена  
    END  
END;
GO
/****** Object:  StoredProcedure [dbo].[DeleteRecieve]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRecieve]  
    @bId INT,  
    @uId INT  
AS  
BEGIN  
    SET NOCOUNT ON;  
      
    DELETE FROM tblRecievedUsers WHERE BookId = @bId AND UserId = @uId;  
      
    IF @@ROWCOUNT = 0  
        RAISERROR ('Record not found', 16, 1);  
END;  
GO
/****** Object:  StoredProcedure [dbo].[DeleteRequest]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRequest]  
    @bId INT,  
    @uId INT,  
    @Deleted BIT OUTPUT  
AS  
BEGIN  
    SET NOCOUNT ON;  

    DELETE FROM tblRequestedUsers WHERE BookId = @bId AND UserId = @uId;  
      
    IF @@ROWCOUNT > 0  
        SET @Deleted = 1; -- Успешное удаление  
    ELSE  
        SET @Deleted = 0; -- Запись не найдена  
END;
GO
/****** Object:  StoredProcedure [dbo].[DeleteReturn]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Создаем процедуру для удаления записи о возврате книги
CREATE PROCEDURE [dbo].[DeleteReturn]
    @bId INT,
    @uId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM Returns WHERE BookId = @bId AND UserId = @uId;
    
    IF @@ROWCOUNT = 0
        RAISERROR ('Запись о возврате не найдена', 16, 1);
END;
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUser]  
    @UserId INT,
    @Deleted INT OUTPUT  
AS  
BEGIN  
    SET NOCOUNT ON;  

    -- Удаление связанных записей
    DELETE FROM dbo.tblRecievedUsers WHERE UserId = @UserId;
    DELETE FROM dbo.tblRequestedUsers WHERE UserId = @UserId;
    DELETE FROM dbo.tblReturnedUsers WHERE UserId = @UserId;

    -- Удаление пользователя
    DELETE FROM dbo.tblUsers WHERE UserId = @UserId;

    -- Количество удалённых строк
    SET @Deleted = @@ROWCOUNT;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetAllBooks]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllBooks]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT BookId, BookName, BookAuthor, BookISBN, BookPrice, BookCopies FROM dbo.tblBooks;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetAllRecieve]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllRecieve]  
AS  
BEGIN  
    SET NOCOUNT ON;  
      
    SELECT * FROM tblRecievedUsers;  
END;  
GO
/****** Object:  StoredProcedure [dbo].[GetAllRecieveUser]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllRecieveUser]  
    @UserId INT  
AS  
BEGIN  
    SET NOCOUNT ON;  
      
    SELECT * FROM tblRecievedUsers WHERE UserId = @UserId;  
END;  
GO
/****** Object:  StoredProcedure [dbo].[GetAllRequest]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllRequest]  
AS  
BEGIN  
    SET NOCOUNT ON;  
      
    SELECT * FROM tblRequestedUsers;  
END;  
GO
/****** Object:  StoredProcedure [dbo].[GetAllRequestUser]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllRequestUser]  
    @UserId INT  
AS  
BEGIN  
    SET NOCOUNT ON;  
      
    SELECT * FROM tblRequestedUsers WHERE UserId = @UserId;  
END;  
GO
/****** Object:  StoredProcedure [dbo].[GetAllReturn]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Создаем процедуру для получения всех возвратов книг
CREATE PROCEDURE [dbo].[GetAllReturn]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM Returns;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetAllUsers]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllUsers]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UserId, UserName, UserAdNo, UserEmail, UserPass FROM tblUsers;
END;
GO
/****** Object:  StoredProcedure [dbo].[IncBookCopy]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IncBookCopy]  
    @BookId INT  
AS  
BEGIN  
    SET NOCOUNT ON;  

    DECLARE @RowsAffected INT;

    UPDATE dbo.tblBooks  
    SET BookCopies = BookCopies + 1  
    WHERE BookId = @BookId;

    SET @RowsAffected = @@ROWCOUNT;

    IF @RowsAffected = 0
    BEGIN
        RAISERROR ('Ошибка: Книга с таким BookId не найдена.', 16, 1);
        RETURN -1;  -- Возвращаем -1 в случае ошибки
    END

    RETURN @RowsAffected;  -- Возвращаем количество изменённых строк
END;
GO
/****** Object:  StoredProcedure [dbo].[TakeUserName]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TakeUserName]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT UserName FROM tblUsers WHERE UserId = @UserId;
END;
GO
/****** Object:  StoredProcedure [dbo].[UpdateBook]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateBook]
    @BookId INT,
    @BookName NVARCHAR(255),
    @BookAuthor NVARCHAR(255),
    @BookISBN NVARCHAR(50),
    @BookPrice DECIMAL(18,2),
    @BookCopies INT
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE tblBooks
    SET 
        BookName = @BookName,
        BookAuthor = @BookAuthor,
        BookISBN = @BookISBN,
        BookPrice = @BookPrice,
        BookCopies = @BookCopies
    WHERE BookId = @BookId;

    -- Возвращаем количество измененных строк
    SELECT @@ROWCOUNT;
END;
GO
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUser]
    @id INT,
    @name NVARCHAR(100),
    @adno INT,
    @email NVARCHAR(100),
    @pass NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE tblUsers
    SET 
        UserName = @name,
        UserAdNo = @adno,
        UserEmail = @email,
        UserPass = @pass
    WHERE UserId = @id;

    -- Возвращаем результат
    IF @@ROWCOUNT > 0
        SELECT 1 AS Result;  -- Успешно обновлено
    ELSE
        SELECT 0 AS Result;  -- Пользователь не найден
END;
GO
/****** Object:  StoredProcedure [dbo].[UserLogin]    Script Date: 13.02.2025 1:35:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UserLogin]
    @UserEmail NVARCHAR(255),
    @UserPass NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UserId, UserName -- Должно быть в SELECT
    FROM tblUsers
    WHERE UserEmail = @UserEmail AND UserPass = @UserPass;
END
GO
USE [master]
GO
ALTER DATABASE [LibraryMSWF] SET  READ_WRITE 
GO
