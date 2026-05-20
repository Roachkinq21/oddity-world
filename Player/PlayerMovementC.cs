using Godot;
using System;

public partial class PlayerMovementC : CharacterBody3D
{

	[Export(PropertyHint.Range, "0,4")]
	public float MovementSpeed { get; set; } = 2f;

	[Export(PropertyHint.Range, "0,10")]
	public float SprintMultiplier { get; set; } = 2f;

	[Export(PropertyHint.Range, "0,10")]
	public float LerpSpeed { get; set; } = 7f;

	[Export(PropertyHint.Range, "0,5")]
	public float AirLerpSpeed { get; set; } = 3f;

	[Export(PropertyHint.Range, "0,5")]
	public float MouseSens { get; set; } = 0.25f;
	Vector3 _direction = Vector3.Zero;

	[Export(PropertyHint.Range, "0,1")]
	public float _BobStrength { get; set; } = 0.01f;

	public float _BobSpeed { get; set; } = 100f;

	public float SprintFov { get; set; } = 85f;
	public float BaseFov { get; set; } = 75f;

	private Camera3D _camera;
	private Marker3D _head;

	private Vector3 _headInitialPosition;
	private float _bobWeight = 1.0f;
	private float _currentSpeed;

	public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public PlayerStateMachine StateMachine { get; private set; }

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("Head/Camera3D");
		_head = GetNode<Marker3D>("Head");
		StateMachine = GetNode<PlayerStateMachine>("PlayerStateMachine");

		// register this player instance
		Global.Player = this;

		Input.MouseMode = Input.MouseModeEnum.Captured;
		_headInitialPosition = _head.Position;
		_currentSpeed = MovementSpeed;

	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * MouseSens));
			_head.RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * MouseSens));
			_head.Rotation = new Vector3(
				Mathf.Clamp(_head.Rotation.X, -1.25f, 1.5f),
				_head.Rotation.Y,
				_head.Rotation.Z
			);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		

		float fDelta = (float)delta;
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity.Y -= Gravity * fDelta;
		}

		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");

		if (IsOnFloor())
		{
			Vector3 wishDir = Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y);
			if (wishDir.Length() > 0.01f)
				_direction = _direction.Lerp(wishDir.Normalized(), LerpSpeed * (float)delta);
			else
				_direction = _direction.Lerp(Vector3.Zero, LerpSpeed * (float)delta);
		}
		else
		{
			if (inputDir != Vector2.Zero)
			{
				_direction = _direction.Lerp(
					(Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized(),
					fDelta * AirLerpSpeed);
			}
		}

		if (_direction.Length() > 0.1f)
		{
			velocity.X = _direction.X * _currentSpeed;
			velocity.Z = _direction.Z * _currentSpeed;
		}
		else
		{
			velocity.X = 0f;
			velocity.Z = 0f;
		}

		Velocity = velocity;
		MoveAndSlide();
		Headbob();
		Pause();
		Sprint();
	}

	public void Headbob()
	{
		if (IsOnFloor() && _direction.Length() > 0.1f)
		{
			_bobWeight = Mathf.Lerp(_bobWeight, 1f, 0.1f); // Fade in
		}
		else
		{
			_bobWeight = Mathf.Lerp(_bobWeight, 0f, 0.1f); // Fade out
		}

		float bobY = Mathf.Sin((float)Time.GetTicksMsec() / 100f) * _BobStrength * _bobWeight;
		float bobX = Mathf.Sin((float)Time.GetTicksMsec() / 200f) * _BobStrength * _bobWeight;
		_head.Position = new Vector3(
			_headInitialPosition.X + bobX,
			_headInitialPosition.Y + bobY,
			_headInitialPosition.Z
		);
	}

	public void Pause()
	{
		if (Input.IsActionJustPressed("pause"))
		{
			GetTree().Quit();
		}
	}

	public void Sprint()
	{
		if (Input.IsActionPressed("shift"))
		{
			_currentSpeed = MovementSpeed * SprintMultiplier;
			_camera.Fov = Mathf.Lerp(_camera.Fov, SprintFov, 0.1f);
		}
		else
		{
			_currentSpeed = MovementSpeed;
			_camera.Fov = Mathf.Lerp(_camera.Fov, BaseFov, 0.1f);
		}
	}
}
