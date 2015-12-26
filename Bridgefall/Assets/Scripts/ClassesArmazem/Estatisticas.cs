using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Estatisticas
{
	// Classes extras
	public class Fases 
	{
		public bool [] 	estrelas = { false, false, false };
		public bool 	perfect = false;
		public bool		completo = false;
		public int 		ultimaPontuacao = 0;
		public int		melhorPontuacao = 0;
		public int 		ultimaPontuacaoDerrota = 0;
		public bool		liberada = false;

		public Fases(){
			liberada = true;
		}

		public void Completou(int pontos = 0, bool perf = false)
		{
			if (perf)
			{
				perfect = true;
			}

			completo = true;
			Pontuar (pontos);
		}

		public void Pontuar(int pontos, bool derrota = false)
		{
			if (derrota)
			{
				ultimaPontuacaoDerrota = pontos;
			}
			else
			{
				ultimaPontuacao = pontos;
				if (pontos > melhorPontuacao)
				{
					melhorPontuacao = pontos;
				}
			}
		}
	}

	public class Mundos
	{
		public bool 		completo = false;
		public List<Fases> 	fases = new List<Fases>();
		public bool [] 		estrelas = { false, false, false };
		public bool 		liberado = false;

		public Mundos(){
			liberado = true;
		}

		public void VerificarFasesCompletas()
		{
			if (fases.Count < Dados.fasesPorMundo)
			{
				int tresFases = 0;
				for (int i = 0; i < fases.Count; i++)
				{
					if (fases[i].completo){
						tresFases++;
						if (tresFases == 3){
							tresFases = 0;
							if (i == fases.Count - 1){
								fases.Add(new Fases());
								fases.Add(new Fases());
								fases.Add(new Fases());
							}
						}
					}
				}
			}
			VerificarMundo();
		}

		public bool VerificarMundo(){
			completo = true;
			estrelas[0] = true;
			estrelas[1] = true;
			estrelas[2] = true;
			foreach(Fases fase in fases){
				if (fase.estrelas[0] == false){
					estrelas[0] = false;
				}
				if (fase.estrelas[1] == false){
					estrelas[1] = false;
				}
				if (fase.estrelas[2] == false){
					estrelas[2] = false;
				}
				if (fase.completo == false){
					completo = false;
				}
			}
			return completo;
		}
	}

	public class JogoRapido
	{
		public bool 	liberado = true;
		public int[] 	ultimaPontuacao = {0,0,0,0,0,0,0,0,0,0};
		public int[]	melhorPontuacao = {0,0,0,0,0,0,0,0,0,0};
		public int[]	ultimaPontuacaoDerrota = {0,0,0,0,0,0,0,0,0,0};

		public void Pontuar(int pontos, int dif, bool derrota = false)
		{
			/*
			if (derrota)
			{
				ultimaPontuacaoDerrota[dif - 1] = pontos;
			}
			else
			{
			*/
				ultimaPontuacao[dif - 1] = pontos;
				if (pontos > melhorPontuacao[dif - 1])
				{
					melhorPontuacao[dif - 1] = pontos;
				}
			//}
		}
	}

	public class Sobrevivencia
	{
		public bool 	liberado = false;
		public int 		ultimaPontuacao = 0;
		public int		melhorPontuacao = 0;
		public int 		melhorOnda = 0;
		public int 		ultimaOnda = 0;

		public void Pontuar(int pontos)
		{
			ultimaPontuacao = pontos;
			if (pontos > melhorPontuacao)
			{
				melhorPontuacao = pontos;
			}
		}
	}

	// Variáveis
	public List<Mundos> 	mundos = new List<Mundos>();
	public JogoRapido 		jogoRapido = new JogoRapido();
	public Sobrevivencia 	sobrevivencia = new Sobrevivencia();


	// Métodos
	public Estatisticas()
	{
		// O primeiro mundo, e suas três primeiras fases,
		// começam liberados
		mundos.Add(new Mundos());
		mundos[0].fases.Add(new Fases());
		mundos[0].fases.Add(new Fases());
		mundos[0].fases.Add(new Fases());
	}

	// Verifica se acabou de completar o ultimo mundo.
	// Se sim, libera o proximo e retorna verdadeiro.
	public bool VerificarMundosExtras()
	{
		if (mundos.Count > Dados.mundosCompletosParaJogoRapido)
		{
			jogoRapido.liberado = true;
		}

		if (mundos.Count > Dados.mundosCompletosParaSobrevivencia)
		{
			sobrevivencia.liberado = true;
		}

		if (mundos.Count >= Dados.totalDeMundos ||
		    Dados.mundoAtual < mundos.Count - 1)
		{
			return false;
		}

		if (mundos[Dados.mundoAtual].completo)
		{
			mundos.Add(new Mundos());

			int novoMundo = mundos.Count - 1;

			mundos[novoMundo].fases.Add(new Fases());
			mundos[novoMundo].fases.Add(new Fases());
			mundos[novoMundo].fases.Add(new Fases());

			return true;
		}

		return false;
	}
}

