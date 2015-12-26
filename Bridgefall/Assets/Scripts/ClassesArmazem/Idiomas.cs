using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Idiomas
{
	public enum Texto {
		TituloTelaCreditos, TituloTelaJogoRapido, TituloTelaModoDeJogo,
		TituloTelaMundos, TituloTelaFases, TituloTelaAjuda, 
		TituloTelaVitoria, TituloTelaDerrota, TituloTelaPausa,
		TituloTelaPontuacao, SubTituloTelaPontuacao,
		BotaoJogoNormal, BotaoJogoRapido,
		DescricaoAjudaLinha1, DescricaoAjudaLinha2, DescricaoAjudaLinha3,
		DescricaoCreditos1, DescricaoCreditos2, DescricaoCreditos3, 
		DescricaoCreditos4, DescricaoCreditos5, 
		MostrarPontos, MostrarBonus, MostrarPerfect, MostrarOnus,
		TextoMundo, TextoFase, TextoDificuldade, TextoPontos,
		TextoModoNormal, TextoModoJogoRapido,
		MensagemSemMacas
	}

	SystemLanguage idioma = SystemLanguage.English;

	Dictionary <Texto, string> mensagens = 
		new Dictionary<Texto, string>();

	/*
	public Idiomas(
		SystemLanguage novoIdioma = SystemLanguage.English)
	{
		CarregarIdioma(novoIdioma);
	}
	//*/

	// funçoes publicas
	public void CarregarIdioma(TextAsset texto)
	{
		string entrada = "Nada";

		if (texto != null)
		{
			entrada = texto.text;
		}

		string [] divisores = {"\n","\r\n","\n\r"};
		string [] linhas = entrada.Split(
			divisores, System.StringSplitOptions.None);

		//Debug.Log("Linhas "+linhas.Length);

		mensagens.Clear();
		
		string [] divisoresTexto = {":::"};
		for (int i = 0; i < linhas.Length; i++)
		{
			string [] textos = linhas[i].Split(
				divisoresTexto, System.StringSplitOptions.None);

			mensagens.Add((Texto) i, textos[1]);

			//Debug.Log ("Texto carregado["+i+"]: "+textos[1]);
		}
	}

	public string PegarTexto(Texto texto)
	{
		if (mensagens.ContainsKey(texto))
		{
			return mensagens[texto];
		}
		return "";
	}

	// funçoes privadas e auxiliares
	public void AtualizarIdioma(
		SystemLanguage novoIdioma = SystemLanguage.Unknown)
	{
		if (novoIdioma != SystemLanguage.Unknown)
		{
			idioma = novoIdioma;
		}
		else
		{
			idioma = ControleIdioma.PegarIdiomaDoSistema();
		}
	}

	string StringIdioma()
	{
		switch(idioma)
		{
		case SystemLanguage.Portuguese: return "pt-br";
		}
		return "en-us";
	}

	public string ArquivoIdioma(
		SystemLanguage novoIdioma = SystemLanguage.Unknown)
	{
		AtualizarIdioma(novoIdioma);
		
		return arquivoIdioma + StringIdioma();
	}
	string arquivoIdioma = "idioma_";


}

