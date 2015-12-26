using UnityEngine;
using System.Collections;

public class TelaInicial : MonoBehaviour
{
	public float tempoMaximo = 6;
	
	float tempo = 0;
	
	void Awake()
	{
		tempo = Time.time + tempoMaximo;
	}
	
	void Update ()
	{
		if (Time.time > tempo)
		{
			CarregarMenu();
		}
		if (Input.GetMouseButtonUp(0))
		{
			CarregarMenu();
		}
	}
	
	void CarregarMenu()
	{
		Debug.Log ("AQUI MENU "+gameObject.name);
		Application.LoadLevel(Dados.nomeTelas[(int)Telas.Menu]);
	}
}

