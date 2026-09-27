using Godot;

public partial class Ball : CharacterBody2D
{
	[Export] public float BaseSpeed = 500.0f;
	public int ExitSide { get; private set; } = 0;
	public bool IsMoving { get; private set; } = false;

	private Vector2 velocity = Vector2.Zero;

	public override void _PhysicsProcess(double delta)
	{
		if (!IsMoving) return;

		Velocity = velocity;
		var collision = MoveAndCollide(Velocity * (float)delta);

		if (collision != null)
		{
			var collider = collision.GetCollider();
			// Atlēkšana no objekta un neliels ātruma pieaugums
			velocity = velocity.Bounce(collision.GetNormal()) * 1.02f;
		}

		// Pārbaudām, vai bumba izlidojusi pa kreiso vai labo malu (ekrāna platums 1280)
		if (Position.X < 0)
		{
			ExitSide = -1; // Izgāja pa kreisi -> punkts labajam
			IsMoving = false;
		}
		else if (Position.X > 1280)
		{
			ExitSide = 1;  // Izgāja pa labi -> punkts kreisajam
			IsMoving = false;
		}
	}

	public void Serve(int direction)
	{
		ExitSide = 0;
		IsMoving = true;
		// Nejaušs vertikālais virziens servējot
		float randomY = (float)GD.RandRange(-0.5, 0.5);
		velocity = new Vector2(direction, randomY).Normalized() * BaseSpeed;
	}

	public void Stop()
	{
		IsMoving = false;
		velocity = Vector2.Zero;
	}

	public void ResetBall()
	{
		Stop();
		ExitSide = 0;
		Position = new Vector2(640, 360); // Centrs
	}
}
