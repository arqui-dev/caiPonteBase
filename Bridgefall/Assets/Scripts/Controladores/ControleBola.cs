using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControleBola : MonoBehaviour
{
	// Variáveis públicas
	public GameObject som;

	// Variáveis privadas
	float 		potencia = 10;
	Rigidbody2D corpo = null;
	float 		tempoCriada = 0;
	int			rebatidas = 0;
	bool 		bateuArvoreMaca = false;

	bool 		destruir = false;
	bool 		destruirPorPassante = false;

	// Destruir bola caso tenha mais que o limite
	static int					bolasRodando = 0;
	static List<ControleBola> 	bolas = new List<ControleBola>();

	Rigidbody2D rigidBody;

	static void VerificarLimiteBolas(ControleBola cb)
	{
		bolasRodando++;
		bolas.Add(cb);

		if (bolasRodando > Dados.bolasMaximasPorVez)
		{
			ControleBola b = bolas[0];
			bolas.RemoveAt(0);
			b.Destruir();
		}
	}

	// Pega o corpo do objeto, e aplica a força inicial.
	void Criar (float pot = 0)
	{
		if (pot > 0){
			potencia = pot;
		}else{
			potencia = Dados.bolaPotencia;
		}

		corpo = GetComponent<Rigidbody2D>();

		VerificarLimiteBolas(this);
	}

	void Lancar(){
		//this.tempoCriada = Time.time;
		corpo.AddRelativeForce(
			potencia * Vector2.up,
			ForceMode2D.Impulse);
	}

	void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		this.tempoCriada = Time.time;
	}

	public void CriarELancar(float pot = 0)
	{
		Criar (pot);
		Lancar();
	}

	// Verifica se a bola deve ser destruída, seja por sair da tela,
	// ou por ficar muito lenta.
	void FixedUpdate()
	{
		VerificarDestruirVelocidade();

		if (gameObject == null){
			return;
		}

		if (transform.position.x < -Dados.bolaLimitesPosicao.x ||
		    transform.position.x >  Dados.bolaLimitesPosicao.x ||
		    transform.position.y < -Dados.bolaLimitesPosicao.y ||
		    transform.position.y >  Dados.bolaLimitesPosicao.y)
		{
			//Debug.Log("Destruiu por sair da tela, pos: "+
			//          transform.position);
			Destruir();
		}

		if (Dados.vento){
			corpo.AddForce(
				Dados.ventoVelocidade *
				Dados.ventoDirecao *
				Time.fixedDeltaTime);
		}
	}

	void Update()
	{
		if (Time.time > this.tempoCriada + Dados.bolaLimiteTempo)
		{
			Debug.Log("Destruiu por muito tempo rebatendo: "
			          +(Time.time - tempoCriada));

			destruir = true;
			destruirPorPassante = false;
		}

		if (destruir)
		{
			Destruir(destruirPorPassante);
		}
	}

	void VerificarDestruirVelocidade()
	{
		if (destruir == false && 
		    rigidBody.velocity.magnitude < 
		    Dados.bolaLimiarVelocidadeDestruir)
		{
			Debug.Log (
				"Bola velocidade: "+rigidBody.velocity.magnitude+
				"; Limiar: "+Dados.bolaLimiarVelocidadeDestruir);
			Dados.bolasLancadasNestaFase--;
			destruir = true;
		}
	}

	// Eliminar o objeto e fazer todo o necessário.
	public void Destruir(bool destruidoPorPassante = false)
	{
		if (bateuArvoreMaca && destruidoPorPassante == false)
		{
			Dados.bolasLancadasNestaFase--;
		}
		bolasRodando--;

		Destroy(gameObject);
	}

	public int Rebatidas()
	{
		return rebatidas;
	}

	// Colisão
	void OnCollisionEnter2D(Collision2D col)
	{
		if (gameObject == null){
			return;
		}

		/*
		if (col.gameObject.tag == Dados.tagPassador)
		{
			destruir = true;
			destruirPorPassante = true;
		}
		else 
		//*/
		if (col.gameObject.tag != Dados.tagPassador &&
		    col.gameObject.tag != Dados.tagRebatedorDestrutivel)
		{
			if (gameObject == null){
				return;
			}

			rebatidas++;
			if (som && Dados.somLigado)
			{
				Instantiate(som, transform.position, transform.rotation);
			}

			if (col.gameObject.tag == Dados.tagArvoreMaca)
			{
				bateuArvoreMaca = true;
			}
		}
		else if (col.gameObject.tag == Dados.tagRebatedorDestrutivel)
		{
			Destruir();
		}
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.tag == Dados.tagPassador)
		{
			destruir = true;
			destruirPorPassante = true;
		}
	}
}
