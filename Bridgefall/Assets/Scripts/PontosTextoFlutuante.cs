using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PontosTextoFlutuante : MonoBehaviour
{
	public float duracao = 1;
	public float decrementoAlfa = 50;
	public float velocidade = 1;
	public Vector2 direcao = Vector2.up;
	//public Color corNormal = Color.cyan;
	public Color corPenalidade = Color.red;

	float tempo = 0;
	float decrementoAlfaTotal = 0;
	Color cor;
	Text texto;
	string textoPontos = "+0";
	bool penalidade = false;

	public void Criar(int pontos, int pontosExtras = 0)
	{
		if (pontosExtras > 0)
		{
			textoPontos = "+" + pontos +
				"\n<color=green>+"+pontosExtras+"</color>";
		}
		else
		{
			if (pontos >= 0)
			{
				textoPontos = "+" + pontos;
			}
			else
			{
				penalidade = true;
				textoPontos = pontos.ToString();
			}
		}

		Awake ();
	}

	void Awake()
	{
		texto = GetComponent<Text>();
		
		cor = texto.color;
		if (penalidade)	{
			cor = corPenalidade;
			texto.color = corPenalidade;
		}
		
		decrementoAlfaTotal = 
			(decrementoAlfa / 100) * (cor.a / duracao);
		
		tempo = Time.time + duracao;
	}

	void Update()
	{
		transform.Translate (direcao * velocidade * Time.deltaTime);

		cor.a -= decrementoAlfaTotal * Time.deltaTime;
		texto.color = cor;

		texto.text = textoPontos;

		if (Time.time > tempo)
		{
			Destroy (gameObject);
		}
	}
}

