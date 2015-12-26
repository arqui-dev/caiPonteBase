using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BrilharImagemUI : MonoBehaviour
{
	Image imagem;

	float duracao = 0.5f;
	float tempo = 0;
	bool podeBrilhar = false;
	bool aumentadoBrilho = true;

	Color corBrilho = Color.white;
	Color corNormal = Color.black;
	Color corFinal = Color.black;

	public void Brilhar(float dur, Color brilho)
	{
		duracao = dur;
		podeBrilhar = true;
		aumentadoBrilho = true;
		corBrilho = brilho;
		tempo = 0;
		imagem.enabled = true;
	}

	public void Brilhar(float dur)
	{
		Brilhar (dur, Color.white);
	}

	public void Reiniciar()
	{
		imagem.enabled = true;
		imagem.color = corNormal;
		aumentadoBrilho = true;
		podeBrilhar = false;
		tempo = 0;
	}

	void Awake()
	{
		imagem = GetComponent<Image>();
		corNormal = imagem.color;
		corFinal = corNormal;
		corFinal.a = 0;

		Reiniciar();
	}

	void Update ()
	{
		if (podeBrilhar)
		{
			if (aumentadoBrilho)
			{
				imagem.color = Color.Lerp(corNormal, corBrilho, tempo);
				if (tempo < 1)
				{
					tempo += Time.deltaTime / duracao;
				}
				else
				{
					tempo = 0;
					aumentadoBrilho = false;
				}
			}
			else
			{
				imagem.color = Color.Lerp(corBrilho, corFinal, tempo);
				if (tempo < 1)
				{
					tempo += Time.deltaTime * 2 / duracao;
				}
				else
				{
					tempo = 0;
					podeBrilhar = false;
					imagem.enabled = false;
				}
			}
		}
	}
}

