using Godot;

public class SinusoidalMovementStrategy : IMovementStrategy
{
	private Vector2 _baseDirection = Vector2.Down;
	private float _amplitude = 50.0f;
	private float _frequency = 2.0f;
	private float _timeElapsed = 0.0f;
	private Vector2 _startPosition;
	private bool _initialized = false;
	
	public void Initialize(Node2D entity)
	{
		_startPosition = entity.GlobalPosition;
		_initialized = true;
		
		// Añadir variación aleatoria
		_frequency += GD.Randf() * 1.0f - 0.5f;
		_amplitude += GD.Randf() * 20.0f - 10.0f;
	}
	
	public void Move(Node2D entity, double delta, float speed, Vector2 direction)
	{
		if (!_initialized)
		{
			Initialize(entity);
		}
		
		_baseDirection = direction;
		_timeElapsed += (float)delta;
		
		// Movimiento sinusoidal horizontal
		float horizontalOffset = Mathf.Sin(_timeElapsed * _frequency) * _amplitude;
		
		if (entity is Area2D area)
		{
			// Movimiento vertical constante + oscilación horizontal
			var verticalMovement = _baseDirection * speed * (float)delta;
			var newPosition = area.GlobalPosition + verticalMovement;
			newPosition.X = _startPosition.X + horizontalOffset;
			area.GlobalPosition = newPosition;
		}
	}
	
	public Vector2 GetCurrentDirection()
	{
		return _baseDirection;
	}
	
	public void SetDirection(Vector2 direction)
	{
		_baseDirection = direction;
	}
}
