using UnityEngine;
using System.Collections;

public class MostrarOuNaoMacaVerde : MonoBehaviour
{
	public int fase = -1;
	public bool botaoPassarDeFase = false;

	void Awake()
	{
		bool mostrar = false;

		if (fase >= 0)
		{
			if (Dados.estatisticas.mundos.Count > Dados.mundoAtual &&

			    Dados.estatisticas.mundos[Dados.mundoAtual].
			    fases.Count > fase &&
				
			    Dados.estatisticas.mundos[Dados.mundoAtual].
			    fases[fase].completo == false)
			{
				mostrar = true;
			}
		}
		else if (Dados.modoDeJogo == ModosDeJogo.Normal)
		{
			if (botaoPassarDeFase == false)
			{
				if (Dados.estatisticas.mundos[Dados.mundoAtual].
				    fases[Dados.faseAtual].completo == false)
				{
					mostrar = true;
				}
			}
			else
			{
				if (Dados.estatisticas.mundos[Dados.mundoAtual]
				    .fases.Count > Dados.faseAtual + 1 &&
				    
				    Dados.estatisticas.mundos[Dados.mundoAtual].
				    fases[Dados.faseAtual+1].completo == false)
				{
					mostrar = true;
				}
			}
		}



		gameObject.SetActive(mostrar);
	}
}

