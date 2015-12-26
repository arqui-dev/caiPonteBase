using UnityEngine;
using System.Collections;

public class SumirForaDoModoNormal : MonoBehaviour
{
	void Awake()
	{
		if (Dados.modoDeJogo != ModosDeJogo.Normal)
		{
			gameObject.SetActive(false);
		}
	}
}

