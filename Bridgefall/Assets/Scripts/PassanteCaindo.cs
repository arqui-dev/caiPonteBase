using UnityEngine;
using System.Collections;

public class PassanteCaindo : MonoBehaviour
{
	/* Essa classa controla o carinha depois que ele é acertado,
	 * fazendo ele sair girando na direção que a bola o acertou.
	 * Se ele bater na terra, faz um som, se bater na água, cria
	 * um splash.
	 */

	// Variáveis públicas
	public GameObject 	splash;
	public GameObject	somAgua;
	public GameObject	somTerra;

	// Variáveis privadas
	float 		velocidade = 3f;
	float 		tempoCair = 0.75f;
	float 		rotacao = 180;
	Transform 	filho;
	float 		tempo = 0;
	Vector2		direcao = Vector2.up;
	float 		margem = Dados.margemEsquerda;

	// Métodos públicos
	public void Criar(Vector2 dir){
		margem = -Dados.margemEsquerda;
		filho = transform.GetChild(0);
		tempo = Time.time + tempoCair;
		if (Random.Range(0,2) > 0){
			rotacao = -rotacao;
		}
		direcao = -dir.normalized;
	}

	// Métodos privados
	void Update(){
		if (Dados.pausado == false){
			transform.Translate(direcao * velocidade * Time.deltaTime);

			filho.Rotate(new Vector3(0,0, rotacao * Time.deltaTime));

			if (Time.time > tempo){
				if (transform.position.x < margem &&
				    transform.position.x > -margem)
				{
					BateuAgua();
				}
				else
				{
					BateuTerra();
				}
			}
		}
	}

	void BateuAgua()
	{
		if (somAgua && Dados.somLigado){
			Instantiate(
				somAgua,
				transform.position,
				transform.rotation);
		}
		Instantiate(splash, filho.position, Quaternion.identity);
		Destroy(gameObject);
	}

	void BateuTerra(){
		if (somTerra && Dados.somLigado){
			Instantiate(
				somTerra,
				transform.position,
				transform.rotation);
		}
		Destroy(gameObject);
	}
}

