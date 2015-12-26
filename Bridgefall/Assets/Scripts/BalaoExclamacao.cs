using UnityEngine;
using System.Collections;

public class BalaoExclamacao : MonoBehaviour
{
	public float duracao = 1;
	public float anguloMax = 10;

	float anguloAtual = 0;
	float velocidade = 1;
	int direcao = 1;
	float tempo = 0;
	Transform imagem;

	void Awake()
	{
		imagem = transform.GetChild(0);
		tempo = Time.time + duracao;
	}

	void Update()
	{
		Rotacionar();
		if (Time.time > tempo){
			Destroy(gameObject);
		}
	}

	void Rotacionar(){
		anguloAtual += 
			direcao *
				velocidade * 
				Dados.ventoVelocidade *
				Time.deltaTime;
		
		if (anguloAtual > anguloMax){
			anguloAtual = anguloMax;
			direcao = -1;
		}
		if (anguloAtual < -anguloMax){
			anguloAtual = -anguloMax;
			direcao = 1;
		}

		imagem.rotation = Quaternion.Euler(
			new Vector3(0,0, anguloAtual));
	}
}

