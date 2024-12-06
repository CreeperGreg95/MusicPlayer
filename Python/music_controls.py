from PyQt5.QtWidgets import QWidget, QHBoxLayout, QVBoxLayout, QPushButton, QSlider, QLabel, QSpacerItem, QSizePolicy
from PyQt5.QtGui import QIcon, QPixmap
from PyQt5.QtCore import Qt, QSize
from resources import *


class MusicControls(QWidget):
    def __init__(self):
        super().__init__()

        self.setFixedHeight(100)
        self.setStyleSheet("background-color: #1E1E1E;")

        layout = QHBoxLayout(self)
        layout.setContentsMargins(10, 5, 10, 5)
        layout.setSpacing(15)

        # SECTION GAUCHE
        self.left_section = QHBoxLayout()
        self.left_section.setSpacing(10)

        self.album_image = QLabel()
        self.album_image.setFixedSize(60, 60)
        self.album_image.setPixmap(QPixmap(ALBUM_PLACEHOLDER).scaled(60, 60, Qt.KeepAspectRatio, Qt.SmoothTransformation))
        self.left_section.addWidget(self.album_image)

        self.info_section = QVBoxLayout()
        self.song_title = QLabel("Song Title")
        self.song_title.setStyleSheet("color: white; font-size: 12px; font-weight: bold;")
        self.info_section.addWidget(self.song_title)

        self.authors_layout = QHBoxLayout()
        self.authors_layout.setSpacing(5)
        for author in ["Artist 1", "Artist 2"]:
            author_label = QLabel(author)
            author_label.setStyleSheet("color: #00FF00; font-size: 10px;")
            self.authors_layout.addWidget(author_label)

        self.info_section.addLayout(self.authors_layout)
        self.left_section.addLayout(self.info_section)
        layout.addLayout(self.left_section)

        # SECTION CENTRALE
        self.center_section = QVBoxLayout()
        button_layout = QHBoxLayout()
        button_layout.setSpacing(15)
        button_layout.setAlignment(Qt.AlignCenter)

        prev_button = QPushButton()
        prev_button.setIcon(QIcon(PREV_ICON))
        prev_button.setFixedSize(36, 36)
        prev_button.setIconSize(QSize(32, 32))
        prev_button.setStyleSheet(self.invisible_button_style())
        button_layout.addWidget(prev_button)

        play_button = QPushButton()
        play_button.setIcon(QIcon(PLAY_ICON))
        play_button.setFixedSize(48, 48)
        play_button.setIconSize(QSize(42, 42))
        play_button.setStyleSheet(self.invisible_button_style())
        button_layout.addWidget(play_button)

        next_button = QPushButton()
        next_button.setIcon(QIcon(NEXT_ICON))
        next_button.setFixedSize(36, 36)
        next_button.setIconSize(QSize(32, 32))
        next_button.setStyleSheet(self.invisible_button_style())
        button_layout.addWidget(next_button)

        self.center_section.addLayout(button_layout)

        # Slider
        slider_layout = QHBoxLayout()
        slider_layout.setSpacing(10)

        self.time_label_start = QLabel("0:00")
        self.time_label_start.setStyleSheet("color: white; font-size: 10px;")
        slider_layout.addWidget(self.time_label_start)

        self.music_slider = QSlider(Qt.Horizontal)
        self.music_slider.setMinimum(0)
        self.music_slider.setMaximum(100)
        self.music_slider.setValue(0)
        slider_layout.addWidget(self.music_slider)

        self.time_label_end = QLabel("-0:00")
        self.time_label_end.setStyleSheet("color: white; font-size: 10px;")
        slider_layout.addWidget(self.time_label_end)

        self.center_section.addLayout(slider_layout)
        layout.addLayout(self.center_section)

        # SECTION DROITE
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

        self.volume_slider = QSlider(Qt.Horizontal)
        self.volume_slider.setMinimum(0)
        self.volume_slider.setMaximum(100)
        self.volume_slider.setValue(50)
        self.volume_slider.valueChanged.connect(self.update_volume_icon)
        self.right_section.addWidget(self.volume_slider)

        equalizer_button = QPushButton()
        equalizer_button.setIcon(QIcon("assets/icons/equalizer.png"))  # Nouveau bouton
        equalizer_button.setFixedSize(30, 30)
        equalizer_button.setStyleSheet(self.invisible_button_style())
        self.right_section.addWidget(equalizer_button)

        layout.addLayout(self.right_section)

    def invisible_button_style(self):
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
        value = self.volume_slider.value()
        if value == 0:
            self.volume_icon.setPixmap(QPixmap(VOLUME_ICON_0).scaled(30, 30, Qt.KeepAspectRatio, Qt.SmoothTransformation))
        elif 1 <= value < 33:
            self.volume_icon.setPixmap(QPixmap(VOLUME_ICON_33).scaled(30, 30, Qt.KeepAspectRatio, Qt.SmoothTransformation))
        elif 33 <= value < 66:
            self.volume_icon.setPixmap(QPixmap(VOLUME_ICON_66).scaled(30, 30, Qt.KeepAspectRatio, Qt.SmoothTransformation))
        else:
            self.volume_icon.setPixmap(QPixmap(VOLUME_ICON_100).scaled(30, 30, Qt.KeepAspectRatio, Qt.SmoothTransformation))
