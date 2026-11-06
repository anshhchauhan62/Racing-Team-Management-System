# Racing Team Management System

A simple single-user desktop application for managing racing teams, drivers, vehicles, races, race results, sponsors, and reports.

## Technology

- Visual Basic .NET
- Windows Forms
- Visual Studio 2010
- .NET Framework 4.0
- SQL Server / SQL Server Express

## Main Modules

1. Login & Authentication
2. Dashboard
3. Team Management
4. Driver Management
5. Vehicle Management
6. Race / Event Management
7. Race Results & Points
8. Sponsor Management
9. Reports

## Database

Database name: `RacingTeamDB`

Tables:

- Users
- Teams
- Drivers
- Vehicles
- Races
- RaceResults
- Sponsors

## How to Run

### 1. Requirements

Install:

- Visual Studio 2010 with Visual Basic support
- .NET Framework 4.0
- SQL Server Express (the default project connection uses `SQLEXPRESS`)
- SQL Server Management Studio, if available

### 2. Create the database

Open `Database/Database_Setup.sql` in SQL Server Management Studio and execute the complete script.

The script creates the `RacingTeamDB` database, all required tables, relationships, and a default login.

Default login:

- Username: `admin`
- Password: `admin123`

### 3. Open the project

Open:

`RacingTeamManagementSystem.sln`

Then choose **Build > Rebuild Solution** and press **F5**.

### 4. If SQL Server uses a different instance

Open:

`RacingTeamManagementSystem/DBConnection.vb`

Change the server name in the connection string. The default is:

`Data Source=.\SQLEXPRESS;Initial Catalog=RacingTeamDB;Integrated Security=True`

## Project Structure

```text
RacingTeamManagementSystem/
├── RacingTeamManagementSystem.sln
├── README.md
├── LICENSE
├── .gitignore
├── Database/
│   └── Database_Setup.sql
├── Docs/
│   ├── Project_Report.pdf
│   └── diagrams/
└── RacingTeamManagementSystem/
    ├── RacingTeamManagementSystem.vbproj
    ├── App.config
    ├── Program.vb
    ├── DBConnection.vb
    ├── frmLogin.vb
    ├── frmMain.vb
    ├── ucDashboard.vb
    ├── ucTeams.vb
    ├── ucDrivers.vb
    ├── ucVehicles.vb
    ├── ucRaces.vb
    ├── ucResults.vb
    ├── ucSponsors.vb
    └── ucReports.vb
```

## Design Notes

- The application is intentionally simple and suitable for a college-level project.
- It is a single-user desktop application and does not use MDI.
- Navigation is handled with Windows Forms UserControls inside the main content panel.
- Database access uses parameterized SQL commands.
- The driver/team relationship is not stored directly. Team and driver participation is recorded through `RaceResults`, allowing a driver to race for different teams when needed.

## GitHub

This repository is structured so the source code, database setup script, documentation, and diagrams can be uploaded together.

Do not commit compiled `bin` or `obj` folders or local Visual Studio user files.
