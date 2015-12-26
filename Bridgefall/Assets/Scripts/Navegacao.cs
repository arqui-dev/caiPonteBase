using UnityEngine;
using System.Collections;

public class Navegacao : MonoBehaviour
{
	// Variáveis Públicas
	public GameObject som = null;
	public GameObject somPagina = null;

	// Variáveis estáticas
	public static Telas telaAtual = Telas.Inicial;

	// Métodos estáticos
	public static void CarregarTelaEstatico(Telas tela)
	{
		telaAtual = tela;
		Debug.Log("AQUI estatico "+tela);
		Application.LoadLevel(Dados.nomeTelas[(int) tela]);
		UnityAnalytics.EnviarPontosMaisTocados();
	}

	// Métodos públicos
	public void MostrarAdMaca(GameObject botao)
	{
		GerenciadorUnityAds.ShowRewardedAd();
		if (botao != null)
			botao.SetActive(false);
	}

	public void CarregarTela(Telas tela, bool tocarSom = true)
	{
		if (tocarSom && som && Dados.somLigado)
		{
			Instantiate(som, Vector3.zero, Quaternion.identity);
		}

		telaAtual = tela;
		Debug.Log("AQUI "+tela);
		Application.LoadLevel(Dados.nomeTelas[(int) tela]);
		UnityAnalytics.EnviarPontosMaisTocados();
	}

	public void CarregarTelaMenu(){
		//CarregarTela(Telas.Menu);
		//ControleMusica.MusicaMenu();
		CarregarTelaMenuVirandoPagina();
	}

	public void CarregarTelaMenuVirandoPagina()
	{
		if (somPagina && Dados.somLigado)
		{
			Instantiate(somPagina, Vector3.zero, Quaternion.identity);
		}
		CarregarTela(Telas.Menu, false);
		//ControleMusica.MusicaMenu();
	}

	public void CarregarTelaModoJogo(){
		ControleTela.CarregarTela(
			Telas.EscolherMundo, GetComponent<Navegacao>());
	}

	public void CarregarTelaEscolherMundos()
	{
		Dados.modoDeJogo = ModosDeJogo.Normal;
		if (Dados.estatisticas.mundos.Count == 1){
			Dados.mundoAtual = 0;
			ControleTela.CarregarTela(
				Telas.EscolherFase, GetComponent<Navegacao>());
		}else{
			ControleTela.CarregarTela(
				Telas.EscolherMundo, GetComponent<Navegacao>());
		}
	}

	public void CarregarTelaVoltarDeEscolherFases()
	{
		if (Dados.estatisticas.mundos.Count == 1){
			ControleTela.CarregarTela(
				Telas.EscolherJogo, GetComponent<Navegacao>());
		}else{
			ControleTela.CarregarTela(
				Telas.EscolherMundo, GetComponent<Navegacao>());
		}
	}

	public void CarregarTelaEscolherFases(){
		ControleTela.CarregarTela(
			Telas.EscolherFase, GetComponent<Navegacao>());
	}

	public void CarregarJogoFase(int fase)
	{
		if (Dados.modoDeJogo == ModosDeJogo.Normal)
		{
			if (Dados.estatisticas.mundos[Dados.mundoAtual].
			    fases[fase].completo == false)
			{
				if (!VerificarGastarMaca(false))
				{
					MensagemMacaNaoTem();
					return;
				}
				else
				{
					VerificarGastarMaca();
				}
			}
		}

		//Dados.modoDeJogo = ModosDeJogo.Normal;
		if (somPagina && Dados.somLigado)
		{
			Instantiate(somPagina, Vector3.zero, Quaternion.identity);
		}
		Dados.faseAtual = fase;
		CarregarTela(Telas.Jogo, false);
	}

	public void CarregarMesmaFase()
	{
		if (Dados.modoDeJogo == ModosDeJogo.Normal)
		{
			if (Dados.estatisticas.mundos[Dados.mundoAtual].
			    fases[Dados.faseAtual].completo == false)
			{
				if (!VerificarGastarMaca(false))
				{
					MensagemMacaNaoTem();
					return;
				}
				else
				{
					VerificarGastarMaca(true);
				}
			}
		}

		//Dados.modoDeJogo = ModosDeJogo.Normal;
		CarregarTela(Telas.Jogo);
	}

	bool VerificarGastarMaca(
		bool gastar = true, bool reiniciar = false)
	{
		if (Dados.modoDeJogo == ModosDeJogo.Normal)
		{
			if (!Utilidade.VerificarMacasJogar())
			{
				if (gastar)
					MensagemMacaNaoTem();
				return false;
			}
			
			if (gastar)
				Utilidade.GastarMaca(1, reiniciar);
		}
		return true;
	}

	void MensagemMacaNaoTem()
	{
		Debug.Log ("Nao tem maças o suficiente. Colocar som");
		ControleMensagens.AdicionarMensagem(
			Utilidade.MensagemSemMacas(), 0);
	}

	public void CarregarProximaFase()
	{
		//*
		if (Dados.faseAtual < 8)
		{
			if (Dados.estatisticas.mundos[Dados.mundoAtual]
			    .fases.Count > Dados.faseAtual + 1 &&

				Dados.estatisticas.mundos[Dados.mundoAtual].
			    fases[Dados.faseAtual+1].completo == false)
			{
				if (!VerificarGastarMaca(false))
				{
					MensagemMacaNaoTem();
					return;
				}
			}
		}
		//*/

		//Dados.modoDeJogo = ModosDeJogo.Normal;
		ControleMusica.ContinuarMusica();

		if (Dados.estatisticas.mundos[Dados.mundoAtual]
		    .fases.Count > Dados.faseAtual + 1)
		{
			if (Dados.estatisticas.mundos[Dados.mundoAtual]
			    .fases[Dados.faseAtual + 1].completo)
			{
				CarregarTelaEscolherFases();
			}
			else
			{
				CarregarJogoFase(Dados.faseAtual + 1);
			}
		}
		else
		{
			if (Dados.estatisticas.mundos[Dados.mundoAtual]
			    .completo == false)
			{
				CarregarTelaEscolherFases();
			}
			else
			{
				if (Dados.mundoAtual < Dados.totalDeMundos - 1)
				{
					Dados.mundoAtual++;
					CarregarTelaEscolherFases();
				}
				else
				{
					CarregarTelaModoJogo();
				}
			}
		}
	}

	public void CarregarTelaJogoRapido()
	{
		CarregarTelaSobrevivencia();
		/*
		//CarregarTela(Telas.Jogo_JogoRapido);
		Dados.modoDeJogo = ModosDeJogo.JogoRapido;
		CarregarTela(Telas.Jogo);
		ControleMusica.MusicaJogoRapido();
		//*/
	}

	public void CarregarTelaSobrevivencia()
	{
		//CarregarTela(Telas.Jogo_Sobrevivencia);
		Dados.modoDeJogo = ModosDeJogo.Sobrevivencia;
		CarregarTela(Telas.Jogo);
		ControleMusica.MusicaSobrevivencia();
	}

	public void Facebook()
	{
		Application.OpenURL("https://www.facebook.com/Bridgefall-958452747513366/");
	}
}
