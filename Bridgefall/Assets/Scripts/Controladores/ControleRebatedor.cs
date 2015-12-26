using UnityEngine;
using System.Collections;

public class ControleRebatedor : MonoBehaviour
{
	// Variáveis públicas
	public GameObject somDestruir;
	public GameObject splash;
	public GameObject textoFlutuante;

	// Variáveis privadas
	Rebatedores.Tipo 	tipo = Rebatedores.Tipo.Fixo;

	// Variáveis de movimentação
	Vector3 	posInicial;
	int 		direcao = 1;
	int 		direcaoExtra = 1;
	float 		distancia = 0;
	float 		distanciaExtra = 0;
	float 		velocidade = 5;
	float 		limitePositivo = 0;
	float 		limiteNegativo = 0;
	float		posicaoExtraPositivo = 0;
	float		posicaoExtraNegativo = 0;
	float 		raioHorizontal = Dados.rebatedorDistanciaHorizontal;
	float 		raioVertical = Dados.rebatedorDistanciaVertical * 0.5f;

	[HideInInspector]
	public int	posicaoGrade = 0;

	// Variáveis dos destrutíveis
	bool	destrutivel = false;
	bool	morto = false;

	// Métodos públicos
	public void Criar(
		Rebatedores.Tipo t, bool d, Vector3 p, int posGrade)
	{
		tipo = t;
		destrutivel = d;
		posInicial = p;
		posicaoGrade = posGrade;

		switch(tipo){
		case Rebatedores.Tipo.HorizontalEsqDir:
			limiteNegativo = p.x;
			limitePositivo = p.x + 2 * Dados.rebatedorDistanciaHorizontal;
			distancia = limiteNegativo;
			direcao = 1;
			velocidade = Dados.rebatedorHorizontalVelocidade;
			break;
		case Rebatedores.Tipo.HorizontalDirEsq:
			limitePositivo = p.x;
			limiteNegativo = p.x - 2 * Dados.rebatedorDistanciaHorizontal;
			distancia = limitePositivo;
			direcao = -1;
			velocidade = Dados.rebatedorHorizontalVelocidade;
			break;
		case Rebatedores.Tipo.VerticalCimaBaixo:
			limitePositivo = p.y;
			limiteNegativo = p.y - Dados.rebatedorDistanciaVertical;
			distancia = limitePositivo;
			direcao = 1;
			velocidade = Dados.rebatedorVerticalVelocidade;
			break;
		case Rebatedores.Tipo.VerticalBaixoCima:
			limiteNegativo = p.y;
			limitePositivo = p.y + Dados.rebatedorDistanciaVertical;
			distancia = limiteNegativo;
			direcao = -1;
			velocidade = Dados.rebatedorVerticalVelocidade;
			break;
		case Rebatedores.Tipo.DiagonalDirEsqBaixoCima:
			limitePositivo = p.x;
			limiteNegativo = p.x - 2 * Dados.rebatedorDistanciaHorizontal;
			distancia = limitePositivo;
			distanciaExtra = p.y;
			posicaoExtraNegativo = p.y;
			posicaoExtraPositivo = p.y + Dados.rebatedorDistanciaVertical;
			direcao = -1;
			direcaoExtra = 1;
			velocidade = Dados.rebatedorHorizontalVelocidade;
			break;
		case Rebatedores.Tipo.DiagonalDirEsqCimaBaixo:
			limitePositivo = p.x;
			limiteNegativo = p.x - 2 * Dados.rebatedorDistanciaHorizontal;
			distancia = limitePositivo;
			distanciaExtra = p.y;
			posicaoExtraPositivo = p.y;
			posicaoExtraNegativo = p.y - Dados.rebatedorDistanciaVertical;
			direcao = -1;
			direcaoExtra = -1;
			velocidade = Dados.rebatedorHorizontalVelocidade;
			break;
		case Rebatedores.Tipo.DiagonalEsqDirBaixoCima:
			limiteNegativo = p.x;
			limitePositivo = p.x + 2 * Dados.rebatedorDistanciaHorizontal;
			distancia = limiteNegativo;
			distanciaExtra = p.y;
			posicaoExtraNegativo = p.y;
			posicaoExtraPositivo = p.y + Dados.rebatedorDistanciaVertical;
			direcao = 1;
			direcaoExtra = 1;
			velocidade = Dados.rebatedorHorizontalVelocidade;
			break;
		case Rebatedores.Tipo.DiagonalEsqDirCimaBaixo:
			limiteNegativo = p.x;
			limitePositivo = p.x + 2 * Dados.rebatedorDistanciaHorizontal;
			distancia = limiteNegativo;
			distanciaExtra = p.y;
			posicaoExtraPositivo = p.y;
			posicaoExtraNegativo = p.y - Dados.rebatedorDistanciaVertical;
			direcao = 1;
			direcaoExtra = -1;
			velocidade = Dados.rebatedorHorizontalVelocidade;
			break;
		case Rebatedores.Tipo.CircularHorario:
			direcao = -1;
			velocidade = Dados.rebatedorAngularVelocidade;
			break;
		case Rebatedores.Tipo.CircularAntihorario:
			direcao = 1;
			velocidade = Dados.rebatedorAngularVelocidade;
			break;
		}
	}

	// Métodos privados
	void Update(){
		switch(tipo){
		case Rebatedores.Tipo.HorizontalDirEsq:
		case Rebatedores.Tipo.HorizontalEsqDir:
			MoverHorizontal();
			break;
		case Rebatedores.Tipo.VerticalBaixoCima:
		case Rebatedores.Tipo.VerticalCimaBaixo:
			MoverVertical();
			break;
		case Rebatedores.Tipo.DiagonalDirEsqBaixoCima:
		case Rebatedores.Tipo.DiagonalEsqDirBaixoCima:
		case Rebatedores.Tipo.DiagonalDirEsqCimaBaixo:
		case Rebatedores.Tipo.DiagonalEsqDirCimaBaixo:
			MoverDiagonal();
			break;
		case Rebatedores.Tipo.CircularHorario:
		case Rebatedores.Tipo.CircularAntihorario:
			MoverCircular();
			break;
		}
	}

	// Movimentação
	void MoverHorizontal(){
		distancia += velocidade * direcao * Time.deltaTime;

		if (distancia > limitePositivo){
			distancia = limitePositivo;
			direcao = -1;
		}
		if (distancia < limiteNegativo){
			distancia = limiteNegativo;
			direcao = 1;
		}

		transform.position = new Vector3(
			distancia,
			posInicial.y,
			posInicial.z);
	}

	void MoverVertical(){
		distancia += velocidade * direcao * Time.deltaTime;
		
		if (distancia > limitePositivo){
			distancia = limitePositivo;
			direcao = -1;
		}
		if (distancia < limiteNegativo){
			distancia = limiteNegativo;
			direcao = 1;
		}
		
		transform.position = new Vector3(
			posInicial.x,
			distancia,
			posInicial.z);
	}

	void MoverDiagonal(){
		distancia += velocidade * direcao * Time.deltaTime;
		distanciaExtra += 
			Dados.rebatedorVerticalVelocidade *
				direcaoExtra * Time.deltaTime;
		
		if (distancia > limitePositivo){
			distancia = limitePositivo;
			direcao = -1;
			if (direcaoExtra == 1){
				distanciaExtra = posicaoExtraPositivo;
				direcaoExtra = -1;
			}else{
				distanciaExtra = posicaoExtraNegativo;
				direcaoExtra = 1;
			}
		}
		if (distancia < limiteNegativo){
			distancia = limiteNegativo;
			direcao = 1;
			if (direcaoExtra == 1){
				distanciaExtra = posicaoExtraPositivo;
				direcaoExtra = -1;
			}else{
				distanciaExtra = posicaoExtraNegativo;
				direcaoExtra = 1;
			}
		}
		
		transform.position = new Vector3(
			distancia,
			distanciaExtra,
			posInicial.z);
	}

	void MoverCircular(){
		distancia += velocidade * direcao * Time.deltaTime;
		
		if (distancia > 360){
			distancia -= 360;
		}
		if (distancia < 0){
			distancia += 360;
		}

		float x = Mathf.Cos(distancia) * raioHorizontal;
		float y = Mathf.Sin(distancia) * raioVertical;
		
		transform.position = new Vector3(
			posInicial.x + x,
			posInicial.y + y,
			posInicial.z);
	}

	// Destrutíveis
	public void Reativar(){
		morto = false;
		gameObject.SetActive(true);
		transform.position = posInicial;
		Criar (tipo, destrutivel, posInicial, posicaoGrade);
	}

	void Acertado(){
		if (destrutivel && morto == false)
		{
			Dados.bolasLancadasNestaFase--;
			Destruir();
		}
	}

	void Destruir(){
		morto = true;

		if (somDestruir && Dados.somLigado){
			Instantiate(
				somDestruir, transform.position, transform.rotation);
		}

		Instantiate(splash, transform.position, transform.rotation);

		Dados.rebatedoresDestruidosNestaFase++;

		if (Dados.modoDeJogo == ModosDeJogo.JogoRapido){
			Desativar();
		}else{
			Remover();
		}
	}

	void Desativar(){
		gameObject.SetActive(false);
	}

	void Remover(){
		//Pontuar();
		CriarTextoFlutuante(-1);
		Destroy(gameObject);
	}

	void Pontuar()
	{
		int pontos = Dados.pontosBase * 
				Dados.pontosMultiplicadorRebatedorDestrutivel;

		int extra = 0;
		switch(tipo){
		case Rebatedores.Tipo.CircularAntihorario:
		case Rebatedores.Tipo.CircularHorario:
			extra = 4;
			break;
		case Rebatedores.Tipo.DiagonalDirEsqBaixoCima:
		case Rebatedores.Tipo.DiagonalDirEsqCimaBaixo:
		case Rebatedores.Tipo.DiagonalEsqDirBaixoCima:
		case Rebatedores.Tipo.DiagonalEsqDirCimaBaixo:
			extra = 3;
			break;
		case Rebatedores.Tipo.HorizontalDirEsq:
		case Rebatedores.Tipo.HorizontalEsqDir:
			extra = 2;
			break;
		case Rebatedores.Tipo.VerticalBaixoCima:
		case Rebatedores.Tipo.VerticalCimaBaixo:
			extra = 1;
			break;
		default:
			extra = 0;
			break;
		}

		pontos += extra * Dados.pontosBase;

		Dados.pontosUltimaFaseOnus += pontos;

		CriarTextoFlutuante(-pontos);
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == Dados.tagBola){
			Acertado();
		}
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

