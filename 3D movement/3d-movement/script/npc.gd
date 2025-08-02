extends Node3D


var player_in_range = false
@onready var interaction_icon: Area3D = $InteractionIcon
@onready var icon: Sprite3D = $Icon

var dialogue_lines: = [
	"Buy Omenveil.",
	{
		"type" : "choice",
		"prompt" : "You're still asking why?",
		"options" : [
			{"text" : "What's Omenveil?", "next_index" : 2},
			{"text" : "Nope, I'm sure", "next_index" : 3}			
		]
	},
	"It's the Game of the Century.",
	"It's so cool."
]

func _ready():
	icon.visible = false
	interaction_icon.body_entered.connect(_on_body_entered)
	interaction_icon.body_exited.connect(_on_body_exited)
	DialogueManager.connect("dialogue_ended", hide_icon)

func _on_body_entered(body):
	if body.name == "Player":
		body.current_npc = self
		player_in_range = true
		icon.visible = true

func _on_body_exited(body):
	if body.name == "Player":
		if body.current_npc == self:
			body.current_npc = null
		player_in_range = false
		icon.visible = false

func hide_icon():
	icon.visible = false

func get_dialogue() -> Array:
	return dialogue_lines
