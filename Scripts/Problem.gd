extends Node

export(int) var number_of_steps = 5

var operation_script = preload("res://Model/OperationHelper.cs")
var operation_helper = operation_script.new()


func generate_problem():
	print(operation_helper)
	var test = operation_helper.GetAddition()
	if test == operation_helper.GetAddition():
		print("working")

func _ready():
	generate_problem()
