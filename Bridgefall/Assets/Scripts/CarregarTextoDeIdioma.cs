using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarregarTextoDeIdioma : MonoBehaviour
{
	public Idiomas.Texto texto = Idiomas.Texto.TextoPontos;

	SystemLanguage idiomaAnterior = SystemLanguage.Unknown;

	void Awake()
	{
		//GetComponent<Text>().text = ControleIdioma.PegarTexto(texto);
		//idiomaAnterior = Application.systemLanguage;
	}

	void Update()
	{
		if (idiomaAnterior != ControleIdioma.lingua)
		{
			idiomaAnterior = ControleIdioma.lingua;
			GetComponent<Text>().text = ControleIdioma.PegarTexto(texto);
		}
	}
}

