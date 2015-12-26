using UnityEngine;
using System.Collections;

public class NaoMostrarGastarSeUltimaFase : MonoBehaviour
{
	void Awake()
	{
		bool mostrar = false;

		if (Dados.estatisticas.mundos[Dados.mundoAtual]
		    .fases.Count > Dados.faseAtual + 1)
		{
			if (Dados.estatisticas.mundos[Dados.mundoAtual]
			    .fases[Dados.faseAtual + 1].completo)
			{
				mostrar = false;
			}
			else
			{
				mostrar = true;
			}
		}
		else
		{
			if (Dados.estatisticas.mundos[Dados.mundoAtual]
			    .completo == false)
			{
				mostrar = false;
			}
			else
			{
				if (Dados.mundoAtual < Dados.totalDeMundos - 1)
				{
					mostrar = false;
				}
				else
				{
					mostrar = false;
				}
			}
		}

		gameObject.SetActive(mostrar);
	}
}

