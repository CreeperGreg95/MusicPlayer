from PyQt5.QtWidgets import (
    QWidget, QHBoxLayout, QVBoxLayout, QPushButton, QSlider,
    QLabel, QSpacerItem, QSizePolicy
)
from PyQt5.QtGui import QIcon, QPixmap
from PyQt5.QtCore import Qt
from resources import *

class MusicControls(QWidget):
    def __init__(self):
        super().__init__()

        self.setFixedHeight(100)  # Hauteur ajustée pour le panneau inférieur
        self.setStyleSheet("background-color: #1E1E1E;")  # Couleur de fond gris foncé

        # Disposition principale horizontale
        layout = QHBoxLayout(self)
        layout.setContentsMargins(10, 5, 10, 5)  # Marges autour du contenu
        layout.setSpacing(15)  # Espacement entre les sections principales

        # Section gauche : Image et infos sur la musique
        self.left_section = QHBoxLayout()
        self.left_section.setContentsMargins(0, 0, 0, 0)
        self.left_section.setSpacing(10)

        # Image de l'album
        self.album_image = QLabel()
        self.album_image.setFixedSize(60, 60)  # Taille de l'image
        self.album_image.setStyleSheet("border-radius: 5px; background-color: #333333;")  # Fond temporaire
        self.album_image.setPixmap(QPixmap(ALBUM_PLACEHOLDER).scaled(60, 60, Qt.KeepAspectRatio, Qt.SmoothTransformation))
        self.left_section.addWidget(self.album_image)

        # Infos musique : Titre et auteurs
        self.info_section = QVBoxLayout()
        self.info_section.setContentsMargins(0, 0, 0, 0)
        self.info_section.setAlignment(Qt.AlignVCenter)  # Centrage vertical par rapport à l'image

        # Titre de la musique
        self.song_title = QLabel("Titre de la musique")
        self.song_title.setStyleSheet("color: white; font-size: 12px; font-weight: bold;")
        self.info_section.addWidget(self.song_title)

        # Auteurs (cliquables)
        self.authors_layout = QHBoxLayout()
        self.authors_layout.setContentsMargins(0, 0, 0, 0)
        self.authors_layout.setSpacing(5)

        # Exemple de plusieurs auteurs
        authors = ["Artiste 1", "Artiste 2"]
        for author in authors:
            author_label = QLabel(author)
            author_label.setStyleSheet("color: #00FF00; font-size: 10px; text-decoration: underline; cursor: pointer;")
            self.authors_layout.addWidget(author_label)

        self.info_section.addLayout(self.authors_layout)
        self.left_section.addLayout(self.info_section)

        layout.addLayout(self.left_section)

        # Section centrale : Boutons de contrôle
        self.center_section = QVBoxLayout()
        self.center_section.setContentsMargins(0, 0, 0, 0)

        # Disposition des boutons
        button_layout = QHBoxLayout()
        button_layout.setContentsMargins(0, 0, 0, 0)
        button_layout.setSpacing(15)

        # Espacement gauche pour centrer horizontalement
        button_layout.addSpacerItem(QSpacerItem(40, 20, QSizePolicy.Expanding, QSizePolicy.Minimum))

        # Bouton précédent
        prev_button = QPushButton()
        prev_button.setIcon(QIcon(PREV_ICON))
        prev_button.setFixedSize(28, 28)
        prev_button.setIconSize(prev_button.size())
        prev_button.setStyleSheet(self.invisible_button_style())
        button_layout.addWidget(prev_button)

        # Bouton Play/Pause
        play_pause_button = QPushButton()
        play_pause_button.setIcon(QIcon(PLAY_ICON))
        play_pause_button.setFixedSize(36, 36)
        play_pause_button.setIconSize(play_pause_button.size())
        play_pause_button.setStyleSheet(self.invisible_button_style())
        button_layout.addWidget(play_pause_button)

        # Bouton suivant
        next_button = QPushButton()
        next_button.setIcon(QIcon(NEXT_ICON))
        next_button.setFixedSize(28, 28)
        next_button.setIconSize(next_button.size())
        next_button.setStyleSheet(self.invisible_button_style())
        button_layout.addWidget(next_button)

        # Espacement droit pour centrer horizontalement
        button_layout.addSpacerItem(QSpacerItem(40, 20, QSizePolicy.Expanding, QSizePolicy.Minimum))

        # Ajouter les boutons à la section centrale
        self.center_section.addLayout(button_layout)

        # Barre de progression
        slider_layout = QHBoxLayout()
        slider_layout.setContentsMargins(0, 0, 0, 0)
        slider_layout.setSpacing(10)

        # Espacement gauche pour centrer le slider
        slider_layout.addSpacerItem(QSpacerItem(40, 20, QSizePolicy.Expanding, QSizePolicy.Minimum))

        # Label du temps actuel
        self.time_label_start = QLabel("0:00")
        self.time_label_start.setStyleSheet("color: white; font-size: 10px;")
        slider_layout.addWidget(self.time_label_start)

        # Barre de progression
        self.music_slider = QSlider(Qt.Horizontal)
        self.music_slider.setMinimum(0)
        self.music_slider.setMaximum(100)
        self.music_slider.setValue(50)
        self.music_slider.setFixedWidth(300)
        self.music_slider.setStyleSheet("""
            QSlider::groove:horizontal {
                height: 6px;
                background: #333333;
                border-radius: 3px;
            }
            QSlider::handle:horizontal {
                width: 12px;
                height: 12px;
                background: #00FF00;
                border-radius: 6px;
                margin: -3px 0;
            }
        """)
        slider_layout.addWidget(self.music_slider)

        # Label du temps total
        self.time_label_end = QLabel("-0:00")
        self.time_label_end.setStyleSheet("color: white; font-size: 10px;")
        slider_layout.addWidget(self.time_label_end)

        # Espacement droit pour centrer le slider
        slider_layout.addSpacerItem(QSpacerItem(40, 20, QSizePolicy.Expanding, QSizePolicy.Minimum))

        # Ajouter le slider à la section centrale
        self.center_section.addLayout(slider_layout)
        layout.addLayout(self.center_section)

        # Espacement droit de la disposition principale
        layout.addSpacerItem(QSpacerItem(40, 20, QSizePolicy.Expanding, QSizePolicy.Minimum))

    def invisible_button_style(self):
        """Retourne le style pour des boutons invisibles ou minimalistes."""
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
