using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CienciaDelas.Editor
{
    public static class ControlesEditor
    {
        [MenuItem("Ciência Delas/Preparar ícones e controles dos jogadores")]
        public static void Aplicar()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) throw new InvalidOperationException("Saia do Play antes de editar.");
            var ui = UnityEngine.Object.FindFirstObjectByType<InterfaceDiretor>();
            if (ui == null) throw new InvalidOperationException("Abra a cena CienciaDelas.");
            var config = ui.GetComponent<InicializadorJogo>().configuracao;
            Undo.RegisterFullObjectHierarchyUndo(ui.gameObject, "Preparar controles dos jogadores");
            Undo.RecordObject(config, "Associar ícones das teclas");
            foreach (string tecla in new[] { "A", "D", "Z", "LEFT ARROW", "RIGHT ARROW", "ENTER", "Q", "E", "TAB", "ESC" })
            {
                string caminho = "Assets/Art/UI/KEYBOARD KEY (16X16)/" + tecla + ".png";
                var importer = AssetImporter.GetAtPath(caminho) as TextureImporter;
                if (importer == null) throw new InvalidOperationException("Ícone não encontrado: " + caminho);
                if (importer.filterMode != FilterMode.Point || importer.textureCompression != TextureImporterCompression.Uncompressed)
                {
                    importer.filterMode = FilterMode.Point;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.SaveAndReimport();
                }
                var sprite = AssetDatabase.LoadAllAssetsAtPath(caminho).OfType<Sprite>().OrderBy(s => s.rect.x).FirstOrDefault();
                if (sprite == null) throw new InvalidOperationException("Importe o ícone como Sprite: " + caminho);
                var arte = config.iconesTeclado.Find(t => t.tecla == tecla);
                if (arte == null) { arte = new ArteTecla { tecla = tecla }; config.iconesTeclado.Add(arte); }
                arte.imagem = sprite;
            }
            var instrucoes = ui.GetComponentsInChildren<ElementoInterface>(true).FirstOrDefault(e => e.Chave == "Como jogar/Instrucoes");
            if (instrucoes != null) instrucoes.atualizarTexto = true;
            try { ui.ConstruirCenaEditavel(config); }
            finally { if (instrucoes != null) instrucoes.atualizarTexto = false; }
            var elementos = ui.GetComponentsInChildren<ElementoInterface>(true).ToDictionary(e => e.Chave);
            void Texto(string chave, float x, float y, float w, float h, float fonte)
            {
                var t = elementos[chave].GetComponent<TextMeshProUGUI>(); var r = t.rectTransform;
                r.anchorMin = r.anchorMax = new Vector2(0, 1); r.pivot = new Vector2(.5f, .5f);
                r.anchoredPosition = new Vector2(x + w / 2, -y - h / 2); r.sizeDelta = new Vector2(w, h);
                t.fontSize = fonte; EditorUtility.SetDirty(t); EditorUtility.SetDirty(r);
            }
            Texto("Mesa/Fila de cartas", 240, 505, 1120, 60, 24);
            Texto("Mesa/Orientacao da rodada", 240, 575, 1120, 28, 18);
            Texto("Mesa/Comando selecionar J2", 1189, 624, 336, 30, 20);
            foreach (var e in elementos.Values.Where(e => e.Chave.StartsWith("Mesa/Tecla ")))
            {
                var r = (RectTransform)e.transform;
                var sprite = e.GetComponent<UnityEngine.UI.Image>().sprite;
                float x = r.anchoredPosition.x - r.sizeDelta.x / 2;
                if (e.Chave == "Mesa/Tecla Pausa" || e.Chave == "Mesa/Tecla Caderno") x = 1405;
                float largura = sprite != null ? 32 * sprite.rect.width / sprite.rect.height : 32;
                r.sizeDelta = new Vector2(largura, 32);
                r.anchoredPosition = new Vector2(x + largura / 2, r.anchoredPosition.y);
                EditorUtility.SetDirty(r);
            }
            var tutorial = elementos["Como jogar/Instrucoes"].GetComponent<TextMeshProUGUI>();
            tutorial.enableAutoSizing = true; tutorial.fontSizeMin = 22; tutorial.fontSizeMax = 26;
            EditorUtility.SetDirty(tutorial); EditorUtility.SetDirty(config); EditorUtility.SetDirty(ui);
            EditorSceneManager.MarkSceneDirty(ui.gameObject.scene);
            AssetDatabase.SaveAssets();
        }
    }
}
