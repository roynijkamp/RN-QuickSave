Imports Autodesk.AutoCAD.DatabaseServices

Public Class CheckItem
    Inherits System.Windows.Forms.UserControl

    Private sCheckItemTekst As String = "Not Set"
    Private oItemObjectID As ObjectId
    Private bIsChecked As Boolean = False

    Public Property ItemTekst As String
        Get
            Return sCheckItemTekst
        End Get
        Set(value As String)
            sCheckItemTekst = value
        End Set
    End Property

    Public Property ItemObjectID As ObjectId
        Get
            Return oItemObjectID
        End Get
        Set(value As ObjectId)
            oItemObjectID = value
        End Set
    End Property

    Public Property IsChecked As Boolean
        Get
            Return bIsChecked
        End Get
        Set(value As Boolean)
            bIsChecked = value
        End Set
    End Property

    Public Event xref_CheckedChanged(sender As Object, e As EventArgs)

    Private Sub chkItem_CheckStateChanged(sender As Object, e As EventArgs) Handles chkItem.CheckStateChanged

        bIsChecked = chkItem.Checked
        RaiseEvent xref_CheckedChanged(Me, e)

    End Sub

    Public Function updateItem()
        chkItem.Text = sCheckItemTekst
        chkItem.Checked = bIsChecked
        Return True
    End Function
End Class
