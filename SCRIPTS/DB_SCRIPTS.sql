DROP DATABASE UCE_TEST
GO
CREATE DATABASE UCE_TEST
GO
USE UCE_TEST
GO
CREATE TABLE Employees(
    Id INT IDENTITY(1,1),
    Name VARCHAR(30) NOT NULL,
    LastName VARCHAR(30) NOT NULL,
    Position VARCHAR(30) NOT NULL,
    BirthDay DATE NOT NULL,
    DateOfHire DATE NOT NULL,
    Phone VARCHAR(10) NOT NULL,
    Email VARCHAR(25) NOT NULL,
    MaritalStatus VARCHAR(25) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    Photo IMAGE,
    CONSTRAINT PK_EMPLOYEES_ID PRIMARY KEY (ID),
    CONSTRAINT CK_EMPLOYEES_MARITALSTATUS CHECK (MaritalStatus IN('Soltero', 'Casado', 'Divorciado'))
)
GO
CREATE TABLE Addresses(
    Id INT IDENTITY(1,1),
    Street VARCHAR(100) NOT NULL,
    Zone VARCHAR(100) NOT NULL,
    PostalCode VARCHAR(6),
    Province VARCHAR(50) NOT NULL,
    Country VARCHAR(50) NOT NULL,
    EmployeeId INT NOT NULL,
    CONSTRAINT PK_ADDRESSES_ID PRIMARY KEY (Id),
    CONSTRAINT FK_ADDRESSES_EMPLOYEEID FOREIGN KEY (EmployeeId) REFERENCES Employees(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_ADDRESSES_EMPLOYEEID UNIQUE (EmployeeId)
)
GO
CREATE TABLE Logs(
    Id INT IDENTITY(1,1),
    Description VARCHAR(MAX) NOT NULL,
    EmployeeId INT NOT NULL,
    CreatedBy VARCHAR(25) NOT NULL DEFAULT USER_NAME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_LOGS_ID PRIMARY KEY (ID),
    CONSTRAINT FK_LOGS_EMPLOYEEID FOREIGN KEY (EmployeeId) REFERENCES Employees(Id) ON DELETE CASCADE
)
GO
SELECT * FROM Employees
SELECT * FROM Addresses
SELECT * FROM Logs