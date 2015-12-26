using UnityEngine;
using System.Collections;

public class ControleVento : MonoBehaviour
{
	// Variáveis públicas
	public GameObject rosa;
	public GameObject balaoExclamacao;
	public Transform localExclamacao;
	public float rosaAnguloVelocidade = 0.25f;
	public float rosaAnguloMax = 5;
	public float tempoAntesDoBalaoDeExclamacao = 0.5f;

	// Variáveis privadas

	// Rosa dos ventos
	float rosaAnguloAtual = 0;
	int rosaAnguloDirecao = 1;

	// Direção do vento
	bool direcaoMuda = false;
	int direcaoQuantidade = 0;
	int direcaoAtual = 0;
	float direcaoTempo = 0;
	float direcaoTempoInicial = 0;
	Vector2 [] direcoes;

	// Velocidade do vento
	bool velocidadeMuda = false;
	int velocidadeQuantidade = 0;
	int velocidadeAtual = 0;
	float velocidadeTempo = 0;
	float velocidadeTempoInicial = 0;
	float [] velocidades;

	// Balão de exclamação
	float tempoAntesExclamacao = 0;
	bool lancarExclamacao = false;

	// Métodos estáticos
	static int PontosPorVelocidade(float vel)
	{
		if (vel <= Dados.VENTO_VELOCIDADE_NORMAL){
			return Dados.pontosBase * 2;
		}

		if (vel <= Dados.VENTO_VELOCIDADE_RAPIDO){
			return Dados.pontosBase * 3;
		}

		if (vel <= Dados.VENTO_VELOCIDADE_MAX){
			return Dados.pontosBase * 4;
		}

		return Dados.pontosBase;
	}

	static int PontosPorDirecao(Vector2 dir)
	{
		if (dir == Dados.VENTO_DIRECAO_SUL ||
		    dir == Dados.VENTO_DIRECAO_SUDOESTE ||
		    dir == Dados.VENTO_DIRECAO_SUDESTE)
		{
			return Dados.pontosBase * 3;
		}
		else if (dir != Dados.VENTO_DIRECAO_NORTE)
		{
			return Dados.pontosBase * 2;
		}
		return Dados.pontosBase;
	}

	// Métodos públicos
	public void Desabilitar(){
		velocidadeMuda = false;
		direcaoMuda = false;
	}

	public void Reiniciar(bool sobrev = false){
		if (Dados.vento){
			gameObject.SetActive(true);
			tempoAntesExclamacao = Time.time +
				tempoAntesDoBalaoDeExclamacao;

			if (sobrev == false)
				lancarExclamacao = true;
		}else{
			gameObject.SetActive(false);
			lancarExclamacao = false;
		}
	}

	public void Direcoes(float tempo, Vector2 [] dirs){
		Dados.vento = true;
		Dados.ventoDirecao = dirs[0];
		direcoes = dirs;
		direcaoQuantidade = dirs.Length;
		direcaoAtual = 0;
		direcaoTempo = tempo;
		direcaoTempoInicial = Time.time + direcaoTempo;
		direcaoMuda = true;
	}

	public void Velocidades(float tempo, float [] vels){
		Dados.vento = true;
		Dados.ventoVelocidade = vels[0];
		velocidades = vels;
		velocidadeQuantidade = vels.Length;
		velocidadeAtual = 0;
		velocidadeTempo = tempo;
		velocidadeTempoInicial = Time.time + velocidadeTempo;
		velocidadeMuda = true;
	}

	// Métodos privados
	void Awake(){
		Reiniciar();
	}

	void Update(){
		if (direcaoMuda){
			if (Time.time > direcaoTempoInicial){
				MudarDirecao();
			}
		}
		if (velocidadeMuda){
			if (Time.time > velocidadeTempoInicial){
				MudarVelocidade();
			}
		}

		AtualizarRosa();

		VerificarExclamacao();
	}

	void MudarDirecao(){
		direcaoAtual++;
		if (direcaoAtual >= direcaoQuantidade){
			direcaoAtual = 0;
		}

		Dados.ventoDirecao = direcoes[direcaoAtual];

		direcaoTempoInicial = Time.time + direcaoTempo;
	}

	void MudarVelocidade(){
		velocidadeAtual++;
		if (velocidadeAtual >= velocidadeQuantidade){
			velocidadeAtual = 0;
		}
		
		Dados.ventoVelocidade = velocidades[velocidadeAtual];
		
		velocidadeTempoInicial = Time.time + velocidadeTempo;
	}

	void AtualizarRosa(){
		rosaAnguloAtual += 
			rosaAnguloDirecao *
			rosaAnguloVelocidade * 
			Dados.ventoVelocidade *
			Time.deltaTime;
		
		if (rosaAnguloAtual > rosaAnguloMax){
			rosaAnguloAtual = rosaAnguloMax;
			rosaAnguloDirecao = -1;
		}
		if (rosaAnguloAtual < -rosaAnguloMax){
			rosaAnguloAtual = -rosaAnguloMax;
			rosaAnguloDirecao = 1;
		}

		float angulo = Vector3.Angle(
			Dados.ventoDirecao, Vector3.up);

		Vector3 cruzado = Vector3.Cross(
			Dados.ventoDirecao, Vector3.up);

		if ( cruzado.z > 0){
			angulo = 360 - angulo;
		}
		
		angulo += rosaAnguloAtual;
		
		while (angulo < 0)
			angulo += 360;
		
		while (angulo >= 360)
			angulo -= 360;
		
		rosa.transform.rotation = Quaternion.Euler(
			new Vector3(0,0, angulo));
	}

	void VerificarExclamacao()
	{
		if (lancarExclamacao){
			if (Time.time > tempoAntesExclamacao){
				LancarExclamacao();
			}
		}
	}

	void LancarExclamacao()
	{
		Instantiate(
			balaoExclamacao, 
			localExclamacao.position,
			Quaternion.identity);

		lancarExclamacao = false;
	}

	public void AlterarDirecaoSobrevivencia()
	{
		Dados.vento = true;
		Reiniciar(true);
		int direcao = Random.Range(0,9);
		switch(direcao){
		case 0: Dados.ventoDirecao = Dados.VENTO_DIRECAO_NORTE; break;
		case 1: Dados.ventoDirecao = Dados.VENTO_DIRECAO_NORDESTE; break;
		case 2: Dados.ventoDirecao = Dados.VENTO_DIRECAO_LESTE; break;
		case 3: Dados.ventoDirecao = Dados.VENTO_DIRECAO_SUDESTE; break;
		case 4: Dados.ventoDirecao = Dados.VENTO_DIRECAO_SUL; break;
		case 5: Dados.ventoDirecao = Dados.VENTO_DIRECAO_SUDOESTE; break;
		case 6: Dados.ventoDirecao = Dados.VENTO_DIRECAO_LESTE; break;
		case 7: Dados.ventoDirecao = Dados.VENTO_DIRECAO_NOROESTE; break;
		}
	}

	public void AlterarVelocidadeSobrevivencia()
	{
		Dados.vento = true;
		Reiniciar(true);
		if (Dados.ventoVelocidade < Dados.VENTO_VELOCIDADE_LENTO)
		{
			Dados.ventoVelocidade = Dados.VENTO_VELOCIDADE_LENTO;
		}
		else if (Dados.ventoVelocidade < Dados.VENTO_VELOCIDADE_NORMAL)
		{
			Dados.ventoVelocidade = Dados.VENTO_VELOCIDADE_NORMAL;
		}
		else if (Dados.ventoVelocidade < Dados.VENTO_VELOCIDADE_RAPIDO)
		{
			Dados.ventoVelocidade = Dados.VENTO_VELOCIDADE_RAPIDO;
		}
		else
		{
			Dados.ventoVelocidade = Dados.VENTO_VELOCIDADE_MAX;
		}
	}
}
