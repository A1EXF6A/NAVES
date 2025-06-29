using Godot;

public static class FileHelper
{
	public static uint LoadHighScore()
	{
		if (!FileAccess.FileExists(Constants.SaveGameFile))
		{
			return 0;
		}
		
		using var file = FileAccess.Open(Constants.SaveGameFile, FileAccess.ModeFlags.Read);
		if (file == null)
		{
			GD.PrintErr($"Failed to open save file for reading: {FileAccess.GetOpenError()}");
			return 0;
		}
		
		return file.Get32();
	}
	
	public static void SaveHighScore(uint highScore)
	{
		using var file = FileAccess.Open(Constants.SaveGameFile, FileAccess.ModeFlags.Write);
		if (file == null)
		{
			GD.PrintErr($"Failed to open save file for writing: {FileAccess.GetOpenError()}");
			return;
		}
		
		file.Store32(highScore);
		file.Flush();
	}
}
