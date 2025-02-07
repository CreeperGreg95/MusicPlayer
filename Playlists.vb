Imports System.Data.SqlClient
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Web.Script.Serialization

Public Class Playlists
    Private serverUrl As String = "https://musicplayer.creepergreg951.eu/playlist_verifier.php"
    Private contextMenu As ContextMenuStrip
    Private playlistPanel As Panel

    ' Constructeur
    Public Sub New(playlistPanel As Panel)
        Me.playlistPanel = playlistPanel
        contextMenu = New ContextMenuStrip()

        ' Créer les éléments du menu
        Dim createPlaylistItem As New ToolStripMenuItem("Créer Playlist")
        Dim createPlaylistFolderItem As New ToolStripMenuItem("Créer Dossier de Playlists")

        ' Ajouter des gestionnaires d'événements
        AddHandler createPlaylistItem.Click, AddressOf CreatePlaylist_Click
        AddHandler createPlaylistFolderItem.Click, AddressOf CreatePlaylistFolder_Click

        ' Ajouter les éléments au menu contextuel
        contextMenu.Items.Add(createPlaylistItem)
        contextMenu.Items.Add(createPlaylistFolderItem)

        ' Assigner le menu contextuel au panel
        playlistPanel.ContextMenuStrip = contextMenu
    End Sub

    ' Fonction pour envoyer une requête POST au serveur PHP
    Private Async Function SendPostRequest(playlistName As String, userID As Integer, type As String) As Task(Of String)
        Try
            Using client As New HttpClient()
                Dim data As New Dictionary(Of String, String) From {
                    {"playlistName", playlistName},
                    {"userID", userID.ToString()},
                    {"type", type}
                }
                Dim content As New FormUrlEncodedContent(data)
                Dim response As HttpResponseMessage = Await client.PostAsync(serverUrl, content)
                Return Await response.Content.ReadAsStringAsync()
            End Using
        Catch ex As Exception
            Return "Erreur: " & ex.Message
        End Try
    End Function

    ' Méthode pour vérifier et créer une playlist via PHP
    Public Async Function CheckAndCreatePlaylist(playlistName As String, userID As Integer, type As String) As Task
        If String.IsNullOrEmpty(playlistName) Then
            MessageBox.Show("Nom invalide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim jsonResponse As String = Await SendPostRequest(playlistName, userID, type)

        Try
            Dim result As Dictionary(Of String, Object) = New JavaScriptSerializer().Deserialize(Of Dictionary(Of String, Object))(jsonResponse)
            If result.ContainsKey("status") AndAlso result("status").ToString() = "success" Then
                MessageBox.Show(result("message").ToString(), "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show(result("message").ToString(), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Réponse invalide du serveur.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    ' Création d'une playlist
    Public Async Sub CreatePlaylist(playlistName As String, userID As Integer)
        Await CheckAndCreatePlaylist(playlistName, userID, "playlist")
    End Sub

    ' Création d'un dossier de playlists
    Public Async Sub CreatePlaylistFolder(folderName As String, userID As Integer)
        Await CheckAndCreatePlaylist(folderName, userID, "folder")
    End Sub

    ' Gestion des événements pour la création de playlists
    Public Sub CreatePlaylist_Click(sender As Object, e As EventArgs)
        Dim playlistName As String = InputBox("Entrez le nom de la nouvelle playlist :")
        Dim userID As Integer = 1 ' Remplacez par l'ID réel de l'utilisateur connecté
        CreatePlaylist(playlistName, userID)
    End Sub

    ' Gestion des événements pour la création de dossiers
    Public Sub CreatePlaylistFolder_Click(sender As Object, e As EventArgs)
        Dim folderName As String = InputBox("Entrez le nom du dossier de playlists :")
        Dim userID As Integer = 1 ' Remplacez par l'ID réel de l'utilisateur connecté
        CreatePlaylistFolder(folderName, userID)
    End Sub
End Class
