<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.flowXrefs = New System.Windows.Forms.FlowLayoutPanel()
        Me.cmdStart = New System.Windows.Forms.Button()
        Me.cmdStop = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Khaki
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(490, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Selecteer de Xrefs welke tijdens saven uitgeschakeld moeten worden."
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(409, 350)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 1
        Me.cmdClose.Text = "Sluiten"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'flowXrefs
        '
        Me.flowXrefs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flowXrefs.AutoScroll = True
        Me.flowXrefs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flowXrefs.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flowXrefs.Location = New System.Drawing.Point(6, 25)
        Me.flowXrefs.Name = "flowXrefs"
        Me.flowXrefs.Size = New System.Drawing.Size(478, 319)
        Me.flowXrefs.TabIndex = 2
        Me.flowXrefs.WrapContents = False
        '
        'cmdStart
        '
        Me.cmdStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdStart.Location = New System.Drawing.Point(6, 350)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.Size = New System.Drawing.Size(97, 23)
        Me.cmdStart.TabIndex = 3
        Me.cmdStart.Text = "Start Xref Monitor"
        Me.cmdStart.UseVisualStyleBackColor = True
        '
        'cmdStop
        '
        Me.cmdStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdStop.Location = New System.Drawing.Point(109, 350)
        Me.cmdStop.Name = "cmdStop"
        Me.cmdStop.Size = New System.Drawing.Size(97, 23)
        Me.cmdStop.TabIndex = 4
        Me.cmdStop.Text = "Stop Xref Monitor"
        Me.cmdStop.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(496, 385)
        Me.Controls.Add(Me.cmdStop)
        Me.Controls.Add(Me.cmdStart)
        Me.Controls.Add(Me.flowXrefs)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(512, 424)
        Me.Name = "frmSettings"
        Me.Text = "RN - QuickSave"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents cmdClose As Windows.Forms.Button
    Friend WithEvents flowXrefs As Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmdStart As Windows.Forms.Button
    Friend WithEvents cmdStop As Windows.Forms.Button
End Class
