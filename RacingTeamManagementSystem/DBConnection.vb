Imports System.Data.SqlClient

Public Class DBConnection
    Private Shared ReadOnly ConnectionString As String = _
        "Data Source=.\SQLEXPRESS;Initial Catalog=RacingTeamDB;Integrated Security=True"

    Public Shared Function GetConnection() As SqlConnection
        Return New SqlConnection(ConnectionString)
    End Function
End Class
