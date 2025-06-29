using Godot;

public interface IMovable
{
	Vector2 Direction { get; set; }
	float Speed { get; set; }
	void Move(double delta);
	void SetMovementStrategy(IMovementStrategy strategy);
	IMovementStrategy GetMovementStrategy();
}
