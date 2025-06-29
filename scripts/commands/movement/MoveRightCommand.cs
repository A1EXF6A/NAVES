using Godot;

public class MoveRightCommand : MoveCommand
{
	public MoveRightCommand(Player player) : base(player, Vector2.Right)
	{
	}
	
	public override void Execute()
	{
		base.Execute();
		// Lógica adicional específica para movimiento hacia la derecha si es necesaria
	}
}
