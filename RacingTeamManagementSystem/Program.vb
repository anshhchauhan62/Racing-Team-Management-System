Imports System.Windows.Forms

Module Program
    <STAThread()> _
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New frmLogin())
    End Sub
End Module
