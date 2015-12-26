using UnityEngine;
using System.Collections;

public class Passante
{
	public enum Direcoes {
		ParaDireita = 1, ParaEsquerda = -1
	}

	public Direcoes 	direcao = Direcoes.ParaDireita;
	public float 		velocidade = 1;
	public Pontes 		ponte = Pontes.Cima;

	public Passante(float vel, Pontes pon)
	{
		if (vel < 0){
			direcao = Direcoes.ParaEsquerda;
		}
		velocidade = vel;
		ponte = pon;
	}
}

