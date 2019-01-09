Imports System.IO
Imports System.Reflection
Imports Autodesk.AutoCAD.ApplicationServices

Public Class clsFunctions
    Public Shared ReadOnly Property AcadVersion() As Version
        Get
            Return GetType(Document).Assembly.GetName().Version
        End Get
    End Property

    Public Shared Function getCoreDir() As String
        Dim strAssemblyPath As String = Assembly.GetExecutingAssembly.Location
        Dim strTmpPath() As String = strAssemblyPath.Split("\")
        Dim arrUbound As Integer = UBound(strTmpPath)
        getCoreDir = strAssemblyPath.Replace(strTmpPath(arrUbound), "").TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
    End Function

    Public Shared Sub makeLog(ByVal sLogFile As String, ByVal sMessage As String, Optional bVersionInfo As Boolean = False)
        Try
            If Not Directory.Exists(Path.GetDirectoryName(sLogFile)) Then
                Directory.CreateDirectory(Path.GetDirectoryName(sLogFile))
            End If
            Dim dtDatum As DateTime = DateTime.Now
            If Not File.Exists(sLogFile) Then
                Using sw As StreamWriter = File.CreateText(sLogFile)
                    sw.WriteLine("LOG FILE AANGEMAAKT OP: " & dtDatum.ToString("dd/MM/yyyy HH:mm:ss"))
                    sw.WriteLine("----------------------------------------------------------------")
                End Using
            End If
            Using sw As StreamWriter = File.AppendText(sLogFile)
                If bVersionInfo Then
                    sw.WriteLine("Computer: " & My.Computer.Name)
                    Dim sAcadVersion As String = clsFunctions.AcadVersion.Major.ToString & "." & clsFunctions.AcadVersion.Minor.ToString & "." & clsFunctions.AcadVersion.Revision.ToString
                    sw.WriteLine("Autodesk Software: R" & sAcadVersion)
                End If
                sw.WriteLine(dtDatum.ToString("dd/MM/yyyy HH:mm:ss") & " - " & sMessage)
            End Using
        Catch ex As Exception
            MsgBox("Fout bij maken LOG")
        End Try
    End Sub
End Class
