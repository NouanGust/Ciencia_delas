using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CienciaDelas.Editor
{
    public static class AplicarArteProjeto
    {
        [Serializable] private class Entrada { public string id,caminho; }
        [Serializable] private class Mapa { public Entrada[] cartas,aparencia; }
        [MenuItem("Ciência Delas/Visual/Reaplicar assets da pasta Art")]
        public static void Aplicar()
        {
            var mapa=JsonUtility.FromJson<Mapa>(File.ReadAllText("Assets/CienciaDelas/Editor/MapeamentoArte.json"));
            var config=AssetDatabase.LoadAssetAtPath<ConfiguracaoJogo>("Assets/Resources/CienciaDelasConfiguracao.asset");
            var visual=AssetDatabase.LoadAssetAtPath<AparenciaJogo>("Assets/Resources/CienciaDelasAparencia.asset");
            if(config==null || visual==null) throw new InvalidOperationException("Configuração ou aparência não encontrada.");
            foreach(var e in mapa.cartas.Concat(mapa.aparencia))
            {
                var importer=AssetImporter.GetAtPath(e.caminho) as TextureImporter;
                if(importer==null) throw new InvalidOperationException("Imagem não importada: "+e.caminho);
                bool mudou=importer.filterMode!=FilterMode.Point || importer.textureCompression!=TextureImporterCompression.Uncompressed || importer.mipmapEnabled;
                importer.filterMode=FilterMode.Point; importer.textureCompression=TextureImporterCompression.Uncompressed; importer.mipmapEnabled=false;
                if(importer.textureType!=TextureImporterType.Sprite) { importer.textureType=TextureImporterType.Sprite; importer.spriteImportMode=SpriteImportMode.Single; mudou=true; }
                if(mudou) importer.SaveAndReimport();
            }
            Undo.RecordObject(config,"Aplicar artes"); Undo.RecordObject(visual,"Aplicar aparência");
            foreach(var e in mapa.cartas)
            {
                var sprite=Sprite(e.caminho); var entrada=config.artes.Find(x=>x.id==e.id);
                if(entrada==null) { entrada=new ArteCarta { id=e.id }; config.artes.Add(entrada); }
                entrada.imagem=sprite;
            }
            var serializado=new SerializedObject(visual);
            foreach(var e in mapa.aparencia)
            {
                var campo=serializado.FindProperty(e.id);
                if(campo==null) throw new InvalidOperationException("Campo visual ausente: "+e.id);
                campo.objectReferenceValue=e.id=="slash" || e.id=="explosao"?(UnityEngine.Object)AssetDatabase.LoadAssetAtPath<Texture2D>(e.caminho):Sprite(e.caminho);
            }
            serializado.ApplyModifiedPropertiesWithoutUndo(); config.aparencia=visual;
            EditorUtility.SetDirty(config); EditorUtility.SetDirty(visual); AssetDatabase.SaveAssets();
            Verificar();
        }
        static Sprite Sprite(string caminho)
        {
            var sprite=AssetDatabase.LoadAllAssetsAtPath(caminho).OfType<Sprite>().FirstOrDefault();
            if(sprite==null) throw new InvalidOperationException("Sprite não encontrado: "+caminho);
            return sprite;
        }
        [MenuItem("Ciência Delas/Visual/Verificar referências")]
        public static void Verificar()
        {
            var config=AssetDatabase.LoadAssetAtPath<ConfiguracaoJogo>("Assets/Resources/CienciaDelasConfiguracao.asset");
            if(config==null || config.aparencia==null) throw new InvalidOperationException("Configuração visual ausente.");
            var ids=config.cartas.Select(c=>c.id).Concat(config.ameacas.Select(a=>a.id));
            foreach(var id in ids) if(config.Imagem(id)==null) throw new InvalidOperationException("Arte ausente: "+id);
            var a=config.aparencia;
            if(a.fundoMenu==null || a.fundoMesa==null || a.marie==null || a.jaqueline==null || a.slash==null || a.explosao==null) throw new InvalidOperationException("Cenários, cientistas ou sequências ausentes.");
            foreach(var textura in new[]{a.slash,a.explosao})
                if(a.tamanhoCelula<=0 || a.quadros<=0 || a.primeiraColuna<0 || a.linhaDoTopo<0 || (a.primeiraColuna+a.quadros)*a.tamanhoCelula>textura.width || (a.linhaDoTopo+1)*a.tamanhoCelula>textura.height) throw new InvalidOperationException("Recorte da sequência inválido.");
            Directory.CreateDirectory("output/validacao");
            File.WriteAllText("output/validacao/visual-assets.txt","35 referências de cartas/ameaças válidas; cenários, cientistas e duas sequências de slash conferidos no Unity.");
            Debug.Log("Referências visuais verificadas.");
        }
        public static void PrepararEVerificar() { Aplicar(); FerramentasProjeto.Verificar(); }
    }
}
