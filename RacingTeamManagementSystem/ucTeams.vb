Public Class ucTeams
    Inherits ucCrudBase

    Public Sub New()
        MyBase.New("TEAM MANAGEMENT", "Teams", "TeamID", "TeamName", "TeamManager")
    End Sub
End Class
