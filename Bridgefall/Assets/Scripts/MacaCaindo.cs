using UnityEngine;
using System.Collections;

public class MacaCaindo : MonoBehaviour
{
	public GameObject som;
	public GameObject textoFlutuante;
	public GameObject macaNoChao;

	public float velocidadeQueda = 2;
	public float tempoMechendo = 0.8f;

	Color [] corImagem = {
		Color.white, Color.yellow, Color.red, Color.cyan
	};

	Animator 	anim;
	int			cor = 0;
	bool		mecheu = false;
	bool		parou = false;
	float 		chao = 0;
	float 		tempo = 0;

	public void Criar(int c, float p)
	{
		cor = c;

		chao = p;

		GetComponent<SpriteRenderer>().color = corImagem[c];

		anim = GetComponent<Animator>();

		Pontuar ();

		tempo = Time.time + tempoMechendo;
	}

	void Update()
	{
		if (parou == false && anim.IsInTransition(0))
		{
			parou = true;
			GameObject m = (GameObject) Instantiate(
				macaNoChao, transform.position, Quaternion.identity);
			
			m.GetComponent<SpriteRenderer>().color = corImagem[cor];
			m.GetComponent<Desvanecer>().Awake();
			
			Destroy (gameObject);
		}



		if (mecheu && transform.position.y > chao)
		{
			transform.Translate(0, -velocidadeQueda * Time.deltaTime, 0);
			if (transform.position.y < chao)
			{
				transform.position = new Vector3 (
					transform.position.x, chao, transform.position.z);
			}
		}
		else if (Time.time > tempo)
		{
			mecheu = true;
		}

	}

	void Pontuar()
	{
		int pontos = Dados.pontosBase * 
			Dados.pontosMultiplicadorMaca[cor];

		Dados.pontosUltimaFasePassantes += pontos;

		CriarTextoFlutuante(pontos);
	}

	void CriarTextoFlutuante(int pontos)
	{
		Transform t = 
			GameObject.FindWithTag(Dados.tagPainelPrincipal).transform;
		
		if (t != null)
		{
			GameObject tf = (GameObject) 
				Instantiate (
					textoFlutuante,
					Vector3.zero,
					Quaternion.identity);
			
			tf.GetComponent<PontosTextoFlutuante>()
				.Criar(pontos);

			tf.transform.SetParent(t, false);
			tf.transform.position = transform.position;
		}
	}
}

