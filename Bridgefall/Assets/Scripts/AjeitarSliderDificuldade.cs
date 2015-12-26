using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AjeitarSliderDificuldade : MonoBehaviour 
{
	public Text textoDificuldade;

	void Awake()
	{
		GetComponent<Slider>().value = Dados.jogoRapidoDificuldade;
		textoDificuldade.text = "" + Dados.jogoRapidoDificuldade;
	}

	public void AlterarDificuldadeJogoRapido(Slider sliderDif)
	{
		int dif = (int) sliderDif.value;
		textoDificuldade.text = "" + dif;
		Dados.jogoRapidoDificuldade = dif;
	}
}
