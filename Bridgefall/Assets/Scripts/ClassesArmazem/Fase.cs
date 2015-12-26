using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fase {
	
	public int numero = 1;

	public int dif = 0;
	
	public int people = 0;
	public int waves = 0;
	public List<int> w = new List<int>();
	public List<float> s = new List<float>();
	
	public bool sbridge = false;
	public int speople = 0;
	public int swaves = 0;
	public List<int> sw = new List<int>();
	public List<float> ss = new List<float>();
	
	public int boat = 0;
	
	public int wind = 0;
	public float wtime = 0;
	public List<int> v = new List<int>();
	
	public int dir = 0;
	public float dtime = 0;
	public List<int> d = new List<int>();

	public List<Rebatedor> bouncers = new List<Rebatedor>();

	public string ParaString()
	{
		string strw = "";
		foreach(int iiw in w)
		{
			strw += ""+iiw+", ";
		}

		string strs = "";
		foreach(float iis in s)
		{
			strs += ""+iis.ToString("0.00")+", ";
		}

		string strsw = "";
		foreach(int isw in sw)
		{
			strsw += ""+isw+", ";
		}

		string strss = "";
		foreach(int iss in ss)
		{
			strss += ""+iss.ToString("0.00")+", ";
		}

		string strv = "";
		foreach(int iv in v)
		{
			strv += ""+iv+", ";
		}

		string strd = "";
		foreach(int id in d)
		{
			strd += ""+id+", ";
		}

		string strrebs = "\n";
		foreach(Rebatedor r in bouncers)
		{
			strrebs += "  " + r.ParaString() + "\n";
		}

		string saida = 
			"Fase, numero: "+numero + ", dif: "+dif +"\n"+
			"  people: "+people+", waves: "+waves +"\n"+
			"   w: " +strw+"\n"+
			"   s: "+strs+"\n"+
			" sbridge: "+sbridge +"\n"+
			"  speople: "+ speople+", swaves: "+swaves+"\n"+
			"   sw: "+strsw +"\n"+
			"   ss: "+strss+"\n"+
			" boat: "+boat+"\n"+
			" wind: "+wind+", wtime: "+wtime+"\n"+
			"  v: "+strv+"\n"+
			" dir: "+dir+", dtime: "+dtime+"\n"+
			"  d: "+strd+"\n"+
			" rebs: "+strrebs+"\n"+
		    "";
		return saida;
	}
}
