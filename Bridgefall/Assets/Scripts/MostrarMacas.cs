using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MostrarMacas : MonoBehaviour 
{
	Text texto;

	void Awake()
	{
		texto = GetComponent<Text>();
		texto.text = "x" + Dados.macasVerdeTotal;
	}
	
	void Update ()
	{
		texto.text = "x" + Dados.macasVerdeTotal;
	}
}
