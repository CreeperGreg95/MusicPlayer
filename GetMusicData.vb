Imports System.Data.SqlClient

Public Class GetMusicData
    Private connectionString As String = "Server=TON_SERVEUR;Database=TA_BASE;User Id=TON_UTILISATEUR;Password=TON_MOT_DE_PASSE;"

    ' Fonction pour récupérer toutes les musiques disponibles
    Public Function GetAllMusics() As List(Of MusicData)
        Dim musics As New List(Of MusicData)

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT Title, Artist, TotalPlays, UserPlays FROM Musics"
                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            musics.Add(New MusicData With {
                                .Title = reader("Title").ToString(),
                                .Artist = reader("Artist").ToString(),
                                .TotalPlays = Convert.ToInt32(reader("TotalPlays")),
                                .UserPlays = Convert.ToInt32(reader("UserPlays"))
                            })
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur lors de la récupération des musiques : " & ex.Message)
        End Try

        Return musics
    End Function
End Class
