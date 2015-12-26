using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ControleMensagens : MonoBehaviour 
{
	public GameObject painelMensagemBase;
	public float tempoMostrar = 1.5f;
	public float tempoDesvanecer = 1.5f;
	
	static float proximoTempo = 0;
	static bool desvanecendo = false;
	static bool mostrando = false;
	
	static float tempoMostrarEstatico = 1;
	static float tempoDesvanecerEstatico = 1;
	static float alfaGeral = 1;
	static float alfaPainel = 1;
	static float alfaTexto = 1;
	static float alfaImagem = 1;
	
	static GameObject painelMensagemEstatico;
	static Image imagemPainelBaseEstatico;
	static Text textoPainelMensagemEstatico;
	static Image imagemPainelMensagemEstatico;
	
	static List<string> mensagens = new List<string>();
	static List<Sprite> imagens = new List<Sprite>();


	public Sprite _imagemNula;
	static Sprite imagemNula;

	public Sprite [] _imagensBase;
	static Sprite [] imagensBase;
	
	void Start()
	{
		painelMensagemEstatico = painelMensagemBase;
		
		painelMensagemEstatico.SetActive(true);
		
		imagemPainelBaseEstatico = painelMensagemEstatico
			.GetComponent<Image>();
		alfaPainel = imagemPainelBaseEstatico.color.a;
		
		textoPainelMensagemEstatico = painelMensagemEstatico
			.GetComponentInChildren<Text>();
		alfaTexto = textoPainelMensagemEstatico.color.a;
		
		imagemPainelMensagemEstatico = painelMensagemEstatico.
			transform.GetChild(2)
			.GetComponentInChildren<Image>();
		alfaImagem = imagemPainelMensagemEstatico.color.a;

		imagensBase = _imagensBase;
		imagemNula = _imagemNula;

		imagemPainelMensagemEstatico.color = Color.white;

		tempoMostrarEstatico = tempoMostrar;
		tempoDesvanecerEstatico = tempoDesvanecer;
		
		painelMensagemEstatico.SetActive(false);

		//AdicionarMensagem("Teste", 0);
	}
	
	void Update()
	{
		VerificarProxima();
	}

	public static void AdicionarMensagem(
		string mensagem, Mensagens.Imagens imagem)
	{
		AdicionarMensagem(mensagem, (int)imagem);
	}

	public static void AdicionarMensagem(string mensagem, int imagem)
	{
		if (imagensBase != null && imagem < imagensBase.Length)
		{
			AdicionarMensagem(mensagem, imagensBase[imagem]);
		}
		else
		{
			AdicionarMensagem(mensagem);
		}
	}
	
	public static void AdicionarMensagem(string mensagem)
	{
		AdicionarMensagem(mensagem, imagemNula);
	}

	public static void AdicionarMensagem(string mensagem, Sprite imagem)
	{
		mensagens.Add(mensagem);
		imagens.Add(imagem);
	}
	
	static void VerificarProxima()
	{
		if (Time.realtimeSinceStartup > proximoTempo)
		{
			if (mostrando)
			{
				mostrando = false;
				desvanecendo = true;
				proximoTempo = Time.realtimeSinceStartup + 
					tempoDesvanecerEstatico;
			}
			else if (desvanecendo)
			{
				desvanecendo = false;
				painelMensagemEstatico.SetActive(false);
			}
			else if (mensagens.Count > 0)
			{
				MostrarProxima();
			}
		}
		else if (desvanecendo)
		{
			alfaGeral = (proximoTempo - Time.realtimeSinceStartup) /
				tempoDesvanecerEstatico;
			AlterarAlfa();
		}
	}
	
	static void AlterarAlfa(float a = -1)
	{
		if (a >= 0)
		{
			alfaGeral = a;
		}
		
		if (alfaGeral >= 0)
		{
			imagemPainelBaseEstatico.color = new Color(
				imagemPainelBaseEstatico.color.r,
				imagemPainelBaseEstatico.color.g,
				imagemPainelBaseEstatico.color.b,
				alfaGeral * alfaPainel);
			
			textoPainelMensagemEstatico.color = new Color(
				textoPainelMensagemEstatico.color.r,
				textoPainelMensagemEstatico.color.g,
				textoPainelMensagemEstatico.color.b,
				alfaGeral * alfaTexto);
			
			imagemPainelMensagemEstatico.color = new Color(
				imagemPainelMensagemEstatico.color.r,
				imagemPainelMensagemEstatico.color.g,
				imagemPainelMensagemEstatico.color.b,
				alfaGeral * alfaImagem);
		}
	}
	
	static void MostrarProxima()
	{
		textoPainelMensagemEstatico.text = mensagens[0];
		imagemPainelMensagemEstatico.sprite = imagens[0];
		mensagens.RemoveAt(0);
		imagens.RemoveAt(0);
		
		AlterarAlfa(1);
		painelMensagemEstatico.SetActive(true);
		mostrando = true;
		proximoTempo = Time.realtimeSinceStartup +
			tempoMostrarEstatico;
	}
}
