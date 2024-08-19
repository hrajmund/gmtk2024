extends Node

var music

func _ready():
	music = $MusicFile

func playMusic():
	music.play()
	
func stopMusic():
	music.stop()
