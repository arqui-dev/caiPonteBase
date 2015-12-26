using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControleFase : MonoBehaviour
{
	// Variáveis públicas
	public Rebatedores 		rebatedores;
	public ControleVento 	controleVento;
	public ControleOndas 	controleOndas;

	// Variáveis privadas
	List<Onda> 	ondasCima = new List<Onda>();
	List<Onda> 	ondasBaixo = new List<Onda>();

	// Métodos públicos
	public void Criar(Fase f, bool inicializar = true)
	{
		//Debug.Log(f.ParaString());

		// Pega os passantes da ponte de cima
		ondasCima.Clear();
		int patual = 0;
		foreach(int o in f.w){
			List<Passante> passantes = new List<Passante>();
			for (int i = 0; i < o; i++){
				Passante passante = new Passante(
					f.s[patual + i], Pontes.Cima);
				passantes.Add(passante);
			}
			patual += passantes.Count;
			ondasCima.Add(new Onda(passantes));
		}

		// Pega os passantes da ponte de baixo
		ondasBaixo.Clear();
		Dados.ponteBaixo = false;
		if (f.sbridge){
			Dados.ponteBaixo = true;
			patual = 0;
			foreach(int o in f.sw){
				List<Passante> passantes = new List<Passante>();
				for (int i = 0; i < o; i++){
					Passante passante = new Passante(
						f.ss[patual + i], Pontes.Baixo);
					passantes.Add(passante);
				}
				patual += passantes.Count;
				ondasBaixo.Add(new Onda(passantes));
			}
		}

		// Verifica o movimento do barco
		Dados.barcoMove = false;
		if (f.boat > 0){
			Dados.barcoMove = true;
			switch(f.boat){
			case 1:
				Dados.barcoVelocidade = Dados.BARCO_VELOCIDADE_LENTO;
				break;
			case 2:
				Dados.barcoVelocidade = Dados.BARCO_VELOCIDADE_NORMAL;
				break;
			case 3:
				Dados.barcoVelocidade = Dados.BARCO_VELOCIDADE_RAPIDO;
				break;
			}
		}

		// Verifica o vento
		controleVento.Desabilitar();
		Dados.vento = false;
		if (f.wind > 0){
			Dados.vento = true;
			switch(f.wind){
			case 1:
				Dados.ventoVelocidade = Dados.VENTO_VELOCIDADE_LENTO;
				break;
			case 2:
				Dados.ventoVelocidade = Dados.VENTO_VELOCIDADE_NORMAL;
				break;
			case 3:
				Dados.ventoVelocidade = Dados.VENTO_VELOCIDADE_RAPIDO;
				break;
			case 4:
				Dados.ventoVelocidade = Dados.VENTO_VELOCIDADE_MAX;
				break;
			}
			switch(f.dir){
			case 1:
				Dados.ventoDirecao = Dados.VENTO_DIRECAO_NORTE;
				break;
			case 2:
				Dados.ventoDirecao = Dados.VENTO_DIRECAO_NORDESTE;
				break;
			case 3:
				Dados.ventoDirecao = Dados.VENTO_DIRECAO_LESTE;
				break;
			case 4:
				Dados.ventoDirecao = Dados.VENTO_DIRECAO_SUDESTE;
				break;
			case 5:
				Dados.ventoDirecao = Dados.VENTO_DIRECAO_SUL;
				break;
			case 6:
				Dados.ventoDirecao = Dados.VENTO_DIRECAO_SUDOESTE;
				break;
			case 7:
				Dados.ventoDirecao = Dados.VENTO_DIRECAO_OESTE;
				break;
			case 8:
				Dados.ventoDirecao = Dados.VENTO_DIRECAO_NOROESTE;
				break;
			default:
				Dados.ventoDirecao = Dados.VENTO_DIRECAO_NORTE;
				break;
			}
		}

		// Verifica se o vento varia de velocidade
		if (f.wind < 0){
			float [] velos = new float[f.v.Count];
			for (int i = 0; i < velos.Length; i++){
				switch(f.v[i]){
				case 1:
					velos[i] = Dados.VENTO_VELOCIDADE_LENTO;
					break;
				case 2:
					velos[i] = Dados.VENTO_VELOCIDADE_NORMAL;
					break;
				case 3:
					velos[i] = Dados.VENTO_VELOCIDADE_RAPIDO;
					break;
				case 4:
					velos[i] = Dados.VENTO_VELOCIDADE_MAX;
					break;
				}
			}
			controleVento.Velocidades(f.wtime, velos);
		}

		// Verifica se o vento varia de direção
		if (f.dir < 0){
			Vector2 [] dirs = new Vector2[f.d.Count];
			for (int i = 0; i < dirs.Length; i++){
				switch(f.d[i]){
				case 1:
					dirs[i] = Dados.VENTO_DIRECAO_NORTE;
					break;
				case 2:
					dirs[i] = Dados.VENTO_DIRECAO_NORDESTE;
					break;
				case 3:
					dirs[i] = Dados.VENTO_DIRECAO_LESTE;
					break;
				case 4:
					dirs[i] = Dados.VENTO_DIRECAO_SUDESTE;
					break;
				case 5:
					dirs[i] = Dados.VENTO_DIRECAO_SUL;
					break;
				case 6:
					dirs[i] = Dados.VENTO_DIRECAO_SUDOESTE;
					break;
				case 7:
					dirs[i] = Dados.VENTO_DIRECAO_OESTE;
					break;
				case 8:
					dirs[i] = Dados.VENTO_DIRECAO_NOROESTE;
					break;
				default:
					dirs[i] = Dados.VENTO_DIRECAO_NORTE;
					break;
				}
			}
			controleVento.Direcoes(f.dtime, dirs);
		}

		// Habilita as coisas
		controleVento.Reiniciar();

		rebatedores.RemoverRebatedores();
		rebatedores.AdicionarTodos(f.bouncers);

		if (inicializar)
			IniciarFase();
	}

	public void CriarJogoRapido(int dif)
	{
		Fase f = Utilidade.GerarFase(dif);

		Criar(f);
	}

	public void CriarSobrevivencia()
	{
		controleOndas.InicializarSobrevivencia(rebatedores);
	}

	//*
	public List<Onda> [] CriarFase(Fase f)
	{
		Criar(f, false);

		List<Onda> [] ondas = new List<Onda>[2];
		ondas[0] = ondasCima;
		ondas[1] = ondasBaixo;

		//Debug.Log("Ondas: "+ondas[0].Count+", "+ondas[1].Count);

		return ondas;
	}
	//*/

	// Métodos privados
	void IniciarFase(){
		controleOndas.Inicializar(ondasCima, ondasBaixo);
	}
}

