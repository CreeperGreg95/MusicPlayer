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
        Me.components = New System.ComponentModel.Container()
        Me.sidebar = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.searchBox = New System.Windows.Forms.TextBox()
        Me.playerControls = New System.Windows.Forms.Panel()
        Me.ModernTrackBar2 = New ModernTrackbar.ModernTrackBar()
        Me.ModernTrackBar1 = New ModernTrackbar.ModernTrackBar()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.btnPause = New System.Windows.Forms.Button()
        Me.btnPlay = New System.Windows.Forms.Button()
        Me.FlowLayoutMusicPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.MySqlCommand1 = New MySql.Data.MySqlClient.MySqlCommand()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Panel2.SuspendLayout()
        Me.playerControls.SuspendLayout()
        Me.SuspendLayout()
        '
        'sidebar
        '
        Me.sidebar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.sidebar.Location = New System.Drawing.Point(1, 39)
        Me.sidebar.Name = "sidebar"
        Me.sidebar.Size = New System.Drawing.Size(205, 526)
        Me.sidebar.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.Controls.Add(Me.searchBox)
        Me.Panel2.Location = New System.Drawing.Point(0, -1)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(979, 42)
        Me.Panel2.TabIndex = 1
        '
        'searchBox
        '
        Me.searchBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.searchBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.searchBox.Location = New System.Drawing.Point(273, 4)
        Me.searchBox.Name = "searchBox"
        Me.searchBox.Size = New System.Drawing.Size(494, 34)
        Me.searchBox.TabIndex = 0
        '
        'playerControls
        '
        Me.playerControls.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.playerControls.Controls.Add(Me.ModernTrackBar2)
        Me.playerControls.Controls.Add(Me.ModernTrackBar1)
        Me.playerControls.Controls.Add(Me.btnNext)
        Me.playerControls.Controls.Add(Me.btnPrev)
        Me.playerControls.Controls.Add(Me.btnPause)
        Me.playerControls.Controls.Add(Me.btnPlay)
        Me.playerControls.Location = New System.Drawing.Point(1, 562)
        Me.playerControls.Name = "playerControls"
        Me.playerControls.Size = New System.Drawing.Size(981, 85)
        Me.playerControls.TabIndex = 2
        '
        'ModernTrackBar2
        '
        Me.ModernTrackBar2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ModernTrackBar2.Location = New System.Drawing.Point(863, 29)
        Me.ModernTrackBar2.Maximum = 100
        Me.ModernTrackBar2.Minimum = 0
        Me.ModernTrackBar2.Name = "ModernTrackBar2"
        Me.ModernTrackBar2.Size = New System.Drawing.Size(102, 30)
        Me.ModernTrackBar2.TabIndex = 5
        Me.ModernTrackBar2.Value = 0
        '
        'ModernTrackBar1
        '
        Me.ModernTrackBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ModernTrackBar1.Location = New System.Drawing.Point(241, 44)
        Me.ModernTrackBar1.Maximum = 100
        Me.ModernTrackBar1.Minimum = 0
        Me.ModernTrackBar1.Name = "ModernTrackBar1"
        Me.ModernTrackBar1.Size = New System.Drawing.Size(494, 30)
        Me.ModernTrackBar1.TabIndex = 4
        Me.ModernTrackBar1.Value = 0
        '
        'btnNext
        '
        Me.btnNext.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnNext.Location = New System.Drawing.Point(538, 7)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(40, 32)
        Me.btnNext.TabIndex = 3
        Me.btnNext.Text = "⏩"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPrev
        '
        Me.btnPrev.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnPrev.Location = New System.Drawing.Point(400, 6)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(40, 32)
        Me.btnPrev.TabIndex = 2
        Me.btnPrev.Text = "⏪"
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'btnPause
        '
        Me.btnPause.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnPause.Location = New System.Drawing.Point(492, 7)
        Me.btnPause.Name = "btnPause"
        Me.btnPause.Size = New System.Drawing.Size(40, 32)
        Me.btnPause.TabIndex = 1
        Me.btnPause.Text = "| |"
        Me.btnPause.UseVisualStyleBackColor = True
        '
        'btnPlay
        '
        Me.btnPlay.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnPlay.Location = New System.Drawing.Point(446, 7)
        Me.btnPlay.Name = "btnPlay"
        Me.btnPlay.Size = New System.Drawing.Size(40, 32)
        Me.btnPlay.TabIndex = 0
        Me.btnPlay.Text = "▶️"
        Me.btnPlay.UseVisualStyleBackColor = True
        '
        'FlowLayoutMusicPanel
        '
        Me.FlowLayoutMusicPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutMusicPanel.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.FlowLayoutMusicPanel.Location = New System.Drawing.Point(205, 43)
        Me.FlowLayoutMusicPanel.Name = "FlowLayoutMusicPanel"
        Me.FlowLayoutMusicPanel.Size = New System.Drawing.Size(777, 518)
        Me.FlowLayoutMusicPanel.TabIndex = 1
        '
        'MySqlCommand1
        '
        Me.MySqlCommand1.CacheAge = 0
        Me.MySqlCommand1.Connection = Nothing
        Me.MySqlCommand1.EnableCaching = False
        Me.MySqlCommand1.Transaction = Nothing
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(120.0!, 120.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(978, 649)
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
    Friend WithEvents ModernTrackBar1 As ModernTrackbar.ModernTrackBar
    Friend WithEvents ModernTrackBar2 As ModernTrackbar.ModernTrackBar
    Friend WithEvents MySqlCommand1 As MySql.Data.MySqlClient.MySqlCommand
    Friend WithEvents Timer1 As Timer
End Class
