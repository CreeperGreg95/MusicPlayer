Imports System.Windows.Forms
Imports System.Drawing
Imports MusicPlayer

Public Class SongPlaceholder
    Private mainPanel As Panel
    Private musicDataFetcher As GetMusicData

    ' Constructeur qui reçoit le panel où afficher la liste
    Public Sub New(panel As Panel)
        mainPanel = panel
        musicDataFetcher = New GetMusicData()
    End Sub

    ' Charge et affiche la liste des musiques
    Public Sub LoadMusicList()
        mainPanel.Controls.Clear() ' On vide le panel avant d'ajouter les éléments
        Dim musics As List(Of MusicData) = musicDataFetcher.GetAllMusics()

        Dim yOffset As Integer = 10 ' Position Y initiale

        For Each music As MusicData In musics
            Dim musicPanel As New Panel With {
                .Size = New Size(mainPanel.Width - 20, 50),
                .BackColor = Color.LightGray,
                .Margin = New Padding(5),
                .Location = New Point(10, yOffset)
            }

            Dim lblTitle As New Label With {
                .Text = music.Title & " - " & music.Artist,
                .AutoSize = True,
                .Location = New Point(10, 15)
            }

            Dim lblPlays As New Label With {
                .Text = music.TotalPlays & " / " & music.UserPlays,
                .AutoSize = True,
                .Location = New Point(musicPanel.Width - 100, 15)
            }

            Dim btnOptions As New Button With {
                .Text = "...",
                .Size = New Size(30, 30),
                .Location = New Point(musicPanel.Width - 40, 10)
            }

            musicPanel.Controls.Add(lblTitle)
            musicPanel.Controls.Add(lblPlays)
            musicPanel.Controls.Add(btnOptions)
            mainPanel.Controls.Add(musicPanel)

            yOffset += 60 ' Décalage vertical pour éviter le chevauchement
        Next
    End Sub
End Class
