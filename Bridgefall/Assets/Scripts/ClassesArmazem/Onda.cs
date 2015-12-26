using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Onda
{
	public int			quantidade = 0;

	bool 				gritou = false;
	List<Passante> 		passantes = new List<Passante>();
	int					atual = 0;

	public Onda(List<Passante> p)
	{
		gritou = false;
		passantes = p;
		quantidade = p.Count;
	}

	public Passante Lancar(){
		//gritou = true;
		if (Acabou ()){
			return null;
		}
		atual++;
		return passantes[atual - 1];
	}

	public bool Acabou(){
		return atual >= quantidade;
	}

	public bool Gritar(){
		bool retorno = gritou;
		gritou = true;
		return !retorno;
	}
}

