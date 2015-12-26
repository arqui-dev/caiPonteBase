using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControleMundosLiberados : MonoBehaviour
{
	public GameObject [] mundos;

	string nomeMundoCompleto = "Completo";

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
		//*
		if (Dados.estatisticas.mundos.Count <= Dados.mundoAtual){
			return;
		}
		//*/
		
		int totalMundos = Dados.estatisticas.mundos.Count;
		
		if (totalMundos > mundos.Length){
			totalMundos = mundos.Length;
		}

		for (int i = 0; i < mundos.Length; i++)
		{
			mundos[i].transform.FindChild(nomeMundoCompleto)
				.gameObject.SetActive(false);
		}
		
		for (int i = 0; i < totalMundos; i++)
		{

			if (Dados.estatisticas.mundos[i].completo)
			{
				mundos[i].transform.FindChild(nomeMundoCompleto)
					.gameObject.SetActive(true);
			}

			mundos[i].GetComponent<Button>().interactable = true;
		}
		
		for(int i = totalMundos; i < mundos.Length; i++)
		{
			mundos[i].GetComponent<Button>().interactable = false;
		}
		recarregar = false;
	}
}
