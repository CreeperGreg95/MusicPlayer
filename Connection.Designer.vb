Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Connection
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtPasswordCreate = New System.Windows.Forms.TextBox()
        Me.txtUsernameCreate = New System.Windows.Forms.TextBox()
        Me.btnCreateAccount = New System.Windows.Forms.Button()
        Me.txtEmailCreate = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnLogin
        '
        Me.btnLogin.Location = New System.Drawing.Point(481, 214)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(129, 60)
        Me.btnLogin.TabIndex = 0
        Me.btnLogin.Text = "Button1"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(425, 109)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(231, 22)
        Me.txtUsername.TabIndex = 1
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(425, 137)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(231, 22)
        Me.txtPassword.TabIndex = 2
        '
        'txtPasswordCreate
        '
        Me.txtPasswordCreate.Location = New System.Drawing.Point(79, 137)
        Me.txtPasswordCreate.Name = "txtPasswordCreate"
        Me.txtPasswordCreate.Size = New System.Drawing.Size(231, 22)
        Me.txtPasswordCreate.TabIndex = 5
        '
        'txtUsernameCreate
        '
        Me.txtUsernameCreate.Location = New System.Drawing.Point(79, 109)
        Me.txtUsernameCreate.Name = "txtUsernameCreate"
        Me.txtUsernameCreate.Size = New System.Drawing.Size(231, 22)
        Me.txtUsernameCreate.TabIndex = 4
        '
        'btnCreateAccount
        '
        Me.btnCreateAccount.Location = New System.Drawing.Point(135, 214)
        Me.btnCreateAccount.Name = "btnCreateAccount"
        Me.btnCreateAccount.Size = New System.Drawing.Size(129, 60)
        Me.btnCreateAccount.TabIndex = 3
        Me.btnCreateAccount.Text = "Button1"
        Me.btnCreateAccount.UseVisualStyleBackColor = True
        '
        'txtEmailCreate
        '
        Me.txtEmailCreate.Location = New System.Drawing.Point(79, 165)
        Me.txtEmailCreate.Name = "txtEmailCreate"
        Me.txtEmailCreate.Size = New System.Drawing.Size(231, 22)
        Me.txtEmailCreate.TabIndex = 6
        '
        'Connection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.txtEmailCreate)
        Me.Controls.Add(Me.txtPasswordCreate)
        Me.Controls.Add(Me.txtUsernameCreate)
        Me.Controls.Add(Me.btnCreateAccount)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.btnLogin)
        Me.Name = "Connection"
        Me.Text = "Connection"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnLogin As Button
    Friend WithEvents txtUsername As TextBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents txtPasswordCreate As TextBox
    Friend WithEvents txtUsernameCreate As TextBox
    Friend WithEvents btnCreateAccount As Button
    Friend WithEvents txtEmailCreate As TextBox
End Class
