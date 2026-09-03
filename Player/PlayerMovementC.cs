using Godot;

namespace OddityWorld.Player;

public partial class PlayerMovementC : CharacterBody3D
{
    [Export(PropertyHint.Range, "0,4")] public float MovementSpeed { get; set; } = 2f;

    [Export(PropertyHint.Range, "0,10")] public float SprintMultiplier { get; set; } = 2f;

    [Export(PropertyHint.Range, "0,10")] public float LerpSpeed { get; set; } = 7f;

    [Export(PropertyHint.Range, "0,5")] public float AirLerpSpeed { get; set; } = 3f;

    [Export(PropertyHint.Range, "0,5")] public float MouseSens { get; set; } = 0.25f;

    [Export(PropertyHint.Range, "0,5")] public float JoystickLookSensitivity { get; set; } = 1.5f;

    Vector3 _direction = Vector3.Zero;

    [Export(PropertyHint.Range, "0,1")] public float BobStrength { get; set; } = 0.01f;
    
    [ExportGroup("Camera")]
    public float BaseBobSpeed { get; private set; } = 12f;
    public float SprintBobSpeed { get; private set; } = 24f;

    public float SprintFov { get; private set; } = 85f;
    public float BaseFov { get; private set; } = 75f;

    private CollisionShape3D _collision;
    private CollisionShape3D _collision3DCrouch;

    public Camera3D Camera;
    private Marker3D _head;
    

    [ExportCategory("Weapon Sway")] 
    private Vector3 _weaponInitPosition;
    private Node3D _fpsGun;
    private float _baseWeaponBobSpeed;
    private float _baseWeaponBobWeight;
    private float _weaponSwayAmount = 0.03f;
    

    private Vector3 _headInitialPosition;
    private float _bobWeight = 1.0f;
    public float CurrentSpeed;
    public float CurrentBobSpeed;
    private float _bobTime;

    public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    private Vector2 _mouseInput;


    public PlayerStateMachine StateMachine { get; private set; }

    public override void _Ready()
    {
        Camera = GetNode<Camera3D>("Head/Camera3D");
        _head = GetNode<Marker3D>("Head");
        _collision = GetNode<CollisionShape3D>("Collision");
        _collision3DCrouch = GetNode<CollisionShape3D>("Collision3dCrouch");
        StateMachine = GetNode<PlayerStateMachine>("PlayerStateMachine");
        _fpsGun = GetNode<Node3D>("Head/Camera3D/FPS_Gun");

        // register this player instance
        Global.Player = this;

        Input.MouseMode = Input.MouseModeEnum.Captured;
        _headInitialPosition = _head.Position;
        _weaponInitPosition = _fpsGun.Position;
        CurrentSpeed = MovementSpeed;
        CurrentBobSpeed = BaseBobSpeed;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventMouseMotion mouseMotion) return;
        RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * MouseSens));
        _head.RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * MouseSens));
        _head.Rotation = new Vector3(
            Mathf.Clamp(_head.Rotation.X, -1.25f, 1.5f),
            _head.Rotation.Y,
            _head.Rotation.Z
        );
        _mouseInput = mouseMotion.Relative;
    }

    public override void _PhysicsProcess(double delta)
    {
        var fDelta = (float)delta;
        var velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity.Y -= Gravity * fDelta;
        }

        var inputDir = Input.GetVector("left", "right", "up", "down");
        var lookDir = Input.GetVector("look_left", "look_right", "look_up", "look_down");

        if (lookDir.Length() > 0.1f)
        {
            RotateY(-lookDir.X * JoystickLookSensitivity * fDelta);

            _head.RotateX(-lookDir.Y * JoystickLookSensitivity * fDelta);

            _head.Rotation = new Vector3(
                Mathf.Clamp(_head.Rotation.X, -1.25f, 1.5f),
                _head.Rotation.Y,
                _head.Rotation.Z
            );
        }


        if (IsOnFloor())
        {
            Vector3 wishDir = Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y);
            _direction = _direction.Lerp(wishDir.Length() > 0.01f ? wishDir : Vector3.Zero, LerpSpeed * (float)delta);
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
        WeaponSway(delta);
        // Sprint(delta);
    }

    private void Headbob(double delta)
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

        var bobY = Mathf.Sin(_bobTime) * BobStrength * _bobWeight;
        var bobX = Mathf.Sin(_bobTime * 0.5f) * BobStrength * _bobWeight;
        _head.Position = new Vector3(
            _headInitialPosition.X + bobX,
            _headInitialPosition.Y + bobY,
            _headInitialPosition.Z
        );
    }

    private void WeaponSway(double delta)
    {
        if (IsOnFloor() && _direction.Length() > 0.1f)
        {
            _baseWeaponBobWeight = Mathf.Lerp(_baseWeaponBobWeight, 1f, 0.1f); // Fade in
            _baseWeaponBobSpeed += (float)delta * CurrentBobSpeed;
        }
        else
        {
            _baseWeaponBobWeight = Mathf.Lerp(_baseWeaponBobWeight, 0f, 0.1f);
        }
        var swayY = Mathf.Sin(_baseWeaponBobSpeed) * BobStrength * _baseWeaponBobWeight;
        var swayX = Mathf.Sin(_baseWeaponBobSpeed * 0.3f) * BobStrength * _baseWeaponBobWeight;
        _fpsGun.Position = new Vector3(
            _weaponInitPosition.X + swayX,
            _weaponInitPosition.Y + swayY,
            _weaponInitPosition.Z
        );

    }

    private void Pause()
    {
        if (Input.IsActionJustPressed("pause"))
        {
            GetTree().Quit();
        }
    }

    private void Sprint(double delta)
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