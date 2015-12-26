using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControleFasesLiberadas : MonoBehaviour
{
	public Text mundoAtual;
	public GameObject [] fases;

	string nomeFaseCompleta = "Completa";

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
		//Carregar();
	}

	void Carregar()
	{
		if (Dados.estatisticas.mundos.Count <= Dados.mundoAtual){
			return;
		}
		
		int totalFases = 
			Dados.estatisticas.mundos[Dados.mundoAtual].fases.Count;

		if (totalFases > fases.Length){
			totalFases = fases.Length;
		}

		for (int i = 0; i < fases.Length; i++)
		{
			fases[i].transform.FindChild(nomeFaseCompleta)
				.gameObject.SetActive(false);
		}
		
		for (int i = 0; i < totalFases; i++)
		{
			if (Dados.estatisticas.mundos[Dados.mundoAtual]
			    .fases[i].completo)
			{
				fases[i].transform.FindChild(nomeFaseCompleta)
					.gameObject.SetActive(true);
			}
			fases[i].GetComponent<Button>().interactable = true;
		}
		
		for(int i = totalFases; i < fases.Length; i++)
		{
			fases[i].GetComponent<Button>().interactable = false;
		}
		
		mundoAtual.text =
			ControleIdioma.PegarTexto(Idiomas.Texto.TextoMundo)
				+ " " + (Dados.mundoAtual + 1);

		recarregar = false;
	}
}
