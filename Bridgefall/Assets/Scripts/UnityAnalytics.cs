using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class UnityAnalytics : MonoBehaviour
{
	static string efaseCompleta 		= "level complete";
	static string efasePerdeu			= "level lost";
	static string epontosTocados		= "touch point";
	static string epontosMaisTocados	= "most touched points";
	static string eVisualizarAds		= "Unity Ad shown";
	static string egastouMaca			= "Apple spent";
	static string eganhouMaca			= "Apple earned";

	static string smundo			= "world";
	static string sjogoRapido		= "quickplay";
	static string sfase				= "level";
	static string spassantes		= "passers down";
	static string spontos			= "score";
	static string stempo			= "time";
	static string stempoReal		= "real time taken";
	static string sbolas			= "balls";
	static string srebatedores		= "bouncers destroyed";

	static string sreiniciar		= "restarted level";
	static string sMacaDeAd			= "apple from Ad";
	static string sMacaQuantidade	= "apple amount";

	static string spontoTocado		= "point";
	static string spontoResolucao	= "screen resolution";
	static string spontoPorcento	= "percent";
	static string spontoArredondado = "round percent";
	static string spontoTela		= "scene";
	static string spontoPausado		= "paused";
	static string spontoTempo		= "time";

	static string sMaisTocadosTela	= "scene";

	static string sAdViuTudo		= "completed Ad";
	static string sAdRodando		= "Ads API working";
	static string sAdTotal			= "Total Ads completed";
	static string sAdIniciados		= "Total Ads started";
	static string sAdTempoTotal		= "time";

	static Dictionary <string, object> pontosMaisTocados =
		new Dictionary<string, object>();
	
	static int totalAdsVisualizados = 0;
	static int totalAdsCompletados = 0;

	static float tempoGastouMacaUltimaVez = 0;
	static float tempoGanhouMacaUltimaVez = 0;

	public static void GastouMaca(bool reiniciar = false)
	{
		Dictionary<string, object> dicionario = 
			new Dictionary<string, object>();

		tempoGastouMacaUltimaVez = Time.realtimeSinceStartup -
			tempoGastouMacaUltimaVez;

		dicionario.Add(smundo, Dados.mundoAtual);
		dicionario.Add(sfase, Dados.faseAtual);
		dicionario.Add(stempo, tempoGastouMacaUltimaVez);
		dicionario.Add(sreiniciar, reiniciar);

		Analytics.CustomEvent(egastouMaca, dicionario);
		
		Debug.Log ("Gastou maça enviado analytics");
	}

	public static void GanhouMaca(bool ad, int macas = 1)
	{
		Dictionary<string, object> dicionario = 
			new Dictionary<string, object>();
		
		tempoGanhouMacaUltimaVez = Time.realtimeSinceStartup -
			tempoGanhouMacaUltimaVez;

		if (Dados.modoDeJogo != ModosDeJogo.Normal)
		{
			dicionario.Add(sjogoRapido, Dados.jogoRapidoDificuldade);
			dicionario.Add(spontos, Dados.pontosUltimaFase);
		}
		else
		{
			dicionario.Add(smundo, Dados.mundoAtual);
			dicionario.Add(sfase, Dados.faseAtual);
		}
		dicionario.Add(sMacaQuantidade, macas);
		dicionario.Add(stempo, tempoGanhouMacaUltimaVez);
		dicionario.Add(sMacaDeAd, ad);
		
		Analytics.CustomEvent(eganhouMaca, dicionario);
		
		Debug.Log ("Ganhou maça enviado analytics");
	}

	public static void AbriuAd(bool viuTudo, bool adsRodando)
	{
		Dictionary<string, object> dicionario = 
			new Dictionary<string, object>();

		totalAdsVisualizados++;
		if (viuTudo)
			totalAdsCompletados++;

		dicionario.Add(sAdViuTudo, viuTudo);
		dicionario.Add(sAdRodando, adsRodando);
		dicionario.Add(sAdIniciados, totalAdsVisualizados);
		dicionario.Add(sAdTotal, totalAdsCompletados);
		dicionario.Add(sAdTempoTotal, Time.realtimeSinceStartup);

		Analytics.CustomEvent(eVisualizarAds, dicionario);

		Debug.Log ("Abriu ad enviado analytics");
	}

	public static void AdicionarPontoTocado()
	{
		Vector2 toque = (Vector2) Input.mousePosition;

		PontoTocado p = new PontoTocado(toque);

		Dictionary<string, object> dicionario = 
			new Dictionary<string, object>();

		string sponto = spontoTocado;

		sponto = spontoTocado;
		dicionario.Add(sponto, p.ponto);
		
		dicionario.Add(spontoResolucao, p.resolucao);
		dicionario.Add(spontoPorcento, p.pontoPorcento);
		dicionario.Add(spontoArredondado, p.pontoArredondado);
		dicionario.Add(spontoTela, p.tela);
		dicionario.Add(spontoTempo, p.tempo);
		
		if (Application.loadedLevelName != 
		    Dados.nomeTelas[(int)Telas.Jogo])
		{
			dicionario.Add(sponto + spontoPausado, false);
		}
		else
		{
			dicionario.Add(sponto + spontoPausado, 
			               Dados.pausado);
		}
		
		if (!pontosMaisTocados.
		    ContainsKey(p.pontoArredondado.ToString()))
		{
			pontosMaisTocados.Add(
				p.pontoArredondado.ToString(), 0);
		}
		
		int valor = (int) pontosMaisTocados
			[p.pontoArredondado.ToString()];
		
		valor++;
		
		pontosMaisTocados
			[p.pontoArredondado.ToString()] = valor;
		
		Analytics.CustomEvent(epontosTocados, dicionario);

		Debug.Log ("Ponto tocado enviado analytics");
	}
	
	public static void EnviarPontosMaisTocados()
	{
		lock (pontosMaisTocados)
		{
			try
			{
				if (pontosMaisTocados.Count > 0)
				{
					Analytics.CustomEvent(
						epontosMaisTocados, 
						pontosMaisTocados);

					pontosMaisTocados.Clear();

					pontosMaisTocados.Add(
						sMaisTocadosTela,
						Application.loadedLevelName);
				}
				else
				{
					pontosMaisTocados.Add(
						sMaisTocadosTela,
						Application.loadedLevelName);
				}
			}
			catch (UnityException e)
			{
				Debug.Log (e);
			}
		}
		Debug.Log ("Mais tocados enviado analytics");
	}

	public static void CompletouFase(
		int mundo, int fase, int passantes, int pontos,
		float tempo, float tempoReal,
		int bolas, int rebatedoresDestruidos)
	{
		Dictionary<string, object> dicionario = 
			new Dictionary<string, object>();

		if (mundo >= 0)
		{
			dicionario.Add(smundo, mundo);
			dicionario.Add(sfase, fase);
		}
		else
		{
			dicionario.Add(sjogoRapido, fase);
		}

		dicionario.Add(spassantes, passantes);
		dicionario.Add(spontos, pontos);
		dicionario.Add(stempo, tempo);
		dicionario.Add(stempoReal, tempoReal);
		dicionario.Add(sbolas, bolas);
		dicionario.Add(srebatedores, rebatedoresDestruidos);

		Analytics.CustomEvent(efaseCompleta, dicionario);

		Debug.Log ("Completou fase enviado analytics");
	}


	public static void PerdeuNaFase(
		int mundo, int fase, int passantes ,int pontos,
		float tempo, float tempoReal,
		int bolas, int rebatedoresDestruidos)
	{
		Dictionary<string, object> dicionario = 
			new Dictionary<string, object>();
		
		dicionario.Add(smundo, mundo);
		dicionario.Add(sfase, fase);
		dicionario.Add(spassantes, passantes);
		dicionario.Add(spontos, pontos);
		dicionario.Add(stempo, tempo);
		dicionario.Add(stempoReal, tempoReal);
		dicionario.Add(sbolas, bolas);
		dicionario.Add(srebatedores, rebatedoresDestruidos);
		
		Analytics.CustomEvent(efasePerdeu, dicionario);

		Debug.Log ("Perdeu fase enviado analytics");
	}
}

