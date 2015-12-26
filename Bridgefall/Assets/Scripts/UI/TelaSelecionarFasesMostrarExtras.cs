using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TelaSelecionarFasesMostrarExtras : MonoBehaviour
{
	public enum Tipo {
		Estrela, Coroa, Pontos
	}

	public Tipo tipo = Tipo.Estrela;
	public int fase = 0;

	public bool jogoRapido = false;

	Text texto;

	void Awake()
	{
		if (jogoRapido)
		{
			texto = GetComponent<Text>();
			return;
		}

		switch (tipo){
		case Tipo.Estrela: 	VerificarEstrela(); 		break;
		case Tipo.Coroa: 	VerificarPontosCoroa(); 	break;
		case Tipo.Pontos:	VerificarPontosTexto(); 	break;
		}
	}

	void Update()
	{
		if (jogoRapido == false || tipo != Tipo.Pontos) return;

		if (Dados.estatisticas.jogoRapido
		    .melhorPontuacao[Dados.jogoRapidoDificuldade-1] > 0)
		{
			texto.text = "" + Dados.estatisticas.jogoRapido
				.melhorPontuacao[Dados.jogoRapidoDificuldade-1];
		}
		else
		{
			texto.text = "0";
		}
	}

	void VerificarEstrela()
	{
		if (Dados.estatisticas.mundos[Dados.mundoAtual]
		    .fases.Count <= fase)
		{
			gameObject.SetActive(false);
			return;
		}

		if (Dados.estatisticas.mundos[Dados.mundoAtual]
		    .fases[fase].perfect == false)
		{
			gameObject.SetActive(false);
		}
	}

	void VerificarPontosCoroa()
	{
		if (Dados.estatisticas.mundos[Dados.mundoAtual]
		    .fases.Count <= fase)
		{
			gameObject.SetActive(false);
			return;
		}

		if (Dados.estatisticas.mundos[Dados.mundoAtual]
		    .fases[fase].melhorPontuacao <= 0)
		{
			gameObject.SetActive(false);
		}
	}

	void VerificarPontosTexto()
	{
		if (Dados.estatisticas.mundos[Dados.mundoAtual]
		    .fases.Count <= fase)
		{
			gameObject.SetActive(false);
			return;
		}

		int pontos = Dados.estatisticas.mundos[Dados.mundoAtual]
			.fases[fase].melhorPontuacao;

		if (pontos <= 0)
		{
			gameObject.SetActive(false);
		}
		else
		{
			GetComponent<Text>().text = "" + pontos;
		}
	}
}

