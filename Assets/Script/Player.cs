using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private Transform tr;
    private Animator an;
    public Transform verificaChao;
    public Transform verificaParede;

    private bool estaAndando;
    private bool estaNoChao;
    private bool estaNaParede;
    private bool estaVivo;
    private bool viradoParaDireita;

    private float axis;
    public float velocidade;
    public float forcaPulo;
    public float raioValidaChao;
    public float raioValidaParede;

    float metadeTelaHorizontal = Screen.width / 2;
    float metadeTelaVertical = Screen.height / 2;
    float esquerda = -1;
    float direita = 1;


    public LayerMask solido;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        an = GetComponent<Animator>();

        estaVivo = true;
        viradoParaDireita = true;
    }


    void Update()
    {
        estaNoChao = Physics2D.OverlapCircle(verificaChao.position, raioValidaChao, solido);
        estaNaParede = Physics2D.OverlapCircle(verificaParede.position, raioValidaParede, solido);



        if (estaVivo)
        {
            processaMovimento();
        }

        Animations();

    }

    void processaMovimento()
    {
        int horizontalMov = isMovendoHorizontal();
        int verticalMov = isMovendoVertical();
        if (horizontalMov > 0) // se maior que 0 tá indo pr adireita, se menor, pra esquerda, se zero, parado
        {
            if (!viradoParaDireita)
                Flip();
            viradoParaDireita = true;
        }
        else if (horizontalMov < 0)
        {
            if (viradoParaDireita)
                Flip();
            viradoParaDireita = false;
        }
        if (verticalMov > 0) //se movendo vertical > 0, tá pulando
        {
            if (Input.GetButtonDown("Jump") && estaNoChao)
            {
                rb.AddForce(tr.up * forcaPulo);
            }
        }
        if (verticalMov == 0 && horizontalMov == 0)
            estaAndando = false;
        else
            estaAndando = true;
    }

    bool isMobile()
    {
        Debug.Log(Application.platform);
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    int isMovendoHorizontal()
    {
        if (isMobile()) // lógica de movimento para mobile
        {
            if (Input.touchCount == 0) // se tocou na tela
                return 0; //se não tá retorna zero (não está movendo)
            if (Input.GetTouch(0).phase == TouchPhase.Stationary) // verifica se o dedo tá parado na tela
            {
                if (Input.GetTouch(0).position.x > metadeTelaHorizontal) // se tá depois da metade da tela retorna 1
                    return 1;
                else
                    return -1; // se tá antes retorna -1
            }
            return 0; // se não cair em nada acima retorna zero (parado)
        }
        else // lógica de movimento para desktop
        {
            int movimento = Input.GetAxis("Horizontal") > 0 ? 1 : Input.GetAxis("Horizontal") < 0 ? -1 : 0;
            if (movimento != 0)
                Debug.Log(movimento);
            return movimento;
        }
    }

    int isMovendoVertical()
    {
        if (isMobile()) // lógica de movimento para mobile
        {
            if (Input.touchCount == 0) // se tocou na tela
                return 0; //se não tá retorna zero (não está movendo)
            if (Input.GetTouch(0).phase == TouchPhase.Stationary) // verifica se o dedo tá parado na tela
            {
                if (Input.GetTouch(0).position.y > metadeTelaVertical) // se tá depois da metade da tela retorna 1
                    return 1;
                else
                    return -1; // se tá antes retorna -1
            }
            return 0; // se não cair em nada acima retorna zero (parado)
        }
        else // lógica de movimend to para desktop
        {
            if (Input.GetButtonDown("Jump"))
                return 1;
            else return 0;
        }
    }

    void FixedUpdate()
    {
        if (estaAndando && !estaNaParede)
        {
            if (viradoParaDireita)
            {
                rb.velocity = new Vector2(velocidade, rb.velocity.y);

            }
            else
            {
                rb.velocity = new Vector2(-velocidade, rb.velocity.y);

            }
        }
    }

    void Flip()
    {
        tr.localScale = new Vector2(-tr.localScale.x, tr.localScale.y);
    }

    void Animations()
    {
        an.SetBool("Correndo", (estaNoChao && estaAndando));
        an.SetBool("Pulando", !estaNoChao);
        an.SetFloat("VelVertical", rb.velocity.y);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(verificaChao.position, raioValidaChao);
        Gizmos.DrawWireSphere(verificaParede.position, raioValidaParede);
    }
}
