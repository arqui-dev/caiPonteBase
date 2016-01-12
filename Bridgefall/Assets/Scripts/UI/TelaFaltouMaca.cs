using UnityEngine;
using System.Collections;

public class TelaFaltouMaca : MonoBehaviour
{
	void Awake()
	{
		Time.timeScale = Dados.fluxoTemporalPausado;
	}

	void OnDestroy()
	{
		Time.timeScale = Dados.fluxoTemporalNormal;
	}

	void OnDisable()
	{
		Time.timeScale = Dados.fluxoTemporalNormal;
	}

	public void IniciarVideo()
	{
		if (Random.value < 0.5f)
		{
			Debug.Log ("Mostrou video unity ads");
		}
		else
		{
			Debug.Log ("Mostrou video adbuddiz");
		}

		Fechar();
	}

	public void IniciarBanner()
	{
		AdicionarMacas(1);
		Debug.Log ("Mostrou banner AdBuddiz");
		Fechar();
	}

	public void IniciarSobrevivencia()
	{
		Debug.Log ("Iniciou sobrevivencia");
		Dados.jogoRapidoDificuldade = 1;
		Dados.modoDeJogo = ModosDeJogo.Sobrevivencia;
		Navegacao.CarregarTelaEstatico(Telas.Jogo);
		ControleMusica.MusicaSobrevivencia();
	}

	public void Fechar()
	{
		Time.timeScale = Dados.fluxoTemporalNormal;
		Destroy(gameObject);
	}

	public static void AdicionarMacas(int macas = 1)
	{
		Utilidade.AdicionarMacasPorQuantidade(macas);
		Utilidade.AjeitarMacasVerdes();
		UnityAnalytics.GanhouMaca(true, macas);
	}
}

