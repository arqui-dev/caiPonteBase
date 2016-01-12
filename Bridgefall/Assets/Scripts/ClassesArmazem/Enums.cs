using System;

// Enums gerais
public enum Telas
{
	Menu = 0,
	Jogo = 1,
	Jogo_Normal_Vitoria = 2,
	Jogo_Normal_Derrota = 3,
	Inicial = 4,
	Jogo_Rapido_Vitoria = 5,
	Jogo_Rapido_Derrota = 6,
	Jogo_Sobrevivencia_Fim = 7,
	Menu_Creditos = 8,
	Menu_Ajuda = 9,
	Menu_ModoJogo = 10,
	Menu_Sobrevivencia = 11,
	Menu_Mundos = 12,
	Menu_Fases  = 13,
	Menu_Pontuacao = 14,
	EscolherFase = 96,
	EscolherMundo = 97,
	EscolherJogo = 98,
	Erro = 99
}

public enum ModosDeJogo {
	Normal = 0,
	JogoRapido = 1,
	Sobrevivencia = 2
}

public enum Pontes {
	Cima = 0, Baixo = 1
}

public enum Musicas {
	Vitoria = 0, Derrota = 1, Menu = 2,
	Piano = 3, Bluegrass = 4, Rock = 5, Parque = 6,
	Nenhuma = 99
}

public class Mensagens {
	public enum Realizacoes {
		pontos_4 = 0
	}
	public enum Imagens {
		Maca = 0
	}
}

public enum Conquistas {

}

public enum LeaderBoards {
	JogoRapidoDif1, JogoRapidoDif2, JogoRapidoDif3, JogoRapidoDif4, 
	JogoRapidoDif5, JogoRapidoDif6, JogoRapidoDif7, JogoRapidoDif8, 
	JogoRapidoDif9, JogoRapidoDif10, ModoNormal
}