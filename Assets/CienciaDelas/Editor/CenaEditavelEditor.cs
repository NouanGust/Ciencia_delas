using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CienciaDelas.Editor
{
    public static class CenaEditavelEditor
    {
        [MenuItem("Ciência Delas/Preparar interface editável na cena aberta")]
        public static void Preparar()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                throw new InvalidOperationException("Saia do modo de jogo para editar a cena.");
            var cena = SceneManager.GetActiveScene();
            var inicio = cena.GetRootGameObjects().SelectMany(g => g.GetComponentsInChildren<InicializadorJogo>(true)).SingleOrDefault();
            if (inicio == null) throw new InvalidOperationException("Abra a cena CienciaDelas, que contém o InicializadorJogo.");
            var config = inicio.configuracao != null ? inicio.configuracao : Resources.Load<ConfiguracaoJogo>("CienciaDelasConfiguracao");
            if (config == null) throw new InvalidOperationException("Associe uma configuração ao InicializadorJogo.");
            PartidaDiretor.Validar(config.regras, config.cartas, config.ameacas);
            Undo.IncrementCurrentGroup();
            int grupo = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Preparar interface editável");
            Undo.RegisterFullObjectHierarchyUndo(inicio.gameObject, "Preparar interface editável");
            var existentes = inicio.GetComponentsInChildren<Transform>(true).Select(t => t.GetInstanceID()).ToHashSet();
            T Componente<T>() where T : Component
            {
                var c = inicio.GetComponent<T>();
                return c != null ? c : Undo.AddComponent<T>(inicio.gameObject);
            }
            inicio.configuracao = config;
            var ui = Componente<InterfaceDiretor>();
            Componente<AudioPartidaDiretor>().PrepararFontes();
            Componente<EntradaDiretor>(); Componente<EfeitosVisuaisDiretor>(); Componente<ParticulasUIDiretor>();
            ui.ConstruirCenaEditavel(config);
            OrganizarMesa(ui);
            foreach (var t in inicio.GetComponentsInChildren<Transform>(true))
                if (!existentes.Contains(t.GetInstanceID()) && t.parent != null && existentes.Contains(t.parent.GetInstanceID()))
                    Undo.RegisterCreatedObjectUndo(t.gameObject, "Criar interface editável");
            EditorUtility.SetDirty(inicio); EditorUtility.SetDirty(ui);
            EditorSceneManager.MarkSceneDirty(cena);
            Undo.CollapseUndoOperations(grupo);
            Selection.activeGameObject = ui.gameObject;
            Debug.Log("Interface editável preparada: oito telas, espaços das cartas, ameaças, áudio e entrada salvos na hierarquia. Salve a cena para manter as alterações.");
        }

        static void OrganizarMesa(InterfaceDiretor ui)
        {
            var mesa = ui.Telas.Single(t => t.nome == "Mesa").raiz;
            foreach (var e in mesa.GetComponentsInChildren<ElementoInterface>(true))
            {
                if (e.transform.parent != mesa) continue;
                string nome = e.name;
                string grupo = nome.Contains("J1") ? "Jogadora 1" : nome.Contains("J2") ? "Jogadora 2" :
                    nome.StartsWith("Ameaca ") || nome == "Mais ameacas" ? "Ameacas" :
                    nome.StartsWith("Icone ") || nome.StartsWith("Rotulo S") || nome.StartsWith("Rotulo I") || nome.StartsWith("Rotulo P") || nome.StartsWith("Valor ") ? "Recursos" :
                    nome.Contains("inspec") ? "Inspecao da carta" : null;
                if (grupo == null) continue;
                var pai = mesa.Find(grupo) as RectTransform;
                if (pai == null)
                {
                    pai = new GameObject(grupo, typeof(RectTransform)).GetComponent<RectTransform>();
                    pai.SetParent(mesa, false); pai.anchorMin = Vector2.zero; pai.anchorMax = Vector2.one;
                    pai.offsetMin = pai.offsetMax = Vector2.zero;
                }
                e.transform.SetParent(pai, true);
            }
        }
    }

    [CustomEditor(typeof(InterfaceDiretor))]
    public sealed class InterfaceDiretorInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Edite os objetos em Interface - Ciencia Delas > Telas. Posição, tamanho, imagens e textos fixos são preservados ao jogar. Os botões abaixo mostram uma tela para edição; o jogo sempre inicia no menu.", MessageType.Info);
            DrawDefaultInspector();
            if (Application.isPlaying) return;
            var ui = (InterfaceDiretor)target;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mostrar tela para edição", EditorStyles.boldLabel);
            foreach (var tela in ui.Telas)
            {
                if (tela.raiz == null || !GUILayout.Button(tela.nome)) continue;
                foreach (var t in ui.Telas) Undo.RecordObject(t.raiz.gameObject, "Mostrar tela");
                ui.MostrarTelaNoEditor(tela.nome);
                EditorSceneManager.MarkSceneDirty(ui.gameObject.scene);
                SceneView.RepaintAll();
            }
        }
    }

    [CustomEditor(typeof(ElementoInterface))]
    public sealed class ElementoInterfaceInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Ligação com o jogo", ((ElementoInterface)target).Chave);
            EditorGUILayout.HelpBox("Edite Rect Transform, Image, TextMeshPro e Button neste objeto. Os campos abaixo controlam quais dados o jogo pode atualizar. Eventos adicionais de Button são preservados.", MessageType.None);
            DrawDefaultInspector();
        }
    }
}
