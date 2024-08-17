extends Node

export(int) var number_of_steps = 5

var operation_script = preload("res://Model/OperationHelper.cs")
var operation_helper = operation_script.new()


func generate_problem(n: int) -> Array:
	var problems = []
	
	var possibilities = [
		operation_helper.GetAddition(),
		operation_helper.GetSubtraction(),
		operation_helper.GetMultiplication(),
		operation_helper.GetDivination(),
	]
	
	for i in range(n):
		var choice = possibilities[randi() % possibilities.size()]
		problems.append(choice)
	
	return problems

func _ready():
	print(generate_problem(number_of_steps))
