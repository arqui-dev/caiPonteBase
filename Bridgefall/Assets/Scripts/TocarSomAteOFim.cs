using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class TocarSomAteOFim : MonoBehaviour {

	//private static TocarSomAteOFim _instancia;
	AudioSource som = null;

	void Awake(){
		/*
		if (!_instancia){
			_instancia = this;
		}else{
			Destroy(this.gameObject);
		}
		*/

		DontDestroyOnLoad(this.gameObject);

		som = GetComponent<AudioSource>();
		som.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (som == null ||
		    Dados.somLigado == false ||
		    som.enabled == false ||
		    som.isPlaying == false)
		{
			Destroy(gameObject);
		}
	}
}
