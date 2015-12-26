using UnityEngine;
using System.Collections;

public class Desvanecer : MonoBehaviour
{
	public float decrementoAlfa = 25;

	SpriteRenderer 	desenhador;
	float 			decrementoAlfaTotal = 0;
	Color			corAlfa;

	public void Awake ()
	{
		desenhador = GetComponent<SpriteRenderer>();

		corAlfa = desenhador.color;

		decrementoAlfaTotal = 
			(decrementoAlfa / 100) * (corAlfa.a);
	}
	
	void Update ()
	{
		corAlfa.a -= decrementoAlfaTotal * Time.deltaTime;
		desenhador.color = corAlfa;
		
		if (desenhador.color.a <= 0)
		{
			Destroy(gameObject);
		}
	}
}

