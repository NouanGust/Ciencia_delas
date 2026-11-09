using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

namespace CienciaDelas.Editor
{
    public static class LegibilidadeEditor
    {
        const string Pasta = "Assets/CienciaDelas/Visual";

        [MenuItem("Ciência Delas/Aplicar fonte e ícones legíveis")]
        public static void Aplicar()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) throw new InvalidOperationException("Saia do Play antes de editar.");
            var ui = UnityEngine.Object.FindFirstObjectByType<InterfaceDiretor>();
            if (ui == null) throw new InvalidOperationException("Abra a cena CienciaDelas.");
            var config = ui.GetComponent<InicializadorJogo>().configuracao;
            Undo.RegisterFullObjectHierarchyUndo(ui.gameObject, "Melhorar legibilidade");
            Undo.RecordObject(config, "Alterar fonte e ícones");
            Directory.CreateDirectory(Pasta); AssetDatabase.Refresh();
            config.fonte = CriarFonte();
            config.iconesRecursos = CriarIcones(config.aparencia);
            ui.ConstruirCenaEditavel(config);
            foreach (var tela in ui.Telas)
            {
                if (tela.raiz.Find("Contraste do fundo") != null) continue;
                var go = new GameObject("Contraste do fundo", typeof(RectTransform), typeof(Image));
                go.transform.SetParent(tela.raiz, false); go.transform.SetSiblingIndex(2);
                var r = (RectTransform)go.transform; r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one;
                r.offsetMin = r.offsetMax = Vector2.zero;
                var imagem = go.GetComponent<Image>(); imagem.color = new Color(.015f, .025f, .055f, .35f); imagem.raycastTarget = false;
            }
            foreach (var t in ui.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                t.font = config.fonte; t.fontSharedMaterial = config.fonte.material;
                t.spriteAsset = config.iconesRecursos;
                t.fontStyle = FontStyles.Normal; t.enableAutoSizing = false;
                t.lineSpacing = 0; t.characterSpacing = 0;
                t.overflowMode = TextOverflowModes.Truncate;
                var e = t.GetComponent<ElementoInterface>();
                if (e == null) { t.fontSize = 28; continue; }
                string k = e.Chave;
                t.fontSize = 26;
                if (k.EndsWith("/Titulo")) { t.fontSize = k == "Menu/Titulo" ? 72 : k == "Mesa/Titulo" ? 40 : 48; t.fontStyle = FontStyles.Bold; }
                else if (k.StartsWith("Mesa/Legenda J")) t.fontSize = 20;
                else if (k.Contains("/Tipo de ")) { t.fontSize = 20; t.fontStyle = FontStyles.Bold; }
                else if (k.Contains("/Requisito ")) t.fontSize = 28;
                else if (k.StartsWith("Mesa/Valor ")) { t.fontSize = 36; t.fontStyle = FontStyles.Bold; }
                else if (k.StartsWith("Mesa/Baralho")) t.fontSize = 24;
                else if (k.EndsWith("/Situacao") || k.EndsWith("/Indicador de alvo")) t.fontSize = 22;
                else if (k.StartsWith("Menu/Nome")) { t.fontSize = 36; t.fontStyle = FontStyles.Bold; }
                else if (k.Contains("/Nome") || k.EndsWith("Titulo da inspecao")) { t.fontSize = 28; t.fontStyle = FontStyles.Bold; }
                else if (k == "Como jogar/Instrucoes") t.fontSize = 28;
                // Give the larger glyphs enough vertical space without shrinking them at runtime.
                var tamanho = t.rectTransform.sizeDelta;
                tamanho.y = Mathf.Max(tamanho.y, t.fontSize * 1.4f);
                t.rectTransform.sizeDelta = tamanho;
                EditorUtility.SetDirty(t);
            }
            var elementos = ui.GetComponentsInChildren<ElementoInterface>(true).ToDictionary(e => e.Chave);
            void Pos(string chave, float x, float y, float w, float h)
            {
                if (!elementos.TryGetValue(chave, out var e)) return;
                var r = (RectTransform)e.transform;
                r.anchorMin = r.anchorMax = new Vector2(0, 1); r.pivot = new Vector2(.5f, .5f);
                r.anchoredPosition = new Vector2(x + w / 2, -y - h / 2); r.sizeDelta = new Vector2(w, h);
            }
            string[] nomes = { "SAÚDE", "INFRAESTRUTURA", "PESQUISA" };
            for (int n = 0; n < 3; n++)
            {
                float x = 300 + 330 * n;
                Pos("Mesa/Icone " + nomes[n], x + 12, 24, 72, 72);
                Pos("Mesa/Valor " + nomes[n], x + 100, 34, 215, 55);
            }
            Pos("Mesa/Rodada e fase", 30, 122, 700, 42);
            Pos("Mesa/Titulo", 30, 6, 245, 110);
            Pos("Mesa/Objetivo", 730, 122, 840, 42);
            for (int n = 1; n <= 3; n++)
            {
                string k = "Mesa/Ameaca " + n;
                Pos(k, 30 + (n - 1) * 520, 175, 500, 260);
                Pos(k + "/Nome", 150, 4, 335, 78);
                Pos(k + "/Ilustracao", 12, 68, 125, 135);
                Pos(k + "/Tipo de requisito", 150, 82, 335, 28);
                for (int r = 0; r < 3; r++) Pos(k + "/Requisito " + r, 150 + r * 110, 112, 110, 42);
                Pos(k + "/Sem requisito", 150, 112, 335, 42);
                Pos(k + "/Tipo de penalidade", 150, 156, 335, 28);
                Pos(k + "/Penalidade", 150, 184, 335, 38);
                Pos(k + "/Situacao", 150, 225, 335, 30);
                Pos(k + "/Indicador de alvo", 12, 225, 125, 30);
            }
            Pos("Mesa/Orientacao da rodada", 30, 446, 1250, 40);
            Pos("Mesa/Mais ameacas", 1310, 446, 250, 42);
            Pos("Mesa/Mais ameacas/Legenda", 8, 3, 234, 36);
            Pos("Mesa/Titulo da inspecao", 110, 491, 1435, 40);
            Pos("Mesa/Descricao da inspecao", 110, 533, 1435, 58);
            for (int j = 1; j <= 2; j++)
            {
                float x = j == 1 ? 30 : 820;
                Pos("Mesa/Nome J" + j, x + 74, 596, 370, 38);
                Pos("Mesa/Retrato J" + j, x + 8, 594, 55, 62);
                Pos("Mesa/Baralho J" + j, x + 74, 633, 650, 36);
                Pos("Mesa/Confirmar J" + j, x + 460, 595, 285, 42);
                Pos("Mesa/Confirmar J" + j + "/Legenda", 8, 3, 269, 36);
                for (int n = 1; n <= 6; n++)
                {
                    Pos("Mesa/Carta J" + j + " espaco " + n, x + 15 + (n - 1) * 120, 672, 105, 157);
                    Pos("Mesa/Legenda J" + j + " espaco " + n, x + 12 + (n - 1) * 120, 831, 114, 84);
                }
            }
            Pos("Mesa/Rotulo da fila", 30, 916, 240, 38);
            elementos["Mesa/Rotulo da fila"].GetComponent<TMP_Text>().text = "FILA DE CARTAS";
            Pos("Mesa/Fila de cartas", 285, 914, 1265, 42);
            Pos("Mesa/Mensagem da partida", 30, 960, 1540, 38);
            Pos("Como jogar/Instrucoes", 245, 240, 1110, 525);
            Pos("Caderno/Conteudo do caderno", 230, 305, 1140, 475);
            ui.MostrarTelaNoEditor("Mesa");
            EditorUtility.SetDirty(config); EditorUtility.SetDirty(ui);
            EditorSceneManager.MarkSceneDirty(ui.gameObject.scene);
            AssetDatabase.SaveAssets();
        }

        static TMP_FontAsset CriarFonte()
        {
            string caminho = Pasta + "/Xmind SDF.asset";
            var fonte = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(caminho);
            if (fonte != null) { ConfigurarFallback(fonte); return fonte; }
            var origem = AssetDatabase.LoadAssetAtPath<Font>("Assets/TextMesh Pro/Fonts/Xmind-VariableFont_wght.ttf");
            if (origem == null) throw new InvalidOperationException("Fonte Xmind não encontrada.");
            fonte = TMP_FontAsset.CreateFontAsset(origem, 90, 9, GlyphRenderMode.SDFAA, 2048, 2048, AtlasPopulationMode.Dynamic);
            fonte.name = "Xmind SDF";
            string letras = new string(Enumerable.Range(32, 95).Select(i => (char)i).ToArray()) + "áàâãäéèêëíìîïóòôõöúùûüçÁÀÂÃÄÉÈÊËÍÌÎÏÓÒÔÕÖÚÙÛÜÇ−·—";
            fonte.TryAddCharacters(letras, out string faltantes);
            if (!string.IsNullOrEmpty(faltantes)) Debug.LogWarning("Caracteres não disponíveis na Xmind: " + faltantes);
            AssetDatabase.CreateAsset(fonte, caminho);
            fonte.material.name = "Xmind UI";
            AssetDatabase.AddObjectToAsset(fonte.material, fonte);
            foreach (var textura in fonte.atlasTextures) if (textura != null) AssetDatabase.AddObjectToAsset(textura, fonte);
            ConfigurarFallback(fonte);
            return fonte;
        }

        static void ConfigurarFallback(TMP_FontAsset fonte)
        {
            var simbolos = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset");
            if (simbolos != null && !fonte.fallbackFontAssetTable.Contains(simbolos)) fonte.fallbackFontAssetTable.Add(simbolos);
            EditorUtility.SetDirty(fonte);
        }

        static TMP_SpriteAsset CriarIcones(AparenciaJogo arte)
        {
            string caminho = Pasta + "/Icones Recursos.asset";
            var asset = AssetDatabase.LoadAssetAtPath<TMP_SpriteAsset>(caminho);
            if (asset != null) return asset;
            var sprites = new[] { arte.saude, arte.infraestrutura, arte.pesquisa, arte.conhecimento };
            string[] nomes = { "Saúde", "Infraestrutura", "Pesquisa", "Conhecimento" };
            var imagens = sprites.Select(LerSprite).ToArray();
            var atlas = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            var retangulos = atlas.PackTextures(imagens, 4, 1024);
            string png = Pasta + "/Icones Recursos.png";
            File.WriteAllBytes(png, atlas.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(atlas);
            foreach (var i in imagens) UnityEngine.Object.DestroyImmediate(i);
            AssetDatabase.ImportAsset(png);
            var importer = (TextureImporter)AssetImporter.GetAtPath(png);
            importer.textureType = TextureImporterType.Default; importer.alphaIsTransparency = true;
            importer.filterMode = FilterMode.Point; importer.mipmapEnabled = false;
            importer.textureCompression = TextureImporterCompression.Uncompressed; importer.SaveAndReimport();
            var textura = AssetDatabase.LoadAssetAtPath<Texture2D>(png);
            asset = ScriptableObject.CreateInstance<TMP_SpriteAsset>(); asset.name = "Icones Recursos";
            var serializado = new SerializedObject(asset);
            serializado.FindProperty("m_Version").stringValue = "1.1.0"; serializado.ApplyModifiedPropertiesWithoutUndo();
            asset.spriteSheet = textura;
            asset.material = new Material(Shader.Find("TextMeshPro/Sprite")) { name = "Icones Recursos", mainTexture = textura };
            for (int n = 0; n < sprites.Length; n++)
            {
                var r = retangulos[n]; int w = Mathf.RoundToInt(r.width * textura.width), h = Mathf.RoundToInt(r.height * textura.height);
                var glyph = new TMP_SpriteGlyph((uint)n, new GlyphMetrics(w, h, 0, h * .85f, w + 3), new GlyphRect(Mathf.RoundToInt(r.x * textura.width), Mathf.RoundToInt(r.y * textura.height), w, h), 1, 0);
                asset.spriteGlyphTable.Add(glyph);
                asset.spriteCharacterTable.Add(new TMP_SpriteCharacter((uint)(0xE000 + n), asset, glyph) { name = nomes[n], scale = 1 });
            }
            asset.UpdateLookupTables();
            AssetDatabase.CreateAsset(asset, caminho); AssetDatabase.AddObjectToAsset(asset.material, asset);
            return asset;
        }

        static Texture2D LerSprite(Sprite sprite)
        {
            if (sprite == null) throw new InvalidOperationException("Falta um ícone na aparência do jogo.");
            var original = RenderTexture.active;
            var rt = RenderTexture.GetTemporary(sprite.texture.width, sprite.texture.height, 0, RenderTextureFormat.ARGB32);
            try
            {
                Graphics.Blit(sprite.texture, rt); RenderTexture.active = rt;
                var rect = sprite.textureRect;
                var tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
                tex.ReadPixels(rect, 0, 0); tex.Apply(); return tex;
            }
            finally { RenderTexture.active = original; RenderTexture.ReleaseTemporary(rt); }
        }
    }
}
