using Godot;

public partial class Main : Node2D
{
	[Export] public int TargetScore = 5;

	public int ScoreLeft { get; private set; } = 0;
	public int ScoreRight { get; private set; } = 0;
	public bool GameOver { get; private set; } = false;

	private Ball ball;
	private Paddle left;
	private Paddle right;
	private Label score;
	private Label message;
	private int serveDirection = 1;

	public override void _Ready()
	{
		ball = GetNode<Ball>("Ball");
		left = GetNode<Paddle>("PaddleLeft");
		right = GetNode<Paddle>("PaddleRight");
		score = GetNode<Label>("HUD/Score");
		message = GetNode<Label>("HUD/Message");
		
		PrepareServe();
		UpdateScore();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("restart"))
		{
			GetTree().ReloadCurrentScene();
			return;
		}
		if (GameOver) return;

		if (ball.ExitSide != 0)
		{
			AddPoint(ball.ExitSide > 0);
			return;
		}

		if (!ball.IsMoving && Input.IsActionJustPressed("serve"))
		{
			ball.Serve(serveDirection);
			message.Text = "Spēle sākusies!";
		}
	}

	private void AddPoint(bool leftScored)
	{
		if (leftScored) ScoreLeft++;
		else ScoreRight++;
		UpdateScore();

		if (ScoreLeft >= TargetScore || ScoreRight >= TargetScore)
		{
			GameOver = true;
			ball.Stop();
			string winner = leftScored ? "Kreisais spēlētājs" : "Labais spēlētājs";
			message.Text = $"Uzvar {winner}! Nospied R jaunai partijai.";
		}
		else
		{
			serveDirection = -serveDirection;
			PrepareServe();
		}
	}

	private void PrepareServe()
	{
		ball.ResetBall();
		left.Position = new Vector2(60, 360);
		right.Position = new Vector2(1220, 360);
		message.Text = "Nākamā izspēle: nospied Space!";
	}

	private void UpdateScore()
	{
		if (score != null)
			score.Text = $"{ScoreLeft} : {ScoreRight}";
	}
}
