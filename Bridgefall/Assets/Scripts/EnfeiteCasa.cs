using UnityEngine;
using System.Collections;

public class EnfeiteCasa : MonoBehaviour 
{
	public Sprite [] enfeites;

	GameObject [] filhos;

	void Awake()
	{
		filhos = new GameObject[2];
		filhos[0] = transform.GetChild(0).gameObject;
		filhos[1] = transform.GetChild(1).gameObject;

		int tam = enfeites.Length;

		int [] img = new int[2];
		img[0] = Random.Range(0, tam);
		img[1] = Random.Range(0, tam);
		if (img[0] == img[1])
		{
			img[1] = (img[1] + 1) % tam;
		}

		int totalEnfeites = Random.Range(0,6);
		switch(totalEnfeites){
		case 5:
			filhos[0].GetComponent<SpriteRenderer>()
				.sprite = enfeites[img[0]];
			filhos[1].GetComponent<SpriteRenderer>()
				.sprite = enfeites[img[1]];
			break;
		case 4:
		case 3:
		case 2:
			int f = Random.Range(0,2);
			filhos[f].GetComponent<SpriteRenderer>()
				.sprite = enfeites[img[f]];
			break;
		default:
			filhos[0].GetComponent<SpriteRenderer>()
				.sprite = null;
			filhos[1].GetComponent<SpriteRenderer>()
				.sprite = null;
			break;
		}
	}
}
