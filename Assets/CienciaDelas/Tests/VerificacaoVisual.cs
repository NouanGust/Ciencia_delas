#if DEVELOPMENT_BUILD || UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CienciaDelas
{
    // Só existe em builds de desenvolvimento; só executa com --validar-visual.
    public sealed class VerificacaoVisual : MonoBehaviour
    {
        private readonly List<string> erros=new List<string>();
        private string destino;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Preparar()
        {
            if(Environment.GetCommandLineArgs().Contains("--validar-visual")) new GameObject("Verificação visual").AddComponent<VerificacaoVisual>();
        }
        void Erro(string mensagem,string stack,LogType tipo) { if(tipo==LogType.Error || tipo==LogType.Exception || tipo==LogType.Assert) erros.Add(mensagem+"\n"+stack); }
        IEnumerator Start()
        {
            Application.runInBackground=true;
            Application.logMessageReceived+=Erro;
            destino=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../output/validacao/visual")); Directory.CreateDirectory(destino);
            Screen.SetResolution(1600,1000,false);
            yield return new WaitForSecondsRealtime(2);
            yield return Capturar("01-menu");
            InterfaceDiretor.Instance.Navegar(2); InterfaceDiretor.Instance.Acionar();
            yield return new WaitForSecondsRealtime(.3f); yield return Capturar("13-como-jogar");
            InterfaceDiretor.Instance.Cancelar(); yield return new WaitForSecondsRealtime(.3f);
            InterfaceDiretor.Instance.Acionar(); // Primeiro botão do menu: Iniciar.
            yield return new WaitForSecondsRealtime(.5f);
            var p=PartidaDiretor.Instance; var config=InicializadorJogo.Instance.configuracao;
            if(p.Fase!=Fase.Planejamento) { erros.Add("O botão Iniciar não abriu o planejamento."); Concluir(); yield break; }
            // Fixture de seis cartas para conferir a capacidade máxima e os efeitos especiais.
            foreach(var j in JogadoresDiretor.Instance.Jogadores) JogadoresDiretor.Instance.Comprar(j,2);
            Colocar(0,config.cartas.Single(c=>c.id=="radioatividade"));
            Colocar(1,config.cartas.Single(c=>c.id=="diagnostico"));
            InterfaceDiretor.Instance.Atualizar();
            yield return new WaitForSecondsRealtime(.7f);
            yield return Capturar("02-mesa-seis-cartas");
            InterfaceDiretor.Instance.Especial(); InterfaceDiretor.Instance.Acionar();
            PlanejamentoDiretor.Instance.Alternar(1,config.cartas.Single(c=>c.id=="diagnostico"));
            yield return new WaitForSecondsRealtime(.3f);
            yield return Capturar("03-selecao");
            InicializadorJogo.Instance.enabled=false;
            PlanejamentoDiretor.Instance.Confirmar(0);
            yield return new WaitForSecondsRealtime(.3f); yield return Capturar("12-aguardando-confirmacao");
            PlanejamentoDiretor.Instance.Confirmar(1); p.AvancarExecucao();
            yield return new WaitForSecondsRealtime(.42f);
            yield return Capturar("04-efeitos");
            yield return new WaitForSecondsRealtime(.8f); p.AvancarExecucao();
            yield return new WaitForSecondsRealtime(.7f);
            yield return Capturar("05-diagnostico");
            p.FinalizarExtras(); p.AvancarExecucao();
            // Com a separação de fases, AvancarExecucao entra em Fase.Ameacas ao esvaziar a fila.
            // No teste (InicializadorJogo desabilitado), avançamos manualmente para validar o retorno.
            if(p.Fase==Fase.Ameacas) p.FinalizarRodada();
            if(p.Fase!=Fase.Planejamento) erros.Add("A execução não retornou ao planejamento.");
            p.Pausar(); yield return new WaitForSecondsRealtime(.3f);
            yield return Capturar("06-pausa");
            InterfaceDiretor.Instance.Navegar(1); InterfaceDiretor.Instance.Acionar();
            yield return new WaitForSecondsRealtime(.3f); yield return Capturar("07-opcoes");
            InterfaceDiretor.Instance.Cancelar(); p.Pausar();
            for(int n=0;n<4;n++) AmeacasDiretor.Instance.Revelar();
            InterfaceDiretor.Instance.Atualizar(); yield return new WaitForSecondsRealtime(.3f); yield return Capturar("08-multiplas-ameacas");
            p.Encerrar(true); yield return new WaitForSecondsRealtime(.5f); yield return Capturar("08-vitoria");
            // Requisitos compostos e custos devem continuar legíveis nas três ameaças lado a lado.
            var regras=JsonUtility.FromJson<Regras>(JsonUtility.ToJson(config.regras)); regras.embaralharAmeacas=false;
            string[] primeiros={"origem","testes","global"};
            var ordenadas=primeiros.Select(id=>config.ameacas.Single(a=>a.id==id)).Concat(config.ameacas.Where(a=>!primeiros.Contains(a.id))).ToList();
            p.Iniciar(Cientista.MarieCurie,regras,config.cartas,ordenadas,34);
            AmeacasDiretor.Instance.Revelar(); AmeacasDiretor.Instance.Revelar(); InterfaceDiretor.Instance.Atualizar();
            yield return new WaitForSecondsRealtime(.7f); yield return Capturar("10-condicoes-e-penalidades");
            EfeitosCartasDiretor.Instance.Executar(new Jogada { jogador=0,carta=config.cartas.Single(c=>c.id=="radioatividade"),alvo=AmeacasDiretor.Instance.Ativas[0] });
            EfeitosCartasDiretor.Instance.Executar(new Jogada { jogador=1,carta=config.cartas.Single(c=>c.id=="controle"),alvo=AmeacasDiretor.Instance.Ativas[2] });
            EfeitosCartasDiretor.Instance.Executar(new Jogada { jogador=1,carta=config.cartas.Single(c=>c.id=="mapeamento") });
            InterfaceDiretor.Instance.Atualizar(); yield return new WaitForSecondsRealtime(.4f); yield return Capturar("11-penalidades-modificadas");
            p.Iniciar(Cientista.MarieCurie,config.regras,config.cartas,config.ameacas,34); RecursosDiretor.Instance.Alterar(new Recursos(-30,0,0));
            yield return new WaitForSecondsRealtime(.5f); yield return Capturar("09-derrota");
            Concluir();
        }
        void Colocar(int jogador,CartaDados carta)
        {
            var j=JogadoresDiretor.Instance.Jogadores[jogador]; if(j.mao.Contains(carta)) return;
            j.baralho.Remove(carta); var devolvida=j.mao[0]; j.mao.RemoveAt(0); j.baralho.Add(devolvida); j.mao.Add(carta);
        }
        IEnumerator Capturar(string nome)
        {
            // A janela oculta não apresenta o backbuffer. Renderiza a mesma UI em uma textura
            // para inspecionar o layout sem abrir uma janela na área de trabalho do usuário.
            yield return new WaitForEndOfFrame();
            var canvas=InterfaceDiretor.Instance.GetComponentInChildren<Canvas>(); var camera=Camera.main;
            var modo=canvas.renderMode; var cameraAnterior=canvas.worldCamera; float plano=canvas.planeDistance;
            var destinoAnterior=camera.targetTexture; var ativoAnterior=RenderTexture.active;
            var render=new RenderTexture(1600,1000,24,RenderTextureFormat.ARGB32); render.Create();
            var captura=new Texture2D(1600,1000,TextureFormat.RGB24,false);
            try
            {
                canvas.renderMode=RenderMode.ScreenSpaceCamera; canvas.worldCamera=camera; canvas.planeDistance=1;
                camera.targetTexture=render; Canvas.ForceUpdateCanvases();
                RenderPipeline.SubmitRenderRequest(camera,new UniversalRenderPipeline.SingleCameraRequest { destination=render });
                RenderTexture.active=render; captura.ReadPixels(new Rect(0,0,1600,1000),0,0); captura.Apply();
                File.WriteAllBytes(Path.Combine(destino,nome+".png"),captura.EncodeToPNG());
                var pixels=captura.GetPixels32(); bool variou=false;
                for(int n=100;n<pixels.Length;n+=100) if(!pixels[n].Equals(pixels[0])) { variou=true; break; }
                if(!variou) erros.Add("Captura sem conteúdo: "+nome);
            }
            finally
            {
                canvas.renderMode=modo; canvas.worldCamera=cameraAnterior; canvas.planeDistance=plano; camera.targetTexture=destinoAnterior; RenderTexture.active=ativoAnterior;
                render.Release(); Destroy(render); Destroy(captura); Canvas.ForceUpdateCanvases();
            }
            yield return new WaitForSecondsRealtime(.2f);
        }
        void Concluir()
        {
            File.WriteAllText(Path.Combine(destino,"resultado.txt"),erros.Count==0?"PASS: menu, início, seis cartas por mão, seleção, execução com efeitos, diagnóstico, rodada seguinte, pausa, opções, encerramentos, requisitos compostos, custos, bloqueio, redução e conversão de penalidades; sem erros de execução.":string.Join("\n\n",erros));
            Application.logMessageReceived-=Erro;
            Application.Quit(erros.Count==0?0:1);
        }
    }
}
#endif
