using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControleCanhao : MonoBehaviour
{
	// Enum
	public enum CDT {
		Lento, Normal, Rapida, Max
	}

	public enum Potencia {
		Minimo, Fraco, Normal, Forte, Maximo
	}

	// Variáveis públicas
	public Transform pontoDeLancamento;
	public Transform pontoDeFaisca;
	public GameObject faisca;
	public GameObject bola;
	public Transform limitador;
	public float rotacaoExtra = 270;
	public Animator atirador;
	public CDT cadencia = CDT.Normal;
	public Potencia potencia = Potencia.Normal;

	// Variáveis privadas
	float rotacao = 0;
	float limiteEsquerdo = 0;
	float limiteDireito = 0;
	float corretorDoLimitadorAngular = 0.6f;
	float tempoAtirar = 0;
	float cadenciaDeTiro = Dados.CANHAO_CDT_NORMAL;
	AudioSource som;

	// Para nao atirar quando estiver na tela de pausa
	public static bool podeAtirar = true;

	// Métodos públicos
	public void Reiniciar(){
		podeAtirar = true;
		tempoAtirar = 0;
		ReiniciarDirecao();
		AlterarCadenciaDeTiros(cadencia);
		AlterarPotenciaTiros();
	}

	// Métodos para prevenir que atire enquanto está no
	// botão de pausa
	public void CursorEntrouPausa(){
		podeAtirar = false;
	}
	public void CursorSaiuPausa(){
		podeAtirar = true;
	}

	public void AlterarPotenciaTiros()
	{
		switch (potencia){
		case Potencia.Minimo:
			Dados.bolaPotencia = Dados.CANHAO_POTENCIA_MIN;
			break;
		case Potencia.Fraco:
			Dados.bolaPotencia = Dados.CANHAO_POTENCIA_FRACO;
			break;
		case Potencia.Normal:
			Dados.bolaPotencia = Dados.CANHAO_POTENCIA_NORMAL;
			break;
		case Potencia.Forte:
			Dados.bolaPotencia = Dados.CANHAO_POTENCIA_FORTE;
			break;
		case Potencia.Maximo:
			Dados.bolaPotencia = Dados.CANHAO_POTENCIA_MAX;
			break;
		}
	}

	public void AlterarCadenciaDeTiros(CDT cdt)
	{
		switch(cdt){
		case CDT.Lento:
			cadenciaDeTiro = Dados.CANHAO_CDT_LENTO;
			break;
		case CDT.Rapida:
			cadenciaDeTiro = Dados.CANHAO_CDT_RAPIDO;
			break;
		case CDT.Max:
			cadenciaDeTiro = Dados.CANHAO_CDT_MAX;
			break;
		default:
			cadenciaDeTiro = Dados.CANHAO_CDT_NORMAL;
			break;
		}
	}

	// Métodos privados
	void Awake(){
		som = GetComponent<AudioSource>();
		LimitarAngulos();
		ReiniciarTempoTiro();
		Reiniciar();
	}

	void LimitarAngulos(){
		float margem = (limitador.position.x + 3) * (-1);
		
		float distanciaAtePonte = limitador.transform.position.y
			- transform.position.y;
		
		float xEsquerdo = transform.position.x + margem
			- corretorDoLimitadorAngular;
		float xDireito = margem - corretorDoLimitadorAngular
			- transform.position.x;
		
		limiteEsquerdo = Mathf.Atan2(xEsquerdo, distanciaAtePonte)
			* Mathf.Rad2Deg;
		limiteDireito = Mathf.Atan2(xDireito, distanciaAtePonte)
			* Mathf.Rad2Deg;
	}

	// Controle do tiro
	void CriarTiro(float pot = 0){
		GameObject tiro = (GameObject) Instantiate(
			bola,
			pontoDeLancamento.position,
			pontoDeLancamento.rotation);
		
		tiro.GetComponent<ControleBola>().CriarELancar(pot);
	}

	void CriarFaisca()
	{
		GameObject f = (GameObject) Instantiate(
			faisca,
			pontoDeFaisca.position,
			pontoDeFaisca.rotation);

		f.transform.Rotate(new Vector3(0,0,90));

		f.transform.SetParent(pontoDeFaisca, true);
	}

	void Atirar(float pot = 0){
		Controlar();
		if (som && Dados.somLigado){
			som.Play();
		}
		atirador.Play(Dados.atiradorAtirando);
		CriarFaisca();
		CriarTiro(pot);
		Dados.bolasLancadasNestaFase++;
		ControleOndas.SobrevivenciaRebatedor();
	}

	void AtualizarCanhao(){
		Controlar();
		if (Time.time > tempoAtirar){
			// Fazer ver se não tocou no botão de pausa
			atirador.Play(Dados.atiradorCarregado);

			if (Dados.pausado == false)
			{
				if (Input.GetMouseButtonUp(0)){
					if (podeAtirar){
						ReiniciarTempoTiro();
						Atirar();
					}else{
						podeAtirar = true;
						ReiniciarDirecao();
					}
				}
			}
		}
	}

	void ReiniciarDirecao(){
		rotacao = 0;
		transform.rotation = Quaternion.Euler (
			0f, 0f, rotacao);
	}

	void ReiniciarTempoTiro(){
		tempoAtirar = Time.time + 1 / cadenciaDeTiro;
	}

	// Controle do canhão
	void Controlar()
	{
		if (Dados.pausado == false &&
		    Input.GetMouseButton(0) &&
		    podeAtirar)
		{
			Vector3 posicaoToque =
				Camera.main.ScreenToWorldPoint (Input.mousePosition);
			
			Vector3 dif = posicaoToque - transform.position;
			
			dif.Normalize();
			
			float rotZ = Mathf.Atan2 (dif.y, dif.x) * Mathf.Rad2Deg;
			
			rotacao = rotZ + rotacaoExtra;

			GirarCanhao();
		}
	}

	void GirarCanhao()
	{
		while (rotacao >= 360){
			rotacao -= 360;
		}
		while (rotacao < 0){
			rotacao += 360;
		}
		if (rotacao >= 0 && rotacao < 180){
			if (rotacao > limiteEsquerdo){
				rotacao = limiteEsquerdo;
			}
		}else {
			if (rotacao < 360 - limiteDireito){
				rotacao = 360 - limiteDireito;
			}
		}
		transform.rotation = Quaternion.Euler (
			0f, 0f, rotacao);
	}

	//
	void Update(){
		AtualizarCanhao();
	}
}

