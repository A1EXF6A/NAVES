using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class CommandInvoker : Node
{
	private Queue<ICommand> _commandQueue = new();
	private Stack<ICommand> _commandHistory = new();
	private const int MAX_HISTORY_SIZE = 100;
	
	// Ejecutar comando inmediatamente
	public void ExecuteCommand(ICommand command)
	{
		if (command?.CanExecute() == true)
		{
			command.Execute();
			AddToHistory(command);
		}
	}
	
	// Encolar comando para ejecutar en el siguiente frame
	public void QueueCommand(ICommand command)
	{
		if (command?.CanExecute() == true)
		{
			_commandQueue.Enqueue(command);
		}
	}
	
	// Procesar comandos encolados (llamar desde _Process o _PhysicsProcess)
	public void ProcessQueuedCommands()
	{
		while (_commandQueue.Count > 0)
		{
			var command = _commandQueue.Dequeue();
			if (command.CanExecute())
			{
				command.Execute();
				AddToHistory(command);
			}
		}
	}
	
	// Deshacer último comando
	public void UndoLastCommand()
	{
		if (_commandHistory.Count > 0)
		{
			var lastCommand = _commandHistory.Pop();
			lastCommand.Undo();
		}
	}
	
	// Limpiar historial
	public void ClearHistory()
	{
		_commandHistory.Clear();
	}
	
	// Limpiar cola de comandos
	public void ClearQueue()
	{
		_commandQueue.Clear();
	}
	
	private void AddToHistory(ICommand command)
	{
		_commandHistory.Push(command);
		
		// Mantener tamaño máximo del historial
		if (_commandHistory.Count > MAX_HISTORY_SIZE)
		{
			var tempCommands = _commandHistory.ToArray();
			_commandHistory.Clear();
			
			// Conservar solo los últimos MAX_HISTORY_SIZE/2 comandos
			for (int i = 0; i < MAX_HISTORY_SIZE / 2; i++)
			{
				_commandHistory.Push(tempCommands[i]);
			}
		}
	}
	
	// Información de debug
	public int GetQueueSize() => _commandQueue.Count;
	public int GetHistorySize() => _commandHistory.Count;
}
