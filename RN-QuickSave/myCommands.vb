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

        <CommandMethod("rnquicksave", "rnquicksave", "rnquicksave", CommandFlags.Modal)>
        Public Sub rnquicksave() ' This method can have any name
            ' Put your command code here

        End Sub

        <CommandMethod("rnquicksaveload", "rnquicksaveload", "rnquicksaveload", CommandFlags.Modal)>
        Public Sub rnquicksaveload() ' This method can have any name
            ' Put your command code here
            clsCommandMonitor.startCommandMonitor()
            clsCommandMonitor.saveSettings("CommandMonitorActive", "true")
        End Sub

        <CommandMethod("rnquicksaveunload", "rnquicksaveunload", "rnquicksaveunload", CommandFlags.Modal)>
        Public Sub rnquicksaveunload() ' This method can have any name
            ' Put your command code here
            clsCommandMonitor.endCommandMonitor()
            clsCommandMonitor.saveSettings("CommandMonitorActive", "false")
        End Sub
        ''' <summary>
        ''' 'Unload all loaded Xrefs
        ''' </summary>
        <CommandMethod("ip", "ip", "ip", CommandFlags.Modal)>
        Public Sub rnisolateproject()
            oColXrefIsolate = clsCommandMonitor.getXrefCollection()
            acCurDb.UnloadXrefs(oColXrefIsolate)
        End Sub
        ''' <summary>
        ''' 'reload previous unloaded xrefs
        ''' </summary>
        <CommandMethod("up", "up", "up", CommandFlags.Modal)>
        Public Sub rnunisolateproject()
            acCurDb.ReloadXrefs(oColXrefIsolate)
        End Sub




    End Class

End Namespace