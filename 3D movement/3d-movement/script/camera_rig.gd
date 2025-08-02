extends Node3D


@export var player: Node3D
@export var follow_speed: float = 5.0
@export var follow_offset: Vector3 = Vector3(0, 5, -10)

func _process(delta: float) -> void:
	if player:
		var target_pos = player.global_transform.origin + follow_offset
		global_transform.origin = global_transform.origin.lerp(target_pos, delta * follow_speed)
		
