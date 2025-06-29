using Godot;

public partial class ScoreManager : Node, IInitializable
{
	private uint _currentScore;
	private uint _highScore;
	
	[Signal] public delegate void ScoreChangedEventHandler(uint score);
	[Signal] public delegate void HighScoreChangedEventHandler(uint highScore);
	
	public uint CurrentScore => _currentScore;
	public uint HighScore => _highScore;
	
	public override void _Ready()
	{
		Initialize();
	}
	
	public void Initialize()
	{
		LoadHighScore();
	}
	
	public void AddScore(uint points)
	{
		_currentScore += points;
		EmitSignal(SignalName.ScoreChanged, _currentScore);
		
		if (_currentScore > _highScore)
		{
			_highScore = _currentScore;
			EmitSignal(SignalName.HighScoreChanged, _highScore);
			SaveHighScore();
		}
	}
	
	public void ResetScore()
	{
		_currentScore = 0;
		EmitSignal(SignalName.ScoreChanged, _currentScore);
	}
	
	private void LoadHighScore()
	{
		_highScore = SaveService.LoadHighScore();
		EmitSignal(SignalName.HighScoreChanged, _highScore);
	}
	
	private void SaveHighScore()
	{
		SaveService.SaveHighScore(_highScore);
	}
}
