extends Node

export(int) var game_level = 5

var operation_helper = preload("res://Model/OperationHelper.cs").new()

var fib = [1, 2, 3, 5, 8]

func generate_problem(level: int) -> Array:
	randomize()
	
	var problems = []
	
	# TODO: add weights to possibilities
	var possibilities = [
		operation_helper.GetAddition(),
		operation_helper.GetSubtraction(),
		operation_helper.GetMultiplication(),
		operation_helper.GetDivision(),
		operation_helper.GetRotation(),
		# operation_helper.GetPower(),
		# operation_helper.GetRoot()
	]
	
	if level >= fib.size() or level < 0:
		level = fib.size() - 2
	
	var number_of_steps = fib[level]
	var last_problem = null
	while problems.size() < number_of_steps:
		var choice = possibilities[randi() % possibilities.size()]
		if last_problem == null:
			last_problem = choice
		elif last_problem == choice:
			continue
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
	var div_rot = operation_helper.GetRotation()
	var div_pow = operation_helper.GetPower()
	var div_root = operation_helper.GetRoot()

	var max_val = level + 1
	if max_val == 1:
		max_val = 2
	elif max_val > 3:
		max_val = 3
	
	match operation:
		add_op:
			var add_number = round(rand_range(1, max_val))/5
			if add_number == 0.0:
				add_number = 0.2
			return add_number
		sub_op:
			var op_number = rand_range(0.2, 1.1)
			if op_number >= 1:
				op_number = 0.8
			elif op_number <= 0:
				op_number = 0.2
			return _round_to_one_decimal(op_number)
		mul_op:
			var mul_numbers = [1.2, 1.25, 1.5, 1.75]
			var selected = mul_numbers[randi() % mul_numbers.size()]
			return selected
		div_op:
			return _round_to_one_decimal(rand_range(1.1, 2) + randf()/5)
		div_pow, div_root:
			var r_numbers = [1.1, 1.2, 1.3, 1.4, 1.5]
			return (r_numbers[randi() % r_numbers.size()])/5
		div_rot:
			var rotation_degrees = [45.0, 90.0, -45.0, -90.0, 180.0, -180.0]
			return rotation_degrees[randi() % rotation_degrees.size()]
		_:
			return 1.0


func _ready():
	print(generate_problem(game_level))
	print(generate_problem_value(operation_helper.GetAddition(), game_level))
