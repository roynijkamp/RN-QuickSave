<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CheckItem
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.chkItem = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'chkItem
        '
        Me.chkItem.AutoSize = True
        Me.chkItem.Location = New System.Drawing.Point(3, 1)
        Me.chkItem.Name = "chkItem"
        Me.chkItem.Size = New System.Drawing.Size(105, 17)
        Me.chkItem.TabIndex = 0
        Me.chkItem.Text = "Check item tekst"
        Me.chkItem.UseVisualStyleBackColor = True
        '
        'CheckItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.chkItem)
        Me.Name = "CheckItem"
        Me.Size = New System.Drawing.Size(460, 19)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents chkItem As Windows.Forms.CheckBox
End Class
