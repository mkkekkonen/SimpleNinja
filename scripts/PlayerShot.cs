namespace SimpleNinja;

public partial class PlayerShot : AnimatableBody2D
{
  public float Speed = 300.0f;

  public override void _Ready()
  {
  }

  public override void _Process(double delta)
  {
  }

  public override void _PhysicsProcess(double delta)
  {
    base._PhysicsProcess(delta);

    MoveAndCollide(new Vector2((float)(Speed * delta), 0));
  }
}
