Imports System.IO
Imports Newtonsoft.Json.Linq

Public Class ConnectionAccount
    Private userInfoFile As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MusicPlayer", "userinfo")

    Public Function GetUserIDFromJson() As Integer
        ' Charger le fichier JSON contenant l'ID de l'utilisateur depuis ApplicationData
        If File.Exists(userInfoFile) Then
            ' Lire le contenu du fichier JSON
            Dim jsonData As String = File.ReadAllText(userInfoFile)

            ' Utiliser Newtonsoft.Json pour analyser le contenu JSON
            Dim jsonObject As JObject = JObject.Parse(jsonData)

            ' Extraire l'ID de l'utilisateur (UserID)
            Dim userID As Integer = jsonObject("UserID").Value(Of Integer)()

            ' Retourner l'ID de l'utilisateur
            Return userID
        Else
            ' Si le fichier JSON n'existe pas, afficher une erreur ou retourner 0
            MessageBox.Show("Le fichier userinfo est introuvable.")
            Return 0 ' Valeur par défaut si le fichier n'est pas trouvé
        End If
    End Function
End Class
