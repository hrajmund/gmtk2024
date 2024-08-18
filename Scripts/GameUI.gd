extends Control

signal submit_button_pressed

export(int) var health = 3
export(int) var level = 1


var max_width

var health_sprite
var level_label

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
	level_label = $AspectRatioContainer/MarginContainer/GridContainer/LevelLabel


func _process(delta):
	level_label.text = "Level: " + str(level)
	
	if health > 3:
		actual_health = 3
	elif health < 0:
		actual_health = 0
	else:
		actual_health = health
	
	health_sprite.texture = hp_textures[actual_health]


func _on_Button_pressed():
	emit_signal("submit_button_pressed");
