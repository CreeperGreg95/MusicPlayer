from PyQt5.QtWidgets import QWidget, QVBoxLayout, QLabel, QListWidget, QPushButton


class Playlist(QWidget):
    def __init__(self, title, songs=None):
        super().__init__()

        self.setStyleSheet("background-color: #121212; color: white;")

        layout = QVBoxLayout(self)
        layout.setContentsMargins(10, 10, 10, 10)
        layout.setSpacing(10)

        # Titre de la playlist
        playlist_title = QLabel(title)
        playlist_title.setStyleSheet("font-size: 16px; font-weight: bold;")
        layout.addWidget(playlist_title)

        # Liste des chansons
        self.song_list = QListWidget()
        self.song_list.setStyleSheet("""
            QListWidget {
                background: #1E1E1E;
                border: none;
                color: white;
                padding: 5px;
            }
            QListWidget::item:hover {
                background: #444444;
            }
        """)
        layout.addWidget(self.song_list)

        # Charger les chansons
        if songs:
            for song in songs:
                self.song_list.addItem(song)

        # Bouton d'ajout
        add_song_button = QPushButton("+ Add Song")
        add_song_button.setStyleSheet("""
            QPushButton {
                background-color: #3A3A3A;
                color: white;
            }
            QPushButton:hover {
                background-color: #4A4A4A;
            }
        """)
        add_song_button.clicked.connect(self.add_song)
        layout.addWidget(add_song_button)

    def add_song(self):
        # Placeholder pour ajouter une chanson (interface utilisateur ou autre)
        self.song_list.addItem(f"Song {self.song_list.count() + 1}")
