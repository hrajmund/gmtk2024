extends Control

signal submit_button_pressed

export(int) var health = 3
export(int) var level = 1


var max_width
var master_bus = AudioServer.get_bus_index("Master")
var health_sprite
var level_label
var button
var animation_player

var actual_health

var hp_textures = [
	preload("res://PixelArts/hearts/0hearts.png"),
	preload("res://PixelArts/hearts/1heart.png"),
	preload("res://PixelArts/hearts/2hearts.png"),
	preload("res://PixelArts/hearts/3hearts.png")	
]


func _ready():
	var panel_container = $AspectRatioContainer/MarginContainer
	health_sprite = $AspectRatioContainer/MarginContainer/GridContainer/HealthSprite
	level_label = $AspectRatioContainer2/MarginContainer/GridContainer/LevelLabel
	button = $AspectRatioContainer2/MarginContainer/GridContainer/Button
	var slider_menu = $HSlider
	slider_menu.value = AudioServer.get_bus_volume_db(master_bus)
	button.disabled = true 
	animation_player = $AnimationPlayer

func _process(_delta):
	level_label.text = "Level " + str(level)
	
	if health > 3:
		actual_health = 3
	elif health < 0:
		actual_health = 0
	else:
		actual_health = health
	
	health_sprite.texture = hp_textures[actual_health]

func _set_health(value):
	if (value != health):
		animation_player.play("HealthChange")
	health = value
	
func _set_level(value):
	if (value != level):
		animation_player.play("LevelChange")
	level = value

func _on_Button_pressed():
	emit_signal("submit_button_pressed");
	button.disabled = true


func _on_HandManager_CardPlayed(hasMoreCards):
	button.disabled = !hasMoreCards


func _on_HSlider_value_changed(value):
	AudioServer.set_bus_volume_db(master_bus, value)
	if(value == -30):
		AudioServer.set_bus_mute(master_bus, true)
	elif (value >= 5):
		AudioServer.set_bus_volume_db(master_bus, 5);
	else:
		AudioServer.set_bus_mute(master_bus, false)
	pass # Replace with function body.
