using Godot;

public partial class GameOverScreen : Control, IInitializable
{
	private Label _scoreLabel;
	private Label _highScoreLabel;

	public override void _Ready()
	{
		Initialize();
	}

	public void Initialize()
	{
		_scoreLabel = GetNode<Label>("Panel/Score");
		_highScoreLabel = GetNode<Label>("Panel/HighScore");
	}

	public void SetScore(uint value)
	{
		if (_scoreLabel != null)
		{
			_scoreLabel.Text = $"Score: {value}";
		}
	}

	public void SetHighScore(uint value)
	{
		if (_highScoreLabel != null)
		{
			_highScoreLabel.Text = $"Hi-Score: {value}";
		}
	}

	public void OnRestartButtonPressed()
	{
		GetTree().ReloadCurrentScene();
	}
}
