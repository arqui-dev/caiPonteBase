using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlePassante : MonoBehaviour
{
	/* Classe para controlar os carinhas que passam, suas velocidades,
	 * local (ponte de cima ou de baixo) e direção, e também para
	 * sinalizar quando é acertado, para a classe ControleOndas.
	 */

	// Variáveis públicas
	public GameObject somAcertado;
	public GameObject somPassou;
	public GameObject passanteCaindo;
	public GameObject textoFlutuante;

	// Variáveis privadas
	Passante 	passante;
	float 		destino = 0;

	// Métodos públicos
	public void Criar(Passante p, float d){
		passante = p;
		destino = d;
	}

	// Métodos privados
	void Update(){
		Mover();
	}

	void Mover(){
		transform.Translate(passante.velocidade * Time.deltaTime, 0, 0);
		if (passante.velocidade > 0){
			if (transform.position.x > destino){
				Passou();
			}
		}else if (passante.velocidade < 0){
			if (transform.position.x < destino){
				Passou();
			}
		}

#if UNITY_EDITOR
		if (Input.GetKeyUp(KeyCode.S)){
			Acertado(Vector2.up);
		}
#endif
	}


	public void Acertado(Vector2 direcao, int rebatidas = 0)
	{
		int pontos = CalcularPontos();
		int pontosVelocidade = PontosPorVelocidade();

		//AtualizarPontosPorVelocidade(PontosPorVelocidade());

		int pontosRebatidas = AtualizarPontosPorRebatias(rebatidas);

		/*
		if (Dados.modoDeJogo != ModosDeJogo.Normal)
		{
			pontos *= Dados.jogoRapidoDificuldade;
			pontosVelocidade *= Dados.jogoRapidoDificuldade;
			pontosRebatidas *= Dados.jogoRapidoDificuldade;
		}
		*/

		CriarTextoFlutuante(
			pontos + pontosVelocidade, pontosRebatidas);

		ControleOndas.DerrubouPassante(
			passante, 
			pontos + pontosRebatidas + pontosVelocidade);

		if (somAcertado && Dados.somLigado){
			Instantiate(
				somAcertado,
				transform.position,
				transform.rotation);
		}

		GameObject pa = (GameObject) Instantiate(
			passanteCaindo, 
			transform.position, 
			Quaternion.identity);

		if (passante.direcao == Passante.Direcoes.ParaEsquerda)
		{
			pa.transform.localScale = new Vector3(
				-pa.transform.localScale.x,
				pa.transform.localScale.y,
				pa.transform.localScale.z);
		}

		pa.GetComponent<PassanteCaindo>().Criar(direcao);

		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == Dados.tagBola)
		{
			ControleBola bola = 
				col.gameObject.GetComponent<ControleBola>();

			Acertado(col.relativeVelocity, bola.Rebatidas());

			//*
			if (bola != null)
			{
				bola.Destruir(true);
			}
			//*/
		}
	}

	void Passou()
	{
		if (somPassou && Dados.somLigado){
			Instantiate(
				somPassou,
				transform.position,
				transform.rotation);
		}

		ControleOndas.Perder();

		Destroy(gameObject);
	}

	void CriarTextoFlutuante(int pontos, int pontosVel = 0)
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
			
			if (pontosVel > 0){
				tf.GetComponent<PontosTextoFlutuante>()
					.Criar(pontos, pontosVel);
			}else{
				tf.GetComponent<PontosTextoFlutuante>()
					.Criar(pontos);
			}
			
			tf.transform.SetParent(t, false);
			tf.transform.position = transform.position;
		}
	}

	int CalcularPontos()
	{
		float des = destino - 1;
		float posicao = transform.position.x + des;
		float distanciaTotal = des * 2;

		if (passante.velocidade < 0)
		{
			des = -destino - 1;
			distanciaTotal = des * 2;
			posicao = des - transform.position.x;
		}

		int porcentagem = (int) 
			(posicao * 100 / 
			(distanciaTotal * Dados.pontosBase)) + 1;

		int pontos = porcentagem * Dados.pontosBase;

		if (pontos < Dados.pontosBase)
		{
			pontos = Dados.pontosBase;
		}

		return pontos;
	}

	int PontosPorVelocidade()
	{
		float v = passante.velocidade;
		if (v < 0){
			v = -v;
		}

		if (v < 1){
			return Dados.pontosBase * 2;
		}
		if (v < 2){
			return Dados.pontosBase;
		}
		if (v < 5){
			return 0;
		}
		if (v < 9){
			return Dados.pontosBase * ((int) (v - 4));
		}
		return Dados.pontosBase * ((int)((v - 6) * 2));
		//	0	1-4	5	6	7	8	9	10	11	12
		//	4	0	4	8	12	16	24	32	40	48
	}

	void AtualizarPontosPorVelocidade(int p)
	{
		Dados.pontosUltimaFaseVelocidade += p;
	}

	static int AtualizarPontosPorRebatias(int rebatidas)
	{
		if (rebatidas > 0)
		{
			Dados.bolaRebatidasTotaisFase += rebatidas;

			int multi = 1;
			switch(rebatidas){
			case 1: multi = 1; break;
			case 2: multi = 4; break;
			case 3: multi = 9; break;
			case 4: multi = 12; break;
			case 5: multi = 15; break;
			case 6: multi = 20; break;
			case 7: multi = 24; break;
			case 8: multi = 27; break;
			case 9: multi = 30; break;
			case 10: multi = 32; break;
			case 11: multi = 34; break;
			default: multi = rebatidas + 23; break;
			}

			int pontos = Dados.pontosBase * multi *
				Dados.pontosMultiplicadorBaseRebatidas;

			//	0	1	2	3	4	5	6	7	8	9	10	11	12
			//	0	4	48	108	144	180	240	288	324	360	384	408	420

			Dados.pontosUltimaFaseRebatidas = Dados.pontosBase *
				rebatidas *	Dados.pontosMultiplicadorBaseRebatidasTotais;

			return pontos;
		}
		return 0;
	}
}

