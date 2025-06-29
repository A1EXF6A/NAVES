using Godot;

public interface IMovementStrategy
{
	void Move(Node2D entity, double delta, float speed, Vector2 direction);
	void Initialize(Node2D entity);
	Vector2 GetCurrentDirection();
	void SetDirection(Vector2 direction);
}
