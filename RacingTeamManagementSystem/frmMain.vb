Imports System.Drawing
Imports System.Windows.Forms

Public Class frmMain
    Inherits Form

    Private pnlSidebar As Panel
    Private pnlHeader As Panel
    Private pnlContent As Panel
    Private lblHeader As Label

    Public Sub New()
        Me.Text = "Racing Team Management System"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.WindowState = FormWindowState.Maximized
        Me.MinimumSize = New Size(900, 600)

        BuildLayout()
        ShowDashboard()
    End Sub

    Private Sub BuildLayout()
        pnlSidebar = New Panel()
        pnlSidebar.Dock = DockStyle.Left
        pnlSidebar.Width = 210
        pnlSidebar.BackColor = Color.DimGray
        Me.Controls.Add(pnlSidebar)

        pnlHeader = New Panel()
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Height = 65
        pnlHeader.BackColor = Color.Navy
        Me.Controls.Add(pnlHeader)

        lblHeader = New Label()
        lblHeader.Text = "RACING TEAM MANAGEMENT SYSTEM"
        lblHeader.ForeColor = Color.White
        lblHeader.Font = New Font("Arial", 15, FontStyle.Bold)
        lblHeader.AutoSize = True
        lblHeader.Location = New Point(25, 20)
        pnlHeader.Controls.Add(lblHeader)

        pnlContent = New Panel()
        pnlContent.Dock = DockStyle.Fill
        pnlContent.BackColor = Color.WhiteSmoke
        Me.Controls.Add(pnlContent)

        AddMenuButton("Dashboard", AddressOf DashboardClick, 0)
        AddMenuButton("Teams", AddressOf TeamsClick, 1)
        AddMenuButton("Drivers", AddressOf DriversClick, 2)
        AddMenuButton("Vehicles", AddressOf VehiclesClick, 3)
        AddMenuButton("Races", AddressOf RacesClick, 4)
        AddMenuButton("Results", AddressOf ResultsClick, 5)
        AddMenuButton("Sponsors", AddressOf SponsorsClick, 6)
        AddMenuButton("Reports", AddressOf ReportsClick, 7)
        AddMenuButton("Logout", AddressOf LogoutClick, 8)
    End Sub

    Private Sub AddMenuButton(ByVal text As String, ByVal handler As EventHandler, ByVal index As Integer)
        Dim button As New Button()
        button.Text = text
        button.Tag = index
        button.Dock = DockStyle.Top
        button.Height = 45
        button.FlatStyle = FlatStyle.Flat
        button.FlatAppearance.BorderSize = 0
        button.ForeColor = Color.White
        button.BackColor = Color.DimGray
        button.Font = New Font("Arial", 10, FontStyle.Bold)
        AddHandler button.Click, handler
        pnlSidebar.Controls.Add(button)
        button.BringToFront()
    End Sub

    Private Sub SetHeader(ByVal title As String)
        lblHeader.Text = title
    End Sub

    Private Sub ShowControl(ByVal control As UserControl, ByVal title As String)
        pnlContent.Controls.Clear()
        control.Dock = DockStyle.Fill
        pnlContent.Controls.Add(control)
        SetHeader(title)
    End Sub

    Private Sub ShowDashboard()
        ShowControl(New ucDashboard(), "DASHBOARD")
    End Sub

    Private Sub DashboardClick(ByVal sender As Object, ByVal e As EventArgs)
        ShowDashboard()
    End Sub

    Private Sub TeamsClick(ByVal sender As Object, ByVal e As EventArgs)
        ShowControl(New ucTeams(), "TEAM MANAGEMENT")
    End Sub

    Private Sub DriversClick(ByVal sender As Object, ByVal e As EventArgs)
        ShowControl(New ucDrivers(), "DRIVER MANAGEMENT")
    End Sub

    Private Sub VehiclesClick(ByVal sender As Object, ByVal e As EventArgs)
        ShowControl(New ucVehicles(), "VEHICLE MANAGEMENT")
    End Sub

    Private Sub RacesClick(ByVal sender As Object, ByVal e As EventArgs)
        ShowControl(New ucRaces(), "RACE / EVENT MANAGEMENT")
    End Sub

    Private Sub ResultsClick(ByVal sender As Object, ByVal e As EventArgs)
        ShowControl(New ucResults(), "RACE RESULTS & POINTS")
    End Sub

    Private Sub SponsorsClick(ByVal sender As Object, ByVal e As EventArgs)
        ShowControl(New ucSponsors(), "SPONSOR MANAGEMENT")
    End Sub

    Private Sub ReportsClick(ByVal sender As Object, ByVal e As EventArgs)
        ShowControl(New ucReports(), "REPORTS")
    End Sub

    Private Sub LogoutClick(ByVal sender As Object, ByVal e As EventArgs)
        If MessageBox.Show("Do you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim login As New frmLogin()
            login.Show()
            Me.Close()
        End If
    End Sub
End Class
