using UnityEngine;
using System.Collections;

public class SomEMusica : MonoBehaviour {

	public GameObject somAceitar;

	public GameObject botaoSomLigado;
	public GameObject botaoSomDesligado;

	public GameObject botaoMusicaLigado;
	public GameObject botaoMusicaDesligado;

	public void TrocarSom(){
		Dados.somLigado = !Dados.somLigado;
		AjeitarSom();

		if (Dados.somLigado && somAceitar){
			Instantiate(somAceitar, Vector3.zero, Quaternion.identity);
		}
	}

	void AjeitarSom(){
		botaoSomLigado.SetActive(Dados.somLigado);
		botaoSomDesligado.SetActive(!Dados.somLigado);
	}

	public void TrocarMusica(){
		Dados.musicaLigada = !Dados.musicaLigada;
		AjeitarMusica();
	}
	
	void AjeitarMusica(){
		botaoMusicaLigado.SetActive(Dados.musicaLigada);
		botaoMusicaDesligado.SetActive(!Dados.musicaLigada);
	}

	void Awake(){
		AjeitarSom();
		AjeitarMusica();
	}
}
