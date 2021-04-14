Imports System
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.EditorInput
Imports System.Windows.Shapes

Public Class clsCommandMonitor
    Shared acDoc As Document = Application.DocumentManager.MdiActiveDocument
    Shared acCurDb As Database = acDoc.Database
    Shared acEd As Editor = acDoc.Editor
    Shared AcApp As Application
    Shared sDictionaryName As String = "QSFILTERLIST"
    Shared sDictionaryNameSettings As String = "QSFILTERLISTSETTINGS"
    Shared aLoadedFilters As List(Of String)
    Shared oColXref As ObjectIdCollection
    Shared oColXrefKlic As ObjectIdCollection
    Shared sCoreDir As String = clsFunctions.getCoreDir()
    Public Shared Function commandStart(ByVal o As Object, ByVal e As CommandEventArgs)
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor
        Select Case e.GlobalCommandName
            Case "QSAVE"
                'aLoadedFilters = loadFilters()
                'oColXref = getXrefCollection(aLoadedFilters)
                Try
                    Dim aLoadFilters As List(Of String) = clsFilterData.loadSettings(acDoc, acCurDb, acEd, sDictionaryNameSettings, "CommandMonitorActive")
                    If aLoadFilters(0) = "true" Then
                        'Collectie van geladen Xrefs ophalen
                        oColXref = getLoadedXrefs()
                        'geladen xrefs uitzetten
                        acCurDb.UnloadXrefs(oColXref)
                    End If
                Catch ex As Autodesk.AutoCAD.Runtime.Exception

                End Try
            Case "QUIT", "CLOSE"
                'remove handlers
                endCommandMonitor()
        End Select
        Return True
    End Function

    Public Shared Function commandEnd(ByVal o As Object, ByVal e As CommandEventArgs)
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor
        Select Case e.GlobalCommandName
            Case "QSAVE"
                'geladen xrefs weer aanzetten na het saven
                Try
                    acCurDb.ReloadXrefs(oColXref)
                Catch ex As Autodesk.AutoCAD.Runtime.Exception

                End Try
        End Select
        Return True
    End Function

    Public Shared Function startCommandMonitor() As Boolean
        'To avoid ambiguity, we have to use the full type here.
        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim db As Database = HostApplicationServices.WorkingDatabase()
        AddHandler doc.CommandWillStart, New CommandEventHandler(AddressOf commandStart)
        AddHandler doc.CommandEnded, New CommandEventHandler(AddressOf commandEnd)
        saveSettings("CommandMonitorActive", "true")
        startCommandMonitor = True
    End Function

    Public Shared Function endCommandMonitor() As Boolean
        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim db As Database = HostApplicationServices.WorkingDatabase()

        RemoveHandler doc.CommandWillStart, AddressOf commandStart
        RemoveHandler doc.CommandEnded, AddressOf commandEnd
        saveSettings("CommandMonitorActive", "false")
        endCommandMonitor = True
    End Function

    Public Shared Function reloadProject() As Boolean
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor
        Try
            aLoadedFilters = loadFilters()
            oColXref = getXrefCollection(aLoadedFilters)
            acCurDb.ReloadXrefs(oColXref)
        Catch ex As Autodesk.AutoCAD.Runtime.Exception

        End Try
    End Function

    Public Shared Function klicaan() As Boolean
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor

        Try
            oColXrefKlic = getLoadedXrefs("KLIC")
            If oColXrefKlic.Count > 0 Then
                acCurDb.ReloadXrefs(oColXrefKlic)
            Else
                acEd.WriteMessage("Geen KLIC onderlegger gevonden!" & vbCrLf)
            End If
        Catch ex As Autodesk.AutoCAD.Runtime.Exception
            MsgBox("Er ging iets mis bij het reloaden van de Klic onderlegger!" & vbCrLf & ex.Message)
        End Try
        Return True
    End Function

    Public Shared Function klicuit() As Boolean
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor

        Try
            oColXrefKlic = getLoadedXrefs("KLIC")
            If oColXrefKlic.Count > 0 Then
                acCurDb.UnloadXrefs(oColXrefKlic)
            Else
                acEd.WriteMessage("Geen KLIC onderlegger gevonden!" & vbCrLf)
            End If
        Catch ex As Autodesk.AutoCAD.Runtime.Exception
            MsgBox("Er ging iets mis bij het unloaden van de Klic onderlegger!" & vbCrLf & ex.Message)
        End Try
        Return True
    End Function

    Public Shared Function LoudUnloadXref(bLoad As Boolean, sWildcard As String)
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor

        oColXrefKlic = getLoadedXrefs(sWildcard)
        If bLoad = True Then
            Try
                If oColXrefKlic.Count > 0 Then
                    acCurDb.ReloadXrefs(oColXrefKlic)
                Else
                    acEd.WriteMessage("Geen onderlegger gevonden!" & vbCrLf)
                End If
            Catch ex As Autodesk.AutoCAD.Runtime.Exception
                MsgBox("Er ging iets mis bij het reloaden van de onderlegger!" & vbCrLf & ex.Message)
            End Try
        Else
            Try
                If oColXrefKlic.Count > 0 Then
                    acCurDb.UnloadXrefs(oColXrefKlic)
                Else
                    acEd.WriteMessage("Geen onderlegger gevonden!" & vbCrLf)
                End If
            Catch ex As Autodesk.AutoCAD.Runtime.Exception
                MsgBox("Er ging iets mis bij het unloaden van de onderlegger!" & vbCrLf & ex.Message)
            End Try
        End If
        Return True
    End Function



    Public Shared Function loadFilters(Optional sFiltername As String = "xrefs", Optional sDictionaryName As String = "QSFILTERLIST") As List(Of String)
        'Dim dict As Dictionary(Of String, List(Of String)) = clsFilterData.getFilterFromDictionary(acDoc, acCurDb, acEd, sDictionaryName, "xrefs")
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor
        Dim dict As Dictionary(Of String, List(Of String)) = clsFilterData.getFilterFromDictionary(acDoc, acCurDb, acEd, sDictionaryName, sFiltername)
        Dim aLoadFilters As List(Of String) = New List(Of String)
        Try
            If dict Is Nothing Or dict.Count = 0 Then
                Return aLoadFilters
            End If
            For Each pair As KeyValuePair(Of String, List(Of String)) In dict
                For Each s As String In pair.Value
                    aLoadFilters.Add(s)
                Next
            Next
        Catch ex As System.Exception
            MsgBox("Fout bij het laden van het filter" & vbCrLf & ex.Message)
            loadFilters = New List(Of String)
        End Try
        loadFilters = aLoadFilters
    End Function

    Public Shared Function getXrefCollection(ByVal aLoadedFilters As List(Of String)) As ObjectIdCollection
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor
        Dim oColXrefLoad As ObjectIdCollection = New ObjectIdCollection
        Using acLock As DocumentLock = acDoc.LockDocument()
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                Dim xgXref As XrefGraph = acCurDb.GetHostDwgXrefGraph(True)
                Dim xgRoot As GraphNode = xgXref.RootNode
                For i As Integer = 0 To xgRoot.NumOut - 1
                    Dim xgChild As XrefGraphNode = TryCast(xgRoot.Out(i), XrefGraphNode)
                    'alleen de xrefs weergeven wie gevonden worden of unloaded zijn
                    If xgChild.XrefStatus = XrefStatus.Resolved Or xgChild.XrefStatus = XrefStatus.Unloaded Then
                        Dim acBTR As BlockTableRecord = TryCast(acTrans.GetObject(xgChild.BlockTableRecordId, OpenMode.ForRead), BlockTableRecord)
                        If acBTR.IsFromOverlayReference Or acBTR.IsUnloaded Then
                            'Dim myCntrl As RNCeckItem.CheckItem = New RNCeckItem.CheckItem
                            If aLoadedFilters.Contains(System.IO.Path.GetFileNameWithoutExtension(acBTR.PathName)) Then
                                oColXrefLoad.Add(acBTR.ObjectId)
                            End If
                        End If
                    End If
                Next i
            End Using
        End Using
        Return oColXrefLoad
    End Function

    ''' <summary>
    ''' 'Get collection of loaded xrefs
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function getLoadedXrefs() As ObjectIdCollection
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor
        Dim oColXrefLoad As ObjectIdCollection = New ObjectIdCollection
        Using acLock As DocumentLock = acDoc.LockDocument()
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                Dim xgXref As XrefGraph = acCurDb.GetHostDwgXrefGraph(True)
                Dim xgRoot As GraphNode = xgXref.RootNode
                For i As Integer = 0 To xgRoot.NumOut - 1
                    Dim xgChild As XrefGraphNode = TryCast(xgRoot.Out(i), XrefGraphNode)
                    'alleen de xrefs terug sturen wie loaded zijn
                    If xgChild.XrefStatus = XrefStatus.Resolved Then ' Or xgChild.XrefStatus = XrefStatus.Unloaded Then
                        Dim acBTR As BlockTableRecord = TryCast(acTrans.GetObject(xgChild.BlockTableRecordId, OpenMode.ForRead), BlockTableRecord)
                        If acBTR.IsFromOverlayReference Then ' Or acBTR.IsUnloaded Then
                            oColXrefLoad.Add(acBTR.ObjectId)
                        End If
                    End If
                Next i
            End Using
        End Using
        Return oColXrefLoad
    End Function

    ''' <summary>
    ''' 'Get collection of loaded xrefs
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function getLoadedXrefs(sWildCard As String) As ObjectIdCollection
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor
        Dim oColXrefLoad As ObjectIdCollection = New ObjectIdCollection
        Using acLock As DocumentLock = acDoc.LockDocument()
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                Dim xgXref As XrefGraph = acCurDb.GetHostDwgXrefGraph(True)
                Dim xgRoot As GraphNode = xgXref.RootNode
                For i As Integer = 0 To xgRoot.NumOut - 1
                    Dim xgChild As XrefGraphNode = TryCast(xgRoot.Out(i), XrefGraphNode)
                    'alleen de xrefs terug sturen wie loaded zijn en voldoen aan de wildcard
                    If xgChild.XrefStatus = XrefStatus.Resolved Or xgChild.XrefStatus = XrefStatus.Unloaded Then
                        Dim acBTR As BlockTableRecord = TryCast(acTrans.GetObject(xgChild.BlockTableRecordId, OpenMode.ForRead), BlockTableRecord)
                        If acBTR.IsFromOverlayReference And acBTR.PathName.ToUpper.Contains(sWildCard) Then ' Or acBTR.IsUnloaded Then
                            oColXrefLoad.Add(acBTR.ObjectId)
                            acEd.WriteMessage("KLIC onderlegger gevonden: " & acBTR.PathName & vbCrLf)
                        End If
                    End If
                Next i
            End Using
        End Using
        Return oColXrefLoad
    End Function


    Public Shared Function getXrefCollection() As ObjectIdCollection
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor
        Dim oColXrefLoad As ObjectIdCollection = New ObjectIdCollection
        Using acLock As DocumentLock = acDoc.LockDocument()
            Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
                Dim xgXref As XrefGraph = acCurDb.GetHostDwgXrefGraph(True)
                Dim xgRoot As GraphNode = xgXref.RootNode
                For i As Integer = 0 To xgRoot.NumOut - 1
                    Dim xgChild As XrefGraphNode = TryCast(xgRoot.Out(i), XrefGraphNode)
                    'alleen de xrefs weergeven wie gevonden worden of loaded zijn
                    If xgChild.XrefStatus = XrefStatus.Resolved Or xgChild.XrefStatus = XrefStatus.Unloaded = False Then
                        Dim acBTR As BlockTableRecord = TryCast(acTrans.GetObject(xgChild.BlockTableRecordId, OpenMode.ForRead), BlockTableRecord)
                        If acBTR.IsFromOverlayReference Or acBTR.IsUnloaded = False Then
                            'Dim myCntrl As RNCeckItem.CheckItem = New RNCeckItem.CheckItem
                            'If TypeOf acBTR.ObjectId = Xref Then
                            oColXrefLoad.Add(acBTR.ObjectId)
                            'End If
                        End If
                    End If
                Next i
            End Using
        End Using
        Return oColXrefLoad
    End Function

    Public Shared Sub saveSettings(ByVal sSetting As String, ByVal sSettingValue As String)
        acDoc = Application.DocumentManager.MdiActiveDocument
        acCurDb = acDoc.Database
        acEd = acDoc.Editor
        Dim dictLoad As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

        Dim val As List(Of String) = New List(Of String)
        val.Add(sSettingValue)
        dictLoad.Add(sSetting, val)
        'save dict
        If clsFilterData.saveFilter(acDoc, acCurDb, acEd, dictLoad, sDictionaryNameSettings) Then
            'MsgBox(val.Count.ToString & " --- " & dictLoad.Count.ToString)
            'MsgBox("Setting: " & sSetting & " ingesteld op: " & sSettingValue)
        Else
            MsgBox("Fout bij het opslaan van de instelling " & sSetting & " !")
        End If
        'Return True
    End Sub
End Class
