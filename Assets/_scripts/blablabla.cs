using System.Collections.Generic;
using UnityEngine;

public class blablabla : MonoBehaviour
{
    public List<blabla> baralho = new List<blabla>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Carta obj = new Carta();

        foreach (blabla carta in baralho)
        {
            obj.custo = carta.custo;

            print(obj.custo);
        }
        
    }

    // Update is called once per frame
    void Update()
    {


        
    }
}
