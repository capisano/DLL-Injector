Imports InjectionLibrary
Imports JLibrary.PortableExecutable
Imports System.IO
Public Class Form1
    Public Filepath As String
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        For Each OneProcess As Process In Process.GetProcesses
            ListBox1.Items.Add(OneProcess.ProcessName)
        Next
    End Sub
    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        Me.Close()
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Public Function MMInject(ByVal pID As Integer, ByVal DLL As Byte()) As Boolean
        Try
            Dim injector As InjectionMethod = InjectionMethod.Create(InjectionMethodType.ManualMap)
            Dim hModule As IntPtr = IntPtr.Zero

            Using img As New PortableExecutable(DLL)
                hModule = injector.Inject(img, pID)
            End Using
            If hModule <> IntPtr.Zero Then
                DLL = Nothing
                Return True
            Else
                DLL = Nothing
                Return False
            End If
        Catch ex As Exception
            MsgBox("Injection Failed...", 47, "Error: 0x001")
            End
        End Try
    End Function
    Public Function THInject(ByVal pID As Integer, ByVal DLL As Byte()) As Boolean
        Try
            Dim injector As InjectionMethod = InjectionMethod.Create(InjectionMethodType.ThreadHijack)
            Dim hModule As IntPtr = IntPtr.Zero
            Using img As New PortableExecutable(DLL)
                hModule = injector.Inject(img, pID)
            End Using
            If hModule <> IntPtr.Zero Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox("Injection Failed...", 47, "Error: 0x001")
            End
        End Try
    End Function
    Public Function LLInject(ByVal pID As Integer, ByVal DLL As Byte()) As Boolean
        Try
            Dim injector As InjectionMethod = InjectionMethod.Create(InjectionMethodType.Standard)
            Dim hModule As IntPtr = IntPtr.Zero

            Using img As New PortableExecutable(DLL)
                hModule = injector.Inject(img, pID)
            End Using
            If hModule <> IntPtr.Zero Then
                DLL = Nothing
                Return True
            Else
                DLL = Nothing
                Return False
            End If
        Catch ex As Exception
            MsgBox("Injection Failed...", 47, "Error: 0x001")
            End
        End Try
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Filepath = Nothing Or ListBox1.SelectedItem = Nothing Then
            Return
        End If
        Dim targeted As Process() = Process.GetProcessesByName(ListBox1.SelectedItem)
        Dim pid As Integer = targeted(0).Id
        Dim DLL As Byte() = File.ReadAllBytes(Filepath)
        ProgressBar1.Value = 50
        If ComboBox1.SelectedItem.ToString = "Manual Mapping" Then
            If MMInject(pid, DLL) Then
                ProgressBar1.Value = 100
                Label7.ForeColor = Color.Green
                Label7.Text = "Success"
                Return
            Else
                ProgressBar1.Value = 0
                Label7.ForeColor = Color.Red
                ProgressBar1.BackColor = Color.Red
                Label7.Text = "Failed"
                Return
            End If
        ElseIf ComboBox1.SelectedItem.ToString = "Load Library" Then
            If LLInject(pid, DLL) Then
                ProgressBar1.Value = 100
                Label7.ForeColor = Color.Green
                Label7.Text = "Success"
                Return
            Else
                ProgressBar1.Value = 0
                Label7.ForeColor = Color.Red
                ProgressBar1.BackColor = Color.Red
                Label7.Text = "Failed"
                Return
            End If
        ElseIf ComboBox1.SelectedItem.ToString = "Thread Hijack" Then
            If THInject(pid, DLL) Then
                ProgressBar1.Value = 100
                Label7.ForeColor = Color.Green
                Label7.Text = "Success"
                Return
            Else
                ProgressBar1.Value = 0
                Label7.ForeColor = Color.Red
                ProgressBar1.BackColor = Color.Red
                Label7.Text = "Failed"
                Return
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Dim dialog As New OpenFileDialog()
        If DialogResult.OK = dialog.ShowDialog Then
            Label4.Text = dialog.FileName
        End If
    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        Dim dialog As New OpenFileDialog()
        dialog.Filter = "Dynamic Link Library(.dll) |*.dll"
        If DialogResult.OK = dialog.ShowDialog Then
            Filepath = dialog.FileName
            Dim dirpath As String = Path.GetFileName(Filepath)
            Label8.Text = dirpath
        End If
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub
    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        ListBox1.Items.Clear()
        For Each OneProcess As Process In Process.GetProcesses
            ListBox1.Items.Add(OneProcess.ProcessName)
        Next
    End Sub
    Private IsFormBeingDragged As Boolean = False
    Private MouseDownX As Integer
    Private MouseDownY As Integer
    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown

        If e.Button = MouseButtons.Left Then
            IsFormBeingDragged = True
            MouseDownX = e.X
            MouseDownY = e.Y
        End If
    End Sub

    Private Sub Form1_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseUp

        If e.Button = MouseButtons.Left Then
            IsFormBeingDragged = False
        End If
    End Sub

    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseMove

        If IsFormBeingDragged Then
            Dim temp As Point = New Point()

            temp.X = Me.Location.X + (e.X - MouseDownX)
            temp.Y = Me.Location.Y + (e.Y - MouseDownY)
            Me.Location = temp
            temp = Nothing
        End If
    End Sub
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Label1.Text = "Proccess: " & ListBox1.SelectedItem
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click

    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs)

    End Sub
End Class
