import sys
from PyQt5.QtCore import Qt
from PyQt5.QtGui import QIcon
from PyQt5.QtWidgets import (
    QApplication, QMainWindow, QWidget, QHBoxLayout, QVBoxLayout,
    QPushButton, QLineEdit, QSpacerItem, QSizePolicy
)
from resources import *  # Import des ressources (icônes)
from music_controls import MusicControls  # Import des contrôles de musique
from library import LibraryPanel  # Import du panneau de bibliothèque


class MusicPlayer(QMainWindow):
    def __init__(self):
        super().__init__()

        # Configuration de la fenêtre principale
        self.setWindowTitle("Lecteur de Musique")
        self.setGeometry(100, 100, 1200, 800)
        self.setStyleSheet("background-color: #000000;")  # Fond noir bien foncé

        # Définir l'icône de la fenêtre
        self.setWindowIcon(QIcon(WINDOW_ICON))

        # Conteneur principal
        self.central_widget = QWidget()
        self.setCentralWidget(self.central_widget)
        self.main_layout = QHBoxLayout(self.central_widget)
        self.main_layout.setContentsMargins(0, 0, 0, 0)
        self.main_layout.setSpacing(0)

        # Ajouter la bibliothèque
        self.add_library_panel()

        # Ajouter un panneau principal pour la barre supérieure et le contenu principal
        self.central_panel = QVBoxLayout()
        self.central_panel.setContentsMargins(0, 0, 0, 0)
        self.central_panel.setSpacing(0)

        # Ajouter la barre supérieure
        self.add_top_bar()

        # Ajouter un espace pour le contenu principal (placeholder)
        self.add_main_content_placeholder()

        # Ajouter un panneau de fond pour les contrôles de musique en bas
        self.add_bottom_panel()

        # Ajouter le panneau central à la disposition principale
        self.main_layout.addLayout(self.central_panel)

    def add_library_panel(self):
        """Ajoute le panneau de bibliothèque à gauche."""
        self.library_panel = LibraryPanel()
        self.library_panel.setFixedWidth(200)
        self.main_layout.addWidget(self.library_panel)

    def add_top_bar(self):
        """Ajoute une barre supérieure fixée tout en haut."""
        self.top_bar = QWidget()
        self.top_bar.setFixedHeight(50)
        self.top_bar.setStyleSheet("background-color: #1E1E1E;")

        self.top_layout = QHBoxLayout(self.top_bar)
        self.top_layout.setContentsMargins(10, 5, 10, 0)
        self.top_layout.setSpacing(10)

        # Partie gauche
        menu_button = QPushButton("...")
        menu_button.setFixedSize(30, 30)
        menu_button.setStyleSheet(self.invisible_button_style())
        self.top_layout.addWidget(menu_button)

        back_button = QPushButton("←")
        back_button.setFixedSize(30, 30)
        back_button.setEnabled(False)
        back_button.setStyleSheet(self.grayed_out_button_style())
        self.top_layout.addWidget(back_button)

        forward_button = QPushButton("→")
        forward_button.setFixedSize(30, 30)
        forward_button.setEnabled(False)
        forward_button.setStyleSheet(self.grayed_out_button_style())
        self.top_layout.addWidget(forward_button)

        # Espacement pour centrer le bouton Home et la barre de recherche
        self.left_spacer = QSpacerItem(40, 20, QSizePolicy.Expanding, QSizePolicy.Minimum)
        self.top_layout.addSpacerItem(self.left_spacer)

        # Partie centrale
        self.home_button = QPushButton()
        self.home_button.setIcon(QIcon(HOME_ICON))
        self.home_button.setIconSize(self.home_button.size())
        self.home_button.setFixedSize(30, 30)
        self.home_button.setStyleSheet(self.invisible_button_style())
        self.top_layout.addWidget(self.home_button)

        self.search_bar = QLineEdit()
        self.search_bar.setPlaceholderText("Qu'est-ce que vous voulez écouter ?")
        self.search_bar.setFixedHeight(30)
        self.search_bar.setStyleSheet("""
            QLineEdit {
                background-color: #333333;
                color: white;
                border: 1px solid #444444;
                padding: 0 10px;
                border-radius: 5px;
            }
        """)
        self.top_layout.addWidget(self.search_bar)

        # Partie droite
        account_button = QPushButton()
        account_button.setIcon(QIcon(PROFILE_ICON))
        account_button.setFixedSize(30, 30)
        account_button.setStyleSheet(self.invisible_button_style())
        self.top_layout.addWidget(account_button)

        notification_button = QPushButton()
        notification_button.setIcon(QIcon(NOTIFICATION_ICON))
        notification_button.setFixedSize(30, 30)
        notification_button.setStyleSheet(self.invisible_button_style())
        self.top_layout.addWidget(notification_button)

        self.central_panel.addWidget(self.top_bar, alignment=Qt.AlignTop)

    def add_main_content_placeholder(self):
        """Ajoute un placeholder pour le contenu principal."""
        content_placeholder = QWidget()
        content_placeholder.setStyleSheet("background-color: #121212;")
        self.central_panel.addWidget(content_placeholder, 1)

    def add_bottom_panel(self):
        """Ajoute un panneau de fond pour les contrôles de musique en bas."""
        self.bottom_panel = QWidget()
        self.bottom_panel.setFixedHeight(100)
        self.bottom_panel.setStyleSheet("background-color: #1E1E1E;")

        self.bottom_layout = QVBoxLayout(self.bottom_panel)
        self.bottom_layout.setContentsMargins(0, 0, 0, 0)

        self.music_controls = MusicControls()
        self.bottom_layout.addWidget(self.music_controls)

        self.central_panel.addWidget(self.bottom_panel, alignment=Qt.AlignBottom)

    def invisible_button_style(self):
        return """
        QPushButton {
            background-color: #1E1E1E;
            border: none;
            color: white;
        }
        QPushButton:hover {
            background-color: #333333;
        }
        """

    def grayed_out_button_style(self):
        return """
        QPushButton {
            background-color: #1E1E1E;
            color: gray;
        }
        """


if __name__ == "__main__":
    app = QApplication(sys.argv)
    app.setStyle("Fusion")
    window = MusicPlayer()
    window.show()
    sys.exit(app.exec_())
