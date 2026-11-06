Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class ucReports
    Inherits UserControl

    Private grid As DataGridView

    Public Sub New()
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White
        BuildScreen()
    End Sub

    Private Sub BuildScreen()
        Dim title As New Label()
        title.Text = "REPORTS"
        title.Font = New Font("Arial", 20, FontStyle.Bold)
        title.AutoSize = True
        title.Location = New Point(30, 20)
        Controls.Add(title)

        Dim btnTeam As Button = MakeButton("TEAM POINTS", 30, 65)
        AddHandler btnTeam.Click, AddressOf TeamPointsClick
        Controls.Add(btnTeam)

        Dim btnDriver As Button = MakeButton("DRIVER POINTS", 140, 65)
        AddHandler btnDriver.Click, AddressOf DriverPointsClick
        Controls.Add(btnDriver)

        Dim btnResults As Button = MakeButton("ALL RESULTS", 250, 65)
        AddHandler btnResults.Click, AddressOf AllResultsClick
        Controls.Add(btnResults)

        Dim btnSponsors As Button = MakeButton("SPONSORS", 360, 65)
        AddHandler btnSponsors.Click, AddressOf SponsorClick
        Controls.Add(btnSponsors)

        grid = New DataGridView()
        grid.Location = New Point(30, 115)
        grid.Size = New Size(900, 390)
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Controls.Add(grid)
    End Sub

    Private Function MakeButton(ByVal text As String, ByVal x As Integer, ByVal y As Integer) As Button
        Dim b As New Button()
        b.Text = text
        b.Location = New Point(x, y)
        b.Width = 100
        b.Height = 35
        Return b
    End Function

    Private Sub RunReport(ByVal sql As String)
        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Using da As New SqlDataAdapter(sql, con)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    grid.DataSource = dt
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub TeamPointsClick(ByVal sender As Object, ByVal e As EventArgs)
        RunReport("SELECT t.TeamName, SUM(rr.Points) AS TotalPoints, COUNT(rr.ResultID) AS ResultsCount FROM RaceResults rr INNER JOIN Teams t ON rr.TeamID=t.TeamID GROUP BY t.TeamName ORDER BY TotalPoints DESC")
    End Sub

    Private Sub DriverPointsClick(ByVal sender As Object, ByVal e As EventArgs)
        RunReport("SELECT d.DriverName, SUM(rr.Points) AS TotalPoints, COUNT(rr.ResultID) AS ResultsCount FROM RaceResults rr INNER JOIN Drivers d ON rr.DriverID=d.DriverID GROUP BY d.DriverName ORDER BY TotalPoints DESC")
    End Sub

    Private Sub AllResultsClick(ByVal sender As Object, ByVal e As EventArgs)
        RunReport("SELECT r.RaceName,r.RaceDate,d.DriverName,t.TeamName,v.VehicleName,rr.Position,rr.Points FROM RaceResults rr INNER JOIN Races r ON rr.RaceID=r.RaceID INNER JOIN Drivers d ON rr.DriverID=d.DriverID INNER JOIN Teams t ON rr.TeamID=t.TeamID INNER JOIN Vehicles v ON rr.VehicleID=v.VehicleID ORDER BY r.RaceDate,rr.Position")
    End Sub

    Private Sub SponsorClick(ByVal sender As Object, ByVal e As EventArgs)
        RunReport("SELECT s.SponsorName,s.ContactPerson,s.Phone,s.Amount,t.TeamName FROM Sponsors s INNER JOIN Teams t ON s.TeamID=t.TeamID ORDER BY s.SponsorName")
    End Sub
End Class
