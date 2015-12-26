using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TelaPausa : MonoBehaviour
{
	// Variáveis públicas
	public GameObject som;
	public GameObject telaPausa;
	public GameObject botaoPausa;
	public ControleJogo controleJogo;
	public Navegacao navegador;
	public Text textoMundoFase;

	string textoCampanha		= Dados.textosTelaPausa[0];
	string textoMundo			= Dados.textosTelaPausa[1];
	string textoFase			= Dados.textosTelaPausa[2];
	string textoJogoRapido 		= Dados.textosTelaPausa[3];
	string textoDificuldade		= Dados.textosTelaPausa[4];
	//string textoSobrevivencia 	= Dados.textosTelaPausa[3];
	//string textoOnda		 	= Dados.textosTelaPausa[6];


	// Variáveis estáticas

	// Métodos públicos
	public void Pausar()
	{
		textoCampanha = 
			ControleIdioma.PegarTexto(Idiomas.Texto.TextoModoNormal);
		textoMundo = 
			ControleIdioma.PegarTexto(Idiomas.Texto.TextoMundo);
		textoFase = 
			ControleIdioma.PegarTexto(Idiomas.Texto.TextoFase);
		textoJogoRapido = 
			ControleIdioma.PegarTexto(Idiomas.Texto.TextoModoJogoRapido);
		textoDificuldade = 
			ControleIdioma.PegarTexto(Idiomas.Texto.TextoDificuldade);

		if (som && Dados.somLigado)
		{
			Instantiate(som, Vector3.zero, Quaternion.identity);
		}

		Time.timeScale = Dados.fluxoTemporalPausado;
		Dados.pausado = true;

		botaoPausa.SetActive(false);
		telaPausa.SetActive(true);

		switch(Dados.modoDeJogo){
		case ModosDeJogo.Normal:
			textoMundoFase.text = 
				textoCampanha + "\n" + 
					textoMundo + " " + (Dados.mundoAtual + 1) + " : " +
					textoFase + " " + (Dados.faseAtual + 1);
			break;
		case ModosDeJogo.JogoRapido:
		case ModosDeJogo.Sobrevivencia:
			textoMundoFase.text = 
				textoJogoRapido+ "\n" + 
					textoDificuldade + " " + Dados.jogoRapidoDificuldade;
			break;
		default:
			textoMundoFase.text = "";
			break;
		}
	}

	public void Despausar()
	{
		Despausar(true);
	}

	public void Despausar(bool tocar)
	{
		if (tocar && som && Dados.somLigado)
		{
			Instantiate(som, Vector3.zero, Quaternion.identity);
		}

		telaPausa.SetActive(false);
		botaoPausa.SetActive(true);

		Time.timeScale = Dados.fluxoTemporalNormal;
		Dados.pausado = false;
	}

	public void Reiniciar()
	{
		Despausar();

		if (Dados.modoDeJogo == ModosDeJogo.Normal)
		{
			if (Dados.estatisticas.mundos[Dados.mundoAtual].
			    fases[Dados.faseAtual].completo == false)
			{
				if (!Utilidade.VerificarMacasJogar())
				{
					MensagemMacaNaoTem();
					return;
				}
				
				Utilidade.GastarMaca(1, true);
			}
		}

		//Dados.faseAnterior = Dados.faseAtual;
		Debug.Log ("AQUI reiniciar tela pausa");
		Application.LoadLevel(Application.loadedLevel);
	}

	void MensagemMacaNaoTem()
	{
		Debug.Log ("Nao tem maças o suficiente. Colocar som");
		ControleMensagens.AdicionarMensagem(
			Utilidade.MensagemSemMacas(), 0);
	}

	public void CarregarTelaMenu()
	{
		Despausar(false);
		navegador.CarregarTelaMenuVirandoPagina();
	}

	public void CarregarTelaFases()
	{
		Despausar(false);
		navegador.CarregarTelaEscolherFases();
	}
}

