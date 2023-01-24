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
Go

ALTER TABLE UserInfo
ADD status varchar(10) DEFAULT 'active'
ALTER TABLE UserInfo
ADD CONSTRAINT CHK_status CHECK (status IN ('active', 'suspended'))
ALTER TABLE UserInfo
ADD address varchar(50) DEFAULT '14850 Hs Blvd, Portland, OR 98032'
ALTER TABLE UserInfo
ADD phone varchar(10) DEFAULT '5772984374'
GO

INSERT INTO userInfo(UserName, Email, Password, FirstName, LastName, DOB, Role) VALUES('user1','111@gmail.com','111','first1','last1','2022-01-01','student' )
INSERT INTO userInfo(UserName, Email, Password, FirstName, LastName, DOB, Role) VALUES('user2','222@gmail.com','222','first2','last2','2000-06-01','student' )
INSERT INTO userInfo(UserName, Email, Password, FirstName, LastName, DOB, Role) VALUES('user3','333@gmail.com','333','first3','last3','1999-01-05','student' )
INSERT INTO userInfo(UserName, Email, Password, FirstName, LastName, DOB, Role) VALUES('user4','444@gmail.com','444','first4','last4','1988-08-12','Admin' )																				
Go

select * from UserInfo


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
Select * From  Options
--ALTER TABLE Options
--ALTER COLUMN ShouldChoose bit NOT NULL;
--ALTER TABLE Options
--ALTER COLUMN QuestionId int NOT NULL;
--ALTER TABLE Options
--ALTER COLUMN  OptionId int not null;
----Truncate Table Question
--Truncate Table options

delete from Question
DBCC CHECKIDENT ('Question', RESEED, 0)

delete from Options
DBCC CHECKIDENT ('Options', RESEED, 0)

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
       ( 'What is the use of the ALL operator?', 1),
	   ('What is the difference between a clustered and a non-clustered index?', 1),
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
       (10, 'The ALL operator is used to select all values of a column from all rows of a table.', 1),
	   (11, 'A clustered index determines the physical order of data in a table.', 1),
       (11, 'A clustered index is a separate object that stores a pointer to the actual data.', 0),
	   (11, 'A non - clustered index using balance tree.', 0),
       (12, 'The GROUP BY clause is used to group rows with the same values into summary rows.', 1),
	   (12, 'The GROUP BY clause is used to group rows with the same values into summary rows.', 1),
       (13, 'A primary key is a unique identifier for a row within a database table.', 1),
       (13, 'A unique key is a constraint that ensures that all values in a column or set of columns are distinct.', 1),
       (14, 'A subquery is a query that is nested inside another query.', 1),
       (14, 'A join is a method of combining data from two or more tables based on a related column between them.', 1),
       (15, 'The DISTINCT keyword is used to return only distinct (different) values.', 1),
	   (15, 'The DISTINCT keyword is used to return only distinct (different) values.', 1),
       (16, 'The UNION operator is used to combine the result-set of two or more SELECT statements.', 1),
	   (16, 'The UNION operator is used to combine the result-set of two or more SELECT statements.', 1),
       (17, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
	   (17, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
	   (17, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
       (18, 'The LIKE operator is used to match text values against a pattern using wildcard characters.', 1),
	   (18, 'The LIKE operator is used to match text values against a pattern using wildcard characters.', 1),
       (19, 'The IN operator is used to determine whether a specified value matches any value in a list or a subquery.', 1),
	   (19, 'The IN operator is used to determine whether a specified value matches any value in a list or a subquery.', 1),
	   (20, 'The ALL operator is used to select all values of a column from all rows of a table.', 1),
       (20, 'The ALL operator is used to select all values of a column from all rows of a table.', 1);
GO

--Insert c# questions
INSERT INTO Question (quesContent, categoryId)
VALUES ('What is the difference between a clustered and a non-clustered index?', 2),
       ( 'What is the purpose of the GROUP BY clause?', 2),
       ( 'What is the difference between a primary key and a unique key?', 2),
       ( 'What is the difference between a subquery and a join?', 2),
       ('What is the use of the DISTINCT keyword?', 2),
       ( 'What is the use of the UNION operator?', 2),
       ('What is the use of the EXISTS operator?', 2),
       ( 'What is the use of the LIKE operator?', 2),
       ('What is the use of the IN operator?', 2),
       ( 'What is the use of the ALL operator?', 2),
	   ('What is the difference between a clustered and a non-clustered index?', 2),
       ( 'What is the purpose of the GROUP BY clause?', 2),
       ( 'What is the difference between a primary key and a unique key?', 2),
       ( 'What is the difference between a subquery and a join?', 2),
       ('What is the use of the DISTINCT keyword?', 2),
       ( 'What is the use of the UNION operator?', 2),
       ('What is the use of the EXISTS operator?', 2),
       ( 'What is the use of the LIKE operator?', 2),
       ('What is the use of the IN operator?', 2),
       ( 'What is the use of the ALL operator?', 2)
INSERT INTO options (questionId, optionValue, shouldChoose)
VALUES (21, 'A clustered index determines the physical order of data in a table.', 1),
       (21, 'A clustered index is a separate object that stores a pointer to the actual data.', 0),
	   (21, 'A non - clustered index using balance tree.', 0),
       (22, 'The GROUP BY clause is used to group rows with the same values into summary rows.', 1),
	   (22, 'The GROUP BY clause is used to group rows with the same values into summary rows.', 1),
       (23, 'A primary key is a unique identifier for a row within a database table.', 1),
       (23, 'A unique key is a constraint that ensures that all values in a column or set of columns are distinct.', 1),
       (24, 'A subquery is a query that is nested inside another query.', 1),
       (24, 'A join is a method of combining data from two or more tables based on a related column between them.', 1),
       (25, 'The DISTINCT keyword is used to return only distinct (different) values.', 1),
	   (25, 'The DISTINCT keyword is used to return only distinct (different) values.', 1),
       (26, 'The UNION operator is used to combine the result-set of two or more SELECT statements.', 1),
	   (26, 'The UNION operator is used to combine the result-set of two or more SELECT statements.', 1),
       (27, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
	   (27, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
	   (27, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
       (28, 'The LIKE operator is used to match text values against a pattern using wildcard characters.', 1),
	   (28, 'The LIKE operator is used to match text values against a pattern using wildcard characters.', 1),
       (29, 'The IN operator is used to determine whether a specified value matches any value in a list or a subquery.', 1),
	   (29, 'The IN operator is used to determine whether a specified value matches any value in a list or a subquery.', 1),
	   (30, 'The ALL operator is used to select all values of a column from all rows of a table.', 1),
       (30, 'The ALL operator is used to select all values of a column from all rows of a table.', 1),
	   (31, 'A clustered index determines the physical order of data in a table.', 1),
       (31, 'A clustered index is a separate object that stores a pointer to the actual data.', 0),
	   (31, 'A non - clustered index using balance tree.', 0),
       (32, 'The GROUP BY clause is used to group rows with the same values into summary rows.', 1),
	   (32, 'The GROUP BY clause is used to group rows with the same values into summary rows.', 1),
       (33, 'A primary key is a unique identifier for a row within a database table.', 1),
       (33, 'A unique key is a constraint that ensures that all values in a column or set of columns are distinct.', 1),
       (34, 'A subquery is a query that is nested inside another query.', 1),
       (34, 'A join is a method of combining data from two or more tables based on a related column between them.', 1),
       (35, 'The DISTINCT keyword is used to return only distinct (different) values.', 1),
	   (35, 'The DISTINCT keyword is used to return only distinct (different) values.', 1),
       (36, 'The UNION operator is used to combine the result-set of two or more SELECT statements.', 1),
	   (36, 'The UNION operator is used to combine the result-set of two or more SELECT statements.', 1),
       (37, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
	   (37, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
	   (37, 'The EXISTS operator is used to test for the existence of any record in a subquery.', 1),
       (38, 'The LIKE operator is used to match text values against a pattern using wildcard characters.', 1),
	   (38, 'The LIKE operator is used to match text values against a pattern using wildcard characters.', 1),
       (39, 'The IN operator is used to determine whether a specified value matches any value in a list or a subquery.', 1),
	   (39, 'The IN operator is used to determine whether a specified value matches any value in a list or a subquery.', 1),
	   (40, 'The ALL operator is used to select all values of a column from all rows of a table.', 1),
       (40, 'The ALL operator is used to select all values of a column from all rows of a table.', 1);
GO

--JS----
INSERT INTO Question (quesContent, categoryId)
VALUES ('What is the difference between == and ===?', 3),
       ( 'What is closure in JavaScript?', 3),
       ( 'What is the use of the map() method in JavaScript?', 3),
       ( 'What is the use of the reduce() method in JavaScript?', 3),
       ('What is the difference between let and var in JavaScript?', 3),
       ( 'What is the use of the filter() method in JavaScript?', 3),
       ('What is the use of the forEach() method in JavaScript?', 3),
       ( 'What is the use of the some() method in JavaScript?', 3),
       ('What is the use of the every() method in JavaScript?', 3),
       ( 'What is the use of the find() method in JavaScript?', 3),
	   ('What is the use of the findIndex() method in JavaScript?', 3),
       ( 'What is the use of the sort() method in JavaScript?', 3),
       ( 'What is the use of the concat() method in JavaScript?', 3),
       ( 'What is the use of the slice() method in JavaScript?', 3),
       ('What is the use of the splice() method in JavaScript?', 3),
       ( 'What is the use of the push() method in JavaScript?', 3),
       ('What is the use of the pop() method in JavaScript?', 3),
	   ( 'What is the use of the shift() method in JavaScript?', 3),
       ('What is the use of the unshift() method in JavaScript?', 3)
INSERT INTO options (questionId, optionValue, shouldChoose)
VALUES (41, '== compares values and === compares both value and type.', 1),
       (41, '== compares both value and type and === compares values.', 0),
	   (42, 'Closures are functions that have access to variables defined in their parent scope.', 1),
	   (42, 'Closures are data structures that have access to variables defined in their parent scope.', 0),
	   (43, 'The map() method creates a new array with the results of calling a provided function on every element in the calling array.', 1),
	   (43, 'The map() method modifies the original array with the results of calling a provided function on every element.', 0),
	   (44, 'The reduce() method applies a function against an accumulator and each element in the array (from left to right) to reduce it to a single value.', 1),
	   (44, 'The reduce() method applies a function against each element in the array (from left to right) to reduce it to a single value.', 0),
	   (45, 'let is used to declare variables with block scope and var is used to declare variables with function scope.', 1),
	   (45, 'let is used to declare variables with function scope and var is used to declare variables with block scope.', 0),
	   (46, 'The filter() method creates a new array with all elements that pass the test implemented by the provided function.', 1),
	   (46, 'The filter() method modifies the original array with all elements that pass the test implemented by the provided function.', 0),
       (47, 'The forEach() method executes a provided function once for each array element.', 1),
       (47, 'The forEach() method modifies the original array with the results of calling a provided function on each element.', 0),
       (48, 'The some() method tests whether at least one element in the array passes the test implemented by the provided function.', 1),
       (48, 'The some() method modifies the original array with the elements that pass the test implemented by the provided function.', 0),
       (49, 'The every() method tests whether all elements in the array pass the test implemented by the provided function.', 1),
       (49, 'The every() method modifies the original array with the elements that pass the test implemented by the provided function.', 0),
       (50, 'The find() method returns the value of the first element in the array that pass the test implemented by the provided function.', 1),
       (50, 'The find() method modifies the original array with the elements that pass the test implemented by the provided function.',1),
	   (51, 'The findIndex() method returns the index of the first element in the array that pass the test implemented by the provided function.', 1),
       (51, 'The findIndex() method modifies the original array with the elements that pass the test implemented by the provided function.', 0),
       (52, 'The sort() method sorts the elements of an array in place and returns the sorted array.', 1),
       (52, 'The sort() method sorts the elements of an array and returns a new sorted array without modifying the original one.', 0),
       (53, 'The concat() method is used to merge two or more arrays.', 1),
       (53, 'The concat() method is used to remove elements from an array.', 0),
       (54, 'The slice() method returns a shallow copy of a portion of an array into a new array object.', 1),
       (54, 'The slice() method removes elements from an array.', 0),
       (55, 'The splice() method changes the content of an array by removing existing elements and/or adding new elements.', 1),
       (55, 'The splice() method returns a shallow copy of a portion of an array.', 0),
       (56, 'The push() method adds one or more elements to the end of an array and returns the new length of the array.', 1),
	   (56, 'The push() method removes the last element of an array and returns it.', 0),
       (57, 'The pop() method removes the last element of an array and returns it.', 1),
       (57, 'The pop() method adds one or more elements to the end of an array and returns the new length of the array.', 0),
       (58, 'The shift() method removes the first element of an array and returns it.', 1),
       (58, 'The shift() method adds one or more elements to the front of an array and returns the new length of the array.', 0),
       (59, 'The unshift() method adds one or more elements to the front of an array and returns the new length of the array.', 1),
       (59, 'The unshift() method removes the first element of an array and returns it.', 0)
GO

drop table if exists SessionLog
CREATE TABLE SessionLog
(
    SessionId VARCHAR(50) Primary Key,
    CategoryID INT,
	UserID INT,
    StartTime Datetime2 ,
    EndTime Datetime2 ,
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
Select* from feedback
insert into feedback (feedbacktext, UserID) values('111',1)





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
Select *  from SessionLog order by EndTime
Select * from QuestionLog
Go
--Truncate Table QuestionLog


delete from QuestionLog
DBCC CHECKIDENT ('QuestionLog', RESEED, 0)
delete from SessionLog
Go