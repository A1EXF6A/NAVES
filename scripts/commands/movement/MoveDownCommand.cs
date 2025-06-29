using Godot;

public class MoveDownCommand : MoveCommand
{
	public MoveDownCommand(Player player) : base(player, Vector2.Down)
	{
	}
	
	public override void Execute()
	{
		base.Execute();
		// Lógica adicional específica para movimiento hacia abajo si es necesaria
	}
}
