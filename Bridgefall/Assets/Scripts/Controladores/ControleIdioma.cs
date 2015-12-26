using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControleIdioma : MonoBehaviour
{
	static ControleIdioma _instancia = null;

	public static SystemLanguage lingua = SystemLanguage.Unknown;

	static Idiomas idioma = new Idiomas();

	static string nomePrefsLingua = "idioma";

	void Start()
	{
		_instancia = this;

		if (PlayerPrefs.HasKey(nomePrefsLingua))
		{
			AlterarLingua(PlayerPrefs.GetString(nomePrefsLingua));
		}
		CarregarIdioma(lingua);

		SalvarIdioma();
	}

	static void CarregarIdioma(SystemLanguage novoIdioma)
	{
		idioma.AtualizarIdioma(novoIdioma);

		TextAsset texto = Resources.Load<TextAsset>(
			idioma.ArquivoIdioma(novoIdioma));

		idioma.CarregarIdioma(texto);
	}

	public static void BotaoAlterarIdioma(Text txt)
	{
		string linguaTexto = "en";
		SystemLanguage novaLingua = SystemLanguage.English;

		if (lingua == SystemLanguage.English)
		{
			novaLingua = SystemLanguage.Portuguese;
			linguaTexto = "pt";
		}

		txt.text = linguaTexto;
		AlterarIdioma(novaLingua);
	}

	static void AlterarLingua(string id)
	{
		lingua = SystemLanguage.English;

		if (id == SystemLanguage.Portuguese.ToString())
		{
			lingua = SystemLanguage.Portuguese;
		}

		AlterarIdioma(lingua);
	}

	static void SalvarIdioma()
	{
		PlayerPrefs.SetString(nomePrefsLingua, lingua.ToString());
	}

	public static string PegarTexto(Idiomas.Texto texto)
	{
		return idioma.PegarTexto(texto);
	}

	public static void AlterarIdioma(
		SystemLanguage novoIdioma = SystemLanguage.Unknown)
	{
		lingua = novoIdioma;
		CarregarIdioma(lingua);
		SalvarIdioma();
	}

	//*
	public static SystemLanguage PegarIdiomaDoSistema()
	{
		if (_instancia != null)
			return _instancia.PegarIdioma();

		return SystemLanguage.English;
	}
	//*/

	public SystemLanguage PegarIdioma()
	{
		return Application.systemLanguage;
	}
}

