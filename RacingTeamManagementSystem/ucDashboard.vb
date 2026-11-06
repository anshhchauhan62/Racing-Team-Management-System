Imports System.Drawing
Imports System.Windows.Forms

Public Class ucDashboard
    Inherits UserControl

    Public Sub New()
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White

        Dim title As New Label()
        title.Text = "WELCOME TO RACING TEAM MANAGEMENT SYSTEM"
        title.Font = New Font("Arial", 20, FontStyle.Bold)
        title.AutoSize = True
        title.Location = New Point(35, 35)
        Controls.Add(title)

        Dim info As New Label()
        info.Text = "Use the menu to manage teams, drivers, vehicles, races, results, sponsors and reports."
        info.Font = New Font("Arial", 11, FontStyle.Regular)
        info.AutoSize = True
        info.Location = New Point(38, 85)
        Controls.Add(info)

        Dim note As New Label()
        note.Text = "Single-user desktop application | VB.NET Windows Forms | SQL Server"
        note.Font = New Font("Arial", 10, FontStyle.Italic)
        note.AutoSize = True
        note.Location = New Point(38, 120)
        Controls.Add(note)
    End Sub
End Class
