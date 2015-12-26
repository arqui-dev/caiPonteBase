using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MostrarPontos : MonoBehaviour
{
	public Text textoPontos;
	string txtPontos = "Pontos";

	public Text textoOndas;
	//public string txtOndas = "ª Onda";

	void Awake()
	{
		txtPontos = ControleIdioma.PegarTexto(Idiomas.Texto.TextoPontos);

		textoOndas.enabled =
			Dados.modoDeJogo == ModosDeJogo.Sobrevivencia;

		// Aqui
		textoOndas.enabled = false;
	}

	void Update()
	{
		if (Dados.modoDeJogo != ModosDeJogo.Sobrevivencia)
		{
			textoPontos.text = 
				txtPontos + " " + Dados.pontosUltimaFasePassantes;
		}
		else
		{
			textoPontos.text = 
				txtPontos + " " + 
					(Dados.sobrevivenciaPontosPassantes + 
					 Dados.pontosUltimaFasePassantes);

			/*
			textoOndas.text = 
				""+(Dados.sobrevivenciaOndaAtual + 1) + txtOndas;
			*/
		}
	}
}

