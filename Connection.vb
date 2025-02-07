Imports System.IO
Imports System.Net.Http
Imports System.Windows
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class Connection
    Private userInfoFile As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MusicPlayer", "userinfo")

    Private Sub Connection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Check if the user info file exists
        If My.Computer.FileSystem.FileExists(userInfoFile) Then
            ' Read user info from the file
            Dim userInfo As String = My.Computer.FileSystem.ReadAllText(userInfoFile)
            Dim jsonUserInfo As JObject = JObject.Parse(userInfo)

            ' Display user info or proceed to main form
            MessageBox.Show("User Info: " & jsonUserInfo.ToString())

            ' Redirect to MainForm
            Dim mainForm As New MainForm()
            mainForm.Show()
            Me.Hide() ' Optionally hide the current form
        End If
    End Sub

    Private Async Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim username As String = txtUsername.Text
        Dim password As String = txtPassword.Text
        Dim url As String = "https://musicplayer.creepergreg951.eu/connection.php" ' URL de votre fichier PHP sur le serveur

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

                        ' Redirigez l'utilisateur vers la page principale
                        Dim mainForm As New MainForm()
                        mainForm.Show()
                        Me.Hide() ' Optionally hide the current form
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
        Dim url As String = "https://musicplayer.creepergreg951.eu/connection.php" ' URL de votre fichier PHP sur le serveur

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
