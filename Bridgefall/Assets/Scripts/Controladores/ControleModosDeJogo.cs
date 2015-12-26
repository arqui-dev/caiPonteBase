using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControleModosDeJogo : MonoBehaviour 
{
	public Button jogoRapido;
	public Button sobrevivencia;

	static bool recarregar = false;
	
	public static void Recarregar()
	{
		recarregar = true;
	}
	
	void Update()
	{
		if (recarregar)
		{
			Carregar();
		}
	}
	
	void Awake()
	{
		//Carregar ();
	}

	void Carregar()
	{
		jogoRapido.interactable = 
			Dados.estatisticas.jogoRapido.liberado;

		sobrevivencia.interactable = 
			Dados.estatisticas.sobrevivencia.liberado;

		recarregar = false;
	}
}
