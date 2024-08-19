extends Control

var animation_player

# Called when the node enters the scene tree for the first time.
func _ready():
	$VBoxContainer/StartButton.grab_focus()
	animation_player = $AnimationPlayer
	Music.playMusic()
	preload("res://Scenes/Main.tscn")


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func _begin_game():
	get_tree().change_scene("res://Scenes/Main.tscn")

func _on_StartButton_pressed():
	animation_player.play("FadeOutBeginGame")


func _on_CreditsButton_pressed():
	get_tree().change_scene("res://Scenes/Credits.tscn")


func _on_QuitButton_pressed():
	get_tree().quit()
