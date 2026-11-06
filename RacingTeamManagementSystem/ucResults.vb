Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class ucResults
    Inherits UserControl

    Private cmbRace As ComboBox
    Private cmbDriver As ComboBox
    Private cmbTeam As ComboBox
    Private cmbVehicle As ComboBox
    Private txtPosition As TextBox
    Private txtPoints As TextBox
    Private txtSearch As TextBox
    Private grid As DataGridView
    Private selectedId As Integer

    Public Sub New()
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White
        BuildScreen()
        LoadLookups()
        LoadData()
    End Sub

    Private Sub BuildScreen()
        Dim title As New Label()
        title.Text = "RACE RESULTS & POINTS"
        title.Font = New Font("Arial", 20, FontStyle.Bold)
        title.AutoSize = True
        title.Location = New Point(30, 20)
        Controls.Add(title)

        AddLabel("Race", 30, 65)
        cmbRace = MakeCombo(75, 62, 150)
        Controls.Add(cmbRace)

        AddLabel("Driver", 240, 65)
        cmbDriver = MakeCombo(295, 62, 150)
        Controls.Add(cmbDriver)

        AddLabel("Team", 460, 65)
        cmbTeam = MakeCombo(505, 62, 150)
        Controls.Add(cmbTeam)

        AddLabel("Vehicle", 30, 105)
        cmbVehicle = MakeCombo(95, 102, 150)
        Controls.Add(cmbVehicle)

        AddLabel("Position", 270, 105)
        txtPosition = New TextBox()
        txtPosition.Location = New Point(335, 102)
        txtPosition.Width = 80
        Controls.Add(txtPosition)

        AddLabel("Points", 440, 105)
        txtPoints = New TextBox()
        txtPoints.Location = New Point(490, 102)
        txtPoints.Width = 80
        Controls.Add(txtPoints)

        Dim btnAdd As Button = MakeButton("ADD", 600, 100)
        AddHandler btnAdd.Click, AddressOf AddClick
        Controls.Add(btnAdd)

        Dim btnUpdate As Button = MakeButton("UPDATE", 695, 100)
        AddHandler btnUpdate.Click, AddressOf UpdateClick
        Controls.Add(btnUpdate)

        Dim btnDelete As Button = MakeButton("DELETE", 790, 100)
        AddHandler btnDelete.Click, AddressOf DeleteClick
        Controls.Add(btnDelete)

        AddLabel("Search", 30, 150)
        txtSearch = New TextBox()
        txtSearch.Location = New Point(85, 147)
        txtSearch.Width = 200
        Controls.Add(txtSearch)

        Dim btnSearch As Button = MakeButton("SEARCH", 295, 145)
        AddHandler btnSearch.Click, AddressOf SearchClick
        Controls.Add(btnSearch)

        Dim btnRefresh As Button = MakeButton("REFRESH", 390, 145)
        AddHandler btnRefresh.Click, AddressOf RefreshClick
        Controls.Add(btnRefresh)

        grid = New DataGridView()
        grid.Location = New Point(30, 190)
        grid.Size = New Size(880, 330)
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        AddHandler grid.CellClick, AddressOf GridCellClick
        Controls.Add(grid)
    End Sub

    Private Sub AddLabel(ByVal text As String, ByVal x As Integer, ByVal y As Integer)
        Dim label As New Label()
        label.Text = text
        label.AutoSize = True
        label.Location = New Point(x, y)
        Controls.Add(label)
    End Sub

    Private Function MakeCombo(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer) As ComboBox
        Dim c As New ComboBox()
        c.Location = New Point(x, y)
        c.Width = width
        c.DropDownStyle = ComboBoxStyle.DropDownList
        Return c
    End Function

    Private Function MakeButton(ByVal text As String, ByVal x As Integer, ByVal y As Integer) As Button
        Dim b As New Button()
        b.Text = text
        b.Location = New Point(x, y)
        b.Width = 90
        b.Height = 32
        Return b
    End Function

    Private Sub LoadLookups()
        Try
            FillCombo(cmbRace, "SELECT RaceID, RaceName FROM Races ORDER BY RaceDate, RaceName", "RaceID", "RaceName")
            FillCombo(cmbDriver, "SELECT DriverID, DriverName FROM Drivers ORDER BY DriverName", "DriverID", "DriverName")
            FillCombo(cmbTeam, "SELECT TeamID, TeamName FROM Teams ORDER BY TeamName", "TeamID", "TeamName")
            FillCombo(cmbVehicle, "SELECT VehicleID, VehicleName FROM Vehicles ORDER BY VehicleName", "VehicleID", "VehicleName")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Lookup Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub FillCombo(ByVal combo As ComboBox, ByVal sql As String, ByVal valueField As String, ByVal displayField As String)
        Using con As SqlConnection = DBConnection.GetConnection()
            Using da As New SqlDataAdapter(sql, con)
                Dim dt As New DataTable()
                da.Fill(dt)
                combo.DataSource = dt
                combo.ValueMember = valueField
                combo.DisplayMember = displayField
                combo.SelectedIndex = -1
            End Using
        End Using
    End Sub

    Private Sub LoadData(Optional ByVal searchValue As String = "")
        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "SELECT rr.ResultID,r.RaceName,d.DriverName,t.TeamName,v.VehicleName,rr.Position,rr.Points " & _
                                    "FROM RaceResults rr INNER JOIN Races r ON rr.RaceID=r.RaceID " & _
                                    "INNER JOIN Drivers d ON rr.DriverID=d.DriverID " & _
                                    "INNER JOIN Teams t ON rr.TeamID=t.TeamID " & _
                                    "INNER JOIN Vehicles v ON rr.VehicleID=v.VehicleID"
                If searchValue <> "" Then
                    sql &= " WHERE r.RaceName LIKE @Search OR d.DriverName LIKE @Search OR t.TeamName LIKE @Search OR v.VehicleName LIKE @Search"
                End If
                sql &= " ORDER BY r.RaceDate,rr.Position"

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

    Private Function ValidNumbers(ByRef position As Integer, ByRef points As Integer) As Boolean
        If Not Integer.TryParse(txtPosition.Text.Trim(), position) OrElse position <= 0 Then
            MessageBox.Show("Enter a valid finishing position.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        If Not Integer.TryParse(txtPoints.Text.Trim(), points) OrElse points < 0 Then
            MessageBox.Show("Enter valid points.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    Private Function ValidLookups() As Boolean
        If cmbRace.SelectedIndex < 0 OrElse cmbDriver.SelectedIndex < 0 OrElse cmbTeam.SelectedIndex < 0 OrElse cmbVehicle.SelectedIndex < 0 Then
            MessageBox.Show("Select Race, Driver, Team and Vehicle.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    Private Sub AddClick(ByVal sender As Object, ByVal e As EventArgs)
        If Not ValidLookups() Then Return
        Dim position As Integer
        Dim points As Integer
        If Not ValidNumbers(position, points) Then Return

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "INSERT INTO RaceResults(RaceID,DriverID,TeamID,VehicleID,Position,Points) VALUES(@Race,@Driver,@Team,@Vehicle,@Position,@Points)"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@Race", CInt(cmbRace.SelectedValue))
                    cmd.Parameters.AddWithValue("@Driver", CInt(cmbDriver.SelectedValue))
                    cmd.Parameters.AddWithValue("@Team", CInt(cmbTeam.SelectedValue))
                    cmd.Parameters.AddWithValue("@Vehicle", CInt(cmbVehicle.SelectedValue))
                    cmd.Parameters.AddWithValue("@Position", position)
                    cmd.Parameters.AddWithValue("@Points", points)
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Race Result Added Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateClick(ByVal sender As Object, ByVal e As EventArgs)
        If selectedId = 0 Then
            MessageBox.Show("Select a result first.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If Not ValidLookups() Then Return
        Dim position As Integer
        Dim points As Integer
        If Not ValidNumbers(position, points) Then Return

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "UPDATE RaceResults SET RaceID=@Race,DriverID=@Driver,TeamID=@Team,VehicleID=@Vehicle,Position=@Position,Points=@Points WHERE ResultID=@ID"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@Race", CInt(cmbRace.SelectedValue))
                    cmd.Parameters.AddWithValue("@Driver", CInt(cmbDriver.SelectedValue))
                    cmd.Parameters.AddWithValue("@Team", CInt(cmbTeam.SelectedValue))
                    cmd.Parameters.AddWithValue("@Vehicle", CInt(cmbVehicle.SelectedValue))
                    cmd.Parameters.AddWithValue("@Position", position)
                    cmd.Parameters.AddWithValue("@Points", points)
                    cmd.Parameters.AddWithValue("@ID", selectedId)
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Race Result Updated Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DeleteClick(ByVal sender As Object, ByVal e As EventArgs)
        If selectedId = 0 Then
            MessageBox.Show("Select a result first.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If MessageBox.Show("Delete selected result?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Using cmd As New SqlCommand("DELETE FROM RaceResults WHERE ResultID=@ID", con)
                    cmd.Parameters.AddWithValue("@ID", selectedId)
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Race Result Deleted Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SearchClick(ByVal sender As Object, ByVal e As EventArgs)
        LoadData(txtSearch.Text.Trim())
    End Sub

    Private Sub RefreshClick(ByVal sender As Object, ByVal e As EventArgs)
        LoadLookups()
        LoadData()
    End Sub

    Private Sub GridCellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        Dim row As DataGridViewRow = grid.Rows(e.RowIndex)
        selectedId = Convert.ToInt32(row.Cells("ResultID").Value)
        SelectComboByText(cmbRace, Convert.ToString(row.Cells("RaceName").Value))
        SelectComboByText(cmbDriver, Convert.ToString(row.Cells("DriverName").Value))
        SelectComboByText(cmbTeam, Convert.ToString(row.Cells("TeamName").Value))
        SelectComboByText(cmbVehicle, Convert.ToString(row.Cells("VehicleName").Value))
        txtPosition.Text = Convert.ToString(row.Cells("Position").Value)
        txtPoints.Text = Convert.ToString(row.Cells("Points").Value)
    End Sub

    Private Sub SelectComboByText(ByVal combo As ComboBox, ByVal text As String)
        For i As Integer = 0 To combo.Items.Count - 1
            Dim drv As DataRowView = TryCast(combo.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso Convert.ToString(drv(combo.DisplayMember)) = text Then
                combo.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub

    Private Sub ClearFields()
        selectedId = 0
        cmbRace.SelectedIndex = -1
        cmbDriver.SelectedIndex = -1
        cmbTeam.SelectedIndex = -1
        cmbVehicle.SelectedIndex = -1
        txtPosition.Clear()
        txtPoints.Clear()
        txtSearch.Clear()
    End Sub
End Class
