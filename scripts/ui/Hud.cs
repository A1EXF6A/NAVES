using Godot;

public partial class Hud : Control, IInitializable
{
	private Label _scoreLabel;
	private Label _highScoreLabel;
	private Label _levelLabel;
	private Label _nextLevelLabel;

	public override void _Ready()
	{
		Initialize();
		SetScore(0);
		SetLevel(1);
		SetNextLevelProgress(0, 2000);
	}

	public void Initialize()
	{
		_scoreLabel = GetNode<Label>("Score");
		_highScoreLabel = GetNode<Label>("HighScore");
		
		// Nuevos labels para nivel - usando GetNodeOrNull para evitar errores si no existen
		_levelLabel = GetNodeOrNull<Label>("Level");
		_nextLevelLabel = GetNodeOrNull<Label>("NextLevel");
		
		// Si no existen los nodos, los creamos dinámicamente
		CreateMissingLevelLabels();
	}
	
	private void CreateMissingLevelLabels()
	{
		// Solo crear si no existen
		if (_levelLabel == null)
		{
			_levelLabel = new Label();
			_levelLabel.Name = "Level";
			_levelLabel.Text = "Level: 1";
			_levelLabel.Position = new Vector2(10, 60); // Posición debajo del score
			AddChild(_levelLabel);
		}
		
		if (_nextLevelLabel == null)
		{
			_nextLevelLabel = new Label();
			_nextLevelLabel.Name = "NextLevel";
			_nextLevelLabel.Text = "Next: 2000";
			_nextLevelLabel.Position = new Vector2(10, 90); // Posición debajo del level
			AddChild(_nextLevelLabel);
		}
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
	
	public void SetLevel(uint level)
	{
		if (_levelLabel != null)
		{
			_levelLabel.Text = $"Level: {level}";
		}
	}
	
	public void SetNextLevelProgress(uint currentScore, uint nextLevelScore)
	{
		if (_nextLevelLabel != null)
		{
			uint remaining = nextLevelScore - currentScore;
			_nextLevelLabel.Text = $"Next: {remaining}";
		}
	}
	
	// Método para mostrar mensaje de level up temporal
	public async void ShowLevelUpMessage(uint newLevel)
	{
		// Crear label temporal para mostrar "LEVEL UP!"
		var levelUpLabel = new Label();
		levelUpLabel.Text = $"LEVEL {newLevel}!";
		levelUpLabel.AddThemeStyleboxOverride("normal", new StyleBoxFlat());
		levelUpLabel.Position = new Vector2(GetViewportRect().Size.X / 2 - 50, GetViewportRect().Size.Y / 2);
		levelUpLabel.AddThemeColorOverride("font_color", Colors.Yellow);
		levelUpLabel.ZIndex = 100; // Asegurar que esté al frente
		
		AddChild(levelUpLabel);
		
		// Crear tween para animación
		var tween = CreateTween();
		tween.TweenProperty(levelUpLabel, "modulate:a", 0.0f, 2.0f);
		tween.TweenCallback(Callable.From(() => levelUpLabel.QueueFree()));
	}
}
