using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DiretorPacote : MonoBehaviour
{

    public List<Carta> cartasDisponiveis;
    int cartasPorPacote = 5;
    public Button proxCarta;
    public GameObject cartaPlaceholder;

    int cartaAtual = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        proxCarta.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AbrirPacote()
    {
        MostrarCarta();
        proxCarta.gameObject.SetActive(true);

        


    }

    public void MostrarCarta()
    {
        
        if (cartaAtual < cartasPorPacote)
        {
         


            int indiceAl = Random.Range(0, cartasDisponiveis.Count);
            Carta carta = cartasDisponiveis[indiceAl];


            Carta cartaObj = Instantiate(carta, cartaPlaceholder.transform.position, Quaternion.identity);
            cartaObj.transform.SetParent(cartaPlaceholder.transform);
            cartaObj.transform.localScale = Vector3.one;
            Debug.Log(cartaObj);
            if (carta.especial)
            {
                cartaObj.GetComponentInChildren<ParticleSystem>().Play();  

            }
            cartaAtual++;

        }
        else
        {
            Debug.Log("Acabou as cartas");
        }
    }

}
