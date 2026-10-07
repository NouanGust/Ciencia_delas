using UnityEngine;

public class Ameaca : MonoBehaviour
{
    public int vida;
    public int dano = 10;



    public void TomarDano(int valor)
    {
        if (valor >= vida)
        {
            vida -= valor;
            Debug.Log($"Tomou {valor} de dano/ Vida restante{vida}");

        }else
        {
            Morrer();
        }

    }

    public void Morrer()
    {
        Destroy(gameObject);
    }

    public int Atacar()
    {
        return dano;
    }

}
