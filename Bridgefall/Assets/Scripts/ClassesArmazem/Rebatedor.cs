using UnityEngine;
using System.Collections;

public class Rebatedor
{
	public Rebatedores.Tipo tipo;
	public int posicaoGrade = 0;
	public int posicaoLocal = 0;
	public bool destrutivel = false;

	public string ParaString()
	{
		string saida = 
			"Rebatedor: grade: "+posicaoGrade+
				", pos: "+posicaoLocal+", destru: "+destrutivel+
				", tipo: "+tipo;
		return saida;
	}
}

