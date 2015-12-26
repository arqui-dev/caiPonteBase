using UnityEngine;
using System.Collections;

public class PontoTocado
{
	public Vector2 ponto = Vector2.zero;
	public Vector2 pontoPorcento = Vector2.zero;
	public Vector2 pontoArredondado = Vector2.zero;
	public Vector2 resolucao = Vector2.zero;
	public string tela = "";
	public float tempo = 0;

	public PontoTocado(Vector2 ponto)
	{
		this.ponto = ponto;
		this.tela = Application.loadedLevelName;

		this.resolucao.x = (float) Screen.width;
		this.resolucao.y = (float) Screen.height;

		this.pontoPorcento.x = ponto.x / this.resolucao.x;
		this.pontoPorcento.y = ponto.y / this.resolucao.y;

		// separa de 5 em 5 %
		this.pontoArredondado.x = 
			Mathf.Round(this.pontoPorcento.x * 20) * 5;
		this.pontoArredondado.y =
			Mathf.Round(this.pontoPorcento.y * 20) * 5;

		this.pontoPorcento.x = Mathf.Round(this.pontoPorcento.x * 100);
		this.pontoPorcento.y = Mathf.Round(this.pontoPorcento.y * 100);

		this.tempo = Time.realtimeSinceStartup;
	}
}

