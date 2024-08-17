extends Control

export(int) var health = 3
export(int) var score = 0
export(int) var level = 1


var health_label
var level_label
var score_label

# Called when the node enters the scene tree for the first time.
func _ready():
	health_label = $PanelContainer/GridContainer/HealthLabel
	level_label = $PanelContainer/GridContainer/LevelLabel
	score_label = $PanelContainer/GridContainer/ScoreLabel



func _process(delta):
	health_label.text = "Health: " + str(health)
	level_label.text = "Level: " + str(level)
	score_label.text = "Score: " + str(level)
