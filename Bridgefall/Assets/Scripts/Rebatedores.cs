using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rebatedores : MonoBehaviour
{
	// Enums
	public enum Tipo {
		Fixo = 0, VerticalCimaBaixo = 1, VerticalBaixoCima = 2,
		HorizontalEsqDir = 3, HorizontalDirEsq = 4,
		DiagonalEsqDir = 5, DiagonalDirEsq = 6, Circular = 7,
		CircularHorario = 8, CircularAntihorario = 9,
		DiagonalEsqDirCimaBaixo = 10, DiagonalDirEsqCimaBaixo = 11,
		DiagonalEsqDirBaixoCima = 12, DiagonalDirEsqBaixoCima = 13
	}

	// Variáveis públicas
	public GameObject rebatedorIndestrutivel;
	public GameObject rebatedorDestrutivel;
	public GameObject [] grade;

	public int [] dificuldadesEstaticos = {2,5,8};
	public List<int> posicoesEstaticos;

	// Variáveis privadas
	List<ControleRebatedor> rebatedores = new List<ControleRebatedor>();

	// 
	void Awake()
	{
		rebatedores = new List<ControleRebatedor>();
		//AdicionarEstaticosJogoRapido();
	}

	public void AdicionarEstaticosJogoRapido()
	{
		if (Dados.modoDeJogo != ModosDeJogo.Normal)
		{
			for (int i = 0; i < dificuldadesEstaticos.Length; i++)
			{
				if (Dados.jogoRapidoDificuldade >= 
				    dificuldadesEstaticos[i])
				{
					int tipo = Random.Range(0, 14);
					
					AdicionarRebatedor(
						(Tipo) tipo, posicoesEstaticos[i], 0, false);
				}
			}
		}
	}

	// Métodos públicos
	public void AdicionarTodos(List<Rebatedor> rebs){
		foreach(Rebatedor r in rebs){
			AdicionarRebatedor(r);
		}
	}

	public void AdicionarDestrutivelAleatorio()
	{
		List<int> posicoesLivres = new List<int>();
		for (int i = 0; i < grade.Length; i++)
		{
			if (!posicoesEstaticos.Contains(i))
			{
				posicoesLivres.Add(i);
			}
		}

		//saida += ";  Removidas:";
		foreach(ControleRebatedor r in rebatedores)
		{
			posicoesLivres.Remove(r.posicaoGrade);

		}

		if (posicoesLivres.Count > 0)
		{
			int grade = Random.Range (0, posicoesLivres.Count);

			int tipo = Random.Range(0, 14);

			AdicionarRebatedor((Tipo) tipo, posicoesLivres[grade], 0, true);
		}
		else
		{
			Debug.Log ("Sem espaço para rebatedores");
		}
	}

	public void AdicionarRebatedor(Rebatedor r){
		AdicionarRebatedor(
			r.tipo, r.posicaoGrade, r.posicaoLocal, r.destrutivel);
	}

	public void AdicionarRebatedor(
		Tipo tipo, int posGrade, int posLocal, bool destrutivel)
	{

		if (tipo == Tipo.DiagonalDirEsq){
			if (posLocal == 0){
				tipo = Tipo.DiagonalDirEsqCimaBaixo;
			}else{
				tipo = Tipo.DiagonalDirEsqBaixoCima;
			}
		}
		if (tipo == Tipo.DiagonalEsqDir){
			if (posLocal == 0){
				tipo = Tipo.DiagonalEsqDirCimaBaixo;
			}else{
				tipo = Tipo.DiagonalEsqDirBaixoCima;
			}
		}
		if (tipo == Tipo.Circular){
			if (posLocal == 0){
				tipo = Tipo.CircularHorario;
			}else{
				tipo = Tipo.CircularAntihorario;
			}
		}

		GameObject rebatedor;

		if (destrutivel){
			rebatedor = (GameObject)
				Instantiate(rebatedorDestrutivel);
		}else {
			rebatedor = (GameObject)
				Instantiate(rebatedorIndestrutivel);
		}

		//rebatedor.transform.parent = grade[posGrade];

		rebatedor.transform.position = 
			grade[posGrade].transform.position;

		rebatedor.transform
			.Translate(
				LocalConverterPosVector3(
					ConverterPos(tipo, posLocal)));

		rebatedor.GetComponent<ControleRebatedor>()
			.Criar(tipo, destrutivel, 
			       rebatedor.transform.position, posGrade);

		if (rebatedores == null){
			rebatedores = new List<ControleRebatedor>();
		}

		rebatedores.Add(rebatedor.GetComponent<ControleRebatedor>());
	}

	public void RemoverRebatedores(){
		if (rebatedores != null){
			for (int i = rebatedores.Count - 1; i >= 0; i--)
			{
				GameObject r = rebatedores[i].gameObject;
				Destroy(r);
			}
			rebatedores.Clear();
		}
	}

	// Métodos privados
	int ConverterPos(Tipo tipo, int pos){
		int posicaoFinal = pos;
		if (pos > 5 || pos < 0){
			posicaoFinal = 0;
		}
		switch(tipo){
		case Tipo.VerticalBaixoCima:
			if (pos < 3){
				posicaoFinal = pos + 3;
			}
			break;
		case Tipo.HorizontalEsqDir:
			if (pos > 0){
				posicaoFinal = 3;
			}
			break;
		case Tipo.HorizontalDirEsq:
			if (pos > 0){
				posicaoFinal = 5;
			}else{
				posicaoFinal = 2;
			}
			break;
		case Tipo.DiagonalDirEsqBaixoCima:
			posicaoFinal = 5;
			break;
		case Tipo.DiagonalDirEsqCimaBaixo:
			posicaoFinal = 2;
			break;
		case Tipo.DiagonalEsqDirBaixoCima:
			posicaoFinal = 3;
			break;
		case Tipo.DiagonalEsqDirCimaBaixo:
			posicaoFinal = 0;
			break;
		case Tipo.CircularHorario:
		case Tipo.CircularAntihorario:
			posicaoFinal = 6;
			break;
		}
		return posicaoFinal;
	}

	Vector3 LocalConverterPosVector3(int p){
		float dh = Dados.rebatedorMultiplicadorDistancia * 
			Dados.rebatedorDistanciaHorizontal;
		float dv = Dados.rebatedorMultiplicadorDistancia * 
			Dados.rebatedorDistanciaVertical;

		if (p > 5){
			return new Vector3(dh, -dv * 0.5f,	0);
		}
		int y = -p / 3;
		int x = p % 3;
		return new Vector3(x * dh, y * dv, 0);
	}
}
