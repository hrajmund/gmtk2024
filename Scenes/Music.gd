extends Node

var music
var is_playing = false

func _ready():
	music = $MusicFile

func playMusic():
	if not is_playing:
		music.play()
		is_playing = true
	
func stopMusic():
	music.stop()
	is_playing = false
