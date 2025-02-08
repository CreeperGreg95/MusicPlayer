Imports MySql.Data.MySqlClient
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

    Private currentUserID As Integer = 1 ' ID temporaire en attendant la gestion des comptes
    Private offlineMode As Boolean = False
    Private contextMenu As ContextMenuStrip
    Private connectionString As String = "..."


    Public Sub New()
        InitializeComponent()
        Me.Text = "Music Player"
        Me.Size = New Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Initialisation de l'interface
        InitializeUI()

        ' Initialisation des composants audio
        waveOut = New WaveOutEvent()
        karaokeSynth = New SpeechSynthesizer()

        ' Charger les playlists
        LoadPlaylists()
    End Sub

    Private Sub InitializeUI()
        ' Panel latéral
        sidebar = New Panel() With {.Size = New Size(200, Me.Height), .BackColor = Color.DarkGray, .Dock = DockStyle.Left}
        Me.Controls.Add(sidebar)

        ' Bouton Settings
        btnSettings = New Button() With {.Text = "Settings", .Dock = DockStyle.Top}
        sidebar.Controls.Add(btnSettings)

        ' Liste des playlists
        resultsList = New ListBox() With {.Dock = DockStyle.Fill}
        AddHandler resultsList.DoubleClick, AddressOf ResultsList_DoubleClick
        sidebar.Controls.Add(resultsList)

        ' Panel principal
        mainPanel = New Panel() With {.Dock = DockStyle.Fill, .BackColor = Color.White}
        Me.Controls.Add(mainPanel)

        ' Barre de recherche
        searchBox = New TextBox() With {.Dock = DockStyle.Top}
        AddHandler searchBox.TextChanged, AddressOf SearchBox_TextChanged
        mainPanel.Controls.Add(searchBox)

        ' Contrôles du lecteur
        playerControls = New Panel() With {.Size = New Size(Me.Width, 50), .Dock = DockStyle.Bottom, .BackColor = Color.LightGray}
        Me.Controls.Add(playerControls)

        btnPrev = New Button() With {.Text = "⏮", .Dock = DockStyle.Left}
        btnPlay = New Button() With {.Text = "▶", .Dock = DockStyle.Left}
        btnPause = New Button() With {.Text = "⏸", .Dock = DockStyle.Left}
        btnNext = New Button() With {.Text = "⏭", .Dock = DockStyle.Left}

        AddHandler btnPlay.Click, AddressOf btnPlay_Click
        AddHandler btnPause.Click, AddressOf btnPause_Click

        playerControls.Controls.AddRange(New Control() {btnPrev, btnPlay, btnPause, btnNext})
    End Sub

    ' Charger les playlists de l'utilisateur
    Private Sub LoadPlaylists()
        resultsList.Items.Clear()
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT PlaylistName FROM Playlists WHERE UserID = @UserID"
                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", currentUserID)
                Dim reader As MySqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    resultsList.Items.Add(reader("PlaylistName").ToString())
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur de connexion à la base de données: " & ex.Message)
        End Try
    End Sub

    ' Lecture d'une chanson
    Private Sub PlaySong(songName As String)
        Dim audioUrl As String = GetSongUrl(songName)
        If Not String.IsNullOrEmpty(audioUrl) Then
            PlayAudioFromUrl(audioUrl)
        End If
    End Sub

    Private Function GetSongUrl(songName As String) As String
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT SongUrl FROM Songs WHERE SongName = @SongName"
                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SongName", songName)
                Dim url As Object = cmd.ExecuteScalar()
                If url IsNot Nothing Then Return url.ToString()
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
        If waveOut IsNot Nothing Then waveOut.Stop()
        If mediaReader IsNot Nothing Then mediaReader.Dispose()
    End Sub

    Private Sub btnPlay_Click(sender As Object, e As EventArgs)
        waveOut.Play()
    End Sub

    Private Sub btnPause_Click(sender As Object, e As EventArgs)
        waveOut.Pause()
    End Sub

    ' Recherche dans la base de données
    Private Sub SearchBox_TextChanged(sender As Object, e As EventArgs)
        If searchBox.Text.Length >= 3 Then
            LoadSearchResults(searchBox.Text)
        Else
            resultsList.Items.Clear()
        End If
    End Sub

    Private Sub LoadSearchResults(searchQuery As String)
        resultsList.Items.Clear()
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT SongName FROM Songs WHERE SongName LIKE @SearchQuery"
                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SearchQuery", "%" & searchQuery & "%")
                Dim reader As MySqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    resultsList.Items.Add(reader("SongName").ToString())
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur de connexion à la base de données: " & ex.Message)
        End Try
    End Sub

    ' Lecture d'un titre via double-clic
    Private Sub ResultsList_DoubleClick(sender As Object, e As EventArgs)
        If resultsList.SelectedItem IsNot Nothing Then
            PlaySong(resultsList.SelectedItem.ToString())
        End If
    End Sub
End Class
