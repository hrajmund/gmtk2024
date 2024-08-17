extends Control


# Called when the node enters the scene tree for the first time.
func _ready():
	$VBoxContainer/StartButton.grab_focus()


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass


func _on_StartButton_pressed():
	get_tree().change_scene("res://Scenes/Main.tscn")


func _on_CreditsButton_pressed():
	pass


func _on_QuitButton_pressed():
	get_tree().quit()
