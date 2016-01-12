using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TelaSemMacaDescricaoSobrevivencia : MonoBehaviour
{
	void Awake()
	{
		string texto = ControleIdioma.PegarTexto(
			Idiomas.Texto.DescricaoTelaSemMacaSobrevivencia);

		GetComponent<Text>().text = string.Format(
			texto, Dados.macasDivisorPontos);
	}
}

