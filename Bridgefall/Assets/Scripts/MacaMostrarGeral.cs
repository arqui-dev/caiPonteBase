using UnityEngine;
using System.Collections;

public class MacaMostrarGeral : MonoBehaviour
{
	GameObject imgMaca;
	GameObject btMaca;

	long ultimoMacas = -1;

	void Awake()
	{
		imgMaca = transform.GetChild(0).gameObject;
		btMaca	= transform.GetChild(1).gameObject;

		Verificar(true);
	}
	
	void Update ()
	{
		// ANALYTICS posicao toque
		if (Input.GetMouseButtonDown(0))
		{
			UnityAnalytics.AdicionarPontoTocado();
		}
		// FIM ANALYTICS

		Verificar();
	}

	void Verificar(bool forcar = false)
	{
		if (!forcar && ultimoMacas == Dados.macasVerdeTotal)
		{
			return;
		}

		ultimoMacas = Dados.macasVerdeTotal;

		bool m = ultimoMacas > 0;

		imgMaca.SetActive(m);
		btMaca.SetActive(!m);
	}
}

