using UnityEngine;
using System.Collections;

public class DestruirAoAcabarTransicaoAnimacao : MonoBehaviour
{
	// Essa classe apenas destrói o objeto quando ele muda
	// de estado de animação. Também toca um som, se tiver acoplado,
	// tocando totalmente, mesmo depois da destruição do objeto.
	
	Animator animator;
	
	// Use this for initialization
	void Awake()
	{
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (animator.IsInTransition(0))
		{
			Destroy(gameObject);
		}
	}
}
