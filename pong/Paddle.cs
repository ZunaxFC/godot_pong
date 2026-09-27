
using Godot;

public partial class Paddle : CharacterBody2D
{
	[Export] public float Speed = 400.0f;
	[Export] public bool IsLeftPaddle = true;
	public bool IsActive = true;

	public override void _PhysicsProcess(double delta)
	{
		if (!IsActive) return;

		Vector2 velocity = Velocity;
		Vector2 direction = Vector2.Zero;

		if (IsLeftPaddle)
		{
			if (Input.IsActionPressed("paddle_left_up")) direction.Y -= 1;
			if (Input.IsActionPressed("paddle_left_down")) direction.Y += 1;
		}
		else
		{
			if (Input.IsActionPressed("paddle_right_up")) direction.Y -= 1;
			if (Input.IsActionPressed("paddle_right_down")) direction.Y += 1;
		}

		if (direction != Vector2.Zero)
		{
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
		
		// Ierobežojam raketes kustību ekrāna robežās (augšā un apakšā)
		Vector2 pos = Position;
		pos.Y = Mathf.Clamp(pos.Y, 60, 660);
		Position = pos;
	}
}
