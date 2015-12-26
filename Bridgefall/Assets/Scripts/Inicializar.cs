using UnityEngine;
using System.Collections;



public class Inicializar : MonoBehaviour 
{
	/*
	public Transform painelDeslizar;
	public float velocidadeDeslizar = 0;
	public float tempoParaCarregar = 3;

	Vector3 movimento = Vector3.zero;
	float tempo = 0;
	//*/

	void Awake ()
	{


		Utilidade.CarregarDados();
		Destroy(gameObject);

		/*
		if (Navegacao.telaAtual != Telas.Menu)
		{
			movimento = new Vector3(0,velocidadeDeslizar,0);
			tempo = Time.time + tempoParaCarregar;
			Navegacao.CarregarTelaEstatico(Telas.Menu);
		}
		else
		{
			Destroy(gameObject);
		}
		//*/
	}

	/*
	void Update()
	{
		if (Time.time > tempo)
		{
			Navegacao.CarregarTelaEstatico(Telas.Menu);
		}
		else if (painelDeslizar)
		{
			painelDeslizar.Translate(movimento * Time.deltaTime);
		}
	}
	//*/
}
