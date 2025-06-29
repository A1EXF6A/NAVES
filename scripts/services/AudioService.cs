using Godot;

public partial class AudioService : Node
{
	private static AudioService _instance;
	public static AudioService Instance => _instance;
	
	// Efectos de sonido existentes
	private AudioStreamPlayer _laserSound;
	private AudioStreamPlayer _hitSound;
	private AudioStreamPlayer _explodeSound;
	
	// Nuevo: Reproductor de música de fondo
	private AudioStreamPlayer _backgroundMusicPlayer;
	
	[Export] public AudioStream BackgroundMusic { get; set; }
	[Export] public float MusicVolume { get; set; } = 0.5f;
	[Export] public float SFXVolume { get; set; } = 0.7f;
	[Export] public bool AutoplayMusic { get; set; } = true;
	
	public override void _Ready()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			QueueFree();
			return;
		}
		
		InitializeAudioPlayers();
		
		// Reproducir música automáticamente si está configurada
		if (AutoplayMusic && BackgroundMusic != null)
		{
			PlayBackgroundMusic();
		}
	}
	
	private void InitializeAudioPlayers()
	{
		// Efectos de sonido existentes - with null checks
		_laserSound = GetNodeOrNull<AudioStreamPlayer>("LaserSound");
		_hitSound = GetNodeOrNull<AudioStreamPlayer>("HitSound");
		_explodeSound = GetNodeOrNull<AudioStreamPlayer>("ExplodeSound");
		
		// Nuevo: Reproductor de música
		_backgroundMusicPlayer = GetNodeOrNull<AudioStreamPlayer>("BackgroundMusic");
		
		// Configurar volúmenes
		SetVolumes();
	}
	
	private void SetVolumes()
	{
		if (IsInstanceValid(_backgroundMusicPlayer))
		{
			_backgroundMusicPlayer.VolumeDb = Mathf.LinearToDb(MusicVolume);
		}
		
		// Aplicar volumen SFX a todos los efectos
		var sfxVolumeDb = Mathf.LinearToDb(SFXVolume);
		if (IsInstanceValid(_laserSound)) _laserSound.VolumeDb = sfxVolumeDb;
		if (IsInstanceValid(_hitSound)) _hitSound.VolumeDb = sfxVolumeDb;
		if (IsInstanceValid(_explodeSound)) _explodeSound.VolumeDb = sfxVolumeDb;
	}
	
	// Métodos existentes para efectos de sonido - with validation
	public void PlayLaserShot()
	{
		if (IsInstanceValid(_laserSound))
		{
			_laserSound.Play();
		}
		else
		{
			GD.PrintErr("LaserSound AudioStreamPlayer is not valid or has been disposed");
		}
	}
	
	public void PlayHit()
	{
		if (IsInstanceValid(_hitSound))
		{
			_hitSound.Play();
		}
		else
		{
			GD.PrintErr("HitSound AudioStreamPlayer is not valid or has been disposed");
		}
	}
	
	public void PlayExplosion()
	{
		if (IsInstanceValid(_explodeSound))
		{
			_explodeSound.Play();
		}
		else
		{
			GD.PrintErr("ExplodeSound AudioStreamPlayer is not valid or has been disposed");
		}
	}
	
	// Nuevos métodos para música de fondo
	public void PlayBackgroundMusic()
	{
		if (IsInstanceValid(_backgroundMusicPlayer) && BackgroundMusic != null)
		{
			_backgroundMusicPlayer.Stream = BackgroundMusic;
			_backgroundMusicPlayer.Play();
		}
	}
	
	public void PlayBackgroundMusic(AudioStream musicStream)
	{
		if (IsInstanceValid(_backgroundMusicPlayer) && musicStream != null)
		{
			BackgroundMusic = musicStream;
			_backgroundMusicPlayer.Stream = musicStream;
			_backgroundMusicPlayer.Play();
		}
	}
	
	public void StopBackgroundMusic()
	{
		if (IsInstanceValid(_backgroundMusicPlayer))
		{
			_backgroundMusicPlayer.Stop();
		}
	}
	
	public void PauseBackgroundMusic()
	{
		if (IsInstanceValid(_backgroundMusicPlayer))
		{
			_backgroundMusicPlayer.StreamPaused = true;
		}
	}
	
	public void ResumeBackgroundMusic()
	{
		if (IsInstanceValid(_backgroundMusicPlayer))
		{
			_backgroundMusicPlayer.StreamPaused = false;
		}
	}
	
	public void SetMusicVolume(float volume)
	{
		MusicVolume = Mathf.Clamp(volume, 0.0f, 1.0f);
		if (IsInstanceValid(_backgroundMusicPlayer))
		{
			_backgroundMusicPlayer.VolumeDb = Mathf.LinearToDb(MusicVolume);
		}
	}
	
	public void SetSFXVolume(float volume)
	{
		SFXVolume = Mathf.Clamp(volume, 0.0f, 1.0f);
		SetVolumes(); // Reaplica los volúmenes
	}
	
	public bool IsMusicPlaying()
	{
		return IsInstanceValid(_backgroundMusicPlayer) ? _backgroundMusicPlayer.Playing : false;
	}
	
	// Clean up when the node is about to be freed
	public override void _ExitTree()
	{
		if (_instance == this)    
		{
			_instance = null;
		}
	}
}
