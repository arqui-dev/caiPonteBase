using UnityEngine;
using System.Collections;
//using UnityEngine.Advertisements;

public class GerenciadorUnityAds : MonoBehaviour
{
	public GameObject _canvasAdProprio;

	static GameObject canvasAdProprio;

	void Awake()
	{
		canvasAdProprio = _canvasAdProprio;
	}

	public static bool Inicializado()
	{
		//return Advertisement.isInitialized;
		return false;
	}

	public static bool ConexaoInternet()
	{
		Debug.Log ("Conexao internet: "+
		           Application.internetReachability);
		if (Application.internetReachability == 
		    NetworkReachability.NotReachable)
		{
			return false;
		}
		return true;
	}

	public static void ShowRewardedAd()
	{
		//MostrarAdProprio();

		if (!Inicializado() || !ConexaoInternet())
		{
			Debug.Log ("Unity ads not initialized.");
			//Utilidade.AdicionarMensagemNaoViuAd();
			MostrarAdProprio();
			return;
		}

		/*
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions 
			{ 
				resultCallback = HandleShowResult
			};
			Advertisement.Show("rewardedVideo", options);
		}
		//*/
	}

	/*
	static void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			AdicionarMacas();
			UnityAnalytics.AbriuAd(true, Inicializado());
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			//Utilidade.AdicionarMensagemNaoViuAd();
			MostrarAdProprio();
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			//Utilidade.AdicionarMensagemNaoViuAd();
			MostrarAdProprio();
			break;
		}
	}
	//*/

	static void AdicionarMacas()
	{
		Utilidade.AdicionarMacasPorQuantidade(1);
		Utilidade.AjeitarMacasVerdes();
		UnityAnalytics.GanhouMaca(true, 1);
	}

	static void MostrarAdProprio()
	{
		UnityAnalytics.AbriuAd(false, Inicializado());

		Debug.Log ("Mostrando Ad proprio.");
		AdicionarMacas();

		if (canvasAdProprio != null)
		{
			Instantiate(canvasAdProprio);
		}
		else
		{
			Debug.Log ("Canvas nulo");
		}
	}
}

