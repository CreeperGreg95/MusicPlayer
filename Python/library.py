from PyQt5.QtWidgets import QWidget, QVBoxLayout, QLabel, QPushButton, QListWidget


class LibraryPanel(QWidget):
    def __init__(self):
        super().__init__()

        self.setStyleSheet("background-color: #2C2C2C; color: white;")

        layout = QVBoxLayout(self)
        layout.setContentsMargins(10, 10, 10, 10)
        layout.setSpacing(10)

        library_title = QLabel("Library")
        library_title.setStyleSheet("font-size: 16px; font-weight: bold;")
        layout.addWidget(library_title)

        self.add_playlist_button(layout, "Liked Titles")
        self.add_playlist_button(layout, "Your Episodes")
        self.add_playlist_button(layout, "Local Files")

        user_playlists_label = QLabel("Your Playlists")
        user_playlists_label.setStyleSheet("font-size: 14px; margin-top: 15px;")
        layout.addWidget(user_playlists_label)

        self.playlist_list = QListWidget()
        self.playlist_list.setStyleSheet("""
            QListWidget {
                background: #1E1E1E;
                border: none;
                padding: 5px;
            }
            QListWidget::item {
                padding: 5px;
            }
            QListWidget::item:hover {
                background: #444444;
            }
        """)
        layout.addWidget(self.playlist_list, 1)

        add_playlist_button = QPushButton("+ Add Playlist")
        add_playlist_button.setStyleSheet("""
            QPushButton {
                background-color: #3A3A3A;
                color: white;
            }
            QPushButton:hover {
                background-color: #4A4A4A;
            }
        """)
        add_playlist_button.clicked.connect(self.add_playlist)
        layout.addWidget(add_playlist_button)

    def add_playlist_button(self, layout, name):
        button = QPushButton(name)
        button.setStyleSheet("""
            QPushButton {
                background-color: #3A3A3A;
                color: white;
                text-align: left;
            }
            QPushButton:hover {
                background-color: #4A4A4A;
            }
        """)
        layout.addWidget(button)

    def add_playlist(self):
        self.playlist_list.addItem(f"Playlist {self.playlist_list.count() + 1}")
