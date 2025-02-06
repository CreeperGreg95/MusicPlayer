Imports System.Data.SqlClient
Imports System.Windows.Forms

Public Class Playlists
    Private connectionString As String = "Data Source=YourServer;Initial Catalog=YourDatabase;Integrated Security=True"
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

    ' Méthode publique pour créer une playlist
    Public Sub CreatePlaylist(playlistName As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                ' Vérifier si la playlist existe déjà
                Dim checkQuery As String = "SELECT COUNT(*) FROM Playlists WHERE PlaylistName = @PlaylistName"
                Dim checkCmd As New SqlCommand(checkQuery, conn)
                checkCmd.Parameters.AddWithValue("@PlaylistName", playlistName)
                Dim result As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                If result > 0 Then
                    MessageBox.Show("Cette playlist existe déjà !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    ' Insérer la playlist dans la base de données
                    Dim query As String = "INSERT INTO Playlists (PlaylistName) VALUES (@PlaylistName)"
                    Dim cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@PlaylistName", playlistName)
                    cmd.ExecuteNonQuery()

                    MessageBox.Show("Playlist créée avec succès!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors de la création de la playlist : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Méthode publique pour créer un dossier de playlists
    Public Sub CreatePlaylistFolder(folderName As String)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                ' Vérifier si le dossier existe déjà
                Dim checkQuery As String = "SELECT COUNT(*) FROM PlaylistFolders WHERE FolderName = @FolderName"
                Dim checkCmd As New SqlCommand(checkQuery, conn)
                checkCmd.Parameters.AddWithValue("@FolderName", folderName)
                Dim result As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                If result > 0 Then
                    MessageBox.Show("Ce dossier existe déjà !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    ' Insérer le dossier de playlists dans la base de données
                    Dim query As String = "INSERT INTO PlaylistFolders (FolderName) VALUES (@FolderName)"
                    Dim cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@FolderName", folderName)
                    cmd.ExecuteNonQuery()

                    MessageBox.Show("Dossier de playlists créé avec succès!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors de la création du dossier : " & ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Gestion des événements pour la création de playlists
    Public Sub CreatePlaylist_Click(sender As Object, e As EventArgs)
        ' Demander un nom pour la nouvelle playlist
        Dim playlistName As String = InputBox("Entrez le nom de la nouvelle playlist :")

        If Not String.IsNullOrEmpty(playlistName) Then
            ' Logique pour créer la playlist
            CreatePlaylist(playlistName)
        Else
            MessageBox.Show("Nom de playlist invalide.")
        End If
    End Sub

    Public Sub CreatePlaylistFolder_Click(sender As Object, e As EventArgs)
        ' Demander un nom pour le dossier de playlists
        Dim folderName As String = InputBox("Entrez le nom du dossier de playlists :")

        If Not String.IsNullOrEmpty(folderName) Then
            ' Logique pour créer le dossier de playlists
            CreatePlaylistFolder(folderName)
        Else
            MessageBox.Show("Nom de dossier invalide.")
        End If
    End Sub
End Class
