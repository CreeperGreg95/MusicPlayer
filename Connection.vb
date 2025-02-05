Imports System.Net.Http
Imports System.Windows
Imports Newtonsoft.Json.Linq


Public Class Connection
    Private Async Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim username As String = txtUsername.Text
        Dim password As String = txtPassword.Text
        Dim url As String = "http://votre-serveur.com/connection.php" ' URL de votre fichier PHP sur le serveur

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
                Dim jsonResponse As JObject = JObject.Parse(result)

                ' Si l'utilisateur est trouvé
                If jsonResponse("status").ToString() = "success" Then
                    MessageBox.Show("Connexion réussie!")
                    ' Redirigez l'utilisateur vers la page principale
                Else
                    MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect")
                End If
            End If
        End Using
    End Sub

#Disable Warning BC30506 ' La clause Handles requiert une variable WithEvents définie dans le type conteneur ou l'un de ses types de base.
    Private Async Sub btnCreateAccount_Click(sender As Object, e As EventArgs) Handles btnCreateAccount.Click
#Enable Warning BC30506 ' La clause Handles requiert une variable WithEvents définie dans le type conteneur ou l'un de ses types de base.
        Dim username As String = txtUsernameCreate.Text
        Dim password As String = txtPasswordCreate.Text
        Dim email As String = txtEmailCreate.Text
        Dim url As String = "http://votre-serveur.com/connection.php" ' URL de votre fichier PHP sur le serveur

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
                Dim jsonResponse As JObject = JObject.Parse(result)

                If jsonResponse("status").ToString() = "success" Then
                    MessageBox.Show("Compte créé avec succès!")
                Else
                    MessageBox.Show("Erreur lors de la création du compte.")
                End If
            End If
        End Using
    End Sub

    Private Sub Connection_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
