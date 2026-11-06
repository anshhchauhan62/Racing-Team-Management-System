Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class ucCrudBase
    Inherits UserControl

    Private ReadOnly tableName As String
    Private ReadOnly idField As String
    Private ReadOnly firstField As String
    Private ReadOnly secondField As String
    Private ReadOnly screenTitle As String

    Protected txtFirst As TextBox
    Protected txtSecond As TextBox
    Protected txtSearch As TextBox
    Protected grid As DataGridView
    Private selectedId As Integer

    Public Sub New(ByVal title As String, ByVal table As String, ByVal idColumn As String, ByVal firstColumn As String, ByVal secondColumn As String)
        screenTitle = title
        tableName = table
        idField = idColumn
        firstField = firstColumn
        secondField = secondColumn

        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White
        BuildScreen()
        LoadData()
    End Sub

    Private Sub BuildScreen()
        Dim title As New Label()
        title.Text = screenTitle
        title.Font = New Font("Arial", 20, FontStyle.Bold)
        title.AutoSize = True
        title.Location = New Point(30, 25)
        Controls.Add(title)

        Dim lblFirst As New Label()
        lblFirst.Text = firstField
        lblFirst.AutoSize = True
        lblFirst.Location = New Point(30, 80)
        Controls.Add(lblFirst)

        txtFirst = New TextBox()
        txtFirst.Location = New Point(125, 77)
        txtFirst.Width = 190
        Controls.Add(txtFirst)

        Dim lblSecond As New Label()
        lblSecond.Text = secondField
        lblSecond.AutoSize = True
        lblSecond.Location = New Point(340, 80)
        Controls.Add(lblSecond)

        txtSecond = New TextBox()
        txtSecond.Location = New Point(435, 77)
        txtSecond.Width = 190
        Controls.Add(txtSecond)

        Dim btnAdd As Button = MakeButton("ADD", 30, 120)
        AddHandler btnAdd.Click, AddressOf AddClick
        Controls.Add(btnAdd)

        Dim btnUpdate As Button = MakeButton("UPDATE", 125, 120)
        AddHandler btnUpdate.Click, AddressOf UpdateClick
        Controls.Add(btnUpdate)

        Dim btnDelete As Button = MakeButton("DELETE", 235, 120)
        AddHandler btnDelete.Click, AddressOf DeleteClick
        Controls.Add(btnDelete)

        Dim lblSearch As New Label()
        lblSearch.Text = "Search"
        lblSearch.AutoSize = True
        lblSearch.Location = New Point(355, 128)
        Controls.Add(lblSearch)

        txtSearch = New TextBox()
        txtSearch.Location = New Point(410, 125)
        txtSearch.Width = 160
        Controls.Add(txtSearch)

        Dim btnSearch As Button = MakeButton("SEARCH", 580, 120)
        AddHandler btnSearch.Click, AddressOf SearchClick
        Controls.Add(btnSearch)

        grid = New DataGridView()
        grid.Location = New Point(30, 175)
        grid.Size = New Size(760, 360)
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        AddHandler grid.CellClick, AddressOf GridCellClick
        Controls.Add(grid)
    End Sub

    Private Function MakeButton(ByVal text As String, ByVal x As Integer, ByVal y As Integer) As Button
        Dim b As New Button()
        b.Text = text
        b.Location = New Point(x, y)
        b.Width = 90
        b.Height = 32
        Return b
    End Function

    Private Sub LoadData(Optional ByVal whereClause As String = "")
        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "SELECT " & idField & "," & firstField & "," & secondField & " FROM " & tableName
                If whereClause <> "" Then sql &= " " & whereClause
                sql &= " ORDER BY " & idField

                Using da As New SqlDataAdapter(sql, con)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    grid.DataSource = dt
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub AddClick(ByVal sender As Object, ByVal e As EventArgs)
        If txtFirst.Text.Trim() = "" Then
            MessageBox.Show("Please enter " & firstField & ".", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "INSERT INTO " & tableName & "(" & firstField & "," & secondField & ") VALUES(@FirstValue,@SecondValue)"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@FirstValue", txtFirst.Text.Trim())
                    cmd.Parameters.AddWithValue("@SecondValue", txtSecond.Text.Trim())
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Record Added Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateClick(ByVal sender As Object, ByVal e As EventArgs)
        If selectedId = 0 Then
            MessageBox.Show("Please select a record first.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "UPDATE " & tableName & " SET " & firstField & "=@FirstValue," & secondField & "=@SecondValue WHERE " & idField & "=@ID"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@FirstValue", txtFirst.Text.Trim())
                    cmd.Parameters.AddWithValue("@SecondValue", txtSecond.Text.Trim())
                    cmd.Parameters.AddWithValue("@ID", selectedId)
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Record Updated Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DeleteClick(ByVal sender As Object, ByVal e As EventArgs)
        If selectedId = 0 Then
            MessageBox.Show("Please select a record first.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("Delete selected record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "DELETE FROM " & tableName & " WHERE " & idField & "=@ID"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@ID", selectedId)
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Record Deleted Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("This record may be used by another table." & Environment.NewLine & ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SearchClick(ByVal sender As Object, ByVal e As EventArgs)
        If txtSearch.Text.Trim() = "" Then
            LoadData()
            Return
        End If

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "SELECT " & idField & "," & firstField & "," & secondField & " FROM " & tableName & _
                                    " WHERE " & firstField & " LIKE @Search OR " & secondField & " LIKE @Search ORDER BY " & idField
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@Search", "%" & txtSearch.Text.Trim() & "%")
                    Using da As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        grid.DataSource = dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GridCellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        Dim row As DataGridViewRow = grid.Rows(e.RowIndex)
        selectedId = Convert.ToInt32(row.Cells(idField).Value)
        txtFirst.Text = Convert.ToString(row.Cells(firstField).Value)
        txtSecond.Text = Convert.ToString(row.Cells(secondField).Value)
    End Sub

    Private Sub ClearFields()
        selectedId = 0
        txtFirst.Clear()
        txtSecond.Clear()
        txtSearch.Clear()
    End Sub
End Class
