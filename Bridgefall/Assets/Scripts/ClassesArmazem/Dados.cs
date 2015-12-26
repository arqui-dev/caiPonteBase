using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dados : MonoBehaviour
{
	// Maças
	public static long macasVerdeTotal = 3;
	public static long macasVerdesUltimaTela = 0;
	public static long macasDivisorPontos = 1000;

	// Apenas para não dar pala no carregamento
	public static string nomeVersaoSalvarAtual = "VersãoAtual";
	public static int versaoSalvarAtual = 3;

	// Modos de jogo
	public static ModosDeJogo modoDeJogo = ModosDeJogo.Normal;
	public static bool carregado = false;
	public static string nomeChaveSalvar = "dados";

	public static int dificuldadeMaximaCampanha = 20;
	public static int dificuldadeMaximaReal = 20;

	// Jogo Rápido
	public static int jogoRapidoDificuldade = 1;
	public static int jogoRapidoDificuldadeMaxima = 10;

	// Sobrevivência
	public static int sobrevivenciaOndaAtual = 0;
	public static int sobrevivenciaPontosTotais = 0;
	public static int sobrevivenciaPontosPassantes = 0;
	public static int sobrevivenciaPontosPerfeito = 0;
	public static int sobrevivenciaPontosBonus = 0;

	// Pontuação
	public static int pontosBase = 4;

	public static int pontosMultiplicadorBaseRebatidas = 1;
	public static int pontosMultiplicadorBaseRebatidasTotais = 2;
	public static int pontosMultiplicadorBasePerfeitoPorBola = 10;
	public static int pontosMultiplicadorRebatedorDestrutivel = 11;
	public static int pontosMultiplicadorDificuldadeJogoRapido =  2;
	public static int pontosDivisorOnus = 20;
	public static int [] pontosMultiplicadorMaca = {
		12, 21, 36, 64
	};

	public static int pontosUltimaFase = 0;
	public static int pontosUltimaFasePassantes = 0;
	public static int pontosUltimaFaseRebatidas = 0;
	public static int pontosUltimaFasePerfeita = 0;
	public static int pontosUltimaFaseDificuldade = 0;
	public static int pontosUltimaFaseVelocidade = 0;
	public static int pontosUltimaFaseBonus = 0;
	public static int pontosUltimaFaseOnus = 0;

	public static int rebatedoresDestruidosNestaFase = 0;

	// Fase e mundo
	public static bool ponteBaixo = false;
	public static int faseAtual = 0;
	public static int mundoAtual = 0;
	//public static int faseAnterior = 0;
	//public static int mundoAnterior = 0;
	public static int fasesPorMundo = 9;
	public static int totalDeMundos = 3;
	public static int mundosCompletosParaJogoRapido = 0;
	public static int mundosCompletosParaSobrevivencia = 1;

	public static int dificuldadeFaseAtual = 0;

	public static float margemEsquerda = -10;

	// Estatisticas salvas das coisas
	public static Estatisticas estatisticas = new Estatisticas();

	// Tela de pausa
	public static bool pausado = false;
	public static float fluxoTemporalPausado = 0;
	public static float fluxoTemporalNormal = 1;

	// Som e Música
	public static bool somLigado = true;
	public static bool musicaLigada = true;

	// Passantes
	public static float passantesTempoEntre = 1;

	// Canhão
	public static float CANHAO_CDT_LENTO 	= 1;
	public static float CANHAO_CDT_NORMAL 	= 1.5f;
	public static float CANHAO_CDT_RAPIDO 	= 2;
	public static float CANHAO_CDT_MAX 		= 2.75f;
	public static float CANHAO_POTENCIA_MIN		= 15;
	public static float CANHAO_POTENCIA_FRACO	= 18;
	public static float CANHAO_POTENCIA_NORMAL	= 21;
	public static float CANHAO_POTENCIA_FORTE	= 24;
	public static float CANHAO_POTENCIA_MAX		= 27;
	public static float bolaPotencia = CANHAO_POTENCIA_NORMAL;

	public static float bolaLimiarVelocidadeDestruir	= 1;

	// Bola
	public static Vector2 bolaLimitesPosicao = new Vector2(18,13);
	public static float bolaLimiteTempo = 15;
	public static int bolaRebatidasTotaisFase = 0;
	public static int bolasLancadasNestaFase = 0;
	public static int bolasMaximasPorVez = 10;

	// Vento
	public static float VENTO_VELOCIDADE_LENTO 	= 75;
	public static float VENTO_VELOCIDADE_NORMAL = 250;
	public static float VENTO_VELOCIDADE_RAPIDO = 500;
	public static float VENTO_VELOCIDADE_MAX 	= 750;

	public static Vector2 VENTO_DIRECAO_NORTE		
		= new Vector2(0,1).normalized;
	public static Vector2 VENTO_DIRECAO_NORDESTE	
		= new Vector2(1,1).normalized;
	public static Vector2 VENTO_DIRECAO_LESTE		
		= new Vector2(1,0).normalized;
	public static Vector2 VENTO_DIRECAO_SUDESTE		
		= new Vector2(1,-1).normalized;
	public static Vector2 VENTO_DIRECAO_SUL			
		= new Vector2(0,-1).normalized;
	public static Vector2 VENTO_DIRECAO_SUDOESTE	
		= new Vector2(-1,-1).normalized;
	public static Vector2 VENTO_DIRECAO_OESTE		
		= new Vector2(-1,0).normalized;
	public static Vector2 VENTO_DIRECAO_NOROESTE	
		= new Vector2(-1,1).normalized;

	public static bool vento = false;
	public static float ventoVelocidade = VENTO_VELOCIDADE_NORMAL;
	public static Vector2 ventoDirecao = VENTO_DIRECAO_NORTE;

	// Barco
	public static bool barcoMove = false;
	public static float barcoVelocidade = BARCO_VELOCIDADE_NORMAL;
	public static float BARCO_VELOCIDADE_LENTO = 1;
	public static float BARCO_VELOCIDADE_NORMAL = 3;
	public static float BARCO_VELOCIDADE_RAPIDO = 5;

	// Rebatedores
	public static float rebatedorDistanciaHorizontal = 2f;
	public static float rebatedorDistanciaVertical = 1.5f;
	public static float rebatedorHorizontalVelocidade = 4f;
	public static float rebatedorVerticalVelocidade = 1.5f;
	public static float rebatedorAngularVelocidade = 2;
	public static float rebatedorMultiplicadorDistancia = 1;

	// Nomes (strings) de várias coisas
	public static string[] nomeTelas = {
		"Menu",
		"Jogo",
		"Jogo_Campanha_Vitória",
		"Jogo_Campanha_Derrota",
		"Inicial",
		"Jogo_Rapido_Vitoria",
		"Jogo_Rapido_Derrota",
		"Jogo_Sobrevivencia_Fim"
	};

	// Tags
	public static string tagPassador = "Passador";
	public static string tagBola = "Bola";
	public static string tagRebatedorDestrutivel = "RebatedorDestrutivel";
	public static string tagPainelPrincipal = "PainelPrincipal";
	public static string tagArvoreMaca = "ArvoreMaca";
	public static string tagMargemEsquerda = "MargemEsquerda";

	// Animações
	public static string atiradorCarregado = "Carregado";
	public static string atiradorAtirando = "Atirar";

	// Arquivos
	//public static string nomeDiretorio = "Assets/Mundos/";
	public static string nomeArquivoMundo = "world";
	//public static string nomeExtensao = ".blv";
	public static string nomeArquivoDificuldade = "quick";

	// Textos da tela de pausa
	public static string [] textosTelaPausa = {
		"Campanha", "Mundo", "Fase",
		"Jogo Rápido","Dificuldade",
		"Sobrevivência","Onda"
	};

}
