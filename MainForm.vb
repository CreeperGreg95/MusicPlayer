Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Http
Imports NAudio.Wave
Imports System.Speech.Synthesis
Imports System.Windows.Forms

Public Class MainForm
    Inherits Form

    Private sidebar As Panel
    Private btnSettings As Button
    Private mainPanel As Panel
    Private playerControls As Panel
    Private btnPlay As Button
    Private btnPause As Button
    Private btnNext As Button
    Private btnPrev As Button
    Private searchBox As TextBox
    Private resultsList As ListBox
    Private waveOut As WaveOutEvent
    Private mediaReader As MediaFoundationReader
    Private currentStream As Stream
    Private karaokeSynth As SpeechSynthesizer

    Private offlineMode As Boolean = False
    Private contextMenu As ContextMenuStrip
    Private connectionString As String = "Data Source=YourServer;Initial Catalog=YourDatabase;Integrated Security=True"

    Public Sub New()
        InitializeComponent()

        Me.Text = "Music Player"
        Me.Size = New Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Création du panel latéral
        sidebar = New Panel() With {
            .Size = New Size(200, Me.Height),
            .BackColor = Color.DarkGray,
            .Dock = DockStyle.Left
        }
        Me.Controls.Add(sidebar)

        ' Création du menu contextuel
        contextMenu = New ContextMenuStrip()
        Dim createPlaylistItem As New ToolStripMenuItem("Créer Playlist")
        Dim createPlaylistFolderItem As New ToolStripMenuItem("Créer Dossier de Playlists")

        ' Ajouter des gestionnaires d'événements pour les options
        AddHandler createPlaylistItem.Click, AddressOf CreatePlaylist_Click
        AddHandler createPlaylistFolderItem.Click, AddressOf CreatePlaylistFolder_Click

        contextMenu.Items.Add(createPlaylistItem)
        contextMenu.Items.Add(createPlaylistFolderItem)

        ' Création du bouton Settings
        btnSettings = New Button() With {.Text = "Settings", .Dock = DockStyle.Top}
        sidebar.Controls.Add(btnSettings)

        ' Liste des playlists (déplacée ici)
        resultsList = New ListBox() With {
            .Dock = DockStyle.Fill
        }
        AddHandler resultsList.DoubleClick, AddressOf ResultsList_DoubleClick
        AddHandler resultsList.MouseDown, AddressOf ResultsList_MouseDown ' Clic droit sur la liste
        sidebar.Controls.Add(resultsList)

        ' Création du panel principal
        mainPanel = New Panel() With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White
        }
        Me.Controls.Add(mainPanel)

        ' Création de la barre de recherche
        searchBox = New TextBox() With {
            .Dock = DockStyle.Top,
            .Width = Me.ClientSize.Width - sidebar.Width ' Ajuster la largeur
        }
        AddHandler searchBox.TextChanged, AddressOf SearchBox_TextChanged
        mainPanel.Controls.Add(searchBox)

        ' Panel des contrôles du player
        playerControls = New Panel() With {
            .Size = New Size(Me.Width, 50),
            .Dock = DockStyle.Bottom,
            .BackColor = Color.LightGray
        }
        Me.Controls.Add(playerControls)

        btnPrev = New Button() With {.Text = "⏮", .Dock = DockStyle.Left}
        btnPlay = New Button() With {.Text = "▶", .Dock = DockStyle.Left}
        btnPause = New Button() With {.Text = "⏸", .Dock = DockStyle.Left}
        btnNext = New Button() With {.Text = "⏭", .Dock = DockStyle.Left}

        AddHandler btnPlay.Click, AddressOf btnPlay_Click
        AddHandler btnPause.Click, AddressOf btnPause_Click

        playerControls.Controls.AddRange(New Control() {btnPrev, btnPlay, btnPause, btnNext})

        ' Initialisation des composants audio
        waveOut = New WaveOutEvent()
        karaokeSynth = New SpeechSynthesizer()

        ' Événements de clavier
        AddHandler Me.KeyDown, AddressOf Form1_KeyDown

        ' Charger les playlists à l'ouverture de la fenêtre
        LoadPlaylists()
    End Sub

    ' Charger les playlists de la base de données
    Private Sub LoadPlaylists()
        resultsList.Items.Clear()

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT PlaylistName FROM Playlists"
                Dim cmd As New SqlCommand(query, conn)
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    resultsList.Items.Add(reader("PlaylistName").ToString())
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur de connexion à la base de données: " & ex.Message)
        End Try
    End Sub

    ' Gestion du clic droit sur la liste des playlists
    Private Sub ResultsList_MouseDown(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            ' Vérifier si le clic droit est bien sur la liste des playlists
            If sender Is resultsList Then
                ' Afficher le menu contextuel à la position du curseur
                contextMenu.Show(resultsList, e.Location)
            End If
        End If
    End Sub

    ' Méthodes liées à la création de playlists
    Private Sub CreatePlaylist_Click(sender As Object, e As EventArgs)
        ' Demander un nom pour la nouvelle playlist
        Dim playlistName As String = InputBox("Entrez le nom de la nouvelle playlist :")

        If Not String.IsNullOrEmpty(playlistName) Then
            ' Appeler la méthode pour créer une playlist
            MessageBox.Show("Playlist '" & playlistName & "' créée avec succès!")
            ' Recharger les playlists après création
            LoadPlaylists()
        Else
            MessageBox.Show("Nom de playlist invalide.")
        End If
    End Sub

    Private Sub CreatePlaylistFolder_Click(sender As Object, e As EventArgs)
        ' Demander un nom pour le dossier de playlists
        Dim folderName As String = InputBox("Entrez le nom du dossier de playlists :")

        If Not String.IsNullOrEmpty(folderName) Then
            ' Appeler la méthode pour créer un dossier de playlists
            MessageBox.Show("Dossier de playlists '" & folderName & "' créé avec succès!")
        Else
            MessageBox.Show("Nom de dossier invalide.")
        End If
    End Sub

    ' Recherche dans la base de données
    Private Sub SearchBox_TextChanged(sender As Object, e As EventArgs)
        Dim searchQuery As String = searchBox.Text
        If searchQuery.Length >= 3 Then
            LoadSearchResults(searchQuery)
        Else
            resultsList.Items.Clear()
        End If
    End Sub

    ' Chargement des résultats de recherche
    Private Sub LoadSearchResults(searchQuery As String)
        resultsList.Items.Clear()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT SongName FROM Songs WHERE SongName LIKE @SearchQuery"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SearchQuery", "%" & searchQuery & "%")
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    resultsList.Items.Add(reader("SongName").ToString())
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur de connexion à la base de données: " & ex.Message)
        End Try
    End Sub

    ' Lecture du double clic sur un résultat de recherche
    Private Sub ResultsList_DoubleClick(sender As Object, e As EventArgs)
        If resultsList.SelectedItem IsNot Nothing Then
            Dim selectedSong As String = resultsList.SelectedItem.ToString()
            PlaySong(selectedSong)
        End If
    End Sub

    ' Lecture d'une chanson
    Private Sub PlaySong(songName As String)
        Dim audioUrl As String = GetSongUrl(songName)
        If Not String.IsNullOrEmpty(audioUrl) Then
            PlayAudioFromUrl(audioUrl)
        End If
    End Sub

    ' Récupération de l'URL de la chanson
    Private Function GetSongUrl(songName As String) As String
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT SongUrl FROM Songs WHERE SongName = @SongName"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SongName", songName)
                Dim url As Object = cmd.ExecuteScalar()
                If url IsNot Nothing Then
                    Return url.ToString()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur de récupération du fichier: " & ex.Message)
        End Try
        Return String.Empty
    End Function

    ' Lecture du fichier audio
    Private Async Sub PlayAudioFromUrl(audioUrl As String)
        Try
            StopPlayback()

            Dim client As New HttpClient()
            currentStream = Await client.GetStreamAsync(audioUrl)

            mediaReader = New MediaFoundationReader(audioUrl)
            waveOut.Init(mediaReader)
            waveOut.Play()
        Catch ex As Exception
            MessageBox.Show("Erreur de lecture audio: " & ex.Message)
        End Try
    End Sub

    ' Arrêter la lecture
    Private Sub StopPlayback()
        If waveOut IsNot Nothing Then
            waveOut.Stop()
        End If
        If mediaReader IsNot Nothing Then
            mediaReader.Dispose()
        End If
    End Sub

    ' Gestion des événements de boutons de lecture
    Private Sub btnPlay_Click(sender As Object, e As EventArgs)
        waveOut.Play()
    End Sub

    Private Sub btnPause_Click(sender As Object, e As EventArgs)
        waveOut.Pause()
    End Sub

    ' Gestion des événements clavier
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Space Then
            ' Interrupteur play/pause avec la barre d'espace
            If waveOut.PlaybackState = PlaybackState.Playing Then
                waveOut.Pause()
            Else
                waveOut.Play()
            End If
        End If
    End Sub
End Class
