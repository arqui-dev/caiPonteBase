using UnityEngine;
using System.Collections;

public class ControleBarco : MonoBehaviour
{
	public Transform limite;

	int direcaoDoLimitador = 1;
	int direcao = 1;

	void Awake()
	{
		if (limite.position.x > 0)
		{
			direcaoDoLimitador = -1;
		}

		if (Utilidade.MeiaChance())
		{
			direcao = -1;
		}
	}

	void Update()
	{
		if (Dados.barcoMove)
		{
			transform.Translate(
				Dados.barcoVelocidade * Time.deltaTime * direcao,0,0);

			if (transform.position.x < 
			    limite.position.x * direcaoDoLimitador)
			{
				transform.position = new Vector3(
					limite.position.x * direcaoDoLimitador,
					transform.position.y,
					transform.position.z);
				direcao = 1;
			}

			if (transform.position.x > 
			    limite.position.x * -direcaoDoLimitador)
			{
				transform.position = new Vector3(
					limite.position.x * -direcaoDoLimitador,
					transform.position.y,
					transform.position.z);
				direcao = -1;
			}
		}
		else
		{
			//GetComponent<ControleBarco>().enabled = false;
		}
	}

	static public void AlterarSobrevivencia()
	{
		if (!Dados.barcoMove)
		{
			Dados.barcoMove = true;
			Dados.barcoVelocidade = Dados.BARCO_VELOCIDADE_LENTO;
		}
		else if (Dados.barcoVelocidade == Dados.BARCO_VELOCIDADE_LENTO)
		{
			Dados.barcoVelocidade = Dados.BARCO_VELOCIDADE_NORMAL;
		}
		else
		{
			Dados.barcoVelocidade = Dados.BARCO_VELOCIDADE_RAPIDO;
		}
	}
}

