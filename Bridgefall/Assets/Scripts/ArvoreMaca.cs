using UnityEngine;
using System.Collections;

public class ArvoreMaca : MonoBehaviour 
{
	public enum CorMaca {
		Verde = 0, Amarela = 1, Vermelha = 2, Azul = 3
	}

	public CorMaca corMaca = CorMaca.Verde;

	public GameObject macaCaindo;

	public bool temMaca = false;

	GameObject maca;

	Color [] corImagem = {
		Color.white, Color.yellow, Color.red, Color.cyan
	};

	void Awake()
	{
		maca = transform.GetChild(0).gameObject;
		maca.GetComponent<SpriteRenderer>()
			.color = corImagem[(int) corMaca];

		maca.SetActive(temMaca);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == Dados.tagBola)
		{
			SoltarMaca();
		}
	}

	void SoltarMaca()
	{
		if (temMaca)
		{
			temMaca = false;

			GameObject mc = (GameObject) Instantiate(
				macaCaindo, 
				maca.transform.position,
				Quaternion.identity);

			if (Utilidade.MeiaChance())
			{
				mc.transform.localScale = new Vector3(
					-mc.transform.localScale.x,
					mc.transform.localScale.y,
					mc.transform.localScale.z);
			}

			mc.GetComponent<MacaCaindo>()
				.Criar((int)corMaca, transform.position.y);

			Destroy (maca);
		}
	}
}
