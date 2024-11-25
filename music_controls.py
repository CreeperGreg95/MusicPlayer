from PyQt5.QtWidgets import QWidget, QHBoxLayout, QPushButton, QSlider, QLabel, QSpacerItem, QSizePolicy, QVBoxLayout
from PyQt5.QtGui import QIcon
from PyQt5.QtCore import Qt
from resources import *  # Import des ressources (icônes)

class MusicControls(QWidget):
    def __init__(self):
        super().__init__()

        self.setFixedHeight(100)  # Hauteur ajustée pour le panneau inférieur
        self.setStyleSheet("background-color: #1E1E1E;")  # Couleur de fond gris foncé

        # Disposition verticale principale
        layout = QVBoxLayout(self)
        layout.setContentsMargins(0, 0, 0, 0)  # Pas de marges internes
        layout.setSpacing(0)  # Pas d'espacement vertical entre les éléments

        # Disposition pour les boutons (Play, Prev, Next)
        button_layout = QHBoxLayout()
        button_layout.setContentsMargins(0, 10, 0, 0)  # Alignement des boutons en haut du panneau
        button_layout.setSpacing(15)  # Espacement entre les boutons

        # Espacement gauche pour centrer horizontalement
        button_layout.addSpacerItem(QSpacerItem(40, 20, QSizePolicy.Expanding, QSizePolicy.Minimum))

        # Bouton précédent
        prev_button = QPushButton()
        prev_button.setIcon(QIcon(PREV_ICON))
        prev_button.setFixedSize(28, 28)  # Taille ajustée à 28px pour les boutons
        prev_button.setIconSize(prev_button.size())  # Redimensionner l'icône à la taille du bouton
        prev_button.setStyleSheet(self.invisible_button_style())  # Applique le style invisible
        button_layout.addWidget(prev_button)

        # Bouton Play/Pause
        play_pause_button = QPushButton()
        play_pause_button.setIcon(QIcon(PLAY_ICON))
        play_pause_button.setFixedSize(36, 36)  # Taille ajustée à 36px pour Play/Pause
        play_pause_button.setIconSize(play_pause_button.size())  # Redimensionner l'icône à la taille du bouton
        play_pause_button.setStyleSheet(self.invisible_button_style())  # Applique le style invisible
        button_layout.addWidget(play_pause_button)

        # Bouton suivant
        next_button = QPushButton()
        next_button.setIcon(QIcon(NEXT_ICON))
        next_button.setFixedSize(28, 28)  # Taille ajustée à 28px pour les boutons
        next_button.setIconSize(next_button.size())  # Redimensionner l'icône à la taille du bouton
        next_button.setStyleSheet(self.invisible_button_style())  # Applique le style invisible
        button_layout.addWidget(next_button)

        # Espacement droit pour centrer horizontalement
        button_layout.addSpacerItem(QSpacerItem(40, 20, QSizePolicy.Expanding, QSizePolicy.Minimum))

        # Ajouter les boutons à la disposition principale
        layout.addLayout(button_layout)

        # Disposition pour la barre de progression (Slider)
        slider_layout = QHBoxLayout()
        slider_layout.setContentsMargins(0, 0, 0, 5)  # Alignement des composants en bas du panneau
        slider_layout.setSpacing(10)  # Espacement entre les composants

        # Espacement gauche pour centrer le slider
        slider_layout.addSpacerItem(QSpacerItem(40, 20, QSizePolicy.Expanding, QSizePolicy.Minimum))

        # Label du temps actuel
        self.time_label_start = QLabel("0:00")
        self.time_label_start.setStyleSheet("color: white; font-size: 10px;")  # Taille réduite du texte
        slider_layout.addWidget(self.time_label_start)

        # Barre de progression
        self.music_slider = QSlider(Qt.Horizontal)
        self.music_slider.setMinimum(0)
        self.music_slider.setMaximum(100)
        self.music_slider.setValue(50)
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
        self.music_slider.setFixedWidth(300)  # Largeur contrôlée pour ne pas prendre toute la fenêtre
        slider_layout.addWidget(self.music_slider)

        # Label du temps total
        self.time_label_end = QLabel("-3:00")
        self.time_label_end.setStyleSheet("color: white; font-size: 10px;")  # Taille réduite du texte
        slider_layout.addWidget(self.time_label_end)

        # Espacement droit pour centrer le slider
        slider_layout.addSpacerItem(QSpacerItem(40, 20, QSizePolicy.Expanding, QSizePolicy.Minimum))

        # Ajouter la disposition du slider
        layout.addLayout(slider_layout)

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
