Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class ucSponsors
    Inherits UserControl

    Private txtSponsorName As TextBox
    Private txtContact As TextBox
    Private txtPhone As TextBox
    Private txtAmount As TextBox
    Private cmbTeam As ComboBox
    Private txtSearch As TextBox
    Private grid As DataGridView
    Private selectedId As Integer

    Public Sub New()
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White
        BuildScreen()
        LoadTeams()
        LoadData()
    End Sub

    Private Sub BuildScreen()
        Dim title As New Label()
        title.Text = "SPONSOR MANAGEMENT"
        title.Font = New Font("Arial", 20, FontStyle.Bold)
        title.AutoSize = True
        title.Location = New Point(30, 20)
        Controls.Add(title)

        AddLabel("Sponsor Name", 30, 65)
        txtSponsorName = MakeTextBox(125, 62, 180)
        Controls.Add(txtSponsorName)

        AddLabel("Contact", 330, 65)
        txtContact = MakeTextBox(385, 62, 170)
        Controls.Add(txtContact)

        AddLabel("Phone", 580, 65)
        txtPhone = MakeTextBox(625, 62, 150)
        Controls.Add(txtPhone)

        AddLabel("Amount", 30, 105)
        txtAmount = MakeTextBox(85, 102, 120)
        Controls.Add(txtAmount)

        AddLabel("Team", 230, 105)
        cmbTeam = New ComboBox()
        cmbTeam.Location = New Point(275, 102)
        cmbTeam.Width = 190
        cmbTeam.DropDownStyle = ComboBoxStyle.DropDownList
        Controls.Add(cmbTeam)

        Dim btnAdd As Button = MakeButton("ADD", 485, 100)
        AddHandler btnAdd.Click, AddressOf AddClick
        Controls.Add(btnAdd)

        Dim btnUpdate As Button = MakeButton("UPDATE", 580, 100)
        AddHandler btnUpdate.Click, AddressOf UpdateClick
        Controls.Add(btnUpdate)

        Dim btnDelete As Button = MakeButton("DELETE", 690, 100)
        AddHandler btnDelete.Click, AddressOf DeleteClick
        Controls.Add(btnDelete)

        AddLabel("Search", 30, 150)
        txtSearch = MakeTextBox(85, 147, 200)
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

    Private Function MakeTextBox(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer) As TextBox
        Dim t As New TextBox()
        t.Location = New Point(x, y)
        t.Width = width
        Return t
    End Function

    Private Function MakeButton(ByVal text As String, ByVal x As Integer, ByVal y As Integer) As Button
        Dim b As New Button()
        b.Text = text
        b.Location = New Point(x, y)
        b.Width = 90
        b.Height = 32
        Return b
    End Function

    Private Sub LoadTeams()
        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Using da As New SqlDataAdapter("SELECT TeamID,TeamName FROM Teams ORDER BY TeamName", con)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    cmbTeam.DataSource = dt
                    cmbTeam.ValueMember = "TeamID"
                    cmbTeam.DisplayMember = "TeamName"
                    cmbTeam.SelectedIndex = -1
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Lookup Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadData(Optional ByVal searchValue As String = "")
        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "SELECT s.SponsorID,s.SponsorName,s.ContactPerson,s.Phone,s.Amount,t.TeamName " & _
                                    "FROM Sponsors s INNER JOIN Teams t ON s.TeamID=t.TeamID"
                If searchValue <> "" Then
                    sql &= " WHERE s.SponsorName LIKE @Search OR s.ContactPerson LIKE @Search OR t.TeamName LIKE @Search"
                End If
                sql &= " ORDER BY s.SponsorID"

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

    Private Function ValidAmount(ByRef amount As Decimal) As Boolean
        If Not Decimal.TryParse(txtAmount.Text.Trim(), amount) OrElse amount < 0D Then
            MessageBox.Show("Enter a valid sponsorship amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    Private Function ValidInput(ByRef amount As Decimal) As Boolean
        If txtSponsorName.Text.Trim() = "" Then
            MessageBox.Show("Please enter Sponsor Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        If cmbTeam.SelectedIndex < 0 Then
            MessageBox.Show("Please select a Team.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return ValidAmount(amount)
    End Function

    Private Sub AddClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim amount As Decimal
        If Not ValidInput(amount) Then Return

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "INSERT INTO Sponsors(SponsorName,ContactPerson,Phone,Amount,TeamID) VALUES(@Name,@Contact,@Phone,@Amount,@Team)"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@Name", txtSponsorName.Text.Trim())
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim())
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim())
                    cmd.Parameters.AddWithValue("@Amount", amount)
                    cmd.Parameters.AddWithValue("@Team", CInt(cmbTeam.SelectedValue))
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Sponsor Added Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateClick(ByVal sender As Object, ByVal e As EventArgs)
        If selectedId = 0 Then
            MessageBox.Show("Select a sponsor first.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim amount As Decimal
        If Not ValidInput(amount) Then Return

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Dim sql As String = "UPDATE Sponsors SET SponsorName=@Name,ContactPerson=@Contact,Phone=@Phone,Amount=@Amount,TeamID=@Team WHERE SponsorID=@ID"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@Name", txtSponsorName.Text.Trim())
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim())
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim())
                    cmd.Parameters.AddWithValue("@Amount", amount)
                    cmd.Parameters.AddWithValue("@Team", CInt(cmbTeam.SelectedValue))
                    cmd.Parameters.AddWithValue("@ID", selectedId)
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Sponsor Updated Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DeleteClick(ByVal sender As Object, ByVal e As EventArgs)
        If selectedId = 0 Then
            MessageBox.Show("Select a sponsor first.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If MessageBox.Show("Delete selected sponsor?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return

        Try
            Using con As SqlConnection = DBConnection.GetConnection()
                Using cmd As New SqlCommand("DELETE FROM Sponsors WHERE SponsorID=@ID", con)
                    cmd.Parameters.AddWithValue("@ID", selectedId)
                    con.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            ClearFields()
            LoadData()
            MessageBox.Show("Sponsor Deleted Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SearchClick(ByVal sender As Object, ByVal e As EventArgs)
        LoadData(txtSearch.Text.Trim())
    End Sub

    Private Sub RefreshClick(ByVal sender As Object, ByVal e As EventArgs)
        LoadTeams()
        LoadData()
    End Sub

    Private Sub GridCellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        Dim row As DataGridViewRow = grid.Rows(e.RowIndex)
        selectedId = Convert.ToInt32(row.Cells("SponsorID").Value)
        txtSponsorName.Text = Convert.ToString(row.Cells("SponsorName").Value)
        txtContact.Text = Convert.ToString(row.Cells("ContactPerson").Value)
        txtPhone.Text = Convert.ToString(row.Cells("Phone").Value)
        txtAmount.Text = Convert.ToString(row.Cells("Amount").Value)
        SelectTeamByName(Convert.ToString(row.Cells("TeamName").Value))
    End Sub

    Private Sub SelectTeamByName(ByVal teamName As String)
        For i As Integer = 0 To cmbTeam.Items.Count - 1
            Dim drv As DataRowView = TryCast(cmbTeam.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso Convert.ToString(drv("TeamName")) = teamName Then
                cmbTeam.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub

    Private Sub ClearFields()
        selectedId = 0
        txtSponsorName.Clear()
        txtContact.Clear()
        txtPhone.Clear()
        txtAmount.Clear()
        txtSearch.Clear()
        cmbTeam.SelectedIndex = -1
    End Sub
End Class
