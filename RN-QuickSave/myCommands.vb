' (C) Copyright 2011 by  
'
Imports System
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.EditorInput

' This line is not mandatory, but improves loading performances
<Assembly: CommandClass(GetType(RN_QuickSave.MyCommands))>
Namespace RN_QuickSave

    ' This class is instantiated by AutoCAD for each document when
    ' a command is called by the user the first time in the context
    ' of a given document. In other words, non static data in this class
    ' is implicitly per-document!
    Public Class MyCommands
        Dim acDoc As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
        Dim acCurDb As Database = acDoc.Database
        Dim acEd As Editor = acDoc.Editor
        Dim AcApp As Autodesk.AutoCAD.ApplicationServices.Application
        Dim oColXrefIsolate As ObjectIdCollection

        <CommandMethod("rnquicksavesetting", "rnquicksavesetting", "rnquicksavesetting", CommandFlags.Modal)>
        Public Sub rnquicksavesetting() ' This method can have any name
            ' open settings window
            Dim frmSettings As New frmSettings
            frmSettings.ShowDialog()
        End Sub

        <CommandMethod("rnquicksavereload", "rnquicksavereload", "rnquicksavereload", CommandFlags.Modal)>
        Public Sub rnquicksavereload() ' This method can have any name
            ' Put your command code here
            clsCommandMonitor.reloadProject()
        End Sub

        <CommandMethod("rnquicksavestart", "rnquicksavestart", "rnquicksavestart", CommandFlags.Modal)>
        Public Sub rnquicksavestart() ' This method can have any name
            ' Put your command code here
            clsCommandMonitor.startCommandMonitor()
            'clsCommandMonitor.saveSettings("CommandMonitorActive", "true")
        End Sub

        <CommandMethod("rnquicksavestop", "rnquicksavestop", "rnquicksavestop", CommandFlags.Modal)>
        Public Sub rnquicksavestop() ' This method can have any name
            ' Put your command code here
            clsCommandMonitor.endCommandMonitor()
            'clsCommandMonitor.saveSettings("CommandMonitorActive", "false")
        End Sub

        <CommandMethod("klicaan", "ka", "klicaan", CommandFlags.Modal)>
        Public Sub rnklicaan()
            clsCommandMonitor.klicaan()
        End Sub

        <CommandMethod("klicuit", "ku", "klicuit", CommandFlags.Modal)>
        Public Sub rnklicuit()
            clsCommandMonitor.klicuit()
        End Sub

        <CommandMethod("xrefaan", "xa", "xrefaan", CommandFlags.Modal)>
        Public Sub xrefaan()
            Dim sXref = getUserInput("Welke xref moet aan? ")
            If sXref = "" Then Exit Sub
            clsCommandMonitor.LoudUnloadXref(True, sXref.ToString.ToUpper)

        End Sub

        <CommandMethod("xrefuit", "xu", "xrefuit", CommandFlags.Modal)>
        Public Sub xrefuit()
            Dim sXref = getUserInput("Welke xref moet uit? ")
            If sXref = "" Then Exit Sub
            clsCommandMonitor.LoudUnloadXref(False, sXref.ToString.ToUpper)

        End Sub

        Public Function getUserInput(sVraag As String)
            Dim pStrOpts As PromptStringOptions = New PromptStringOptions(vbLf & sVraag)
            pStrOpts.AllowSpaces = True
            Dim pStrRes As PromptResult = acDoc.Editor.GetString(pStrOpts)
            If pStrRes.Status = PromptStatus.OK Then
                Return pStrRes.StringResult
            Else
                Return ""
            End If
        End Function

        Public Function getUserKeyWord(listKeys As List(Of String))
            Dim pKeyOpts As PromptKeywordOptions = New PromptKeywordOptions("")
            pKeyOpts.Message = vbLf & "Maak een keuze "
            'pKeyOpts.Keywords.Add("Line")
            'pKeyOpts.Keywords.Add("Circle")
            'pKeyOpts.Keywords.Add("Arc")
            For Each sKey As String In listKeys
                pKeyOpts.Keywords.Add(sKey)
            Next
            pKeyOpts.AllowNone = False
            Dim pKeyRes As PromptResult = acDoc.Editor.GetKeywords(pKeyOpts)
            If pKeyRes.Status = PromptStatus.OK Then
                Return pKeyRes.StringResult
            Else
                Return ""
            End If
        End Function


    End Class

End Namespace