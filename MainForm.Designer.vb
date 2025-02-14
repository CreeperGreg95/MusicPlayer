<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.sidebar = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.searchBox = New System.Windows.Forms.TextBox()
        Me.playerControls = New System.Windows.Forms.Panel()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.btnPause = New System.Windows.Forms.Button()
        Me.btnPlay = New System.Windows.Forms.Button()
        Me.FlowLayoutMusicPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.Panel2.SuspendLayout()
        Me.playerControls.SuspendLayout()
        Me.SuspendLayout()
        '
        'sidebar
        '
        Me.sidebar.Location = New System.Drawing.Point(1, 39)
        Me.sidebar.Name = "sidebar"
        Me.sidebar.Size = New System.Drawing.Size(205, 443)
        Me.sidebar.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.searchBox)
        Me.Panel2.Location = New System.Drawing.Point(0, -1)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(941, 42)
        Me.Panel2.TabIndex = 1
        '
        'searchBox
        '
        Me.searchBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.searchBox.Location = New System.Drawing.Point(273, 4)
        Me.searchBox.Name = "searchBox"
        Me.searchBox.Size = New System.Drawing.Size(456, 34)
        Me.searchBox.TabIndex = 0
        '
        'playerControls
        '
        Me.playerControls.Controls.Add(Me.btnNext)
        Me.playerControls.Controls.Add(Me.btnPrev)
        Me.playerControls.Controls.Add(Me.btnPause)
        Me.playerControls.Controls.Add(Me.btnPlay)
        Me.playerControls.Location = New System.Drawing.Point(1, 479)
        Me.playerControls.Name = "playerControls"
        Me.playerControls.Size = New System.Drawing.Size(943, 85)
        Me.playerControls.TabIndex = 2
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(519, 7)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(40, 32)
        Me.btnNext.TabIndex = 3
        Me.btnNext.Text = "⏩"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPrev
        '
        Me.btnPrev.Location = New System.Drawing.Point(381, 6)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(40, 32)
        Me.btnPrev.TabIndex = 2
        Me.btnPrev.Text = "⏪"
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'btnPause
        '
        Me.btnPause.Location = New System.Drawing.Point(473, 7)
        Me.btnPause.Name = "btnPause"
        Me.btnPause.Size = New System.Drawing.Size(40, 32)
        Me.btnPause.TabIndex = 1
        Me.btnPause.Text = "| |"
        Me.btnPause.UseVisualStyleBackColor = True
        '
        'btnPlay
        '
        Me.btnPlay.Location = New System.Drawing.Point(427, 7)
        Me.btnPlay.Name = "btnPlay"
        Me.btnPlay.Size = New System.Drawing.Size(40, 32)
        Me.btnPlay.TabIndex = 0
        Me.btnPlay.Text = "▶️"
        Me.btnPlay.UseVisualStyleBackColor = True
        '
        'FlowLayoutMusicPanel
        '
        Me.FlowLayoutMusicPanel.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.FlowLayoutMusicPanel.Location = New System.Drawing.Point(205, 43)
        Me.FlowLayoutMusicPanel.Name = "FlowLayoutMusicPanel"
        Me.FlowLayoutMusicPanel.Size = New System.Drawing.Size(732, 435)
        Me.FlowLayoutMusicPanel.TabIndex = 1
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(120.0!, 120.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(940, 566)
        Me.Controls.Add(Me.FlowLayoutMusicPanel)
        Me.Controls.Add(Me.playerControls)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.sidebar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "MainForm"
        Me.Text = "MainForm"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.playerControls.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents sidebar As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents playerControls As Panel
    Friend WithEvents searchBox As TextBox
    Friend WithEvents btnPrev As Button
    Friend WithEvents btnPause As Button
    Friend WithEvents btnPlay As Button
    Friend WithEvents btnNext As Button
    Friend WithEvents FlowLayoutMusicPanel As FlowLayoutPanel
End Class
