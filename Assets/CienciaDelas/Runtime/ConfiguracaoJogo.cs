using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CienciaDelas
{
    [Serializable]
    public class ArteCarta { public string id; public Sprite imagem; }
    [Serializable]
    public class ArteTecla { public string tecla; public Sprite imagem; }
    [CreateAssetMenu(menuName="Ciência Delas/Configuração do jogo")]
    public sealed class ConfiguracaoJogo : ScriptableObject
    {
        public Regras regras = new Regras();
        public List<CartaDados> cartas = CatalogoPadrao.Cartas();
        public List<AmeacaDados> ameacas = CatalogoPadrao.Ameacas();
        [Header("Visuais opcionais — IDs correspondem ao catálogo")]
        public List<ArteCarta> artes = new List<ArteCarta>();
        public Sprite fundo;
        public AparenciaJogo aparencia;
        public TMP_FontAsset fonte;
        public TMP_SpriteAsset iconesRecursos;
        public List<ArteTecla> iconesTeclado = new List<ArteTecla>();
        public Color fundoCalmo = new Color(.045f,.09f,.14f), fundoCrise = new Color(.22f,.055f,.075f);
        public Color painel = new Color(.09f,.16f,.22f), destaque = new Color(.12f,.43f,.48f);
        [Header("Áudio substituível")]
        public AudioClip menu, partida, crise, vitoria, derrota, ambiente;
        public AudioClip selecionar, cancelar, comprar, confirmar, especial;
        // Tempo entre a execução de cada carta da fila. Editável no Inspector.
        [Min(.05f)] public float intervaloExecucao = 1.0f;
        // Pausa antes de verificar as ameaças, após a última carta da fila.
        [Min(.1f)]  public float intervaloResolucao = 1.8f;
        public Sprite Imagem(string id) => artes.Find(a=>a.id==id)?.imagem;
        public Sprite IconeTecla(string tecla) => iconesTeclado.Find(t => t.tecla == tecla)?.imagem;
    }
}
