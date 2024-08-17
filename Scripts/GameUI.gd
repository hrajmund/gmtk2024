extends Control

export(int) var health = 3
export(int) var level = 1


var max_width

var health_label
var level_label

func _ready():
	max_width = get_parent().get_size().x
	var panel_container = $MarginContainer
	panel_container.size_flags_horizontal = Control.SIZE_EXPAND + Control.SIZE_SHRINK_END
	
	health_label = $MarginContainer/GridContainer/HealthLabel
	level_label = $MarginContainer/GridContainer/LevelLabel



func _process(delta):
	health_label.text = "Health: " + str(health)
	level_label.text = "Level: " + str(level)

