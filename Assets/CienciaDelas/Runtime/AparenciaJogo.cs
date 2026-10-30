using UnityEngine;

namespace CienciaDelas
{
    [CreateAssetMenu(menuName="Ciência Delas/Aparência")]
    public sealed class AparenciaJogo : ScriptableObject
    {
        [Header("Cenários e cientistas")]
        public Sprite fundoMenu, fundoMesa, marie, jaqueline, retratoMarie, retratoJaqueline;
        [Header("Interface")]
        public Sprite iniciar, opcoes, sair, tutorial, voltar, pausa, som, semSom;
        public Sprite saude, infraestrutura, pesquisa, conhecimento, livro, trofeu, trofeuQuebrado, vitoria, derrota;
        public Sprite botaoNormal, botaoHover, botaoPressionado;
        [Header("Sequências de Slash_shadder — células de 48 x 48")]
        public Texture2D slash, explosao;
        [Min(1)] public int tamanhoCelula=48, quadros=10;
        [Min(0)] public int primeiraColuna=10, linhaDoTopo=0;
        [Range(8,60)] public int quadrosPorSegundo=24;
        [Header("Animação e partículas")]
        [Range(0,20)] public float intensidadeShake=7;
        [Range(.1f,1)] public float duracaoShake=.3f;
        [Range(0,40)] public int quantidadeParticulas=18;
        [Range(.1f,1)] public float duracaoHover=.16f;
        public Color brilhoMarie=new Color(.78f,.56f,1), brilhoJaqueline=new Color(.27f,.94f,.84f);
    }
}
