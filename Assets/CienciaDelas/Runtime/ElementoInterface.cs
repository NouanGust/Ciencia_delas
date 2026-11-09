using UnityEngine;
using UnityEngine.Events;

namespace CienciaDelas
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Ciência Delas/Elemento da interface")]
    public sealed class ElementoInterface : MonoBehaviour
    {
        [SerializeField, HideInInspector] private string chave;
        public string Chave => chave;
        [Tooltip("Oculta este elemento durante a partida, sem excluir sua ligação com o jogo.")]
        public bool ocultar;
        [Tooltip("Atualiza o conteúdo com os dados da partida. Desative para usar um texto fixo do componente TMP.")]
        public bool atualizarTexto;
        [Tooltip("Atualiza a imagem com a carta ou cientista atual. Desative para usar uma imagem fixa.")]
        public bool atualizarImagem;
        [Tooltip("Atualiza a cor para indicar requisitos, ganhos e perdas.")]
        public bool atualizarCor;
        [System.NonSerialized] public UnityAction clique;

        public void Configurar(string identificador) { chave = identificador; }
    }
}
