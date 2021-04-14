Imports System.Drawing
Imports System.IO
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.EditorInput

Public Class frmSettings
    Dim acDoc As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
    Dim acCurDb As Database = acDoc.Database
    Dim acEd As Editor = acDoc.Editor
    Dim AcApp As Autodesk.AutoCAD.ApplicationServices.Application
    Dim sDictionaryName As String = "QSFILTERLIST"
    Shared sDictionaryNameSettings As String = "QSFILTERLISTSETTINGS"
    Dim bIsLoading As Boolean = False

    Private Shared m_DocData As clsMyDocData = New clsMyDocData

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor
        'AcApp = Autodesk.AutoCAD.ApplicationServices.Application
        loadXrefs()
        loadSettings()
    End Sub

    Function loadXrefs()
        bIsLoading = True
        flowXrefs.Controls.Clear()
        Dim aFilterXrefs As List(Of String) = loadFilters()
        'MsgBox(aFilterXrefs.Count & " filters gevonden")
        Using acLock As DocumentLock = acDoc.LockDocument()
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                Dim xgXref As XrefGraph = acCurDb.GetHostDwgXrefGraph(True)
                Dim xgRoot As GraphNode = xgXref.RootNode
                'Dim sReturn As String = ""
                For i As Integer = 0 To xgRoot.NumOut - 1
                    Dim xgChild As XrefGraphNode = TryCast(xgRoot.Out(i), XrefGraphNode)
                    'alleen de xrefs weergeven wie gevonden worden of unloaded zijn
                    If xgChild.XrefStatus = XrefStatus.Resolved Or xgChild.XrefStatus = XrefStatus.Unloaded Then
                        Dim acBTR As BlockTableRecord = TryCast(acTrans.GetObject(xgChild.BlockTableRecordId, OpenMode.ForRead), BlockTableRecord)
                        If acBTR.IsFromOverlayReference Or acBTR.IsUnloaded Then
                            Dim myCntrl As RNCeckItem.CheckItem = New RNCeckItem.CheckItem
                            'sReturn = sReturn & ", " & acBTR.ObjectId.ToString
                            'If aFilterXrefs.Contains(acBTR.ObjectId.ToString) Then
                            If aFilterXrefs.Contains(Path.GetFileNameWithoutExtension(acBTR.PathName)) Then
                                myCntrl.IsChecked = True
                            End If
                            myCntrl.ItemObjectID = acBTR.ObjectId
                            myCntrl.ItemTekst = Path.GetFileNameWithoutExtension(acBTR.PathName)
                            'add handlers to register functions for items
                            AddHandler myCntrl.xref_CheckedChanged, AddressOf addXrefToFilter
                            myCntrl.updateItem()
                            flowXrefs.Controls.Add(myCntrl)
                        End If
                    End If
                Next i
                'MsgBox(sReturn)
                'InputBox(sReturn, sReturn, sReturn)
            End Using
        End Using
        bIsLoading = False
        Return True
    End Function

    Function loadSettings()
        Try
            'Dim dict As Dictionary(Of String, List(Of String)) = clsFilterData.getFilterFromDictionary(acDoc, acCurDb, acEd, sDictionaryNameSettings, "CommandMonitorActive")
            'Dim aLoadFilters As List(Of String) = New List(Of String)
            'For Each pair As KeyValuePair(Of String, List(Of String)) In dict
            '    For Each s As String In pair.Value
            '        aLoadFilters.Add(s)
            '    Next
            'Next
            Dim aLoadFilters As List(Of String) = clsFilterData.loadSettings(acDoc, acCurDb, acEd, sDictionaryNameSettings, "CommandMonitorActive")
            If aLoadFilters(0) = "true" Then
                'commandmonitor actief
                cmdStart.BackColor = Color.DarkGreen
                lblQSstatus.Text = "QuickSave INGESCHAKELD"
                lblQSstatus.BackColor = Color.DarkGreen
            Else
                'commandmonitor inactief
                cmdStart.BackColor = DefaultBackColor
                lblQSstatus.Text = "QuickSave UIGESCHAKELD"
                lblQSstatus.BackColor = Color.Red
            End If
        Catch ex As Exception
            'geen filter gevonden
        End Try
        Return True
    End Function

    Function loadFilters() As List(Of String)
        Dim dict As Dictionary(Of String, List(Of String)) = clsFilterData.getFilterFromDictionary(acDoc, acCurDb, acEd, sDictionaryName, "xrefs")
        Dim aLoadFilters As List(Of String) = New List(Of String)
        Try
            'If dict Is Nothing Or dict.Count = 0 Then
            '    Return aLoadFilters
            'End If
            'Dim sReturn As String = ""
            For Each pair As KeyValuePair(Of String, List(Of String)) In dict
                For Each s As String In pair.Value
                    aLoadFilters.Add(s)
                    'sReturn = sReturn & ", " & s
                Next
            Next
            'MsgBox(sReturn)
            'InputBox(sReturn, sReturn, sReturn)
        Catch ex As Exception
            MsgBox("Fout bij het laden van het filter" & vbCrLf & ex.Message)
            loadFilters = New List(Of String)
        End Try
        loadFilters = aLoadFilters
    End Function

    Public Function addXrefToFilter()
        If bIsLoading = True Then
            Return True 'verlaat functie, bij laden hoef het niet gesaved te worden
        End If
        'Dim dict As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))
        Dim dictLoad As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

        Dim val As List(Of String) = New List(Of String)

        Try
            For Each myCntrl As RNCeckItem.CheckItem In flowXrefs.Controls
                If myCntrl.IsChecked = True Then
                    'val.Add(myCntrl.ItemObjectID.ToString)
                    val.Add(myCntrl.ItemTekst)
                End If
            Next
            dictLoad.Add("xrefs", val)
        Catch ex As Exception
            MsgBox("Fout bij het aanmaken van het filter!" & vbCrLf & ex.Message & ex.InnerException.ToString)
        End Try
        'save dict
        If clsFilterData.saveFilter(acDoc, acCurDb, acEd, dictLoad, sDictionaryName) Then

        Else
            MsgBox("Fout bij het toevoegen van het filter!")
        End If

        Return True
    End Function

    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        Close()
    End Sub

    Private Sub cmdStop_Click(sender As Object, e As EventArgs) Handles cmdStop.Click
        clsCommandMonitor.endCommandMonitor()
        loadSettings()
    End Sub

    Private Sub cmdStart_Click(sender As Object, e As EventArgs) Handles cmdStart.Click
        clsCommandMonitor.startCommandMonitor()
        loadSettings()
    End Sub
End Class