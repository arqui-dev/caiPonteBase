using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControleTela : MonoBehaviour
{
	// Controle de instancia
	public static ControleTela _instancia = null;

	// Variáveis Públicas
	public RectTransform painel;
	public RectTransform [] paineisEsquerda;
	public RectTransform [] paineisDireita;

	public Text textoDificuldadeJogoRapido;

	public GameObject som;

	public MostrarPontuacao mostradorDePontos;

	public float tempoEsperarAd = 0.25f;

	// Variáveis Privadas
	//float tempo = 0.75f;
	float tempoMovimento = 0;
	//bool mover = false;
	bool podeTocarSom = false;

	//float proximoTempoAd = 0;

	Vector3 deslocamentoPorTela = Vector2.zero;

	Vector3 velocidade = Vector2.zero;
	Vector3 alvo = Vector2.zero;

	// Variáveis estáticas
	//static bool irPara = false;
	//static int tx = 0;
	//static int ty = 0;


	//static int xatual = 0;
	//static int yatual = 0;
	static int xanterior = 0;
	static int yanterior = 0;

	//static bool mostrou = false;

	static bool podeMover = true;

	// Métodos públicos
	public void TelaMenu()
	{
		ControleMusica.MusicaMenu();

		if (podeMover == false)
		{
			podeMover = true;
			return;
		}

		PrepararMovimento(0,0);

		Navegacao.CarregarTelaEstatico(Telas.Menu);
	}

	public void TelaConfiguracoes()
	{
		PrepararMovimento(0,-1);
		Navegacao.CarregarTelaEstatico(Telas.Menu);
	}

	public void TelaCreditos()
	{
		PrepararMovimento(1,-1);
		Navegacao.CarregarTelaEstatico(Telas.Menu_Creditos);
	}

	public void TelaAjuda()
	{
		PrepararMovimento(0,1);
		Navegacao.CarregarTelaEstatico(Telas.Menu_Ajuda);
	}

	public void TelaPontuacao()
	{
		mostradorDePontos.Carregar();
		PrepararMovimento(-1,-1);

		//GooglePlay.MostrarLeaderBoards();
		Navegacao.CarregarTelaEstatico(Telas.Menu_Pontuacao);
	}

	public void TelaGooglePlayPontuacao()
	{
		//GooglePlay.MostrarLeaderBoards();
		Debug.Log ("Tela mostrar leaderboards google play");
	}

	public void TelaRealizacoes()
	{
		PrepararMovimento(-1,0);
	}

	public void TelaModoJogo()
	{
		ControleModosDeJogo.Recarregar();
		PrepararMovimento(0,0);
		Navegacao.CarregarTelaEstatico(Telas.Menu_ModoJogo);
	}

	public void TelaEscolherMundo()
	{
		ControleMundosLiberados.Recarregar();
		if (Dados.estatisticas.mundos.Count == 1){
			TelaFases(0);
		}else{
			PrepararMovimento(-1,0);
			Navegacao.CarregarTelaEstatico(Telas.Menu_Mundos);
		}
	}

	public void TelaJogoRapido()
	{
		Dados.modoDeJogo = ModosDeJogo.JogoRapido;
		PrepararMovimento(1,0);
		Navegacao.CarregarTelaEstatico(Telas.Menu_Sobrevivencia);
	}

	public void TelaSobrevivencia()
	{
		TelaJogoRapido();
	}

	public void TelaFases(int mundo)
	{
		Dados.modoDeJogo = ModosDeJogo.Normal;
		Dados.mundoAtual = mundo;
		ControleFasesLiberadas.Recarregar();
		PrepararMovimento(-1,1);
		Navegacao.CarregarTelaEstatico(Telas.Menu_Fases);
	}

	public void VoltarDaTelaEscolherFases()
	{
		if (Dados.estatisticas.mundos.Count == 1){
			TelaModoJogo();
		}else{
			TelaEscolherMundo();
		}
	}

	public void AlterarDificuldadeJogoRapido(Slider sliderDif)
	{
		int dif = (int) sliderDif.value;
		if (textoDificuldadeJogoRapido != null)
			textoDificuldadeJogoRapido.text = "" + dif;

		Dados.jogoRapidoDificuldade = dif;
	}


		// Métodos Privados
	void Awake()
	{
		_instancia = this;

		mostradorDePontos.Carregar();

		ControleMusica.MusicaMenu();

		deslocamentoPorTela.x = Screen.width;
		deslocamentoPorTela.y = Screen.height;
		deslocamentoPorTela.z = 0;

		foreach(RectTransform trans in paineisEsquerda)
		{
			float x = Camera.main.ScreenToWorldPoint(new Vector2(
				-deslocamentoPorTela.x + Screen.width / 2, 0)).x;

			trans.position = new Vector3(
				x, trans.position.y, trans.position.z);
		}
		foreach(RectTransform trans in paineisDireita)
		{
			float x = Camera.main.ScreenToWorldPoint(new Vector2(
				deslocamentoPorTela.x + Screen.width / 2, 0)).x;
			
			trans.position = new Vector3(
				x, trans.position.y, trans.position.z);
		}

		Utilidade.CarregarDados();

		Recarregar();
		//TelaMenu();

		//mostrou = false;
		//proximoTempoAd = Time.time + tempoEsperarAd;

		if (enviouAnalyticsAoComecar == false)
		{
			enviouAnalyticsAoComecar = true;
			UnityAnalytics.EnviarPontosMaisTocados();
		}
	}

	static bool enviouAnalyticsAoComecar = false;

	void Update()
	{
		/*
		if (!mostrou && Time.time > proximoTempoAd &&
		    GerenciadorUnityAds.Inicializado())
		{
			mostrou = true;
			//GerenciadorUnityAds.ShowRewardedAd();
		}
		//*/

		/*
		if (mover)
		{
			Mover();
		}

		if (irPara)
		{
			irPara = false;

			alvo = Camera.main.ScreenToWorldPoint(new Vector3(
				tx * deslocamentoPorTela.x + Screen.width / 2,
				ty * deslocamentoPorTela.y + Screen.height / 2,
				0));
			alvo.z = painel.position.z;
			
			painel.position = alvo;
		}
		//*/
	}

	public void VoltarUmaTela()
	{
		PrepararMovimento(xanterior,yanterior, true);
		Navegacao.VoltarUmaTela();
	}

	void PrepararMovimento(int telax, int telay, bool voltar = false)
	{
		/*
		if (voltar == false)
		{
			xanterior = xatual;
			yanterior = yatual;
		}
		*/

		//xatual = telax;
		//yatual = telay;

		/*
		alvo = Camera.main.ScreenToWorldPoint(new Vector3(
			telax * deslocamentoPorTela.x + Screen.width / 2,
			telay * deslocamentoPorTela.y + Screen.height / 2,
			0));
		alvo.z = painel.position.z;

		velocidade = (alvo - painel.position) / tempo;
		velocidade.z = 0;

		mover = true;

		tempoMovimento = Time.time + tempo;
		*/

		//IrPara(telax, telay);

		TocarSom();
	}

	void Mover()
	{
		painel.Translate(velocidade * Time.deltaTime);

		if (Time.time > tempoMovimento)
		{
			painel.position = alvo;
			//mover = false;
		}
	}

	void TocarSom()
	{
		if (podeTocarSom && som && Dados.somLigado)
		{
			Instantiate(som, Vector3.zero, Quaternion.identity);
		}
		else
		{
			podeTocarSom = true;
		}
	}

	// Metodos estáticos
	public static bool Existe()
	{
		return _instancia != null;
	}

	public static void CarregarTela(Telas tela, Navegacao nav)
	{
		podeMover = false;

		nav.CarregarTelaMenuVirandoPagina();

		switch(tela)
		{
		case Telas.EscolherFase: 	
		case Telas.EscolherMundo:
			switch(Dados.modoDeJogo){
			case ModosDeJogo.Normal:
				_instancia.TelaEscolherMundo();
				break;
			case ModosDeJogo.JogoRapido: 	
			case ModosDeJogo.Sobrevivencia:
				_instancia.TelaJogoRapido();
				break;
			default:
				_instancia.TelaMenu();
				break;
			}
			break;
		case Telas.EscolherJogo:
			_instancia.TelaModoJogo(); 
			break;
		default:
			_instancia.TelaMenu();
			break;
		}
	}

	static void IrPara(int x, int y)
	{
		//irPara = true;
		//tx = x;
		//ty = y;
	}

	public static void Recarregar()
	{
		ControleModosDeJogo.Recarregar();
		ControleMundosLiberados.Recarregar();
		ControleFasesLiberadas.Recarregar();
	}

	// REMOVER DEPOIS
	public void LiberarTudo(GameObject botao)
	{
		Dados.estatisticas.mundos.Clear();
		for (int m = 0; m < Dados.totalDeMundos; m++)
		{
			Dados.estatisticas.mundos.Add(new Estatisticas.Mundos());
			Dados.estatisticas.mundos[m].fases.Clear();
			for (int i = 0; i < Dados.fasesPorMundo; i++)
			{
				Dados.estatisticas.mundos[m].fases
					.Add(new Estatisticas.Fases());
			}
		}
		Dados.estatisticas.jogoRapido.liberado = true;
		Dados.estatisticas.sobrevivencia.liberado = true;

		Recarregar();
		if (botao != null)
			botao.SetActive(false);
	}
	
	public void ResetarDados(GameObject botao)
	{
		Dados.estatisticas = new Estatisticas();

		Recarregar();
		if (botao != null)
			botao.SetActive(false);

		PlayerPrefs.DeleteAll();
	}
}
