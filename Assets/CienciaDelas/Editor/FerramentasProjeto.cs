using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CienciaDelas.Editor
{
    public static class FerramentasProjeto
    {
        const string Cena="Assets/Scenes/CienciaDelas.unity";
        const string Config="Assets/Resources/CienciaDelasConfiguracao.asset";
        [MenuItem("Ciência Delas/Preparar cena e configuração")]
        public static void Preparar()
        {
            var config=AssetDatabase.LoadAssetAtPath<ConfiguracaoJogo>(Config);
            if(config==null)
            {
                config=ScriptableObject.CreateInstance<ConfiguracaoJogo>();
                config.menu=Audio("Menu Principal.mp3"); config.partida=Audio("Tema fase.mp3"); config.crise=Audio("batalha.wav");
                config.vitoria=Audio("Vitoria.mp3"); config.derrota=Audio("game over.mp3");
                config.selecionar=Audio("Select.ogg"); config.cancelar=Audio("Cancel.ogg"); config.comprar=Audio("Swap.ogg"); config.confirmar=Audio("Play.ogg"); config.especial=Audio("Special.ogg");
                foreach(var c in config.cartas) config.artes.Add(new ArteCarta { id=c.id });
                foreach(var a in config.ameacas) config.artes.Add(new ArteCarta { id=a.id });
                AssetDatabase.CreateAsset(config,Config);
            }
            PartidaDiretor.Validar(config.regras,config.cartas,config.ameacas);
            if(!File.Exists(Cena))
            {
                var cena=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
                var camera=new GameObject("Main Camera",typeof(Camera),typeof(AudioListener)); camera.tag="MainCamera";
                camera.GetComponent<Camera>().clearFlags=CameraClearFlags.SolidColor; camera.GetComponent<Camera>().backgroundColor=config.fundoCalmo;
                var inicio=new GameObject("Diretores Ciência Delas").AddComponent<InicializadorJogo>(); inicio.configuracao=config;
                EditorSceneManager.SaveScene(cena,Cena);
            }
            // A cena cooperativa é a entrada. Cenas antigas continuam disponíveis no projeto.
            EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(Cena,true)};
            AssetDatabase.SaveAssets();
        }
        static AudioClip Audio(string nome) => AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/Unity sfx/"+nome);
        [MenuItem("Ciência Delas/Verificar regras")]
        public static void Verificar()
        {
            Directory.CreateDirectory("output/validacao");
            string resultado=VerificacaoRegras.ExecutarTudo();
            File.WriteAllText("output/validacao/regras.txt",resultado); Debug.Log(resultado);
        }
        public static void PrepararEVerificar() { Preparar(); Verificar(); }
        public static void GerarValidacaoVisual()
        {
            AplicarArteProjeto.Verificar(); Verificar(); Directory.CreateDirectory("Builds/Visual");
            var result=BuildPipeline.BuildPlayer(new BuildPlayerOptions { scenes=new[]{Cena},locationPathName="Builds/Visual/CienciaDelas.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.Development });
            if(result.summary.result!=BuildResult.Succeeded) throw new Exception("Falha no build de validação visual.");
        }
        [MenuItem("Ciência Delas/Gerar jogo para Windows")]
        public static void GerarWindows()
        {
            Preparar(); Verificar();
            Directory.CreateDirectory("Builds/Windows");
            var result=BuildPipeline.BuildPlayer(new BuildPlayerOptions { scenes=new[]{Cena},locationPathName="Builds/Windows/CienciaDelas.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.None });
            File.WriteAllText("output/validacao/build.txt",$"Resultado: {result.summary.result}\nErros: {result.summary.totalErrors}\nAvisos: {result.summary.totalWarnings}\nTamanho: {result.summary.totalSize} bytes");
            if(result.summary.result!=BuildResult.Succeeded) throw new Exception("Falha ao gerar o executável Windows.");
        }
    }
}
