IF EXISTS 
   (
     SELECT name FROM master.dbo.sysdatabases 
     WHERE name = N'Quiz2'
    )
DROP DATABASE Quiz2

CREATE DATABASE Quiz2
GO

USE Quiz2
GO

drop table if exists UserInfo
 create table UserInfo
(
    UserID INT PRIMARY KEY IDENTITY,
	UserName VARCHAR(20),
    Email VARCHAR(20),
    Password VARCHAR(20),
    FirstName VARCHAR(20),
    LastName VARCHAR(20),
    DOB DATE ,
    Role VARCHAR(10)
);

drop table if exists Category
CREATE TABLE Category (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(255) NOT NULL
);

drop table if exists Question
CREATE TABLE Question
(
    QuestionID INT PRIMARY KEY IDENTITY,
	CategoryID INT NOT NULL,
    QuesContent VARCHAR(MAX) NOT NULL,
	FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID)
);

drop table if exists Options
CREATE TABLE Options
(
    OptionID INT PRIMARY KEY IDENTITY,
    QuestionID INT ,
    OptionValue VARCHAR(255) ,
    ShouldChoose BIT ,
    FOREIGN KEY (QuestionID) REFERENCES Question(QuestionID)
);
GO;

insert into Category (categoryName) Values  ('SQL'), ('C#'), ('JS');
Select * From Category
Select * From Question
Select* From Options

delete from Question
DBCC CHECKIDENT ('Question', RESEED, 0)

delete from Options
DBCC CHECKIDENT ('Question', RESEED, 0)

--Insert sql questions
INSERT INTO Question (quesContent, categoryId)
VALUES ('What is the difference between a clustered and a non-clustered index?', 1),
       ( 'What is the purpose of the GROUP BY clause?', 1),
       ( 'What is the difference between a primary key and a unique key?', 1),
       ( 'What is the difference between a subquery and a join?', 1),
       ('What is the use of the DISTINCT keyword?', 1),
       ( 'What is the use of the UNION operator?', 1),
       ('What is the use of the EXISTS operator?', 1),
       ( 'What is the use of the LIKE operator?', 1),
       ('What is the use of the IN operator?', 1),
       ( 'What is the use of the ALL operator?', 1);
INSERT INTO options (questionId, optionValue, shouldChoose)
VALUES (1, 'A clustered index determines the physical order of data in a table.', 1),
       (1, 'A clustered index is a separate object that stores a pointer to the actual data.', 0),
	   (1, 'A non - clustered index using balance tree.', 0),
       (2, 'The GROUP BY clause is used to group rows with the same values into summary rows.', 1),
	   (2, 'The GROUP BY clause is used to group rows with the same values into summary rows.', 1),
       (3, 'A primary key is a unique identifier for a row within a database table.', 1),
       (3, 'A unique key is a constraint that ensures that all values in a column or set of columns are distinct.', 1),
       (4, 'A subquery is a query that is nested inside another query.', 1),
       (4, 'A join is a method of combining data from two or more tables based on a related column between them.', 1),
       (5, 'The DISTINCT keyword is used to return only distinct (different) values.', 1),
	   (5, 'The DISTINCT keyword is used to return only distinct (different) values.', 1),
       (6, 'The UNION operator is used to combine the result-set of two or more SELECT statements.', 1),
	   (6, 'The UNION operator is used to combine the result-set of two or more SELECT statements.', 1),
       (7, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
	   (7, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
	   (7, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
       (8, 'The LIKE operator is used to match text values against a pattern using wildcard characters.', 1),
	   (8, 'The LIKE operator is used to match text values against a pattern using wildcard characters.', 1),
       (9, 'The IN operator is used to determine whether a specified value matches any value in a list or a subquery.', 1),
	   (9, 'The IN operator is used to determine whether a specified value matches any value in a list or a subquery.', 1),
	   (10, 'The ALL operator is used to select all values of a column from all rows of a table.', 1),
       (10, 'The ALL operator is used to select all values of a column from all rows of a table.', 1);
GO

drop table if exists SessionLog
CREATE TABLE SessionLog
(
    SessionId VARCHAR(50) Primary Key,
    CategoryID INT,
	UserID INT,
    StartTime Datetime ,
    EndTime Datetime ,
    Score INT,
    FOREIGN KEY (UserID) REFERENCES UserInfo(UserID),
    FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID)
);
GO
Select* FROM SessionLog

CREATE TABLE Feedback
(
    FeedbackID INT PRIMARY KEY IDENTITY,
    UserID INT ,
    Rating INT ,
    FeedbackText VARCHAR(300),
    FOREIGN KEY (UserID) REFERENCES UserInfo(UserID)
);




drop table if exists QuestionLog
CREATE TABLE QuestionLog
(
    QuestionLogId INT Primary Key Identity,
    SessionId VARCHAR(50),
	QuestionId INT,
	QuesionInSessionId INT,
	Answer VARCHAR(20),
    FOREIGN KEY (SessionId) REFERENCES SessionLog(SessionId),
    FOREIGN KEY (QuestionId) REFERENCES Question(QuestionId)
);
GO
Select *  from SessionLog
Select * from QuestionLog
Go

delete from QuestionLog
DBCC CHECKIDENT ('QuestionLog', RESEED, 0)
delete from SessionLog
Go