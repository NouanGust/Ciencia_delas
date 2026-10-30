using System.Collections.Generic;
using UnityEngine;

public class blablabla : MonoBehaviour
{
    public Carta card_prefab;
    public List<blabla> baralho_1 = new List<blabla>();

    List<Carta> mao_jogador_1 = new List<Carta>();

    private void Start()
    {
        if (card_prefab == null || MaoJogador1.Instance == null) return;

        for (int i = 0; i < 4 && baralho_1.Count > 0; i++)
        {
            Carta card_inst = Instantiate(card_prefab);
            int carta_index = Random.Range(0, baralho_1.Count);
            blabla carta_lista = baralho_1[carta_index];
            Debug.Log($"[Logic] {baralho_1[carta_index]}");
            Debug.Log($"[Logic] {baralho_1[carta_index].nome}");

            card_inst.custo = carta_lista.custo;
            card_inst.nome = carta_lista.nome;
            card_inst.dano = carta_lista.dano;
            card_inst.img = carta_lista.img;
            Debug.Log($"[Logic] {card_prefab.nome}");



            MaoJogador1.Instance.cartas.Add(card_inst);
            mao_jogador_1.Add(card_inst);
            baralho_1.RemoveAt(carta_index);
        }
        MaoJogador1.Instance.MontarMao();



    }

    // Lista baralho player 1 => todas as cartas + especial Currie
    // Lista baralho player 2 => Todas as cartas + especial Gois

    // Lista mão player 1 => 5 cartas aleatórias do baralho 1
    // Lista mão player 2 => 5 cartas aleatórias do baralho 2


    // Função para iniciar as mãos
    // foreach em uma lista baralho 1, pegar 5 cartas aleatórias e colocar na lista da mão do player 1
    // foreach em uma lista baralho 1, pegar 5 cartas aleatórias e colocar na lista da mão do player 1

    // Função para instanciar as cartas
    // foreach na mão do jogador 1, criar um objeto de Carta(script) e instanciar o objeto com as informações do ScriptableObject
    // foreach na mão do jogador 2, criar um objeto de Carta(script) e instanciar o objeto com as informações do ScriptableObject
    //
}
