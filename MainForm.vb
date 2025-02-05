Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Http
Imports NAudio.Wave
Imports System.Speech.Synthesis
Imports System.Windows.Forms

Public Class MainForm
    Inherits Form

    Private sidebar As Panel
    Private btnLibrary As Button
    Private btnPlaylists As Button
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
#Disable Warning BC40004 ' variable 'contextMenu' est en conflit avec property 'contextMenu' dans le class 'Control' de base et doit être déclaré 'Shadows'.
    Private contextMenu As ContextMenuStrip
#Enable Warning BC40004 ' variable 'contextMenu' est en conflit avec property 'contextMenu' dans le class 'Control' de base et doit être déclaré 'Shadows'.

    Private connectionString As String = "Data Source=YourServer;Initial Catalog=YourDatabase;Integrated Security=True"

    Public Sub New()
#Disable Warning BC30451 ' 'InitializeComponent' n'est pas déclaré. Il peut être inaccessible en raison de son niveau de protection.
        InitializeComponent()
#Enable Warning BC30451 ' 'InitializeComponent' n'est pas déclaré. Il peut être inaccessible en raison de son niveau de protection.

        Me.Text = "Music Player"
        Me.Size = New Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen

        sidebar = New Panel() With {
            .Size = New Size(200, Me.Height),
            .BackColor = Color.DarkGray,
            .Dock = DockStyle.Left
        }
        Me.Controls.Add(sidebar)

        ' Ajouter le menu contextuel à la sidebar
        contextMenu = New ContextMenuStrip()
        Dim createPlaylistItem As New ToolStripMenuItem("Créer Playlist")
        Dim createPlaylistFolderItem As New ToolStripMenuItem("Créer Dossier de Playlists")

        ' Ajouter des événements pour les éléments du menu
        AddHandler createPlaylistItem.Click, AddressOf CreatePlaylist_Click
        AddHandler createPlaylistFolderItem.Click, AddressOf CreatePlaylistFolder_Click

        ' Ajouter les éléments au menu contextuel
        contextMenu.Items.Add(createPlaylistItem)
        contextMenu.Items.Add(createPlaylistFolderItem)

        ' Attacher le menu contextuel à la sidebar
        sidebar.ContextMenuStrip = contextMenu

        btnLibrary = New Button() With {.Text = "Library", .Dock = DockStyle.Top}
        btnPlaylists = New Button() With {.Text = "Playlists", .Dock = DockStyle.Top}
        btnSettings = New Button() With {.Text = "Settings", .Dock = DockStyle.Top}
        sidebar.Controls.AddRange(New Control() {btnLibrary, btnPlaylists, btnSettings})

        mainPanel = New Panel() With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White
        }
        Me.Controls.Add(mainPanel)

        searchBox = New TextBox() With {
            .Dock = DockStyle.Top
        }
        AddHandler searchBox.TextChanged, AddressOf SearchBox_TextChanged
        mainPanel.Controls.Add(searchBox)

        resultsList = New ListBox() With {
            .Dock = DockStyle.Fill
        }
        AddHandler resultsList.DoubleClick, AddressOf ResultsList_DoubleClick
        mainPanel.Controls.Add(resultsList)

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

        waveOut = New WaveOutEvent()
        karaokeSynth = New SpeechSynthesizer()

        AddHandler Me.KeyDown, AddressOf Form1_KeyDown
    End Sub

    ' Gestion du clic sur "Créer Playlist"
    Private Sub CreatePlaylist_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Création d'une nouvelle playlist")
        ' Logique pour créer une playlist
    End Sub

    ' Gestion du clic sur "Créer Dossier de Playlists"
    Private Sub CreatePlaylistFolder_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Création d'un nouveau dossier de playlists")
        ' Logique pour créer un dossier de playlists
    End Sub

    Private Sub SearchBox_TextChanged(sender As Object, e As EventArgs)
        Dim searchQuery As String = searchBox.Text
        If searchQuery.Length >= 3 Then
            LoadSearchResults(searchQuery)
        Else
            resultsList.Items.Clear()
        End If
    End Sub

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

    Private Sub ResultsList_DoubleClick(sender As Object, e As EventArgs)
        If resultsList.SelectedItem IsNot Nothing Then
            Dim selectedSong As String = resultsList.SelectedItem.ToString()
            PlaySong(selectedSong)
        End If
    End Sub

    Private Sub PlaySong(songName As String)
        Dim audioUrl As String = GetSongUrl(songName)
        If Not String.IsNullOrEmpty(audioUrl) Then
            PlayAudioFromUrl(audioUrl)
        End If
    End Sub

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

    Private Sub StopPlayback()
        If waveOut IsNot Nothing Then
            waveOut.Stop()
        End If
        If mediaReader IsNot Nothing Then
            mediaReader.Dispose()
        End If
        If currentStream IsNot Nothing Then
            currentStream.Dispose()
        End If
    End Sub

    Private Sub btnPlay_Click(sender As Object, e As EventArgs)
        If waveOut IsNot Nothing Then
            waveOut.Play()
        End If
    End Sub

    Private Sub btnPause_Click(sender As Object, e As EventArgs)
        If waveOut IsNot Nothing Then
            waveOut.Pause()
        End If
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.Space
                btnPlay.PerformClick()
            Case Keys.P
                btnPause.PerformClick()
        End Select
    End Sub

    Private Sub ActivateKaraokeMode(text As String)
        karaokeSynth.SpeakAsyncCancelAll()
        karaokeSynth.SpeakAsync(text)
    End Sub
End Class
