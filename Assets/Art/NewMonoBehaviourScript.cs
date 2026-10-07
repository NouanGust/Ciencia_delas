//using UnityEngine;
//using System.Collections.Generic;
//using UnityEngine.InputSystem;


//public class NewMonoBehaviourScript : MonoBehaviour
//{

//    public List<Carta> hand;
//    public List<string> nomes;
//    public Carta cardPrefab;
//    public Canvas canva;
//    private int indiceAtual = 0;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//        // Compra
//        hand.Add(cardPrefab);

//        // Descrate/Uso
//        //hand.Remove(cardPrefab);





//        Debug.Log(hand);

//        foreach (Carta card in hand)
//        {
//            if (hand.Count < 10)
//            {
//                Instantiate(card, Vector3.zero, Quaternion.identity,transform);

//            }

//        }

        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        CartaSelecionada(indiceAtual);
//        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
//        {
//            indiceAtual++;
//            CartaSelecionada(indiceAtual);
//            hand[indiceAtual].AtualizarVisual();
//        }


//    }


//    public void CartaSelecionada(int indiceAtual)
//    {
//        hand[indiceAtual].taSelecionada = true;

//    }


    
//}
