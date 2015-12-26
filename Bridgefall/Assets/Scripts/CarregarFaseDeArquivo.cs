using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarregarFaseDeArquivo : MonoBehaviour
{
	// Variável de apoio, caso não consiga ler do arquivo.
	static string arquivoBasico = 
		"world 1/"+
			"level 1/people=1/waves=-1/s=1/end/"+
			"level 2/people=1/waves=-1/s=1/end/"+
			"level 3/people=1/waves=-1/s=1/end/"+
			"level 4/people=1/waves=-1/s=1/end/"+
			"level 5/people=1/waves=-1/s=1/end/"+
			"level 6/people=1/waves=-1/s=1/end/"+
			"level 7/people=1/waves=-1/s=1/end/"+
			"level 8/people=1/waves=-1/s=1/end/"+
			"level 9/people=1/waves=-1/s=1/end/"+
			"end world";

	// Métodos privados
	static string PegarDeString(int mundo)
	{
		return ArquivosString.mundos[mundo];
	}

	static string PegarDifDeString(int dif)
	{
		return ArquivosString.dificuldades[dif];
	}

	static string Ler(string arquivo, int mundo)
	{
		if (mundo >= 0)
		{
			//*
			if (Application.platform != RuntimePlatform.Android)
			{
				TextAsset texto = Resources.Load<TextAsset>(arquivo);

				if (texto != null)
				{
					return texto.text;
				}
			}
			//*/

			return PegarDeString(mundo);
		}
		else
		{
			//*
			if (Application.platform != RuntimePlatform.Android)
			{
				TextAsset texto = Resources.Load<TextAsset>(arquivo);
				
				if (texto != null)
				{
					return texto.text;
				}
			}
			//*/

			mundo = -mundo - 1;

			return PegarDifDeString(mundo);
		}
	}

	static string[] PegarLinhasDeArquivo(string arquivo, int mundo)
	{
		string arq = Ler(arquivo, mundo);

		//Debug.Log ("Arquivo \""+arquivo+"\": "+arq);

		if (string.IsNullOrEmpty(arq)){
			arq = arquivoBasico;
		}

		arq = arq.Replace(" ","");
		arq = arq.Replace("\t","");
		arq = arq.Replace("\r\n","/");
		arq = arq.Replace("\r","/");
		arq = arq.Replace("\n","/");
		//while (arq.Contains("//")){
		arq = arq.Replace("//","/");
		arq = arq.Replace("//","/");
		//}

		string[] linhas = arq.Split('/');
		string[] semcomentarios = new string[linhas.Length];
		int i = 0;
		foreach(string l in linhas)
		{
			if (l.StartsWith("#") == false &&
			    l.StartsWith("/") == false &&
			    string.IsNullOrEmpty(l) == false)
			{
				semcomentarios[i] = l;
				i++;
			}
		}
		string[] retorno = new string[i];
		for(int l = 0; l < i; l++){
			retorno[l] = semcomentarios[l];
		}
		return retorno;
	}

	static Mundo CarregarMundoDoArquivo(string arquivo, int mun)
	{
		string[] linhas = PegarLinhasDeArquivo(arquivo, mun);

		if (linhas == null){
			return null;
		}
		
		int nivel = 1;
		Mundo mundo = null;
		
		string w = "";
		string s = "";
		string sw = "";
		string ss = "";
		string v = "";
		string d = "";
		
		for (int i = 0; i < linhas.Length; i++){
			if (mundo == null && linhas[i].StartsWith("world")){
				mundo = new Mundo();
			}else if (mundo != null){
				if (linhas[i].StartsWith("level")){
					mundo.fases.Add(new Fase());
					nivel = mundo.totalDeFases;
					mundo.totalDeFases++;
					w = "";
					s = "";
					sw = "";
					ss = "";
					v = "";
					d = "";
				}else{
					string[] lin = linhas[i].Split('=');
					
					switch(lin[0]){
					case "dif":
						mundo.fases[nivel].dif = 
							int.Parse(lin[1]);
						break;
					case "people":
						mundo.fases[nivel].people =
							int.Parse(lin[1]);
						break;
					case "waves":
						mundo.fases[nivel].waves =
							int.Parse(lin[1]);
						if (mundo.fases[nivel].waves < 0){
							int q = -mundo.fases[nivel].waves;
							int totalwaves =
								mundo.fases[nivel].people / q;
							int r = mundo.fases[nivel].people % q;
							
							for(int j = 0; j < totalwaves; j++){
								mundo.fases[nivel].w.Add(q);
								w += "w:" + q + "\n";
							}
							if (r > 0){
								mundo.fases[nivel].w.Add(r);
								w += "w:" + r + "\n";
							}
						}
						break;
					case "w":
						mundo.fases[nivel].w.Add(int.Parse(lin[1]));
						w += "w:" + lin[1] + "\n";
						break;
					case "s":
						mundo.fases[nivel].s.Add(float.Parse(lin[1]));
						s += "s:" + lin[1] + "\n";
						break;
					case "sbridge":
						if (lin[1].Contains("1") ||
						    lin[1].Contains("true"))
						{
							mundo.fases[nivel].sbridge = true;
						}
						break;
					case "speople":
						mundo.fases[nivel].speople =
							int.Parse(lin[1]);
						break;
					case "swaves":
						mundo.fases[nivel].swaves =
							int.Parse(lin[1]);
						if (mundo.fases[nivel].swaves < 0){
							int q = -mundo.fases[nivel].swaves;
							int totalwaves =
								mundo.fases[nivel].speople / q;
							int r = mundo.fases[nivel].speople % q;
							
							for(int j = 0; j < totalwaves; j++){
								mundo.fases[nivel].sw.Add(q);
								sw += "sw:" + q + "\n";
							}
							if (r > 0){
								mundo.fases[nivel].sw.Add(r);
								sw += "sw:" + q + "\n";
							}
						}
						break;
					case "sw":
						mundo.fases[nivel].sw.Add(int.Parse(lin[1]));
						sw += "sw:" + lin[1] + "\n";
						break;
					case "ss":
						mundo.fases[nivel].ss.Add(float.Parse(lin[1]));
						ss += "ss:" + lin[1] + "\n";
						break;
					case "boat":
						mundo.fases[nivel].boat =
							int.Parse(lin[1]);
						break;
					case "wind":
						mundo.fases[nivel].wind =
							int.Parse(lin[1]);
						break;
					case "wtime":
						mundo.fases[nivel].wtime =
							float.Parse(lin[1]);
						break;
					case "v":
						mundo.fases[nivel].v.Add(int.Parse(lin[1]));
						v += "v:" + lin[1] + "\n";
						break;
					case "wdir":
						mundo.fases[nivel].dir =
							int.Parse(lin[1]);
						break;
					case "dtime":
						mundo.fases[nivel].dtime =
							float.Parse(lin[1]);
						break;
					case "d":
						mundo.fases[nivel].d.Add(int.Parse(lin[1]));
						d += "d:" + lin[1] + "\n";
						break;
					case "b":
						mundo.fases[nivel].bouncers = 
							PegarRebatedores(
								mundo.fases[nivel].bouncers, lin[1]);
						break;
					default:
						/*
						Debug.Log(
							"dif:"+mundo.fases[nivel].dif+"\n"+
							"people:"+mundo.fases[nivel].people+"\n"+
							"waves:"+mundo.fases[nivel].waves+"\n"+
							w+s+
							"sbridge:"+mundo.fases[nivel].sbridge+"\n"+
							"speople:"+mundo.fases[nivel].speople+"\n"+
							"swaves:"+mundo.fases[nivel].swaves+"\n"+
							sw+ss+
							"boat:"+mundo.fases[nivel].boat+"\n"+
							"wind:"+mundo.fases[nivel].wind+"\n"+
							"wtime:"+mundo.fases[nivel].wtime+"\n"+
							v+
							"dir:"+mundo.fases[nivel].dir+"\n"+
							"dtime:"+mundo.fases[nivel].dtime+"\n"+
							d+"\n"
							);
						//*/
						if (lin[0].StartsWith("endworld")){
							return mundo;
						}else if (lin[0].StartsWith("end")){
							continue;
						}
						break;
					}
				}
			}
		}
		return mundo;
	}

	static List<Rebatedor> PegarRebatedores(
		List<Rebatedor> rebs, string s)
	{
		string[] lin = s.Split(';');
		string[] quad = lin[0].Split(':');
		int quadrante = 0;

		switch(quad[1]){
		case "tl":
			quadrante = 0;
			break;
		case "tm":
			quadrante = 1;
			break;
		case "tr":
			quadrante = 2;
			break;
		case "ml":
			quadrante = 3;
			break;
		case "mm":
			quadrante = 4;
			break;
		case "mr":
			quadrante = 5;
			break;
		case "bl":
			quadrante = 6;
			break;
		case "bm":
			quadrante = 7;
			break;
		case "br":
			quadrante = 8;
			break;
		default:
			quadrante = int.Parse(quad[1]);
			break;
		}

		for (int i = 1; i < lin.Length; i++){
			string [] com = lin[i].Split(':');
			string [] pos;
			string tipoRebate = com[0];
			bool destrutivel = false;
			if (com[0].StartsWith("d")){
				destrutivel = true;
				tipoRebate = com[0].Substring(1);
			}
			switch(tipoRebate){
			case "s":
				pos = com[1].Split(',');
				for (int j = 0; j < pos.Length; j++){
					int plocal = int.Parse(pos[j]);
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = plocal;
					reb.tipo = Rebatedores.Tipo.Fixo;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}
				break;
			case "vu":
				pos = com[1].Split(',');
				for (int j = 0; j < pos.Length; j++){
					int plocal = int.Parse(pos[j]);
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = plocal;
					reb.tipo = Rebatedores.Tipo.VerticalBaixoCima;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}
				break;
			case "vd":
				pos = com[1].Split(',');
				for (int j = 0; j < pos.Length; j++){
					int plocal = int.Parse(pos[j]);
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = plocal;
					reb.tipo = Rebatedores.Tipo.VerticalCimaBaixo;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}
				break;
			case "hr":
				pos = com[1].Split(',');
				for (int j = 0; j < pos.Length; j++){
					int plocal = int.Parse(pos[j]);
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = plocal;
					reb.tipo = Rebatedores.Tipo.HorizontalEsqDir;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}
				break;
			case "hl":
				pos = com[1].Split(',');
				for (int j = 0; j < pos.Length; j++){
					int plocal = int.Parse(pos[j]);
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = plocal;
					reb.tipo = Rebatedores.Tipo.HorizontalDirEsq;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}
				break;
			case "dr":
				pos = com[1].Split(',');
				for (int j = 0; j < pos.Length; j++){
					int plocal = int.Parse(pos[j]);
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = plocal;
					reb.tipo = Rebatedores.Tipo.DiagonalEsqDir;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}
				break;
			case "dl":
				pos = com[1].Split(',');
				for (int j = 0; j < pos.Length; j++){
					int plocal = int.Parse(pos[j]);
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = plocal;
					reb.tipo = Rebatedores.Tipo.DiagonalDirEsq;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}
				break;
			case "c":
				pos = com[1].Split(',');
				for (int j = 0; j < pos.Length; j++){
					int plocal = int.Parse(pos[j]);
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = plocal;
					reb.tipo = Rebatedores.Tipo.Circular;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}
				break;
			case "zig":
				if (com[1] == "0"){
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = 0;
					reb.tipo = Rebatedores.Tipo.Circular;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);

					reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = 2;
					reb.tipo = Rebatedores.Tipo.Circular;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);

					reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = 4;
					reb.tipo = Rebatedores.Tipo.Circular;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}else{
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = 1;
					reb.tipo = Rebatedores.Tipo.Circular;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
					
					reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = 3;
					reb.tipo = Rebatedores.Tipo.Circular;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
					
					reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = 5;
					reb.tipo = Rebatedores.Tipo.Circular;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}
				break;
			case "diag":
				if (com[1] == "0"){
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = 0;
					reb.tipo = Rebatedores.Tipo.Circular;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
					
					reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = 5;
					reb.tipo = Rebatedores.Tipo.Circular;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}else{
					Rebatedor reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = 2;
					reb.tipo = Rebatedores.Tipo.Circular;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
					
					reb = new Rebatedor();
					reb.posicaoGrade = quadrante;
					reb.posicaoLocal = 3;
					reb.tipo = Rebatedores.Tipo.Circular;
					reb.destrutivel = destrutivel;
					rebs.Add(reb);
				}
				break;
			}
		}

		return rebs;
	}
	
	// Métodos públicos

	// Carrega apenas arquivo ".txt", onde no caminho a extensão
	// deve ser omitida, e o arquivo deve estar na pasta
	// "Resources", dentro da pasta "Assets".
	public static Mundo CarregarMundo(int mundo)
	{
		string arquivo = Dados.nomeArquivoMundo + (mundo + 1);

		return CarregarMundo(arquivo, mundo);
	}

	public static Mundo CarregarMundo(string arquivo, int mundo)
	{
		return CarregarMundoDoArquivo(arquivo, mundo);
	}

	public static Mundo CarregarDificuldade(int dif)
	{
		string arquivo = Dados.nomeArquivoDificuldade + dif;

		return CarregarMundoDoArquivo(arquivo, -dif);
	}
}

