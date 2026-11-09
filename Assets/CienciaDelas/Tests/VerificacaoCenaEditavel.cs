#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CienciaDelas
{
    // Invocação explícita no Editor: não executa automaticamente nem altera os assets.
    public sealed class VerificacaoCenaEditavel : MonoBehaviour
    {
        readonly List<string> erros = new List<string>();
        InterfaceDiretor ui;
        ElementoInterface[] elementos;
        ElementoInterface Elemento(string chave) => elementos.Single(e => e.Chave == chave);
        void Exigir(bool condicao, string mensagem) { if (!condicao) erros.Add(mensagem); }
        void Clicar(string chave) { Elemento(chave).GetComponent<Button>().onClick.Invoke(); }
        void Erro(string mensagem, string stack, LogType tipo)
        {
            if (tipo == LogType.Error || tipo == LogType.Exception || tipo == LogType.Assert) erros.Add(mensagem + "\n" + stack);
        }

        IEnumerator Start()
        {
            Application.logMessageReceived += Erro;
            yield return new WaitForSecondsRealtime(.5f);
            ui = InterfaceDiretor.Instance;
            elementos = ui.GetComponentsInChildren<ElementoInterface>(true);
            var ids = elementos.Select(e => e.GetInstanceID()).OrderBy(i => i).ToArray();
            var p = PartidaDiretor.Instance;
            Exigir(ui.Telas.Count == 8, "Oito telas precisam estar salvas na cena.");
            Exigir(p.Fase == Fase.Menu, "A partida deve iniciar no menu.");
            Exigir(ui.GetComponents<InterfaceDiretor>().Length == 1 && ui.GetComponents<AudioPartidaDiretor>().Length == 1, "Diretores duplicados.");
            Exigir(ui.GetComponentsInChildren<Canvas>(true).Length == 1, "Canvas duplicado.");
            Exigir(ui.GetComponentsInChildren<AudioSource>(true).Length == 3, "Fontes de áudio duplicadas.");

            var titulo = Elemento("Menu/Titulo").GetComponent<TextMeshProUGUI>();
            var texto = titulo.text; var posicao = titulo.rectTransform.anchoredPosition;
            var tamanho = titulo.fontSize; var cor = titulo.color;
            titulo.text = "Texto editado por uma aluna"; titulo.fontSize = 37;
            titulo.color = Color.cyan; titulo.rectTransform.anchoredPosition += new Vector2(13, -9);
            var enfeite = new GameObject("Decoracao da aluna", typeof(RectTransform));
            enfeite.transform.SetParent(titulo.transform.parent, false);
            for (int n = 0; n < 5; n++) { ui.Atualizar(); yield return null; }
            Exigir(titulo.text == "Texto editado por uma aluna" && titulo.fontSize == 37 && titulo.color == Color.cyan, "A atualização sobrescreveu o estilo ou texto fixo.");
            Exigir(titulo.rectTransform.anchoredPosition == posicao + new Vector2(13, -9), "A atualização sobrescreveu a posição.");
            Clicar("Menu/Trocar cientistas"); yield return null;
            Exigir(Elemento("Menu/Nome J1").GetComponent<TMP_Text>().text == "Jaqueline Goes", "Troca de cientistas não funcionou ou acumulou ações.");
            Clicar("Menu/Trocar cientistas"); yield return null;
            Clicar("Menu/Como jogar"); yield return null;
            Exigir(ui.Telas.Single(t => t.nome == "Como jogar").raiz.gameObject.activeSelf, "Tutorial não abriu.");
            Clicar("Como jogar/Voltar"); yield return null;
            Exigir(titulo.text == "Texto editado por uma aluna" && enfeite != null, "A troca de tela recriou os objetos.");
            titulo.text = texto; titulo.fontSize = tamanho; titulo.color = cor; titulo.rectTransform.anchoredPosition = posicao;
            Clicar("Menu/Iniciar partida"); yield return new WaitForSecondsRealtime(.5f);
            Exigir(p.Fase == Fase.Planejamento && JogadoresDiretor.Instance.Jogadores.Length == 2, "Início da partida falhou.");
            var plan = PlanejamentoDiretor.Instance;
            var j = JogadoresDiretor.Instance.Jogadores[0];
            int indice = j.Mao.ToList().FindIndex(c => c.conhecimento <= j.conhecimento && RecursosDiretor.Instance.Atual.Cobre(c.custo));
            if (indice >= 0)
            {
                string chave = "Mesa/Carta J1 espaco " + (indice + 1);
                Clicar(chave); yield return null;
                Exigir(plan.Fila.Count == 1, "Seleção da carta falhou.");
                Clicar(chave); yield return null;
                Exigir(plan.Fila.Count == 0, "Cancelamento da carta falhou.");
            }
            else erros.Add("Nenhuma carta selecionável para validar interação.");
            foreach (var jogador in JogadoresDiretor.Instance.Jogadores)
                JogadoresDiretor.Instance.Comprar(jogador, 2);
            for (int n = 0; n < 4; n++) AmeacasDiretor.Instance.Revelar();
            ui.Atualizar(); yield return null;
            Exigir(Elemento("Mesa/Carta J1 espaco 6").gameObject.activeInHierarchy, "Sexta carta não aparece.");
            Clicar("Mesa/Mais ameacas"); yield return null;
            Clicar("Mesa/Mais ameacas"); yield return null;
            Clicar("Mesa/Pausa"); yield return null;
            Exigir(p.Pausada, "Pausa falhou.");
            Clicar("Pausa/Opcoes"); yield return null;
            Clicar("Opcoes/Voltar"); yield return null;
            Clicar("Pausa/Continuar"); yield return null;
            Clicar("Mesa/Informacoes"); yield return null;
            Clicar("Caderno/Ameacas superadas"); yield return null;
            Clicar("Caderno/Historico"); yield return null;
            Clicar("Caderno/Voltar a mesa"); yield return null;
            // Recomeça com uma ameaça, para testar a resolução automática de uma rodada.
            Clicar("Mesa/Pausa"); yield return null;
            Clicar("Pausa/Reiniciar partida"); yield return null;
            int rodada = p.Rodada;
            Clicar("Mesa/Confirmar J1"); yield return null;
            Clicar("Mesa/Confirmar J2"); yield return null;
            yield return new WaitForSecondsRealtime(InicializadorJogo.Instance.configuracao.intervaloExecucao + InicializadorJogo.Instance.configuracao.intervaloResolucao + .5f);
            Exigir(p.Rodada > rodada && p.Fase == Fase.Planejamento, "A rodada não avançou automaticamente.");
            p.Encerrar(true); yield return null;
            Exigir(ui.Telas.Single(t => t.nome == "Vitoria").raiz.gameObject.activeSelf, "Tela de vitória não abriu.");
            Clicar("Vitoria/Jogar novamente"); yield return null;
            p.Encerrar(false); yield return null;
            Exigir(ui.Telas.Single(t => t.nome == "Derrota").raiz.gameObject.activeSelf, "Tela de derrota não abriu.");
            Clicar("Derrota/Menu principal"); yield return null;
            Exigir(enfeite != null, "Decoração adicionada foi destruída.");
            Exigir(ids.SequenceEqual(ui.GetComponentsInChildren<ElementoInterface>(true).Select(e => e.GetInstanceID()).OrderBy(i => i)), "A partida criou ou destruiu elementos permanentes da interface.");
            Destroy(enfeite);
            Directory.CreateDirectory("output/cena-editavel");
            string resultado = erros.Count == 0 ? "PASS: oito telas, inicialização sem duplicação, estilos e textos editados preservados, decoração preservada, troca de cientistas, tutorial, início, seleção/cancelamento, seis cartas, paginação, pausa/opções, caderno, rodada automática, vitória/derrota e identidades dos objetos preservadas. Sem erros de execução." : string.Join("\n\n", erros);
            File.WriteAllText("output/cena-editavel/verificacao.txt", resultado);
            Debug.Log(resultado);
            Application.logMessageReceived -= Erro;
        }
        void OnDestroy() { Application.logMessageReceived -= Erro; }
    }
}
#endif
