using UnityEngine;
using System.Collections;

public class ControleMusica : MonoBehaviour
{
	public static ControleMusica _instancia;

	public AudioClip [] musicas;
	
	//private static TocarSomAteOFim _instancia;
	static AudioSource musica = null;
	static AudioSource som = null;

	static bool tocarSom = false;
	bool estavaSemSom = false;

	Musicas musicaPassada = Musicas.Derrota;
	static Musicas musicaAtual = Musicas.Menu;
	static Musicas somAtual = Musicas.Nenhuma;
	
	void Awake(){
		if(_instancia != null && _instancia != this) 
		{
			DestroyImmediate(gameObject);
			return;
		}
		_instancia = this;

		DontDestroyOnLoad(gameObject);

		AudioSource [] audi = GetComponents<AudioSource>();

		if (audi != null)
		{
			if (audi.Length > 0){
				musica = audi[0];
				musica.loop = true;
			}
			if (audi.Length > 1){
				som = audi[1];
				som.loop = false;
			}else{
				Debug.LogWarning("O controlador de musica" +
					"precisa de 2 componentes AudioSource!");
			}
		}
	}

	void Update () {
		if (som == null || musica == null)
		{
			Destroy(gameObject);
			return;
		}

		if (Dados.musicaLigada)
		{
			if (estavaSemSom)
			{
				musica.UnPause();
				if (tocarSom)
				{
					som.UnPause();
				}
				estavaSemSom = false;
			}

			if (musicaAtual != musicaPassada)
			{
				musicaPassada = musicaAtual;
				musica.Stop();
				musica.clip = musicas[(int) musicaAtual];
				musica.Play();
			}

			if (tocarSom)
			{
				//tocarSom = false;
				if (som.isPlaying == false)
				{
					if (somAtual != Musicas.Nenhuma)
					{
						musica.Pause();
						som.clip = musicas[(int) somAtual];
						som.Play();
						somAtual = Musicas.Nenhuma;
					}
					else
					{
						tocarSom = false;
						musica.UnPause();
					}
				}
			}
		}
		else
		{
			musica.Pause();
			som.Pause();
			estavaSemSom = true;
		}
	}

	public static void ContinuarMusica()
	{
		if (Dados.musicaLigada && musica && musica.isPlaying == false)
		{
			tocarSom = false;
			somAtual = Musicas.Nenhuma;
			musica.UnPause();

			if (som.isPlaying)
			{
				som.Stop();
			}
		}
	}

	public static void MusicaJogar()
	{
		switch(Dados.modoDeJogo){
		case ModosDeJogo.Normal:
			switch(Dados.mundoAtual){
			case 0: musicaAtual = Musicas.Bluegrass; break;
			case 1: musicaAtual = Musicas.Parque; break;
			case 2: musicaAtual = Musicas.Rock; break;
			default: musicaAtual = Musicas.Piano; break;
			}
			break;
		case ModosDeJogo.JogoRapido:
		case ModosDeJogo.Sobrevivencia:
			musicaAtual = Musicas.Piano;
			break;
		}
	}

	public static void MusicaJogoRapido()
	{
		musicaAtual = Musicas.Parque;
	}

	public static void MusicaSobrevivencia()
	{
		musicaAtual = Musicas.Piano;
	}

	public static void MusicaMenu()
	{
		musicaAtual = Musicas.Menu;
	}

	public static void Vitoria()
	{
		somAtual = Musicas.Vitoria;
		tocarSom = true;
	}

	public static void Derrota()
	{
		somAtual = Musicas.Derrota;
		tocarSom = true;
	}

	public static void MusicaZerou()
	{
		musicaAtual = Musicas.Parque;
	}
}

