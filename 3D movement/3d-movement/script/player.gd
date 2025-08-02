extends CharacterBody3D


const SPEED = 1.0
const JUMP_VELOCITY = 4.5

var in_dialogue = DialogueManager.is_open
var current_npc: Node = null
var awaiting_choice = DialogueManager.awaiting_choice

func _physics_process(delta: float) -> void:
	in_dialogue = DialogueManager.is_open
	if not in_dialogue and Input.is_action_just_pressed("accept_dialogue") and not awaiting_choice:
		if current_npc and current_npc.player_in_range:
			start_dialogue(current_npc)
	
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta
	
	if not in_dialogue:
		handle_movement(delta)
	
	move_and_slide()
		
func handle_movement(delta):
	# Handle jump.
	if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var input_dir := Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
	var direction := (transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	if direction:
		velocity.x = direction.x * SPEED
		velocity.z = direction.z * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		velocity.z = move_toward(velocity.z, 0, SPEED)

func start_dialogue(npc):
	DialogueManager.start(npc.get_dialogue())
	in_dialogue = DialogueManager.is_open
