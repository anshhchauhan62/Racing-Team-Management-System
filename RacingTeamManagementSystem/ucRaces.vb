Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class ucRaces
    Inherits UserControl

    Private txtRaceName As TextBox
    Private dtpRaceDate As DateTimePicker
    Private txtLocation As TextBox
    Private txtSearch As TextBox
    Private grid As DataGridView
    Private selectedId As Integer

    Public Sub New()
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White
        BuildScreen()
        LoadData()
    End Sub

    Private Sub BuildScreen()
        Dim title As New Label()
        title.Text = "RACE / EVENT MANAGEMENT"
        title.Font = New Font("Arial", 20, FontStyle.Bold)
        title.AutoSize = True
        title.Location = New Point(30, 25)
        Controls.Add(title)

        Dim lblName As New Label()
        lblName.Text = "Race Name"
        lblName.AutoSize = True
        lblName.Location = New Point(30, 80)
        Controls.Add(lblName)

        txtRaceName = New TextBox()
        txtRaceName.Location = New Point(110, 77)
        txtRaceName.Width = 180
        Controls.Add(txtRaceName)

        Dim lblDate As New Label()
        lblDate.Text = "Date"
        lblDate.AutoSize = True
        lblDate.Location = New Point(315, 80)
        Controls.Add(lblDate)

        dtpRaceDate = New DateTimePicker()
        dtpRaceDate.Location = New Point(360, 77)
        dtpRaceDate.Width = 140
        dtpRaceDate.Format = DateTimePickerFormat.Short
        Controls.Add(dtpRaceDate)

        Dim lblLocation As New Label()
        lblLocation.Text = "Location"
        lblLocation.AutoSize = True
        lblLocation.Location = New Point(520, 80)
        Controls.Add(lblLocation)

        txtLocation = New TextBox()
        txtLocation.Location = New Point(580, 77)
        txtLocation.Width = 180
        Controls.Add(txtLocation)

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
        lblSearch.Location = New Point(360, 128)
        Controls.Add(lblSearch)

        txtSearch = New TextBox()
        txtSearch.Location = New Point(415, 125)
        txtSearch.Width = 180
        Controls.Add(txtSearch)

        Dim btnSearch As Button = MakeButton("SEARCH", 605, 120)
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

    Private Sub LoadData(Optional ByVal searchValue As String = "")
        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "SELECT RaceID,RaceName,RaceDate,Location FROM Races"
                If searchValue <> "" Then
                    sql &= " WHERE RaceName LIKE @Search OR Location LIKE @Search"
                End If
                sql &= " ORDER BY RaceDate, RaceID"

                Using cmd As New SqlCommand(sql, con)
                    If searchValue <> "" Then cmd.Parameters.AddWithValue("@Search", "%" & searchValue & "%")
                    Using da As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)
                        grid.DataSource = dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub AddClick(ByVal sender As Object, ByVal e As EventArgs)
        If txtRaceName.Text.Trim() = "" Then
            MessageBox.Show("Please enter Race Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Using cmd As New SqlCommand("INSERT INTO Races(RaceName,RaceDate,Location) VALUES(@Name,@Date,@Location)", con)
                    cmd.Parameters.AddWithValue("@Name", txtRaceName.Text.Trim())
                    cmd.Parameters.AddWithValue("@Date", dtpRaceDate.Value.Date)
                    cmd.Parameters.AddWithValue("@Location", txtLocation.Text.Trim())
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Race Added Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateClick(ByVal sender As Object, ByVal e As EventArgs)
        If selectedId = 0 Then
            MessageBox.Show("Please select a race first.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Using cmd As New SqlCommand("UPDATE Races SET RaceName=@Name,RaceDate=@Date,Location=@Location WHERE RaceID=@ID", con)
                    cmd.Parameters.AddWithValue("@Name", txtRaceName.Text.Trim())
                    cmd.Parameters.AddWithValue("@Date", dtpRaceDate.Value.Date)
                    cmd.Parameters.AddWithValue("@Location", txtLocation.Text.Trim())
                    cmd.Parameters.AddWithValue("@ID", selectedId)
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Race Updated Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DeleteClick(ByVal sender As Object, ByVal e As EventArgs)
        If selectedId = 0 Then
            MessageBox.Show("Please select a race first.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("Delete selected race?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Using cmd As New SqlCommand("DELETE FROM Races WHERE RaceID=@ID", con)
                    cmd.Parameters.AddWithValue("@ID", selectedId)
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Race Deleted Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("This race may already have results." & Environment.NewLine & ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SearchClick(ByVal sender As Object, ByVal e As EventArgs)
        LoadData(txtSearch.Text.Trim())
    End Sub

    Private Sub GridCellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        Dim row As DataGridViewRow = grid.Rows(e.RowIndex)
        selectedId = Convert.ToInt32(row.Cells("RaceID").Value)
        txtRaceName.Text = Convert.ToString(row.Cells("RaceName").Value)
        dtpRaceDate.Value = Convert.ToDateTime(row.Cells("RaceDate").Value)
        txtLocation.Text = Convert.ToString(row.Cells("Location").Value)
    End Sub

    Private Sub ClearFields()
        selectedId = 0
        txtRaceName.Clear()
        txtLocation.Clear()
        dtpRaceDate.Value = DateTime.Today
        txtSearch.Clear()
    End Sub
End Class
