using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utilidade
{
	public static string MensagemSemMacas()
	{
		string saida = ControleIdioma.PegarTexto(
			Idiomas.Texto.MensagemSemMacas);
		long macas = Dados.macasDivisorPontos;
		string modo = ControleIdioma.PegarTexto(
			Idiomas.Texto.TituloTelaJogoRapido);

		return string.Format(saida, macas, modo);
	}

	public static string MensagemAdNaoVisto()
	{
		return "Para receber uma maçã é necessário ver o Ad até o fim.\n"+
			"Ou você não viu, ou os Ads não puderam ser carregados.";
	}

	public static void AdicionarMensagemNaoViuAd()
	{
		ControleMensagens.AdicionarMensagem(MensagemAdNaoVisto(),0);
	}

	public static void AdicionarMacasPorQuantidade(int quantidade = 0)
	{
		Dados.macasVerdesUltimaTela = (long) quantidade;

		Dados.macasVerdeTotal += Dados.macasVerdesUltimaTela;

		Utilidade.SalvarDados();

		Debug.Log ("Maçãs verdes última tela: "+
		           Dados.macasVerdesUltimaTela);
	}

	public static void AdicionarMacasPorPontos(int pontos)
	{
		long pts = (long) pontos;
		Dados.macasVerdesUltimaTela = 0;

		while (pts >= Dados.macasDivisorPontos)
		{
			Dados.macasVerdesUltimaTela++;
			pts -= Dados.macasDivisorPontos;
		}

		Debug.Log ("Macas verdes última tela:"+
		           Dados.macasVerdesUltimaTela+
		           ", pontos restantes "+pts);

		Dados.macasVerdeTotal += Dados.macasVerdesUltimaTela;

		UnityAnalytics.GanhouMaca(
			false, (int) Dados.macasVerdesUltimaTela);
	}

	public static void AjeitarMacasVerdes()
	{
		long anterior = Dados.macasVerdeTotal - 
			Dados.macasVerdesUltimaTela;

		Dados.macasVerdesUltimaTela = 0;

		Debug.Log ("Maçãs verdes totais: " + anterior +
		           " -> " + Dados.macasVerdeTotal);
	}

	public static bool VerificarMacasJogar()
	{
		if (Dados.macasVerdeTotal < 1)
		{
			return false;
		}
		return true;
	}

	public static void GastarMaca(
		long quantidade = 1, bool reiniciar = false)
	{
		if (Dados.macasVerdeTotal > 0)
		{
			Dados.macasVerdeTotal -= quantidade;

			UnityAnalytics.GastouMaca(reiniciar);
		}
	}

	public static bool MeiaChance()
	{
		return (Random.Range(0,2) > 0);
	}

	public static T AleatorioLista<T>(List<T> lista)
	{
		int pos = Random.Range(0, lista.Count);
		T retorno = lista[pos];
		//Debug.Log ("Lista antes: "+lista.Count);
		lista.RemoveAt(pos);
		//Debug.Log ("Lista depois: "+lista.Count);
		return retorno;
	}

	static List<Rebatedor> CriarRebatedores(
		int rc, int rm, int rb, bool f, int dif)
	{
		List<Rebatedor> rebs = new List<Rebatedor>();

		rc = rc > 3 ? 3 : rc;
		rm = rm > 3 ? 3 : rm;
		rb = rb > 3 ? 3 : rb;


		if (rc > 0)
		{
			List<int> pos = new List<int>();
			pos.Add(0);
			pos.Add(1);
			pos.Add(2);

			while(pos.Count > 3 - rc)
			{
				int p = AleatorioLista<int>(pos);
				foreach(Rebatedor r in CriarRebatedoresGrade(p,f,dif))
				{
					rebs.Add(r);
				}
			}
		}
		if (rm > 0)
		{
			List<int> pos = new List<int>();
			pos.Add(3);
			pos.Add(4);
			pos.Add(5);
			
			while(pos.Count > 3 - rm)
			{
				int p = AleatorioLista<int>(pos);
				foreach(Rebatedor r in CriarRebatedoresGrade(p,f,dif))
				{
					rebs.Add(r);
				}
			}
		}
		if (rb > 0)
		{
			List<int> pos = new List<int>();
			pos.Add(6);
			pos.Add(7);
			pos.Add(8);
			
			while(pos.Count > 3 - rb)
			{
				int p = AleatorioLista<int>(pos);
				foreach(Rebatedor r in CriarRebatedoresGrade(p,f,dif))
				{
					rebs.Add(r);
				}
			}
		}

		return rebs;
	}

	static List<Rebatedor> CriarRebatedoresGrade(int pos, bool f, int dif)
	{
		List<Rebatedor> rebs = new List<Rebatedor>();

		float chanceDestrutivel =
			1 - ((dif - 1f) / Dados.jogoRapidoDificuldadeMaxima);
		//bool destru = Random.value <= chanceDestrutivel;


		Rebatedores.Tipo tipo = PegarTipo(f);
		int m = ModPos(tipo);
		int qMaxPorDif = dif / 2;
		int q = QuantidadeMaxima(tipo);
		q = q <= qMaxPorDif ? q : qMaxPorDif;
		q = q > 1 ? q : 1;

		List<int> posicoes = new List<int>();
		for (int i = 0; i < m; i++)
		{
			posicoes.Add (i);
		}
		
		for (int i = 0; i < q; i++)
		{
			Rebatedor reb = new Rebatedor();
			reb.destrutivel = Random.value <= chanceDestrutivel;
			reb.posicaoGrade = pos;
			reb.tipo = tipo;
			reb.posicaoLocal = AleatorioLista<int>(posicoes);

			rebs.Add(reb);
		}

		return rebs;
	}

	static int QuantidadeMaxima(Rebatedores.Tipo tipo)
	{
		switch(tipo){
		case Rebatedores.Tipo.Fixo:			
			return 4;
		case Rebatedores.Tipo.VerticalCimaBaixo:
		case Rebatedores.Tipo.VerticalBaixoCima:
			return 2;
		}
		return 1;
	}

	static int ModPos(Rebatedores.Tipo tipo)
	{
		switch(tipo){
		case Rebatedores.Tipo.Fixo:			
			return 6;
		case Rebatedores.Tipo.DiagonalEsqDir:
		case Rebatedores.Tipo.DiagonalDirEsq:		
			return 3;
		}
		return 2;
	}

	static Rebatedores.Tipo PegarTipo(bool f)
	{
		if (f == false)
		{
			switch(Random.Range(0,14)){
			case 0:
			case 1:
				return Rebatedores.Tipo.VerticalBaixoCima;
			case 2:
			case 3:
				return Rebatedores.Tipo.VerticalCimaBaixo;
			case 4:
				return Rebatedores.Tipo.DiagonalDirEsq;
			case 5:
				return Rebatedores.Tipo.DiagonalEsqDir;
			case 6:
			case 7:
				return Rebatedores.Tipo.Circular;
			default:
				return Rebatedores.Tipo.Fixo;
			}
		}
		return Rebatedores.Tipo.Fixo;
	}

	public static Fase GerarFase(int dif)
	{
		if (Dados.modoDeJogo == ModosDeJogo.JogoRapido)
		{
			return GerarFaseJogorRapido(dif);
		}
		
		if (Dados.modoDeJogo == ModosDeJogo.Sobrevivencia)
		{
			return GerarFaseSobrevivencia(dif);
		}
		
		Fase fase = new Fase();
		fase.people = 1;
		fase.waves = -1;
		fase.s.Add(1);
		
		return fase;
	}

	static Fase GerarFaseSobrevivencia(int dif)
	{
		Mundo mundo = CarregarFaseDeArquivo.CarregarDificuldade(dif);
		
		int n = Random.Range(0, mundo.fases.Count);
		
		return mundo.fases[n];
	}
	
	static Fase GerarFaseJogorRapido(int dif)
	{
		Mundo mundo = CarregarFaseDeArquivo.CarregarDificuldade(dif);

		int n = Random.Range(0, mundo.fases.Count);

		return mundo.fases[n];

		/*
		Fase f = new Fase();
		
		int extrap = Random.Range(0, dif / 3 + 3);
		int basePassantes = 3;
		int basePassantes2 = 1;
		
		int passantes = basePassantes + dif / 2 + extrap;
		int ondas = -1;
		
		bool ponte2 = false;
		int passantes2 = basePassantes2 + dif / 3 + extrap;
		int ondas2 = -1;
		
		float velBase = 3;
		float velMin = 0;
		float velMax = 1;
		
		int barco = 0;
		
		int vento = 0;
		float tvento = 1;
		
		int dvento = 0;
		float dtvento = 1;
		
		bool soDaEsquerda = false;
		
		int rebCima = 0;
		int rebMeio = 0;
		int rebBaixo = 0;
		
		if (dif == Dados.jogoRapidoDificuldadeMaxima)
		{
			rebMeio = 2;
			rebBaixo = 2;
			
			velMin = 6f;
			velMax = 8f;
			
			passantes += 2;
			ondas = Random.Range(2,6);
			
			ponte2 = true;
			passantes2 += 3;
			
			barco = 3;
			
			vento = Random.Range(6,11) * -1;
			tvento = Random.Range(1.5f, 4f);
			
			dvento = Random.Range(6,11) * -1;
			dtvento = Random.Range(1.5f, 4f);
		}
		else if (dif > Dados.jogoRapidoDificuldadeMaxima * 3 / 4)
		{
			rebCima = Random.Range(0,4);
			rebMeio = 1 + Random.Range(0,2);
			rebBaixo = 1 + Random.Range(0,2);
			
			velMin = 3f;
			velMax = 7f;
			
			passantes += 1;
			
			ponte2 = true;
			passantes2 += 1;
			
			barco = 2 + Random.Range(0,2);
			
			vento = Random.Range(2,6) * -1;
			tvento = Random.Range(3f,7f);
			
			dvento = Random.Range(2,6) * -1;
			dtvento = Random.Range(3f,7f);
		}
		else if (dif > Dados.jogoRapidoDificuldadeMaxima / 2)
		{
			rebCima = 1 + Random.Range(0,3);
			rebMeio = 1;
			rebBaixo = Random.Range(0,2);
			
			velMin = 2f;
			velMax = 5f;
			
			ponte2 = true;
			
			barco = 1 + Random.Range(0,2);
			
			int v = Random.Range(2,5);
			vento = MeiaChance() ? v : -2;
			tvento = Random.Range(5f, 10f);
			
			int d = Random.Range(1,9);
			dvento = MeiaChance() ? d : -2;
			dtvento = Random.Range(5f, 10f);
		}
		else if (dif > Dados.jogoRapidoDificuldadeMaxima / 4)
		{
			rebCima = 3;
			rebMeio = 1 + Random.Range(0,2);
			
			velMin = 0.5f;
			velMax = 3.5f;
			
			barco = Random.Range(0,2);
			if (dif == Dados.jogoRapidoDificuldadeMaxima / 2)
			{
				rebCima = 2 + Random.Range(0,2);
				rebMeio = Random.Range(0,2);
				rebBaixo = Random.Range(0,2);
				
				ponte2 = MeiaChance();
				barco = 1;
			}
		}
		else
		{
			if (dif > 1){
				rebCima = 1;
			}
			
			velMin = 0f;
			velMax = 2f;

			soDaEsquerda = true;
		}
		
		List<int> w = new List<int>();
		List<float> s = new List<float>();
		
		if (ondas > 0)
		{
			int total = 0;
			int dir = 0;
			float dec = 0;
			float v = 0;
			
			for (int i = 0; i < ondas; i++)
			{
				int valor = Random.Range(1, passantes / ondas + 1);
				w.Add(valor);
				dir = MeiaChance() ? 1 : -1;
				dec = MeiaChance() ? 0 : 0.9f;
				v = velBase + Random.Range(velMin,velMax);
				v *= dir;
				
				for (int j = 0; j < valor; j++)
				{
					s.Add(v);
					v = v * Random.Range(dec * 0.75f, dec);
					total++;
				}
			}

			if (total < passantes)
			{
				total = passantes - total;
				w.Add(total);
				dir = MeiaChance() ? 1 : -1;
				dec = MeiaChance() ? 0 : 0.9f;
				v = velBase + Random.Range(velMin,velMax);
				v *= dir;
				
				for (int j = 0; j < total; j++)
				{
					s.Add(v);
					v = v * Random.Range(dec * 0.75f, dec);
				}
			}
		}
		else
		{
			int total = 0;
			int dir = 0;
			float dec = 0;
			float v = 0;
			
			int tondas = -ondas;
			if (tondas == 0)
			{
				tondas = 1;
			}
			
			for (int i = 0; i < passantes / tondas; i++)
			{
				w.Add(tondas);
				
				dir = (MeiaChance() || soDaEsquerda) ? 1 : -1;
				dec = MeiaChance() ? 0 : 0.9f;
				v = velBase + Random.Range(velMin,velMax);
				v *= dir;
				
				for (int j = 0; j < tondas; j++)
				{
					s.Add(v);
					v = v * Random.Range(dec * 0.75f, dec);
					total++;
				}
			}

			if (total < passantes)
			{
				total = passantes - total;
				w.Add(total);
				dir = (MeiaChance() || soDaEsquerda) ? 1 : -1;
				dec = MeiaChance() ? 0 : 0.9f;
				v = velBase + Random.Range(velMin,velMax);
				v *= dir;
				
				for (int j = 0; j < total; j++)
				{
					s.Add(v);
					v = v * Random.Range(dec * 0.75f, dec);
				}
			}
		}
		
		
		List<int> w2 = new List<int>();
		List<float> s2 = new List<float>();

		if (ponte2)
		{
			if (ondas2 > 0)
			{
				int total = 0;
				int dir = 0;
				float dec = 0;
				float v = 0;
				
				for (int i = 0; i < ondas2; i++)
				{
					int valor = Random.Range(1, passantes2 / ondas2 + 1);
					w2.Add(valor);
					dir = MeiaChance() ? 1 : -1;
					dec = MeiaChance() ? 0 : 0.9f;
					v = velBase + Random.Range(velMin,velMax);
					v *= dir;
					
					for (int j = 0; j < valor; j++)
					{
						s2.Add(v);
						v = v * Random.Range(dec * 0.75f, dec);
						total++;
					}
				}
				

				if (total < passantes2)
				{
					total = passantes2 - total;
					w2.Add(total);
					dir = MeiaChance() ? 1 : -1;
					dec = MeiaChance() ? 0 : 0.9f;
					v = velBase + Random.Range(velMin,velMax);
					v *= dir;
					
					for (int j = 0; j < total; j++)
					{
						s2.Add(v);
						v = v * Random.Range(dec * 0.75f, dec);
					}
				}
			}
			else
			{
				int total = 0;
				int dir = 0;
				float dec = 0;
				float v = 0;
				
				int tondas2 = -ondas2;
				if (tondas2 == 0)
				{
					tondas2 = 1;
				}
				
				for (int i = 0; i < passantes2 / tondas2; i++)
				{
					w2.Add(tondas2);
					
					dir = (MeiaChance() || soDaEsquerda) ? 1 : -1;
					dec = MeiaChance() ? 0 : 0.9f;
					v = velBase + Random.Range(velMin,velMax);
					v *= dir;
					
					for (int j = 0; j < tondas2; j++)
					{
						s2.Add(v);
						v = v * Random.Range(dec * 0.75f, dec);
						total++;
					}
				}
				
				if (total < passantes2)
				{
					total = passantes2 - total;
					w2.Add(total);
					dir = (MeiaChance() || soDaEsquerda) ? 1 : -1;
					dec = MeiaChance() ? 0 : 0.9f;
					v = velBase + Random.Range(velMin,velMax);
					v *= dir;
					
					for (int j = 0; j < total; j++)
					{
						s2.Add(v);
						v = v - v * Random.Range(dec * 0.75f, dec);
					}
				}
			}
		}
		
		List<int> ves = new List<int>();
		
		if (vento < 0)
		{
			int q = -vento;
			
			for (int i = 0; i < q; i++)
			{
				ves.Add(Random.Range(1,5));
			}
		}
		
		List<int> des = new List<int>();
		
		if (dvento < 0)
		{
			int q = -dvento;
			
			for (int i = 0; i < q; i++)
			{
				des.Add(Random.Range(1,9));
			}
		}
		
		f.dif = dif;
		
		f.people = passantes;
		f.waves = ondas;
		f.w = w;
		f.s = s;
		
		f.sbridge = ponte2;
		f.speople = passantes2;
		f.swaves = ondas2;
		f.sw = w2;
		f.ss = s2;
		
		f.boat = barco;
		
		f.wind = vento;
		f.wtime = tvento;
		f.v = ves;
		
		f.dir = dvento;
		f.dtime = dtvento;
		f.d = des;
		
		f.bouncers = CriarRebatedores(
			rebCima, rebMeio, rebBaixo, soDaEsquerda, dif);

		return f;
		//*/
	}

	public static void SalvarDados()
	{
		string saida = CriarStringSalvar();
		
		//Debug.Log("Saida: "+saida);
		
		PlayerPrefs.SetString(Dados.nomeChaveSalvar, saida);
	}
	
	public static void CarregarDados()
	{
		// REMOVER quando ajeitar os saves certinho
		if (PlayerPrefs.GetInt(Dados.nomeVersaoSalvarAtual)
		    != Dados.versaoSalvarAtual)
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.SetInt(
				Dados.nomeVersaoSalvarAtual, Dados.versaoSalvarAtual);
		}

		if (Dados.carregado == false)
		{
			if (PlayerPrefs.HasKey(Dados.nomeChaveSalvar))
			{
				Dados.estatisticas = 
					CarregarStringLocal(Dados.nomeChaveSalvar);
			}
			Dados.carregado = true;
		}
	}

	static Estatisticas CarregarStringLocal(string dados)
	{
		string entrada = PlayerPrefs.GetString(dados);
		//Debug.Log("entrada: "+entrada);

		string [] divisores = {divisorNovo};
		string [] lista = entrada.Split(
			divisores, System.StringSplitOptions.None);

		Dados.macasVerdeTotal = long.Parse(lista[0]);

		entrada = lista[1];
		
		Estatisticas es = new Estatisticas();
		es.mundos.Clear();
		
		for (int i = 0; i < Dados.totalDeMundos; i++)
		{
			int indice = i * (Dados.fasesPorMundo + 1);
			if (entrada[indice] == '1' || entrada[indice] == '2')
			{
				es.mundos.Add(new Estatisticas.Mundos());
				es.mundos[i].fases.Clear();
				for (int f = 0; f < Dados.fasesPorMundo; f++)
				{
					int ind = indice + f + 1;
					if (entrada[ind] == '1' || entrada[ind] == '2')
					{
						es.mundos[i].fases.Add(new Estatisticas.Fases());
						if (entrada[ind] == '2')
						{
							es.mundos[i].fases[f].completo = true;
						}
					}
				}
				if (entrada[indice] == '2')
				{
					es.mundos[i].completo = true;
				}
			}
		}
		
		es.VerificarMundosExtras();

		es = PegarDadosPontuacao(es, entrada);

		return es;
	}

	static string divisorNovo = "$";
	static string CriarStringSalvar()
	{
		// mundos, fazes no mundo
		// 0: nada
		// 1: aberto
		// 2: completo
		string saida = "";

		saida += "" + Dados.macasVerdeTotal + divisorNovo;
		
		Estatisticas.Mundos [] mundos = 
			Dados.estatisticas.mundos.ToArray();
		
		for (int m = 0; m < mundos.Length; m++)
		{
			if (mundos[m].completo)
			{
				saida += "2";
			}
			else
			{
				saida += "1";
			}
			
			for (int f = 0; f < mundos[m].fases.Count; f++)
			{
				if (mundos[m].fases[f].completo)
				{
					saida += "2";
				}
				else
				{
					saida += "1";
				}
			}
			
			for (int f = mundos[m].fases.Count; 
			     f < Dados.fasesPorMundo; f++)
			{
				saida += "0";
			}
		}
		
		for (int m = mundos.Length; m < Dados.totalDeMundos; m++)
		{
			saida += "0";
			for (int f = 0; f < Dados.fasesPorMundo; f++)
			{
				saida += "0";
			}
		}

		saida += CriarStringPontuacao();
		
		return saida;
	}

	static string divisor = "|";
	static Estatisticas PegarDadosPontuacao(Estatisticas es, string dados)
	{
		string [] divs = {divisor};
		string [] valores = 
			dados.Split(divs, System.StringSplitOptions.None);

		//Debug.Log ("Dados: "+dados);

		int i = 1;
		foreach(Estatisticas.Mundos m in es.mundos)
		{
			foreach(Estatisticas.Fases f in m.fases)
			{
				f.melhorPontuacao = int.Parse(valores[i]);
				i++;
			}
			i += Dados.fasesPorMundo - m.fases.Count;
		}
		i += (Dados.totalDeMundos - es.mundos.Count) * Dados.fasesPorMundo;
		//Debug.Log ("i depois dos mundos: "+i);

		// extra pro habilitado do jogorapido
		if (valores[i] == "1")
		{
			es.jogoRapido.liberado = true;
		}
		i++;

		for (int d = 0; d < Dados.jogoRapidoDificuldadeMaxima; d++)
		{
			es.jogoRapido.melhorPontuacao[d] = int.Parse(valores[i]);
			i++;
		}

		if (valores[i] == "1")
		{
			es.sobrevivencia.liberado = true;
		}
		i++;

		es.sobrevivencia.melhorOnda = int.Parse(valores[i]);
		i++;
		es.sobrevivencia.melhorPontuacao = int.Parse(valores[i]);

		return es;
	}

	static string CriarStringPontuacao()
	{
		string saida = "";

		int mundos = Dados.estatisticas.mundos.Count;

		for (int m = 0; m < Dados.totalDeMundos; m++)
		{
			if (mundos > m)
			{
				int fases = Dados.estatisticas.mundos[m].fases.Count;
				for (int f = 0; f < Dados.fasesPorMundo; f++)
				{
					if (fases > f)
					{
						saida += divisor + Dados.estatisticas.mundos[m]
							.fases[f].melhorPontuacao;
					}
					else
					{
						saida += divisor + "-1";
					}
				}
			}
			else
			{
				for (int f = 0; f < Dados.fasesPorMundo; f++)
				{
					saida += divisor + "-1";
				}
			}
		}

		saida += divisor + "1";
		for (int d = 0; d < Dados.jogoRapidoDificuldadeMaxima; d++)
		{
			saida += divisor + Dados.estatisticas.jogoRapido
				.melhorPontuacao[d];
		}

		if (Dados.estatisticas.sobrevivencia.liberado){
			saida += divisor + "1";
		}else{
			saida += divisor + "0";
		}
		saida += divisor + Dados.estatisticas.sobrevivencia
			.melhorOnda;
		saida += divisor + Dados.estatisticas.sobrevivencia
			.melhorPontuacao;

		return saida;
	}
}

