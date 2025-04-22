Imports System.IO
Imports System.Net.Http
Imports System.Windows
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Drawing

Public Class NewConnection
    Private userInfoFile As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MusicPlayer", "userinfo")

    ' Liste des valeurs interdites (modifiable)
    Private invalidValues As List(Of String) = New List(Of String) From {
    " ", vbTab, vbCrLf ' Espace, tabulation et retour à la ligne
}

    ' Méthode pour vérifier si un champ contient une valeur invalide
    Private Function IsValidInput(input As String) As Boolean
        ' Enlever les espaces avant et après la chaîne
        input = input.Trim()

        ' Afficher la chaîne pour déboguer
        Debug.WriteLine("Validation de l'input: '" & input & "'")

        ' Vérifie si la chaîne est vide après le trim
        If String.IsNullOrEmpty(input) Then
            Debug.WriteLine("Champ invalide: vide ou nul.")
            Return False
        End If

        ' Vérifier les autres valeurs interdites comme des espaces seuls ou des tabulations
        For Each invalidValue In invalidValues
            ' Afficher chaque valeur interdite pour débogage
            Debug.WriteLine("Valeur interdite: '" & invalidValue & "'")

            ' Si la chaîne contient un espace ou un caractère interdit
            If input.Contains(invalidValue) Then
                Debug.WriteLine("Champ invalide: contient des espaces ou des caractères interdits.")
                Return False
            End If
        Next

        ' Si tout est bon, on considère que l'input est valide
        Return True
    End Function

    Private Sub NewConnection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Assurer que la fenêtre est à l'état normal et ne peut pas être redimensionnée
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.WindowState = FormWindowState.Normal
        Me.Size = New Drawing.Size(789, 485) ' Ajuste cette taille selon tes besoins
        Me.StartPosition = FormStartPosition.CenterScreen ' Centre la fenêtre sur l'écran
        Me.FormBorderStyle = FormBorderStyle.FixedDialog ' Empêche le redimensionnement de la fenêtre

        ' Vérifie si le fichier contenant les infos utilisateur existe
        If My.Computer.FileSystem.FileExists(userInfoFile) Then
            ' Lis les infos utilisateur depuis le fichier
            Dim userInfo As String = My.Computer.FileSystem.ReadAllText(userInfoFile)
            Dim jsonUserInfo As JObject = JObject.Parse(userInfo)

            ' Redirige vers MainForm si les infos existent
            If Not MainForm.Visible Then
                MainForm.Show()
            End If
            Me.Hide() ' Masque plutôt que de fermer la fenêtre de connexion après le succès
        Else
            ' Sinon, reste sur la page de connexion (ne fait rien ici)
            Me.Show()
        End If
    End Sub


    Private Async Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim username As String = txtUsername.Text
        Dim password As String = txtPassword.Text

        ' Affichage des valeurs des champs pour débogage
        Debug.WriteLine("Valeur du champ Username: '" & username & "'")
        Debug.WriteLine("Valeur du champ Password: '" & password & "'")

        ' Vérification des champs de connexion
        If Not IsValidInput(username) OrElse Not IsValidInput(password) Then
            MessageBox.Show("Les champs de texte ne peuvent pas être vides ou contenir des espaces.")
            Return
        End If

        ' Assurer que la fenêtre ne soit pas minimisée ou redimensionnée
        If Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
        End If

        Dim url As String = "https://musicplayer.creepergreg951.eu/connection.php" ' URL de ton fichier PHP sur le serveur

        ' Créez un client HTTP pour envoyer les données au serveur
        Using client As New HttpClient()
            Dim parameters As New Dictionary(Of String, String) From {
                {"action", "login"},
                {"username", username},
                {"password", password}
            }

            Dim content As New FormUrlEncodedContent(parameters)
            Dim response As HttpResponseMessage = Await client.PostAsync(url, content)

            ' Si la réponse est réussie, vérifier la connexion
            If response.IsSuccessStatusCode Then
                Dim result As String = Await response.Content.ReadAsStringAsync()

                ' Log the response for debugging
                MessageBox.Show("Server Response: " & result)

                Try
                    Dim jsonResponse As JObject = JObject.Parse(result)

                    ' Si l'utilisateur est trouvé
                    If jsonResponse("status").ToString() = "success" Then
                        MessageBox.Show("Connexion réussie!")

                        ' Save user info to the file
                        Dim userInfo As JObject = jsonResponse("user")

                        ' Ensure the directory exists
                        Dim directory As String = Path.GetDirectoryName(userInfoFile)
                        If Not My.Computer.FileSystem.DirectoryExists(directory) Then
                            My.Computer.FileSystem.CreateDirectory(directory)
                        End If

                        ' Write user info to the file
                        My.Computer.FileSystem.WriteAllText(userInfoFile, userInfo.ToString(), False)

                        ' Avant de fermer, vérifie si MainForm est déjà ouvert
                        If Not MainForm.Visible Then
                            MainForm.Show()
                        End If

                        ' Masquer la fenêtre de connexion plutôt que de la fermer
                        Me.Hide()
                    Else
                        MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect")
                    End If
                Catch ex As JsonReaderException
                    MessageBox.Show("Invalid JSON response from server.")
                End Try
            Else
                MessageBox.Show("Failed to connect to server.")
            End If
        End Using
    End Sub

    Private Async Sub btnCreateAccount_Click(sender As Object, e As EventArgs) Handles btnCreateAccount.Click
        Dim username As String = txtUsernameCreate.Text
        Dim password As String = txtPasswordCreate.Text
        Dim email As String = txtEmailCreate.Text

        ' Affichage des valeurs des champs pour débogage
        Debug.WriteLine("Valeur du champ UsernameCreate: '" & username & "'")
        Debug.WriteLine("Valeur du champ PasswordCreate: '" & password & "'")
        Debug.WriteLine("Valeur du champ EmailCreate: '" & email & "'")

        ' Vérification des champs de création de compte
        If Not IsValidInput(username) OrElse Not IsValidInput(password) OrElse Not IsValidInput(email) Then
            MessageBox.Show("Les champs de texte ne peuvent pas être vides ou contenir des espaces.")
            Return
        End If

        Dim url As String = "https://musicplayer.creepergreg951.eu/connection.php" ' URL de ton fichier PHP sur le serveur

        ' Créez un client HTTP pour envoyer les données au serveur
        Using client As New HttpClient()
            Dim parameters As New Dictionary(Of String, String) From {
                {"action", "create"},
                {"username", username},
                {"password", password},
                {"email", email}
            }

            Dim content As New FormUrlEncodedContent(parameters)
            Dim response As HttpResponseMessage = Await client.PostAsync(url, content)

            ' Si la réponse est réussie, informer l'utilisateur
            If response.IsSuccessStatusCode Then
                Dim result As String = Await response.Content.ReadAsStringAsync()

                ' Log the response for debugging
                MessageBox.Show("Server Response: " & result)

                Try
                    Dim jsonResponse As JObject = JObject.Parse(result)

                    If jsonResponse("status").ToString() = "success" Then
                        MessageBox.Show("Compte créé avec succès!")
                    Else
                        MessageBox.Show("Erreur lors de la création du compte.")
                    End If
                Catch ex As JsonReaderException
                    MessageBox.Show("Invalid JSON response from server.")
                End Try
            Else
                MessageBox.Show("Failed to connect to server.")
            End If
        End Using
    End Sub
End Class
