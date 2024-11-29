from PyQt5.QtWidgets import (
    QWidget, QHBoxLayout, QVBoxLayout, QPushButton, QSlider,
    QLabel, QSpacerItem, QSizePolicy
)
from PyQt5.QtGui import QIcon, QPixmap
from PyQt5.QtCore import Qt, QSize
from resources import *


class MusicControls(QWidget):
    def __init__(self):
        super().__init__()

        self.setFixedHeight(100)  # Hauteur ajustée pour le panneau inférieur
        self.setStyleSheet("background-color: #1E1E1E;")  # Couleur de fond gris foncé

        # Disposition principale horizontale
        layout = QHBoxLayout(self)
        layout.setContentsMargins(10, 5, 10, 5)
        layout.setSpacing(15)

        # SECTION GAUCHE : Image et infos sur la musique
        self.left_section = QHBoxLayout()
        self.left_section.setSpacing(10)

        # Image de l'album
        self.album_image = QLabel()
        self.album_image.setFixedSize(60, 60)
        self.album_image.setPixmap(QPixmap(ALBUM_PLACEHOLDER).scaled(60, 60, Qt.KeepAspectRatio, Qt.SmoothTransformation))
        self.left_section.addWidget(self.album_image)

        # Infos musique
        self.info_section = QVBoxLayout()
        self.info_section.setAlignment(Qt.AlignCenter)  # Aligne les éléments au centre verticalement

        # Titre de la musique
        self.song_title = QLabel("Titre de la musique")
        self.song_title.setStyleSheet("color: white; font-size: 12px; font-weight: bold;")
        self.info_section.addWidget(self.song_title)

        # Liste des auteurs
        self.authors_layout = QHBoxLayout()
        authors = ["Artiste 1", "Artiste 2"]
        self.authors_layout.setSpacing(5)  # Ajout d'un petit espacement entre les auteurs
        for author in authors:
            author_label = QLabel(author)
            author_label.setStyleSheet("color: #00FF00; font-size: 10px; text-decoration: underline; cursor: pointer;")
            self.authors_layout.addWidget(author_label)

        self.info_section.addLayout(self.authors_layout)

        # Ajout de la section d'infos à la gauche
        self.left_section.addLayout(self.info_section)

        # Ajout de la section gauche au layout principal
        layout.addLayout(self.left_section)

        # SECTION CENTRALE : Boutons + slider
        self.center_section = QVBoxLayout()

        # Boutons de contrôle (Play, Previous, Next)
        button_layout = QHBoxLayout()
        button_layout.setSpacing(15)
        button_layout.setAlignment(Qt.AlignCenter)

        # Bouton Previous
        prev_button = QPushButton()
        prev_button.setIcon(QIcon(PREV_ICON))
        prev_button.setFixedSize(36, 36)  # Taille augmentée
        prev_button.setIconSize(QSize(32, 32))  # Taille de l'icône ajustée
        prev_button.setStyleSheet(self.invisible_button_style())
        button_layout.addWidget(prev_button)

        # Bouton Play
        play_button = QPushButton()
        play_button.setIcon(QIcon(PLAY_ICON))
        play_button.setFixedSize(48, 48)  # Taille augmentée
        play_button.setIconSize(QSize(42, 42))  # Taille de l'icône ajustée
        play_button.setStyleSheet(self.invisible_button_style())
        button_layout.addWidget(play_button)

        # Bouton Next
        next_button = QPushButton()
        next_button.setIcon(QIcon(NEXT_ICON))
        next_button.setFixedSize(36, 36)  # Taille augmentée
        next_button.setIconSize(QSize(32, 32))  # Taille de l'icône ajustée
        next_button.setStyleSheet(self.invisible_button_style())
        button_layout.addWidget(next_button)

        self.center_section.addLayout(button_layout)

        # Slider de lecture (trackbar)
        slider_layout = QHBoxLayout()
        slider_layout.setSpacing(10)

        # Spacer gauche pour centrer
        left_spacer = QSpacerItem(20, 20, QSizePolicy.Expanding, QSizePolicy.Minimum)
        slider_layout.addItem(left_spacer)

        # Label du début du temps
        self.time_label_start = QLabel("0:00")
        self.time_label_start.setStyleSheet("color: white; font-size: 10px;")
        slider_layout.addWidget(self.time_label_start)

        # Slider principal
        self.music_slider = QSlider(Qt.Horizontal)
        self.music_slider.setMinimum(0)
        self.music_slider.setMaximum(100)
        self.music_slider.setValue(0)
        self.music_slider.setSizePolicy(QSizePolicy.Expanding, QSizePolicy.Fixed)
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

        # Label de la fin du temps
        self.time_label_end = QLabel("-0:00")
        self.time_label_end.setStyleSheet("color: white; font-size: 10px;")
        slider_layout.addWidget(self.time_label_end)

        # Spacer droit pour centrer
        right_spacer = QSpacerItem(20, 20, QSizePolicy.Expanding, QSizePolicy.Minimum)
        slider_layout.addItem(right_spacer)

        # Ajouter le layout du slider dans la section centrale
        self.center_section.addLayout(slider_layout)
        layout.addLayout(self.center_section)

        # SECTION DROITE : Volume et paroles
        self.right_section = QHBoxLayout()
        self.right_section.setSpacing(5)

        lyrics_button = QPushButton()
        lyrics_button.setIcon(QIcon(LYRICS_ICON))
        lyrics_button.setFixedSize(28, 28)
        lyrics_button.setStyleSheet(self.invisible_button_style())
        self.right_section.addWidget(lyrics_button)

        self.volume_icon = QLabel()
        self.volume_icon.setFixedSize(30, 30)
        self.volume_icon.setPixmap(QPixmap(VOLUME_ICON_33).scaled(30, 30, Qt.KeepAspectRatio, Qt.SmoothTransformation))
        self.right_section.addWidget(self.volume_icon)

        # Réduire la longueur du slider de volume
        self.volume_slider = QSlider(Qt.Horizontal)
        self.volume_slider.setMinimum(0)
        self.volume_slider.setMaximum(100)
        self.volume_slider.setValue(101)
        self.volume_slider.setFixedWidth(150)  # Réduit la longueur du slider
        self.volume_slider.setSizePolicy(QSizePolicy.Fixed, QSizePolicy.Fixed)  # Fixe la taille du slider
        self.volume_slider.valueChanged.connect(self.update_volume_icon)
        self.right_section.addWidget(self.volume_slider)

        layout.addLayout(self.right_section)

    def invisible_button_style(self):
        """Retourne un style minimaliste pour les boutons."""
        return """
        QPushButton {
            background-color: transparent;
            border: none;
        }
        QPushButton:hover {
            background-color: #333333;
        }
        """

    def update_volume_icon(self):
        """Met à jour l'icône du volume selon la valeur."""
        value = self.volume_slider.value()
        if value == 0:
            self.volume_icon.setPixmap(QPixmap(VOLUME_ICON_0).scaled(30, 30, Qt.KeepAspectRatio, Qt.SmoothTransformation))
        elif 1 <= value < 33:
            self.volume_icon.setPixmap(QPixmap(VOLUME_ICON_33).scaled(30, 30, Qt.KeepAspectRatio, Qt.SmoothTransformation))
        elif 33 <= value < 66:
            self.volume_icon.setPixmap(QPixmap(VOLUME_ICON_66).scaled(30, 30, Qt.KeepAspectRatio, Qt.SmoothTransformation))
        else:
            self.volume_icon.setPixmap(QPixmap(VOLUME_ICON_100).scaled(30, 30, Qt.KeepAspectRatio, Qt.SmoothTransformation))
