using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PontosTelaVitoria : MonoBehaviour
{
	public GameObject somReceberPontos;

	public Image painelPontos;
	public Text textoPontos;

	public Image painelBonus;
	public Image painelPerfeito;
	public Image painelOnus;

	public Text textoBonus;
	public Text textoPerfeito;
	public Text textoOnus;

	public Text textoMacas;
	public Image imgMaca;

	public float tempoEntreAtualizacoes = 0.5f;
	public float tempoBrilho = 0.25f;

	string baseTextoPontos = "Pontos: ";
	string baseTextoBonus = "Pontos: ";
	string baseTextoPerfeito = "Pontos: ";
	string baseTextoOnus = "Pontos: ";

	BrilharImagemUI [] paineis;
	Text [] textos;
	string [] baseTextos;
	int [] pontos;
	string [] txtAdicionar;
	int [] multi;

	float tempoProximaAtualizacao = 0;

	int pontosAtuais = 0;

	int painelAtual = 1;
	int totalPaineis = 4;

	void Awake()
	{
		if (textoMacas != null && imgMaca != null)
		{
			if (Dados.macasVerdesUltimaTela <= 0)
			{
				textoMacas.enabled = false;
				imgMaca.enabled = false;
			}
			else
			{
				textoMacas.enabled = true;
				imgMaca.enabled = true;
				textoMacas.text = "+"+Dados.macasVerdesUltimaTela;
			}
		}

		Utilidade.AjeitarMacasVerdes();

		/*
		Dados.pontosUltimaFasePassantes = 10;
		Dados.pontosUltimaFaseBonus = 10;
		Dados.pontosUltimaFasePerfeita = 10;
		Dados.pontosUltimaFaseOnus = 10;
		//*/
		/*
		baseTextoPontos = textoPontos.text.Split('0')[0];
		baseTextoBonus = textoBonus.text.Split('+')[0];
		baseTextoPerfeito = textoPerfeito.text.Split('+')[0];
		baseTextoOnus = textoOnus.text.Split('-')[0];
		//*/

		baseTextoPontos = ControleIdioma.PegarTexto(
			Idiomas.Texto.MostrarPontos).Split('0')[0];
		baseTextoBonus = ControleIdioma.PegarTexto(
			Idiomas.Texto.MostrarBonus).Split('+')[0];
		baseTextoPerfeito = ControleIdioma.PegarTexto(
			Idiomas.Texto.MostrarPerfect).Split('+')[0];
		baseTextoOnus = ControleIdioma.PegarTexto(
			Idiomas.Texto.MostrarOnus).Split('-')[0];

		if (Dados.modoDeJogo == ModosDeJogo.Normal)
		{

			textoBonus.text = 
				baseTextoBonus + "+" + 
					Dados.pontosUltimaFaseBonus;

			textoPerfeito.text = 
				baseTextoPerfeito + "+" + 
					Dados.pontosUltimaFasePerfeita;
			
			textoOnus.text = 
				baseTextoOnus + "-" +
					Dados.pontosUltimaFaseOnus;

			textos = new Text[totalPaineis - 1];
			baseTextos = new string[totalPaineis - 1];
			txtAdicionar = new string[totalPaineis - 1];
			multi = new int[totalPaineis - 1];
			pontos = new int[totalPaineis - 1];
			paineis = new BrilharImagemUI[totalPaineis];
			paineis[0] = painelPontos.GetComponent<BrilharImagemUI>();

			totalPaineis = 1;
			if (Dados.pontosUltimaFaseBonus > 0)
			{
				painelBonus.gameObject.SetActive(true);
				pontos[totalPaineis - 1] = Dados.pontosUltimaFaseBonus;
				textos[totalPaineis - 1] = textoBonus;
				txtAdicionar[totalPaineis - 1] = "+";
				multi[totalPaineis - 1] = 1;
				baseTextos[totalPaineis - 1] = baseTextoBonus;
				paineis[totalPaineis] = 
					painelBonus.GetComponent<BrilharImagemUI>();
				totalPaineis++;
			}
			else
			{
				painelBonus.gameObject.SetActive(false);
			}

			if (Dados.pontosUltimaFasePerfeita > 0)
			{
				painelPerfeito.gameObject.SetActive(true);
				pontos[totalPaineis - 1] = Dados.pontosUltimaFasePerfeita;
				textos[totalPaineis - 1] = textoPerfeito;
				txtAdicionar[totalPaineis - 1] = "+";
				multi[totalPaineis - 1] = 1;
				baseTextos[totalPaineis - 1] = baseTextoPerfeito;
				paineis[totalPaineis] = 
					painelPerfeito.GetComponent<BrilharImagemUI>();
				totalPaineis++;
			}
			else
			{
				painelPerfeito.gameObject.SetActive(false);
			}

			if (Dados.pontosUltimaFaseOnus > 0)
			{
				painelOnus.gameObject.SetActive(true);
				pontos[totalPaineis - 1] = Dados.pontosUltimaFaseOnus;
				textos[totalPaineis - 1] = textoOnus;
				txtAdicionar[totalPaineis - 1] = "-";
				multi[totalPaineis - 1] = -1;
				baseTextos[totalPaineis - 1] = baseTextoOnus;
				paineis[totalPaineis] = 
					painelOnus.GetComponent<BrilharImagemUI>();
				totalPaineis++;
			}
			else
			{
				painelOnus.gameObject.SetActive(false);
			}
		}

		Mostrar ();
	}

	public void MostrarNovamente()
	{
		if (painelAtual >= totalPaineis)
		{
			painelAtual = 1;
			pontosAtuais = 0;

			for (int i = 1; i < totalPaineis; i++)
			{
				paineis[i].Reiniciar();
				textos[i - 1].text = 
					baseTextos[i - 1] + 
						txtAdicionar[i - 1] +
						pontos[i - 1];
			}

			Mostrar();
		}
	}

	void Mostrar()
	{
		pontosAtuais = Dados.pontosUltimaFasePassantes;

		AtualizarPontos();

		tempoProximaAtualizacao = 
			Time.time + tempoEntreAtualizacoes * 1.25f;
	}

	void Update()
	{
		// ANALYTICS posicao toque
		if (Input.GetMouseButtonDown(0))
		{
			UnityAnalytics.AdicionarPontoTocado();
		}

		if (Dados.modoDeJogo != ModosDeJogo.Normal)
		{
			return;
		}
		Atualizar();
		//MostrarPontos();
	}

	void AtualizarPontos(int pontosExtra = 0)
	{
		if (Dados.modoDeJogo == ModosDeJogo.Normal)
		{
			pontosAtuais += pontosExtra * multi[painelAtual - 1];
		}


		MostrarPontos();
	}

	void MostrarPontos()
	{
		textoPontos.text = baseTextoPontos + pontosAtuais;
	}

	void Atualizar()
	{
		if (painelAtual < totalPaineis)
		{
			if (Time.time > tempoProximaAtualizacao)
			{
				Brilhar();
				PegarProximosPontos();
				AjeitarTempoProximaAtualizacao();
				painelAtual++;
			}
		}
	}

	void PegarProximosPontos()
	{
		AtualizarPontos(pontos[painelAtual - 1]);
		textos[painelAtual - 1].text = 
			baseTextos[painelAtual - 1] + 
				txtAdicionar[painelAtual - 1] + "0";
	}

	void Brilhar()
	{
		if (somReceberPontos && Dados.somLigado){
			Instantiate(
				somReceberPontos, 
				transform.position,
				transform.rotation);
		}
		paineis[painelAtual].Brilhar(tempoBrilho);
		paineis[0].Brilhar(tempoBrilho);
	}

	void AjeitarTempoProximaAtualizacao()
	{
		tempoProximaAtualizacao = 
			Time.time + tempoEntreAtualizacoes + tempoBrilho;
	}
}

