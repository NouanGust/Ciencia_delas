using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CienciaDelas.Editor
{
    public static class CarrosselEditor
    {
        [MenuItem("Ciência Delas/Aplicar mesa com carrossel")]
        public static void Aplicar()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) throw new InvalidOperationException("Saia do Play antes de editar.");
            var ui = UnityEngine.Object.FindFirstObjectByType<InterfaceDiretor>();
            if (ui == null) throw new InvalidOperationException("Abra a cena CienciaDelas.");
            Undo.RegisterFullObjectHierarchyUndo(ui.gameObject, "Aplicar mesa com carrossel");
            ui.AplicarLayoutCarrossel(ui.GetComponent<InicializadorJogo>().configuracao);
            EditorUtility.SetDirty(ui);
            EditorSceneManager.MarkSceneDirty(ui.gameObject.scene);
        }
    }
}
