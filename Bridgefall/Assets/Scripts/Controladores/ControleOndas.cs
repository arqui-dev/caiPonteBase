using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ControleOndas : MonoBehaviour
{
	static ControleOndas _instancia = null;

	// Variáveis públicas
	public Navegacao navegador;

	public GameObject ponteDeBaixo;

	public Transform ponteCimaDireita;
	public Transform ponteCimaEsquerda;
	public Transform ponteBaixoDireita;
	public Transform ponteBaixoEsquerda;
	
	public ControleVento controleVento;
	public ControleFase controleFase;

	public GameObject somVitoria;

	public GameObject painelTempo;
	public Text txtTempo;

	public GameObject [] passantes;

	public GameObject [] sonsGrito;

	public float sobrevivenciaTempoAlterarVelocidadeVento = 20;
	public float sobrevivenciaTempoAlterarDirecaoVento = 12;
	public float sobrevivenciaTempoAlterarBarco = 15;

	public float multiplicadorAdicionarRebatorSobrevivencia = 2;

	// Variáveis privadas
	List<Onda> 	ondasCima = new List<Onda>();
	List<Onda> 	ondasBaixo = new List<Onda>();
	int			atualCima = 0;
	int			atualBaixo = 0;
	bool		terminouCima = false;
	bool		terminouBaixo = false;

	float 		tempoProximoCima = 0;
	float 		tempoProximoBaixo = 0;

	bool		esperandoAcabarFase = false;
	float 		tempoAcabarFase = 0;

	float		tempoDecorrido = 0;
	float		tempoInicial = 0;

	float sobrevivenciaProximoTempoAleterarVelocidadeVento = 20;
	float sobrevivenciaProximoTempoAleterarDirecaoVento = 12;
	float sobrevivenciaProximoTempoAlterarBarco = 15;

	Rebatedores rebatedores;

	// Variáveis estáticas
	public static bool faseCompleta = false;
	static bool perdeu = false;

	static int derrubadosCima = 0;
	static int derrubadosBaixo = 0;

	static int totalDerrubados = 0;

	// Variaveis para o analytics
	static float analyticsTempoInicial = 0;
	static float analyticsTempoInicialReal = 0;

	static void InicializarAnalytics()
	{
		analyticsTempoInicial = Time.time;
		analyticsTempoInicialReal = Time.realtimeSinceStartup;
	}

	static void EnviarAnalyticsVencer()
	{
		float tempoTotal = Time.time - analyticsTempoInicial;
		float tempoTotalReal = Time.realtimeSinceStartup - 
			analyticsTempoInicialReal;

		int mundo = Dados.mundoAtual;
		int fase = Dados.faseAtual;

		if (Dados.modoDeJogo != ModosDeJogo.Normal)
		{
			mundo = -1;
			fase = Dados.jogoRapidoDificuldade;
		}

		UnityAnalytics.CompletouFase(
			mundo, fase, totalDerrubados,
			Dados.pontosUltimaFase, tempoTotal,
			tempoTotalReal, Dados.bolasLancadasNestaFase,
			Dados.rebatedoresDestruidosNestaFase);
	}

	static void EnviarAnalyticsPerder()
	{
		float tempoTotal = Time.time - analyticsTempoInicial;
		float tempoTotalReal = Time.realtimeSinceStartup - 
			analyticsTempoInicialReal;

		UnityAnalytics.PerdeuNaFase(
			Dados.mundoAtual, Dados.faseAtual, totalDerrubados,
			Dados.pontosUltimaFase, tempoTotal,
			tempoTotalReal, Dados.bolasLancadasNestaFase,
			Dados.rebatedoresDestruidosNestaFase);
	}

	// Métodos públicos
	public void Inicializar(
		List<Onda> oc, List<Onda> ob)
	{
		_instancia = this;

		ondasCima = oc;
		ondasBaixo = ob;

		faseCompleta = false;
		perdeu = false;
		atualCima = 0;
		atualBaixo = 0;
		derrubadosCima = 0;
		derrubadosBaixo = 0;
		totalDerrubados = 0;
		terminouCima = false;
		terminouBaixo = false;
		esperandoAcabarFase = false;

		Dados.pontosUltimaFase = 0;
		Dados.pontosUltimaFasePassantes = 0;
		Dados.pontosUltimaFasePerfeita = 0;
		Dados.pontosUltimaFaseRebatidas = 0;
		Dados.pontosUltimaFaseDificuldade = 0;
		Dados.pontosUltimaFaseVelocidade = 0;
		Dados.pontosUltimaFaseBonus = 0;
		Dados.pontosUltimaFaseOnus = 0;

		Dados.bolasLancadasNestaFase = 0;
		Dados.bolaRebatidasTotaisFase = 0;
		Dados.rebatedoresDestruidosNestaFase = 0;

		ponteDeBaixo.SetActive(Dados.ponteBaixo);

		tempoProximoCima = 
			Time.time + Dados.passantesTempoEntre;

		tempoProximoBaixo = 
			Time.time + Dados.passantesTempoEntre * 2;

		Dados.margemEsquerda = GameObject.FindGameObjectWithTag(
			Dados.tagMargemEsquerda).transform.position.x;

		ControleMusica.MusicaJogar();

		InicializarAnalytics();

		painelTempo.SetActive(
			Dados.modoDeJogo != ModosDeJogo.Normal);

		tempoInicial = Time.time;
	}

	public void InicializarSobrevivencia(
		Rebatedores rebatedores, bool comeco = true)
	{
		this.rebatedores = rebatedores;

		/*
		if (comeco)
		{
			Dados.sobrevivenciaPontosTotais = 0;
			Dados.sobrevivenciaPontosPerfeito = 0;
			Dados.sobrevivenciaPontosBonus = 0;
			Dados.sobrevivenciaOndaAtual = 0;
			Dados.sobrevivenciaPontosPassantes = 0;
		}
		//*/

		List<Onda> ondasC = new List<Onda>();
		ondasC.Add(CriarNovaOndaSobrevivencia());

		List<Onda> ondasB = new List<Onda>();
		//ondasB.Add(CriarNovaOndaSobrevivencia());

		Inicializar(ondasC, ondasB);

		sobrevivenciaProximoTempoAleterarVelocidadeVento =
			Time.time + sobrevivenciaTempoAlterarVelocidadeVento
				- Dados.jogoRapidoDificuldade;

		sobrevivenciaProximoTempoAleterarDirecaoVento =
			Time.time + sobrevivenciaTempoAlterarDirecaoVento
				- Dados.jogoRapidoDificuldade;

		sobrevivenciaProximoTempoAlterarBarco =
			Time.time + sobrevivenciaTempoAlterarBarco
				- Dados.jogoRapidoDificuldade;

		Dados.ventoVelocidade = Dados.VENTO_VELOCIDADE_LENTO * 0.9f;
		Dados.vento = false;
		Dados.barcoMove = false;
		controleVento.Desabilitar();
		controleVento.Reiniciar();

		rebatedores.RemoverRebatedores();

		rebatedores.AdicionarEstaticosJogoRapido();
	}

	// Métodos privados
	void Awake()
	{
		_instancia = this;
	}

	void Update()
	{
		// ANALYTICS posicao toque
		if (Dados.pausado == false && Input.GetMouseButtonDown(0))
		{
			UnityAnalytics.AdicionarPontoTocado();
		}
		// FIM ANALYTICS

		tempoDecorrido = Time.time - tempoInicial;
		if (Dados.modoDeJogo != ModosDeJogo.Normal)
		{
			AtualizarTempo();
		}

		if (Dados.modoDeJogo == ModosDeJogo.Sobrevivencia)
		{
			AtualizarSobrevivencia();
			return;
		}

		if (faseCompleta == false)
		{
			AtualizarOndaCima();

			if (Dados.ponteBaixo){
				AtualizarOndaBaixo();
			}

			if (terminouCima){
				if (Dados.ponteBaixo == false ||
				    terminouBaixo)	
				{
					faseCompleta = true;
				}
			}

			if (perdeu){
				PerderNaFase();
			}
		} 
		else
		{
			if (esperandoAcabarFase == false)
			{
				FimDaFase();
			}
			else
			{
				if (Time.time > tempoAcabarFase){
					CompletarFase();
				}
			}
		}

#if UNITY_EDITOR
		if (Input.GetKeyUp(KeyCode.A)){
			CompletarFase();
		}
#endif
	}

	// JECA
	void AtualizarSobrevivencia()
	{
		AtualizarVentoEBarcoSobrevivencia();

		AtualizarOndaCima();
		
		if (Dados.ponteBaixo){
			AtualizarOndaBaixo();
		}
		
		if (perdeu){
			//PerderNaFase();
			CompletarFase();
		}
	}


	void AtualizarTempo()
	{
		txtTempo.text = tempoDecorrido.ToString("0");
	}

	void AtualizarVentoEBarcoSobrevivencia()
	{
		if (Time.time > sobrevivenciaProximoTempoAleterarVelocidadeVento)
		{
			controleVento.AlterarVelocidadeSobrevivencia();

			sobrevivenciaProximoTempoAleterarVelocidadeVento =
				Time.time + sobrevivenciaTempoAlterarVelocidadeVento
					- Dados.jogoRapidoDificuldade;
		}

		if (Time.time > sobrevivenciaProximoTempoAleterarDirecaoVento)
		{
			controleVento.AlterarDirecaoSobrevivencia();
			
			sobrevivenciaProximoTempoAleterarDirecaoVento =
				Time.time + sobrevivenciaTempoAlterarDirecaoVento
					- Dados.jogoRapidoDificuldade;
		}

		if (Time.time > sobrevivenciaProximoTempoAlterarBarco)
		{
			ControleBarco.AlterarSobrevivencia();

			sobrevivenciaProximoTempoAlterarBarco =
				Time.time + sobrevivenciaTempoAlterarBarco
					- Dados.jogoRapidoDificuldade;
		}
	}
	
	public static void SobrevivenciaRebatedor()
	{
		if (Dados.modoDeJogo != ModosDeJogo.Sobrevivencia ||
		    _instancia == null)
		{
			return;
		}
		_instancia.VerificarRebatedor();
	}

	void VerificarRebatedor()
	{
		float valor = ((float) Dados.jogoRapidoDificuldade /
		               Dados.jogoRapidoDificuldadeMaxima);

		valor *= multiplicadorAdicionarRebatorSobrevivencia / 10;

		//Debug.Log("Valor "+valor);
		
		if (Random.value < valor)
		{
			AdicionarRebatedor();
		}
		//AdicionarRebatedor();
	}

	void AdicionarRebatedor()
	{
		rebatedores.AdicionarDestrutivelAleatorio();
	}

	void CriarNovaOndaSobrevivenciaCima()
	{
		ondasCima.Clear();
		ondasCima.Add(CriarNovaOndaSobrevivencia());
		atualCima = 0;
	}

	public bool podeLancarMaisDeUmSobrevivencia = false;
	Onda CriarNovaOndaSobrevivencia()
	{
		int dir = 1;
		if (Utilidade.MeiaChance()) dir = -1;

		List<Passante> listapassantes = new List<Passante>();

		float vel = Random.Range(1.0f, 1.5f) * 
			((float)Dados.jogoRapidoDificuldade * dir);

		Passante passante = new Passante(vel, Pontes.Cima);
		listapassantes.Add(passante);

		if (podeLancarMaisDeUmSobrevivencia)
		{
			float changeVir1 = ((float)Dados.jogoRapidoDificuldadeMaxima - 
			                    Dados.jogoRapidoDificuldade + 1);
			
			changeVir1 /= ((float) Dados.jogoRapidoDificuldadeMaxima);
			
			float chanceVir2 = 1f - changeVir1;
			float chanceVir3 = chanceVir2 * 0.333f;
			
			float aleatorio = Random.value;

			if (aleatorio < chanceVir2)
			{
				Passante passante2 = new Passante(vel * 0.8f, Pontes.Cima);
				listapassantes.Add(passante2);
			}

			if (aleatorio < chanceVir3)
			{
				Passante passante3 = new Passante(vel * 0.6f, Pontes.Cima);
				listapassantes.Add(passante3);
			}
		}

		Onda onda = new Onda(listapassantes);

		return onda;
	}

	void SobrevivenciaLancarPassanteCima()
	{
		if (ondasCima.Count == 0 || ondasCima[0].Acabou())
		{
			ondasCima.Clear();
			ondasCima.Add(CriarNovaOndaSobrevivencia());
		}

		Passante p = ondasCima[atualCima].Lancar();

		if (p != null)
		{
			Lancar(p);
			if (ondasCima[atualCima].Gritar()){
				Gritar();
			}
		}
	}

	void Gritar()
	{
		if (sonsGrito == null)
		{
			Debug.Log("Colocar som do grito");
			return;
		}

		GameObject somGrito = 
			sonsGrito[Random.Range(0,sonsGrito.Length)];

		Instantiate<GameObject>(somGrito);
	}

	void AtualizarOndaCima()
	{
		if (terminouCima == false){
			if (ondasCima[atualCima].Acabou() == false)
			{
				if (Time.time > tempoProximoCima)
				{
					tempoProximoCima = 
						Time.time + Dados.passantesTempoEntre;

					if (Dados.modoDeJogo == ModosDeJogo.Sobrevivencia)
					{
						SobrevivenciaLancarPassanteCima();
					}
					else
					{
						LancarPassanteCima();
					}
				}
			}
			else if (derrubadosCima >= ondasCima[atualCima].quantidade)
			{
				derrubadosCima = 0;
				atualCima++;

				if (atualCima >= ondasCima.Count)
				{
					if (Dados.modoDeJogo == ModosDeJogo.Sobrevivencia)
					{
						CriarNovaOndaSobrevivenciaCima();
						tempoProximoCima = 
							Time.time + Dados.passantesTempoEntre;
					}
					else
					{
						terminouCima = true;
						atualCima = 0;
					}
				}
				else
				{
					tempoProximoCima = 
						Time.time + Dados.passantesTempoEntre;

					// Se não tiver tempo entre as ondas, lançar o
					// próximo direto (descomentar a linha abaixo)
					//LancarPassanteCima();
				}
			}
		}
	}

	void AtualizarOndaBaixo()
	{
		if (terminouBaixo == false){
			if (ondasBaixo[atualBaixo].Acabou() == false)
			{
				if (Time.time > tempoProximoBaixo)
				{
					tempoProximoBaixo = 
						Time.time + Dados.passantesTempoEntre;
					
					LancarPassanteBaixo();
				}
			}
			else if (derrubadosBaixo >= ondasBaixo[atualBaixo].quantidade)
			{
				derrubadosBaixo = 0;
				atualBaixo++;
				if (atualBaixo >= ondasBaixo.Count){
					terminouBaixo = true;
					atualBaixo = 0;
				}else{
					tempoProximoBaixo = 
						Time.time + Dados.passantesTempoEntre;

					// Se não tiver tempo entre as ondas, lançar o
					// próximo direto (descomentar a linha abaixo)
					//LancarPassanteBaixo();
				}
			}
		}
	}

	void LancarPassanteCima()
	{
		Passante p = ondasCima[atualCima].Lancar();
		if (p != null){
			Lancar(p);
			if (ondasCima[atualCima].Gritar()){
				Gritar();
			}
		}
	}

	void LancarPassanteBaixo()
	{
		Passante p = ondasBaixo[atualBaixo].Lancar();
		if (p != null){
			Lancar(p);
			if (ondasBaixo[atualBaixo].Gritar()){
				Gritar();
			}
		}
	}

	void Lancar(Passante p)
	{
		float 		destino = ponteCimaDireita.position.x;
		Vector3 	posicao = ponteCimaEsquerda.position;

		int 		indice = Random.Range(0, passantes.Length);
		GameObject 	prePass = passantes[indice];

		if (p.ponte == Pontes.Cima){
			if (p.direcao == Passante.Direcoes.ParaEsquerda){
				destino = ponteCimaEsquerda.position.x;
				posicao = ponteCimaDireita.position;
			}
		}else{
			if (p.direcao == Passante.Direcoes.ParaEsquerda){
				destino = ponteBaixoEsquerda.position.x;
				posicao = ponteBaixoDireita.position;
			}else{
				destino = ponteBaixoDireita.position.x;
				posicao = ponteBaixoEsquerda.position;
			}
		}

		GameObject passante = (GameObject) Instantiate(
			prePass, posicao, Quaternion.identity);

		if (p.direcao == Passante.Direcoes.ParaEsquerda)
		{
			passante.transform.localScale = new Vector3(
				-passante.transform.localScale.x,
				passante.transform.localScale.y,
				passante.transform.localScale.z);
		}

		passante.GetComponent<ControlePassante>()
			.Criar(p, destino);
	}

	// Fim da fase
	void FimDaFase()
	{
		esperandoAcabarFase = true;
		tempoAcabarFase = 
			Time.time + Dados.passantesTempoEntre * 2;

		if (somVitoria && Dados.somLigado){
			Instantiate(
				somVitoria, 
				transform.position,
				transform.rotation);
		}
	}

	void CompletarFase()
	{
		/*
		if (Dados.modoDeJogo == ModosDeJogo.Sobrevivencia)
		{
			CompletarOnda();
			return;
		}
		//*/

		Dados.pontosUltimaFasePerfeita = PontosFasePerfeita();
		Dados.pontosUltimaFaseDificuldade = PontosDificuldade();
		Dados.pontosUltimaFaseOnus = CalcularPenalidades();

		Dados.pontosUltimaFaseBonus = 
			Dados.pontosUltimaFaseRebatidas 	+ 
			Dados.pontosUltimaFaseVelocidade 	+
			Dados.pontosUltimaFaseDificuldade;

		Dados.pontosUltimaFase = 
			Dados.pontosUltimaFasePassantes +
			Dados.pontosUltimaFasePerfeita 	+ 
			Dados.pontosUltimaFaseBonus 	-
			Dados.pontosUltimaFaseOnus;

		/*
		Debug.Log("Pontuação final:\n"+
		          "Passantes: "+Dados.pontosUltimaFasePassantes+"\n"+
		          "Bônus: "+Dados.pontosUltimaFaseBonus+"\n"+
		          "Ônus: "+Dados.pontosUltimaFaseOnus+"\n"+
		          "Perfeita: "+Dados.pontosUltimaFasePerfeita+"\n"+
		          "Dificuldade: "+Dados.pontosUltimaFaseDificuldade+"\n"+
		          "Velocidade: "+Dados.pontosUltimaFaseVelocidade+"\n"+
		          "Rebatidas: "+Dados.pontosUltimaFaseRebatidas+"\n"+
		          "");
		//*/

		EnviarAnalyticsVencer();

		Debug.Log("Pontos ultima fase: "+Dados.pontosUltimaFase);

		switch(Dados.modoDeJogo){
		case ModosDeJogo.Normal:

			if (Dados.pontosUltimaFase > 0)
			{
				Debug.Log("Pontuar plataforma social");
			}

			if (Dados.estatisticas.mundos[Dados.mundoAtual]
			    .fases[Dados.faseAtual].completo == false)
			{
				Utilidade.AdicionarMacasPorQuantidade(1);
				UnityAnalytics.GanhouMaca(false, 1);
			}

			bool perfect = 
				Dados.bolasLancadasNestaFase <= totalDerrubados;

			Dados.estatisticas.mundos[Dados.mundoAtual]
				.fases[Dados.faseAtual]
				.Completou(Dados.pontosUltimaFase, perfect);

			Debug.Log("Estatisticas: \n"+
			          "Pontos Agora:  "+Dados.pontosUltimaFase+"\n"+
			          "Pontos Melhor: "+Dados.estatisticas.mundos[Dados.mundoAtual].fases[Dados.faseAtual].melhorPontuacao+"\n"+
			          "Perfect:       "+Dados.estatisticas.mundos[Dados.mundoAtual].fases[Dados.faseAtual].perfect+"\n"+
			          "");
			
			Dados.estatisticas.mundos[Dados.mundoAtual]
				.VerificarFasesCompletas();
			
			Dados.estatisticas.VerificarMundosExtras();

			Utilidade.SalvarDados();

			ControleMusica.Vitoria();
			navegador.CarregarTela(Telas.Jogo_Normal_Vitoria, false);
			break;
		case ModosDeJogo.JogoRapido:
		case ModosDeJogo.Sobrevivencia:

			//GerenciadorUnityAds.ShowRewardedAd();
			if (Dados.pontosUltimaFase > 0)
			{
				Debug.Log("Pontuar plataforma social");
			}

			Utilidade.AdicionarMacasPorPontos(Dados.pontosUltimaFase);

			Dados.estatisticas.jogoRapido
				.Pontuar(Dados.pontosUltimaFase,
				         Dados.jogoRapidoDificuldade);

			Utilidade.SalvarDados();

			ControleMusica.Vitoria();
			navegador.CarregarTela(Telas.Jogo_Rapido_Vitoria, false);
			break;
		default:
			navegador.CarregarTelaMenu();
			break;
		}
	}


	static int CalcularPenalidades()
	{
		if (Dados.modoDeJogo == ModosDeJogo.Sobrevivencia)
		{
			return 0;
		}

		float r = Dados.rebatedoresDestruidosNestaFase;
		float d = Dados.pontosDivisorOnus;
		float p = r / (r + d);
		float f = p * Dados.pontosUltimaFasePassantes;
		int v = (int) (f / Dados.pontosBase);

		/*
		Debug.Log ("Penal: "+(Dados.pontosBase * v)+"\n"+
		           "r: "+r+"\n"+
		           "d: "+d+"\n"+
		           "p: "+p+"\n"+
		           "f: "+f+"\n"+
		           "v: "+v);
		//*/

		return Dados.pontosBase * v;
	}

	static int PontosDificuldadeRapido()
	{
		int pontos = Dados.pontosUltimaFasePassantes / Dados.pontosBase;
		pontos *= Dados.jogoRapidoDificuldade;
		pontos /= Dados.jogoRapidoDificuldadeMaxima;
		pontos = pontos > 1 ? pontos : 1;
		pontos *= Dados.pontosBase;

		return pontos;
	}

	static int PontosDificuldade()
	{
		if (Dados.modoDeJogo == ModosDeJogo.Sobrevivencia)
		{
			return 0;
		}

		if (Dados.modoDeJogo == ModosDeJogo.JogoRapido)
		{
			return PontosDificuldadeRapido();
		}

		int pontos = Dados.pontosUltimaFasePassantes / Dados.pontosBase;
		pontos *= Dados.dificuldadeFaseAtual;
		pontos /= Dados.dificuldadeMaximaCampanha;
		pontos = pontos > 0 ? pontos : 0;
		pontos *= Dados.pontosBase;
		
		return pontos;
	}

	int PontosFasePerfeita()
	{
		if (Dados.modoDeJogo == ModosDeJogo.Sobrevivencia)
		{
			return 0;
		}

		if (Dados.bolasLancadasNestaFase <= totalDerrubados)
		{
			int p = Dados.pontosBase * Dados.bolasLancadasNestaFase *
				Dados.pontosMultiplicadorBasePerfeitoPorBola;

			return p;
		}

		return 0;
	}

	void CompletarOnda()
	{
		Dados.sobrevivenciaPontosPassantes += 
			Dados.pontosUltimaFasePassantes;
		Dados.sobrevivenciaPontosTotais +=
			Dados.pontosUltimaFase;
		Dados.sobrevivenciaPontosPerfeito +=
			Dados.pontosUltimaFasePerfeita;
		Dados.sobrevivenciaPontosBonus +=
			Dados.pontosUltimaFaseBonus;

		Dados.sobrevivenciaOndaAtual++;

		// Fazer algo mostrando que acabou onda,
		// dar pontos por perfect, sinalizar

		InicializarSobrevivencia(rebatedores, false);
	}

	void PerdeuSobrevivencia()
	{
		Dados.estatisticas.sobrevivencia
			.Pontuar(Dados.sobrevivenciaPontosTotais);
		
		Utilidade.SalvarDados();
		
		ControleMusica.Derrota();
		navegador.CarregarTela(Telas.Jogo_Sobrevivencia_Fim, false);
	}

	void PerderNaFase()
	{
		//GerenciadorUnityAds.ShowRewardedAd();

		EnviarAnalyticsPerder();

		switch(Dados.modoDeJogo){
		case ModosDeJogo.Normal:
			Dados.estatisticas
				.mundos[Dados.mundoAtual]
				.fases[Dados.faseAtual]
				.Pontuar(Dados.pontosUltimaFase, true);

			Utilidade.SalvarDados();

			ControleMusica.Derrota();
			navegador.CarregarTela(Telas.Jogo_Normal_Derrota, false);
			break;
		case ModosDeJogo.JogoRapido:
		case ModosDeJogo.Sobrevivencia:

			Utilidade.AdicionarMacasPorPontos(Dados.pontosUltimaFase);
			//Debug.Log ("Pontos ultima fase "+Dados.pontosUltimaFase);

			Dados.estatisticas.jogoRapido
				.Pontuar(Dados.pontosUltimaFase,
				         Dados.jogoRapidoDificuldade, true);

			Utilidade.SalvarDados();

			ControleMusica.Derrota();
			//navegador.CarregarTela(Telas.Jogo_Rapido_Derrota, false);
			navegador.CarregarTela(Telas.Jogo_Rapido_Vitoria, false);
			break;
		//case ModosDeJogo.Sobrevivencia:
		//	PerdeuSobrevivencia();
		//	break;
		default: 
			navegador.CarregarTelaMenu(); 
			break;
		}
	}

	// Métodos estáticos
	public static void DerrubouPassante(Passante p, int pontos)
	{
		Dados.pontosUltimaFasePassantes += pontos;

		totalDerrubados++;
		
		if (p.ponte == Pontes.Cima){
			derrubadosCima++;
		}else{
			derrubadosBaixo++;
		}
	}
	
	public static void Perder(){
		perdeu = true;
	}
}

