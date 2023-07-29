namespace SimpleNinja;

public partial class Player : CharacterBody2D
{
  private const int SHOT_WIDTH = 8;

  private PackedScene shot;

  public const float Speed = 300.0f;
  public const float JumpVelocity = -400.0f;

  // Get the gravity from the project settings to be synced with RigidBody nodes.
  public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

  public override void _Ready()
  {
    shot = GD.Load<PackedScene>("res://objects/player_shot.tscn");
  }

  public override void _Process(double delta)
  {
    base._Process(delta);

    var sprite = GetNode<Sprite2D>("Sprite2D");

    if (Input.IsActionJustPressed("shoot"))
    {
      Shoot();
    }

    if (Velocity.X < 0 && !sprite.FlipH)
    {
      sprite.FlipH = true;
    }
    else if (Velocity.X > 0 && sprite.FlipH)
    {
      sprite.FlipH = false;
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    Vector2 velocity = Velocity;

    // Add the gravity.
    if (!IsOnFloor())
      velocity.Y += gravity * (float)delta;

    // Handle Jump.
    if (Input.IsActionJustPressed("jump") && IsOnFloor())
      velocity.Y = JumpVelocity;

    // Get the input direction and handle the movement/deceleration.
    // As good practice, you should replace UI actions with custom gameplay actions.
    Vector2 direction = Input.GetVector("left", "right", "up", "down");
    if (direction != Vector2.Zero)
    {
      velocity.X = direction.X * Speed;
    }
    else
    {
      velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
    }

    Velocity = velocity;
    MoveAndSlide();
  }

  private void Shoot()
  {
    var shotInstance = (Node2D)shot.Instantiate();

    var sprite = GetNode<Sprite2D>("Sprite2D");
    var spriteWidth = sprite.Texture != null ? sprite.Texture.GetWidth() : 1;

    shotInstance.Position += new Vector2((spriteWidth / 2) + SHOT_WIDTH, 0);

    AddChild(shotInstance);
  }
}
