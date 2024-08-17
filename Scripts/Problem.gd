extends Node

export(int) var game_level = 5

var operation_helper = preload("res://Model/OperationHelper.cs").new()

var fib = [1, 2, 3, 5, 8]

func generate_problem(level: int) -> Array:
	randomize()
	
	var problems = []
	
	var possibilities = [
		operation_helper.GetAddition(),
		operation_helper.GetSubtraction(),
		operation_helper.GetMultiplication(),
		operation_helper.GetDivision(),
	]
	
	if level >= fib.size() or level < 0:
		level = fib.size() - 1
	
	var number_of_steps = fib[level]
	for _i in range(number_of_steps):
		var choice = possibilities[randi() % possibilities.size()]
		problems.append(choice)
	
	return problems
	

func _round_to_one_decimal(value: float) -> float:
	return round(value * 10) / 10.0


func generate_problem_value(operation, level: int) -> float:
	randomize()
	
	var add_op = operation_helper.GetAddition()
	var sub_op = operation_helper.GetSubtraction()
	var mul_op = operation_helper.GetMultiplication()
	var div_op = operation_helper.GetDivision()
	
	match operation:
		add_op, sub_op:
			return round(rand_range(1, level + 2))
		mul_op, div_op:
			return _round_to_one_decimal(rand_range(1, level + 1) + randf())
		_:
			return 1.0


func _ready():
	print(generate_problem(game_level))
	print(generate_problem_value(operation_helper.GetAddition(), game_level))
