using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MostrarPontuacao : MonoBehaviour
{
	public Text textoDescricao;
	public Text textoPontos;
	public RectTransform painelTexto;
	public Scrollbar barraLateral;

	string campanha = "Campanha";
	string mundo = "Mundo";
	string fase = "Fase";
	string jogoRapido = "Jogo Rápido";
	string dificuldade = "Dificuldade";
	//string sobrevivencia = "Sobrevivência";
	//string sobronda = "Ondas";
	//string sobrpontos = "Pontos";

	static string pl = "\n";

	public void Carregar()
	{
		campanha = ControleIdioma.PegarTexto(
			Idiomas.Texto.TextoModoNormal);
		mundo = ControleIdioma.PegarTexto(
			Idiomas.Texto.TextoMundo);
		fase = ControleIdioma.PegarTexto(
			Idiomas.Texto.TextoFase);
		jogoRapido = ControleIdioma.PegarTexto(
			Idiomas.Texto.TextoModoJogoRapido);
		dificuldade = ControleIdioma.PegarTexto(
			Idiomas.Texto.TextoDificuldade);

		int linhas = 0;

		string descri = campanha + pl;
		string pontos = pl;

		linhas += 1;

		for (int m = 0; m < Dados.estatisticas.mundos.Count; m++)
		{
			linhas += 1;
			pontos += pl;
			descri += " " + mundo  + " " + (m + 1) + pl;
			for(int f = 0; f < Dados.estatisticas
			    .mundos[m].fases.Count; f++)
			{
				linhas++;
				descri += "  " + fase + " " + (f + 1) + pl;
				pontos += "" + Dados.estatisticas.mundos[m]
					.fases[f].melhorPontuacao + pl;
			}
		}

		linhas += 2;
		descri += pl + jogoRapido + pl;
		pontos += pl + pl;
		for (int d = 0; d < Dados.jogoRapidoDificuldadeMaxima; d++)
		{
			linhas++;
			pontos += "" + Dados.estatisticas.jogoRapido
				.melhorPontuacao[d] + pl;
			descri += " " + dificuldade + " " + (d + 1) + pl;
		}

		/*
		if (Dados.estatisticas.sobrevivencia.liberado)
		{
			linhas += 3;
			descri += pl + sobrevivencia + pl + 
				sobronda + pl + sobrpontos;
			pontos += pl + pl + Dados.estatisticas.sobrevivencia
				.melhorOnda	+ pl + Dados.estatisticas
					.sobrevivencia.melhorPontuacao;
		}
		*/

		float tamanhoLinha = textoDescricao.fontSize 
			* textoDescricao.lineSpacing * 0.9f;

		painelTexto.sizeDelta = new Vector2(
			painelTexto.sizeDelta.x, linhas * tamanhoLinha);

		//Debug.Log ("Linhas: "+ linhas + ", Tam: "+tamanhoLinha);

		textoDescricao.text = descri;
		textoPontos.text = pontos;

		//painelTexto.position = new Vector3(
		//	painelTexto.position.x, 0, painelTexto.position.z);
		barraLateral.value = 1;
	}
}

