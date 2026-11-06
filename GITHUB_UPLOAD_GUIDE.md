# GitHub Upload Guide

## Recommended repository name

`Racing-Team-Management-System`

## Recommended description

`VB.NET Windows Forms + SQL Server desktop application for managing racing teams, drivers, vehicles, races, results, sponsors and reports.`

## Upload using GitHub website

1. Sign in to GitHub.
2. Click **New repository**.
3. Enter the repository name: `Racing-Team-Management-System`.
4. Add the description above.
5. Choose **Public** if you want the project to be visible to teachers/recruiters.
6. Do not create another README, `.gitignore`, or license because they are already included in this project ZIP.
7. Create the repository.
8. Extract this ZIP on your computer.
9. Open the extracted project folder.
10. Upload the project files and folders to GitHub. The top level should show `README.md`, `RacingTeamManagementSystem.sln`, `Database`, `Docs`, and `RacingTeamManagementSystem`.
11. Commit the upload with a message such as `Initial project upload`.

## Upload using Git

Open Command Prompt / Git Bash in the extracted project folder:

```bash
git init
git add .
git commit -m "Initial Racing Team Management System project"
git branch -M main
git remote add origin https://github.com/YOUR-USERNAME/Racing-Team-Management-System.git
git push -u origin main
```

Replace `YOUR-USERNAME` with your GitHub username.

## Before making the repository public

- Replace any personal placeholders in your report if needed.
- Do not upload passwords, personal credentials, or real private database files.
- The included demo login is only `admin / admin123` for the college project.
- Keep the SQL setup script in the repository so another computer can recreate the database.

## What a reviewer should see

The README should be the first thing they see. It explains the project, modules, technology, database setup, and how to run it. The `.gitignore` keeps build files out of the repository.
