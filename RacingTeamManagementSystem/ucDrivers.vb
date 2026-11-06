Public Class ucDrivers
    Inherits ucCrudBase

    Public Sub New()
        MyBase.New("DRIVER MANAGEMENT", "Drivers", "DriverID", "DriverName", "Phone")
    End Sub
End Class
