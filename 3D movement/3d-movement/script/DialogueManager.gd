extends Node

var DialogueBoxScene = preload("res://scene/db_canvaslayer.tscn")
var dialogue_box : CanvasLayer = null
var is_open = false
var just_open = false
var current_lines: Array = []
var current_index = 0
var awaiting_choice := false
@onready var player: CharacterBody3D = $Player
@onready var npc: Node3D = $npc

signal dialogue_ended

func start(lines: Array):
	if not dialogue_box:
		dialogue_box = DialogueBoxScene.instantiate()
		get_tree().get_root().add_child(dialogue_box)
	
	current_lines = lines
	current_index = 0
	is_open = true
	just_open = true
	dialogue_box.visible = true
	dialogue_box.get_node("DialogueBox/Background/dialoguetext").text = current_lines[0]

func _process(_delta):
	if just_open:
		just_open = false
		return
	
	if is_open and Input.is_action_just_pressed("accept_dialogue"):
		if awaiting_choice == false:
			current_index += 1
			show_line()
		else:
			show_line()

func end():
	dialogue_box.visible = false
	is_open = false
	emit_signal("dialogue_ended")
	get_node("res://scene/player.tscn")

func show_line():
	if current_index >= current_lines.size():
		end()
	else:
		var c_lines = current_lines[current_index]
		if typeof(c_lines) == TYPE_DICTIONARY and c_lines.has("type") and c_lines.type == "choice":
			dialogue_box.get_node("DialogueBox/Background/dialoguetext").text = current_lines[current_index].prompt
			print("Debug2")
			if Input.is_action_just_pressed("accept_dialogue"):
				dialogue_box.visible = false
				show_choices(c_lines)
			else:
				pass
		else:
			print("Debug1")
			dialogue_box.get_node("DialogueBox/Background/dialoguetext").text = current_lines[current_index]


func show_choices(choice_data):
	awaiting_choice = true
	
	var choice_container = dialogue_box.get_node("DialogueBox/Background/choices")
	choice_container.visible = true
	clear_children(choice_container)
	
	for option in choice_data.options:
		var button = Button.new()
		choice_container.add_child(button)
		button.position = Vector2(600, 600)
		print(button.position)
		print(option.text)
		button.text = option.text
		button.pressed.connect(func():
			current_index = option.next_index
			choice_container.visible = false
			awaiting_choice = false
			dialogue_box.visible = true
			show_line()
			)

func clear_children(node):
	for child in node.get_children():
		child.queue_free()
