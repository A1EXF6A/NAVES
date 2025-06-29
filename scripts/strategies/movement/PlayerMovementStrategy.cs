using Godot;

public class PlayerMovementStrategy : IMovementStrategy
{
	private Vector2 _direction = Vector2.Zero;
	
	public void Initialize(Node2D entity)
	{
		// Inicialización específica para jugador
	}
	
	public void Move(Node2D entity, double delta, float speed, Vector2 direction)
	{
		_direction = direction;
		
		if (entity is CharacterBody2D body)
		{
			body.Velocity = _direction * speed;
			body.MoveAndSlide();
			
			// Mantener al jugador dentro de la pantalla
			ClampToScreen(body);
		}
	}
	
	private void ClampToScreen(CharacterBody2D body)
	{
		body.GlobalPosition = body.GlobalPosition.Clamp(Vector2.Zero, body.GetViewportRect().Size);
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
