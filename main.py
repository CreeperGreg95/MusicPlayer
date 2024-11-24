import sys
from PyQt5.QtCore import Qt
from PyQt5.QtGui import QIcon
from PyQt5.QtWidgets import (
    QApplication, QMainWindow, QWidget, QHBoxLayout, QVBoxLayout,
    QPushButton, QLineEdit, QSpacerItem, QSizePolicy
)
from resources import *  # Import des ressources (icônes)

class MusicPlayer(QMainWindow):
    def __init__(self):
        super().__init__()

        # Configuration de la fenêtre principale
        self.setWindowTitle("Lecteur de Musique")
        self.setGeometry(100, 100, 800, 500)
        self.setStyleSheet("background-color: #000000;")  # Fond noir bien foncé

        # Définir l'icône de la fenêtre
        self.setWindowIcon(QIcon(WINDOW_ICON))  # Utilisation de l'icône de la fenêtre

        # Conteneur principal
        self.central_widget = QWidget()
        self.setCentralWidget(self.central_widget)
        self.layout = QVBoxLayout(self.central_widget)
        self.layout.setContentsMargins(0, 0, 0, 0)
        self.layout.setSpacing(0)

        # Ajouter la barre supérieure
        self.add_top_bar()

    def add_top_bar(self):
        """Ajoute une barre supérieure fixée tout en haut."""
        self.top_bar = QWidget()
        self.top_bar.setFixedHeight(50)  # Hauteur de la barre
        self.top_bar.setStyleSheet("background-color: #1E1E1E;")  # Fond gris foncé

        self.top_layout = QHBoxLayout(self.top_bar)
        self.top_layout.setContentsMargins(10, 5, 10, 0)  # Réduction des marges hautes
        self.top_layout.setSpacing(10)  # Espacement réduit entre les éléments

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
        self.home_button.setIcon(QIcon(HOME_ICON))  # Utilisation de l'icône Home
        self.home_button.setIconSize(self.home_button.size())
        self.home_button.setFixedSize(30, 30)
        self.home_button.setStyleSheet(self.invisible_button_style())
        self.top_layout.addWidget(self.home_button)

        self.search_bar = QLineEdit()
        self.search_bar.setPlaceholderText("Rechercher...")
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

        # Espacement pour équilibrer les côtés
        self.right_spacer = QSpacerItem(40, 20, QSizePolicy.Expanding, QSizePolicy.Minimum)
        self.top_layout.addSpacerItem(self.right_spacer)

        # Partie droite
        account_button = QPushButton()
        account_button.setIcon(QIcon(PROFILE_ICON))  # Utilisation de l'icône Profil
        account_button.setIconSize(account_button.size())
        account_button.setFixedSize(30, 30)
        account_button.setStyleSheet(self.invisible_button_style())
        self.top_layout.addWidget(account_button)

        notification_button = QPushButton()
        notification_button.setIcon(QIcon(NOTIFICATION_ICON))  # Utilisation de l'icône Notification
        notification_button.setIconSize(notification_button.size())
        notification_button.setFixedSize(30, 30)
        notification_button.setStyleSheet(self.invisible_button_style())
        self.top_layout.addWidget(notification_button)

        # Ajouter la barre supérieure en haut
        self.layout.addWidget(self.top_bar, alignment=Qt.AlignTop)

    def resizeEvent(self, event):
        """Recentrer dynamiquement la barre de recherche et le bouton Home."""
        # Ajuster la largeur de la barre de recherche pour s'adapter à la taille de la fenêtre
        window_width = self.width()

        # Ajuste la largeur de la barre de recherche dynamiquement
        max_search_width = window_width // 3  # La barre occupe un tiers de la fenêtre
        min_search_width = 200  # Largeur minimale de la barre
        self.search_bar.setFixedWidth(max(min_search_width, max_search_width))

        super().resizeEvent(event)

    def invisible_button_style(self):
        """Style pour un bouton invisible ou minimaliste."""
        return """
        QPushButton {
            background-color: #1E1E1E;
            border: none;
            color: white;
            font-size: 14px;
        }
        QPushButton:hover {
            background-color: #333333;
        }
        """

    def grayed_out_button_style(self):
        """Style pour un bouton désactivé."""
        return """
        QPushButton {
            background-color: #1E1E1E;
            border: none;
            color: gray;
            font-size: 14px;
        }
        QPushButton:hover {
            background-color: #1E1E1E;  /* Pas de changement au survol */
        }
        """


if __name__ == "__main__":
    app = QApplication(sys.argv)
    app.setStyle("Fusion")  # Style uniforme entre plateformes
    window = MusicPlayer()
    window.show()
    sys.exit(app.exec_())
