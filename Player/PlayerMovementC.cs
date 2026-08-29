using Godot;

public partial class PlayerMovementC : CharacterBody3D
{
    [Export(PropertyHint.Range, "0,4")] public float MovementSpeed { get; set; } = 2f;

    [Export(PropertyHint.Range, "0,10")] public float SprintMultiplier { get; set; } = 2f;

    [Export(PropertyHint.Range, "0,10")] public float LerpSpeed { get; set; } = 7f;

    [Export(PropertyHint.Range, "0,5")] public float AirLerpSpeed { get; set; } = 3f;

    [Export(PropertyHint.Range, "0,5")] public float MouseSens { get; set; } = 0.25f;
    Vector3 _direction = Vector3.Zero;

    [Export(PropertyHint.Range, "0,1")] public float BobStrength { get; set; } = 0.01f;

    public float BaseBobSpeed { get; set; } = 12f;
    public float SprintBobSpeed { get; set; } = 24f;

    public float SprintFov { get; set; } = 85f;
    public float BaseFov { get; set; } = 75f;

    private CollisionShape3D _collision;
    private CollisionShape3D _collision3DCrouch;

    public Camera3D Camera;
    private Marker3D _head;

    private Vector3 _headInitialPosition;
    private float _bobWeight = 1.0f;
    public float CurrentSpeed;
    public float CurrentBobSpeed;
    private float _bobTime;

    public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public PlayerStateMachine StateMachine { get; set; }

    public override void _Ready()
    {
        Camera = GetNode<Camera3D>("Head/Camera3D");
        _head = GetNode<Marker3D>("Head");
        _collision = GetNode<CollisionShape3D>("Collision");
        _collision3DCrouch = GetNode<CollisionShape3D>("Collision3dCrouch");
        StateMachine = GetNode<PlayerStateMachine>("PlayerStateMachine");

        // register this player instance
        Global.Player = this;

        Input.MouseMode = Input.MouseModeEnum.Captured;
        _headInitialPosition = _head.Position;
        CurrentSpeed = MovementSpeed;
        CurrentBobSpeed = BaseBobSpeed;
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
                _direction = _direction.Lerp(wishDir, LerpSpeed * (float)delta);
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
            velocity.X = _direction.X * CurrentSpeed;
            velocity.Z = _direction.Z * CurrentSpeed;
        }
        else
        {
            velocity.X = 0f;
            velocity.Z = 0f;
        }

        Velocity = velocity;
        MoveAndSlide();
        Headbob(delta);
        Pause();
        // Sprint(delta);
    }

    public void Headbob(double delta)
    {
        if (IsOnFloor() && _direction.Length() > 0.1f)
        {
            _bobWeight = Mathf.Lerp(_bobWeight, 1f, 0.1f); // Fade in
            _bobTime += (float)delta * CurrentBobSpeed;
        }
        else
        {
            _bobWeight = Mathf.Lerp(_bobWeight, 0f, 0.1f); // Fade out
        }

        float bobY = Mathf.Sin(_bobTime) * BobStrength * _bobWeight;
        float bobX = Mathf.Sin(_bobTime * 0.5f) * BobStrength * _bobWeight;
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

    public void Sprint(double delta)
    {
        if (Input.IsActionPressed("shift"))
        {
            CurrentSpeed = MovementSpeed * SprintMultiplier;
            Camera.Fov = Mathf.Lerp(Camera.Fov, SprintFov, (float)delta * 10f);
            CurrentBobSpeed = Mathf.Lerp(CurrentBobSpeed, SprintBobSpeed, (float)delta * 10f);
        }
        else
        {
            CurrentSpeed = MovementSpeed;
            Camera.Fov = Mathf.Lerp(Camera.Fov, BaseFov, (float)delta * 10f);
            CurrentBobSpeed = Mathf.Lerp(CurrentBobSpeed, BaseBobSpeed, (float)delta * 10f);
        }
    }

    public void SetHeadHeight(float height)
    {
        Camera.Position = new Vector3(Camera.Position.X, height, Camera.Position.Z);
    }

    public void SetCrouch(bool crouching)
    {
        if (crouching)
        {
            _collision.Hide();
            _collision3DCrouch.Show();
        }
        else
        {
            _collision.Show();
            _collision3DCrouch.Hide();
        }
    }
}