IF DB_ID(N'RacingTeamDB') IS NULL
BEGIN
    CREATE DATABASE RacingTeamDB;
END
GO

USE RacingTeamDB;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        UserID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Username VARCHAR(50) NOT NULL UNIQUE,
        Password VARCHAR(100) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = 'admin')
BEGIN
    INSERT INTO dbo.Users (Username, Password) VALUES ('admin', 'admin123');
END
GO

IF OBJECT_ID(N'dbo.Teams', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Teams
    (
        TeamID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        TeamName VARCHAR(100) NOT NULL,
        TeamManager VARCHAR(100) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.Drivers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Drivers
    (
        DriverID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        DriverName VARCHAR(100) NOT NULL,
        Phone VARCHAR(20) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.Vehicles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Vehicles
    (
        VehicleID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        VehicleName VARCHAR(100) NOT NULL,
        VehicleNumber VARCHAR(50) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.Races', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Races
    (
        RaceID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        RaceName VARCHAR(100) NOT NULL,
        RaceDate DATE NOT NULL,
        Location VARCHAR(100) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.RaceResults', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RaceResults
    (
        ResultID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        RaceID INT NOT NULL,
        DriverID INT NOT NULL,
        TeamID INT NOT NULL,
        VehicleID INT NOT NULL,
        Position INT NOT NULL,
        Points INT NOT NULL,
        CONSTRAINT FK_RaceResults_Races FOREIGN KEY (RaceID) REFERENCES dbo.Races(RaceID),
        CONSTRAINT FK_RaceResults_Drivers FOREIGN KEY (DriverID) REFERENCES dbo.Drivers(DriverID),
        CONSTRAINT FK_RaceResults_Teams FOREIGN KEY (TeamID) REFERENCES dbo.Teams(TeamID),
        CONSTRAINT FK_RaceResults_Vehicles FOREIGN KEY (VehicleID) REFERENCES dbo.Vehicles(VehicleID)
    );
END
GO

IF OBJECT_ID(N'dbo.Sponsors', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Sponsors
    (
        SponsorID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        SponsorName VARCHAR(100) NOT NULL,
        ContactPerson VARCHAR(100) NULL,
        Phone VARCHAR(20) NULL,
        Amount DECIMAL(18,2) NOT NULL DEFAULT 0,
        TeamID INT NOT NULL,
        CONSTRAINT FK_Sponsors_Teams FOREIGN KEY (TeamID) REFERENCES dbo.Teams(TeamID)
    );
END
GO

PRINT 'RacingTeamDB setup completed successfully.';
PRINT 'Default login: admin / admin123';
GO
