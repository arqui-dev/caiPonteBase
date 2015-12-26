using UnityEngine;
using System.Collections;

public class ControleJogo : MonoBehaviour
{
	public ControleFase controleFase;

	Mundo mundo = null;
	Fase fase;

	// Métodos públicos
	public void Reiniciar()
	{
		if (Dados.modoDeJogo == ModosDeJogo.JogoRapido)
		{
			ModoJogoRapido();
		}
		else if (Dados.modoDeJogo == ModosDeJogo.Sobrevivencia)
		{
			ModoSobrevivencia();
		}
		else
		{
			ModoNormal();
		} 
	}

	// Métodos privados
	void Awake(){
		Reiniciar();
	}

	// Carregamento do mundo
	void CarregarMundo(int m)
	{
		mundo = CarregarFaseDeArquivo.CarregarMundo(m);
		mundo.numero = m + 1;
	}

	void CarregarMundo(string arquivo){
		mundo = CarregarFaseDeArquivo.CarregarMundo(arquivo,0);
		mundo.numero = -1;
	}

	// Carregamento da fase atual
	void CarregarFase(int m, int f)
	{
		//if (mundo == null){
		CarregarMundo(m);
		//}
		fase = mundo.fases[f];
		fase.numero = f + 1;

		Dados.dificuldadeFaseAtual = fase.dif;
	}

	// Cria a fase realmente, instanciando os objetos e afins
	void CriarFase(int m, int f)
	{
		CarregarFase(m, f);

		controleFase.Criar(fase);
	}

	//
	void ModoNormal()
	{
		CriarFase(Dados.mundoAtual, Dados.faseAtual);
	}

	void ModoJogoRapido()
	{
		controleFase.CriarJogoRapido(Dados.jogoRapidoDificuldade);
	}

	void ModoSobrevivencia()
	{
		controleFase.CriarSobrevivencia();
	}
}

