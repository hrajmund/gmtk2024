extends Control

export(int) var health = 3
export(int) var level = 1


var max_width

var health_label
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
	max_width = get_parent().get_size().x
	var panel_container = $MarginContainer
	panel_container.size_flags_horizontal = Control.SIZE_EXPAND + Control.SIZE_SHRINK_END
	
	health_label = $MarginContainer/GridContainer/HealthLabel
	health_sprite = $MarginContainer/GridContainer/HealthSprite
	level_label = $MarginContainer/GridContainer/LevelLabel


func _process(delta):
	health_label.text = "Health: " + str(health)
	level_label.text = "Level: " + str(level)
	
	if health > 3:
		actual_health = 3
	elif health < 0:
		actual_health = 0
	else:
		actual_health = health
	
	health_sprite.texture = hp_textures[actual_health]
