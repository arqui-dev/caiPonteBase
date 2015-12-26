using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasAdProprio : MonoBehaviour
{
	public GameObject imgContador;
	public Text txtContador;
	public GameObject btFechar;

	public float tempoMinimoFechar = 5;
	public float tempoFecharAutomatico = 9;

	float proximoTempo = 0;
	bool podeFechar = false;

	float timeScaleAnterior = 1;

	void Awake()
	{
		btFechar.SetActive(false);
		imgContador.SetActive(true);
		podeFechar = false;
		proximoTempo = Time.realtimeSinceStartup + tempoMinimoFechar;
		timeScaleAnterior = Time.timeScale;
		Time.timeScale = 0;
	}

	void Update()
	{
		int tempo = (int) (proximoTempo - Time.realtimeSinceStartup);
		txtContador.text = "" + (tempo + 1);
		//Debug.Log ("Tempo "+tempo);

		if (Time.realtimeSinceStartup > proximoTempo)
		{
			if (podeFechar == false)
			{
				podeFechar = true;
				proximoTempo = Time.realtimeSinceStartup + 
					tempoFecharAutomatico;

				btFechar.SetActive(true);
				imgContador.SetActive(false);
			}
			else
			{
				Fechar();
			}
		}
	}

	public void Fechar()
	{
		Time.timeScale = timeScaleAnterior;
		Destroy(gameObject);
	}
}

