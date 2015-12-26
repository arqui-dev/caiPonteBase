using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MostrarPontosTelaDerrota : MonoBehaviour
{
	void Awake()
	{
		GetComponent<Text>().text = 
			ControleIdioma.PegarTexto(Idiomas.Texto.MostrarPontos)
				.Split('0')[0]
					+ ": " + Dados.pontosUltimaFasePassantes;
	}
}

