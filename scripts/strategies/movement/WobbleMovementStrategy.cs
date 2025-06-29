using Godot;

public class WobbleMovementStrategy : IMovementStrategy
{
	private Vector2 _baseDirection = Vector2.Down;
	private float _wobbleAmplitude = 20.0f;
	private float _wobbleFrequency = 1.5f;
	private float _timeElapsed = 0.0f;
	private float _rotationSpeed = 1.5f;
	
	public void Initialize(Node2D entity)
	{
		// Configurar rotación aleatoria inicial
		entity.Rotation = GD.Randf() * Mathf.Tau;
		
		// Añadir variación aleatoria a la frecuencia
		_wobbleFrequency += GD.Randf() * 0.5f - 0.25f;
	}
	
	public void Move(Node2D entity, double delta, float speed, Vector2 direction)
	{
		_baseDirection = direction;
		_timeElapsed += (float)delta;
		
		// Añadir rotación continua para efecto visual
		entity.Rotation += _rotationSpeed * (float)delta;
		
		// Calcular movimiento serpenteante
		float horizontalOffset = Mathf.Sin(_timeElapsed * _wobbleFrequency) * _wobbleAmplitude;
		Vector2 wobbleDirection = new Vector2(horizontalOffset * 0.008f, 1.0f).Normalized();
		
		if (entity is Area2D area)
		{
			var newPosition = area.GlobalPosition + wobbleDirection * speed * (float)delta;
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
