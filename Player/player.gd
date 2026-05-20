extends CharacterBody3D


@onready var head: Marker3D = $Head

@export_range(0,4) var movement_speed : float  
@export_range(0,10) var lerp_speed : float 
@export_range(0,5) var air_lerp_speed : float 


@export_range(0,5) var mouse_sens : float = 0.25


var direction = Vector3.ZERO

var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		rotate_y(deg_to_rad(-event.relative.x * mouse_sens))
		head.rotate_x(deg_to_rad(-event.relative.y * mouse_sens))
		head.rotation.x = clamp(head.rotation.x, -1.25, 1.5)

func _process(delta: float) -> void:
	
	if not is_on_floor():
		velocity.y -= gravity * delta
	
	var input_dir = Input.get_vector("left", "right", "up", "down")
	
	if is_on_floor():
		direction = lerp(direction,(transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized(),delta*lerp_speed)
	else:
		if input_dir != Vector2.ZERO:
			direction = lerp(direction,(transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized(),delta*air_lerp_speed)
	if direction:
		velocity.x = direction.x * movement_speed
		velocity.z = direction.z * movement_speed
	move_and_slide()
