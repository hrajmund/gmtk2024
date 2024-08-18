extends Control

var names = [
	"Bence Szabó - UI, Development", "László Kaiser - Development",
	"Rajmund Hubai - Development", "Zalán Tóth - Development"
]


func _ready():
	var vbox_container = $VBoxContainer
	var title_label = $VBoxContainer/TitleLabel
	var npc_theme = load("res://Theme/Default_Theme.tres")

	names.invert()
	for name in names:
		var new_label = Label.new()
		new_label.theme = npc_theme
		new_label.text = name
		vbox_container.add_child_below_node(title_label, new_label)


func _on_GoBackButton_pressed():
	get_tree().change_scene("res://Scenes/MainMenu.tscn")
