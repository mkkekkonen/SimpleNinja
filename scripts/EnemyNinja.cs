// Maybe move this global using to some kind of Global.cs or GameManager.cs script
// later on
global using Godot;

namespace SimpleNinja;

public partial class EnemyNinja : CharacterBody2D
{
  public const float SPEED = 150.0f;
  public const float JUMP_VELOCITY = -400.0f;

  public Vector2 Direction = new(1, 0);

  // Get the gravity from the project settings to be synced with RigidBody nodes.
  public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

  public override void _Process(double delta)
  {
    base._Process(delta);

    var sprite = GetNode<Sprite2D>("Sprite2D");

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

    HandleDirection();

    // Get the input direction and handle the movement/deceleration.
    // As good practice, you should replace UI actions with custom gameplay actions.
    if (Direction != Vector2.Zero)
    {
      velocity.X = Direction.X * SPEED;
    }
    else
    {
      velocity.X = Mathf.MoveToward(Velocity.X, 0, SPEED);
    }

    Velocity = velocity;
    MoveAndSlide();
  }

  private void HandleDirection()
  {
    var direction = Direction;

    var rayCastLeft = GetNode<RayCast2D>("RayCast2DLeft");
    var rayCastRight = GetNode<RayCast2D>("RayCast2DRight");

    var endOfPlatform = rayCastLeft.GetCollider() == null
      || rayCastRight.GetCollider() == null;

    if (IsOnWall() || endOfPlatform)
    {
      Direction = direction * new Vector2(-1, 0);
    }
  }
}
