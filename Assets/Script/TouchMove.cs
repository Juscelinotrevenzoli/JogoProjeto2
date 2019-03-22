using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMove : MonoBehaviour
{

    int numeroToques;
    float direita = 1;
    float esquerda = -1;
    float metadeTela = Screen.width / 2;

    float maxTimeWait = 0.1f;

    float contadorTempo;
    float newTime;
    int contadorToque;
    int velocidade;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Mover()
    {
        //Faz o personagem andar
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        if (Input.touchCount == 1)
        {
            Touch toque = Input.touches[0];

            if (toque.phase == TouchPhase.Stationary)
            {

                if (toque.position.x > metadeTela)
                {
                    rigidbody.velocity = new Vector2(direita * velocidade, rigidbody.velocity.y);

                }
                if (toque.position.x < metadeTela)
                {
                    rigidbody.velocity = new Vector2(esquerda * velocidade, rigidbody.velocity.y);

                }

            }

        }
        else
        {
            GetComponent<Animator>().SetBool("Run", false);
        }



    }
}
