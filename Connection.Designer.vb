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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Connection))
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtPasswordCreate = New System.Windows.Forms.TextBox()
        Me.txtUsernameCreate = New System.Windows.Forms.TextBox()
        Me.btnCreateAccount = New System.Windows.Forms.Button()
        Me.txtEmailCreate = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnLogin
        '
        Me.btnLogin.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnLogin.Location = New System.Drawing.Point(520, 260)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(174, 72)
        Me.btnLogin.TabIndex = 0
        Me.btnLogin.Text = "Login"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'txtUsername
        '
        Me.txtUsername.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtUsername.Location = New System.Drawing.Point(520, 173)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(231, 22)
        Me.txtUsername.TabIndex = 1
        '
        'txtPassword
        '
        Me.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtPassword.Location = New System.Drawing.Point(520, 206)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(231, 22)
        Me.txtPassword.TabIndex = 2
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'txtPasswordCreate
        '
        Me.txtPasswordCreate.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtPasswordCreate.Location = New System.Drawing.Point(100, 187)
        Me.txtPasswordCreate.Name = "txtPasswordCreate"
        Me.txtPasswordCreate.Size = New System.Drawing.Size(231, 22)
        Me.txtPasswordCreate.TabIndex = 5
        Me.txtPasswordCreate.UseSystemPasswordChar = True
        '
        'txtUsernameCreate
        '
        Me.txtUsernameCreate.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtUsernameCreate.Location = New System.Drawing.Point(100, 159)
        Me.txtUsernameCreate.Name = "txtUsernameCreate"
        Me.txtUsernameCreate.Size = New System.Drawing.Size(231, 22)
        Me.txtUsernameCreate.TabIndex = 4
        '
        'btnCreateAccount
        '
        Me.btnCreateAccount.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCreateAccount.Location = New System.Drawing.Point(63, 260)
        Me.btnCreateAccount.Name = "btnCreateAccount"
        Me.btnCreateAccount.Size = New System.Drawing.Size(222, 72)
        Me.btnCreateAccount.TabIndex = 3
        Me.btnCreateAccount.Text = "Create your account"
        Me.btnCreateAccount.UseVisualStyleBackColor = True
        '
        'txtEmailCreate
        '
        Me.txtEmailCreate.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtEmailCreate.Location = New System.Drawing.Point(100, 215)
        Me.txtEmailCreate.Name = "txtEmailCreate"
        Me.txtEmailCreate.Size = New System.Drawing.Size(231, 22)
        Me.txtEmailCreate.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 162)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 17)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Username"
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 190)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 17)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Password"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(41, 218)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 17)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Email"
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(434, 209)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 17)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Password"
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(430, 173)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 17)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Username"
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(104, 83)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(123, 32)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Sign Up"
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(536, 83)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(110, 32)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Sign In"
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(16, 432)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(766, 68)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = resources.GetString("Label8.Text")
        '
        'Connection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(810, 504)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
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
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
End Class
