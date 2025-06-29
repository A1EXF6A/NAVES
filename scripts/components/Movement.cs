using Godot;

public partial class Movement : Node, IMovable
{
	[Export] public float Speed { get; set; } = 100.0f;
	[Export] public Vector2 Direction { get; set; } = Vector2.Down;
	
	private IMovementStrategy _movementStrategy;
	private Node2D _owner;
	
	public override void _Ready()
	{
		_owner = GetParent<Node2D>();
		
		// Estrategia por defecto
		if (_movementStrategy == null)
		{
			SetMovementStrategy(new LinearMovementStrategy());
		}
	}
	
	public void Move(double delta)
	{
		_movementStrategy?.Move(_owner, delta, Speed, Direction);
	}
	
	public void SetMovementStrategy(IMovementStrategy strategy)
	{
		_movementStrategy = strategy;
		_movementStrategy?.Initialize(_owner);
	}
	
	public IMovementStrategy GetMovementStrategy()
	{
		return _movementStrategy;
	}
	
	public void SetMovementParameters(float speed, Vector2 direction)
	{
		Speed = speed;
		Direction = direction;
		_movementStrategy?.SetDirection(direction);
	}
	
	// Métodos legacy para compatibilidad (DEPRECATED)
	public void MoveBody(CharacterBody2D body, double delta)
	{
		_movementStrategy?.Move(body, delta, Speed, Direction);
	}
	
	public void MoveArea(Area2D area, double delta)
	{
		_movementStrategy?.Move(area, delta, Speed, Direction);
	}
}
