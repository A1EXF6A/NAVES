using Godot;

public class MoveCommand : ICommand
{
	protected Player _player;
	protected Vector2 _direction;
	protected Vector2 _previousDirection;
	
	public MoveCommand(Player player, Vector2 direction)
	{
		_player = player;
		_direction = direction;
	}
	
	public virtual void Execute()
	{
		if (!CanExecute()) return;
		
		_previousDirection = _player.GetCurrentDirection();
		_player.SetMovementDirection(_direction);
	}
	
	public virtual void Undo()
	{
		if (_player != null)
		{
			_player.SetMovementDirection(_previousDirection);
		}
	}
	
	public virtual bool CanExecute()
	{
		return _player != null && _player.IsAlive;
	}
}
