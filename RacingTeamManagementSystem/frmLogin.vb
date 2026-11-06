Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class frmLogin
    Inherits Form

    Private txtUsername As TextBox
    Private txtPassword As TextBox

    Public Sub New()
        Me.Text = "Racing Team Management System - Login"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ClientSize = New Size(480, 300)

        Dim title As New Label()
        title.Text = "RACING TEAM MANAGEMENT SYSTEM"
        title.Font = New Font("Arial", 16, FontStyle.Bold)
        title.AutoSize = True
        title.Location = New Point(58, 35)
        Me.Controls.Add(title)

        Dim subtitle As New Label()
        subtitle.Text = "Login Authentication"
        subtitle.Font = New Font("Arial", 10, FontStyle.Regular)
        subtitle.AutoSize = True
        subtitle.Location = New Point(176, 70)
        Me.Controls.Add(subtitle)

        Dim lblUsername As New Label()
        lblUsername.Text = "Username"
        lblUsername.AutoSize = True
        lblUsername.Location = New Point(75, 115)
        Me.Controls.Add(lblUsername)

        txtUsername = New TextBox()
        txtUsername.Location = New Point(170, 112)
        txtUsername.Width = 220
        Me.Controls.Add(txtUsername)

        Dim lblPassword As New Label()
        lblPassword.Text = "Password"
        lblPassword.AutoSize = True
        lblPassword.Location = New Point(75, 155)
        Me.Controls.Add(lblPassword)

        txtPassword = New TextBox()
        txtPassword.Location = New Point(170, 152)
        txtPassword.Width = 220
        txtPassword.PasswordChar = "*"c
        Me.Controls.Add(txtPassword)

        Dim btnLogin As New Button()
        btnLogin.Text = "LOGIN"
        btnLogin.Location = New Point(170, 205)
        btnLogin.Width = 100
        btnLogin.Height = 35
        AddHandler btnLogin.Click, AddressOf LoginClick
        Me.Controls.Add(btnLogin)

        Dim btnExit As New Button()
        btnExit.Text = "EXIT"
        btnExit.Location = New Point(290, 205)
        btnExit.Width = 100
        btnExit.Height = 35
        AddHandler btnExit.Click, AddressOf ExitClick
        Me.Controls.Add(btnExit)

        Me.AcceptButton = btnLogin
        Me.CancelButton = btnExit
    End Sub

    Private Sub LoginClick(ByVal sender As Object, ByVal e As EventArgs)
        If txtUsername.Text.Trim() = "" OrElse txtPassword.Text = "" Then
            MessageBox.Show("Please enter username and password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "SELECT COUNT(*) FROM Users WHERE Username=@Username AND Password=@Password"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim())
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text)
                    con.Open()
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                    If count = 1 Then
                        Dim mainForm As New frmMain()
                        mainForm.Show()
                        Me.Hide()
                    Else
                        MessageBox.Show("Invalid Username or Password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        txtPassword.Clear()
                        txtPassword.Focus()
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Unable to connect to the database." & Environment.NewLine & _
                            "Make sure RacingTeamDB has been created and SQL Server Express is running." & _
                            Environment.NewLine & Environment.NewLine & ex.Message, _
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ExitClick(ByVal sender As Object, ByVal e As EventArgs)
        Me.Close()
    End Sub
End Class
