using Godot;

public class LinearMovementStrategy : IMovementStrategy
{
	private Vector2 _direction = Vector2.Down;
	
	public void Initialize(Node2D entity)
	{
		// Inicialización básica para movimiento lineal
	}
	
	public void Move(Node2D entity, double delta, float speed, Vector2 direction)
	{
		_direction = direction;
		
		if (entity is Area2D area)
		{
			var newPosition = area.GlobalPosition + _direction * speed * (float)delta;
			area.GlobalPosition = newPosition;
		}
		else if (entity is CharacterBody2D body)
		{
			body.Velocity = _direction * speed;
			body.MoveAndSlide();
		}
	}
	
	public Vector2 GetCurrentDirection()
	{
		return _direction;
	}
	
	public void SetDirection(Vector2 direction)
	{
		_direction = direction;
	}
}
