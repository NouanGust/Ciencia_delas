using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using TMPro;

namespace CienciaDelas
{
    public sealed class InterfaceDiretor : SingletonComponente<InterfaceDiretor>
    {
        private ConfiguracaoJogo config;
        private AparenciaJogo arte => config.aparencia;
        private RectTransform raiz, camadaEfeitos;
        private TMP_FontAsset fonte;
        private bool suja = true, opcoes, detalhes, tutorial;
        private int aba, paginaAmeacas, foco;
        private Cientista primeira = Cientista.MarieCurie;
        private AmeacaAtiva alvo;
        private CartaDados cartaInspecionada;
        private TextMeshProUGUI tituloDetalhe, textoDetalhe;
        private Image imagemDetalhe;
        private readonly List<Button> botoes = new List<Button>();
        private readonly Dictionary<Button, Action> cancelamentos = new Dictionary<Button, Action>();
        private readonly Dictionary<string, Vector2> posicoesCartas = new Dictionary<string, Vector2>();
        private readonly Dictionary<string, Vector2> posicoesAmeacas = new Dictionary<string, Vector2>();
        private readonly HashSet<string> maoAnterior = new HashSet<string>();
        private string telaAnterior = "";
        private Color painel => new Color(.055f, .07f, .16f, 1);
        private Color ouro => new Color(1, .82f, .44f);
        private Color positivo => new Color(.65f, 1, .78f);
        private Color perda => new Color(1, .65f, .64f);

        public void Inicializar(ConfiguracaoJogo c)
        {
            config = c;
            fonte = c.fonte;
            var canvas = new GameObject("Mesa Ciência Delas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)); canvas.transform.SetParent(transform, false);
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvas.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1600, 1000); scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            raiz = Centro("Conteúdo", canvas.transform); camadaEfeitos = Centro("Efeitos", canvas.transform);
            if (EventSystem.current == null) new GameObject("Entrada UI", typeof(EventSystem), typeof(InputSystemUIInputModule)).transform.SetParent(transform, false);
            EventSystem.current.sendNavigationEvents = false;
            gameObject.AddComponent<EfeitosVisuaisDiretor>().Inicializar(c, raiz, camadaEfeitos);
            PartidaDiretor.Instance.Alterada += Atualizar;
        }

        RectTransform Centro(string nome, Transform pai)
        {
            var r = new GameObject(nome, typeof(RectTransform)).GetComponent<RectTransform>(); r.SetParent(pai, false); r.anchorMin = r.anchorMax = r.pivot = new Vector2(.5f, .5f); r.sizeDelta = new Vector2(1600, 1000); return r;
        }

        public void Atualizar() { suja = true; }

        void LateUpdate() { if (suja && config != null) { suja = false; Desenhar(); } }

        protected override void OnDestroy() { PartidaDiretor.Instance.Alterada -= Atualizar; base.OnDestroy(); }

        RectTransform Objeto(string nome, Transform pai, float x, float y, float w, float h)
        {
            var r = new GameObject(nome, typeof(RectTransform)).GetComponent<RectTransform>(); r.SetParent(pai, false); r.anchorMin = r.anchorMax = new Vector2(0, 1); r.pivot = new Vector2(.5f, .5f); r.anchoredPosition = new Vector2(x + w / 2, -y - h / 2); r.sizeDelta = new Vector2(w, h); return r;
        }

        Image Imagem(Transform pai, Sprite sprite, float x, float y, float w, float h, Color? cor = null, bool proporcao = true)
        {
            var i = Objeto("Imagem", pai, x, y, w, h).gameObject.AddComponent<Image>(); i.sprite = sprite; i.color = cor ?? Color.white; i.preserveAspect = proporcao; i.raycastTarget = false; return i;
        }

        void Painel(float x, float y, float w, float h, Color? cor = null)
        {
            var i = Imagem(raiz, null, x, y, w, h, cor ?? painel); var borda = i.gameObject.AddComponent<Outline>(); borda.effectColor = new Color(.48f, .55f, .8f, .65f); borda.effectDistance = new Vector2(1, -1);
        }

        TextMeshProUGUI Texto(Transform pai, string valor, float x, float y, float w, float h, float tamanho = 22, Color? cor = null, TextAlignmentOptions alinhamento = TextAlignmentOptions.TopLeft)
        {
            var t = Objeto("Texto", pai, x, y, w, h).gameObject.AddComponent<TextMeshProUGUI>();
            t.text = valor;
            if (fonte != null) t.font = fonte;
            t.fontSize = tamanho;
            t.color = cor ?? Color.white;
            t.raycastTarget = false;
            t.alignment = alinhamento;
            t.enableWordWrapping = true;
            t.overflowMode = TextOverflowModes.Truncate;
            return t;
        }

        Button Botao(string texto, float x, float y, float w, float h, Action acao, bool ativo = true, bool marcado = false, Action cancelar = null, Sprite sprite = null)
        {
            var r = Objeto(texto, raiz, x, y, w, h); var i = r.gameObject.AddComponent<Image>(); i.sprite = sprite != null ? sprite : arte?.botaoNormal; i.preserveAspect = sprite != null; i.color = sprite != null ? Color.white : marcado ? config.destaque : new Color(.38f, .43f, .68f);
            var b = r.gameObject.AddComponent<Button>(); b.targetGraphic = i; b.interactable = ativo;
            var cores = b.colors; cores.highlightedColor = new Color(1, 1, .8f); cores.selectedColor = new Color(1, 1, .8f); cores.disabledColor = new Color(.65f, .65f, .7f, .9f); b.colors = cores;
            if (sprite == null) Texto(r, texto, 8, 5, w - 16, h - 10, 20, null, TextAlignmentOptions.Center);
            if (marcado) { var outline = r.gameObject.AddComponent<Outline>(); outline.effectColor = ouro; outline.effectDistance = new Vector2(3, -3); }
            var anim = r.gameObject.AddComponent<AnimacaoInteracao>(); anim.duracao = arte != null ? arte.duracaoHover : .16f;
            b.onClick.AddListener(() => { AudioPartidaDiretor.Instance?.Som(); acao(); suja = true; });
            if (ativo)
            {
                botoes.Add(b); if (cancelar != null) cancelamentos[b] = cancelar;
                anim.aoFocar = () => { int index = botoes.IndexOf(b); if (index >= 0) foco = index; };
            }
            return b;
        }

        void Fundo(bool menu)
        {
            var sprite = arte != null ? (menu ? arte.fundoMenu : arte.fundoMesa) : config.fundo;
            Imagem(raiz, null, 0, 0, 1600, 1000, config.fundoCalmo);
            if (sprite != null) { float largura = Mathf.Max(1600, 1000 * sprite.rect.width / sprite.rect.height); Imagem(raiz, sprite, (1600 - largura) / 2, 0, largura, 1000, Color.white); }
        }

        void Desenhar()
        {
            for (int n = raiz.childCount - 1; n >= 0; n--) { var g = raiz.GetChild(n).gameObject; g.SetActive(false); Destroy(g); }
            botoes.Clear(); cancelamentos.Clear(); tituloDetalhe = null; textoDetalhe = null; imagemDetalhe = null;
            var p = PartidaDiretor.Instance; Fundo(p.Fase == Fase.Menu);
            string tela = opcoes ? "opcoes" : tutorial ? "tutorial" : p.Terminou ? "fim" : p.Pausada ? "pausa" : detalhes ? "detalhes" : p.Fase == Fase.Menu ? "menu" : "mesa";
            switch (tela) { case "opcoes": Opcoes(); break; case "tutorial": Tutorial(); break; case "fim": Fim(); break; case "pausa": PausaTela(); break; case "detalhes": Detalhes(); break; case "menu": Menu(); break; default: Mesa(); break; }
            if (tela != telaAnterior)
            {
                foco = 0;
                if (!EfeitosVisuaisDiretor.Instance.MovimentoReduzido) foreach (Transform filho in raiz)
                    {
                        if (!filho.gameObject.activeSelf) continue;
                        var grupo = filho.GetComponent<CanvasGroup>(); if (grupo == null) grupo = filho.gameObject.AddComponent<CanvasGroup>();
                        grupo.alpha = 0; grupo.DOFade(1, .22f).SetUpdate(true).SetLink(filho.gameObject);
                    }
                telaAnterior = tela;
            }
            if (botoes.Count > 0) { foco = Mathf.Clamp(foco, 0, botoes.Count - 1); EventSystem.current.SetSelectedGameObject(botoes[foco].gameObject); }
        }

        void Menu()
        {
            Texto(raiz, "CIÊNCIA DELAS", 120, 65, 1360, 90, 64, ouro, TextAlignmentOptions.Center);
            bool marieEsq = primeira == Cientista.MarieCurie;
            Imagem(raiz, marieEsq ? arte?.marie : arte?.jaqueline, 60, 260, 440, 490);
            Imagem(raiz, marieEsq ? arte?.jaqueline : arte?.marie, 1100, 260, 440, 490);
            Texto(raiz, "J1", 110, 268, 340, 30, 22, new Color(.6f, .85f, 1f), TextAlignmentOptions.Center);
            Texto(raiz, "J2", 1150, 268, 340, 30, 22, new Color(.6f, .85f, 1f), TextAlignmentOptions.Center);
            Texto(raiz, marieEsq ? "Marie Curie" : "Jaqueline Goes", 110, 770, 340, 45, 30, ouro, TextAlignmentOptions.Center);
            Texto(raiz, marieEsq ? "Jaqueline Goes" : "Marie Curie", 1150, 770, 340, 45, 30, ouro, TextAlignmentOptions.Center);
            Botao("Iniciar partida", 605, 280, 390, 108, Iniciar, sprite: arte?.iniciar);
            Botao("Opções", 605, 415, 390, 100, () => opcoes = true, sprite: arte?.opcoes);
            Botao("Como jogar", 605, 545, 390, 85, () => tutorial = true, sprite: arte?.tutorial);
            Botao("Sair", 605, 670, 390, 100, Sair, sprite: arte?.sair);
            Botao($"J1: {Nome(primeira)}    ↔    J2: {Nome(Outra(primeira))}", 300, 860, 1000, 56, () => primeira = Outra(primeira));
        }

        void Iniciar()
        {
            opcoes = detalhes = tutorial = false; alvo = null; paginaAmeacas = 0; foco = 0; cartaInspecionada = null; maoAnterior.Clear();
            try { PartidaDiretor.Instance.Iniciar(primeira, config.regras, config.cartas, config.ameacas, Environment.TickCount); }
            catch (Exception e) { Debug.LogException(e); PartidaDiretor.Instance.Notificar(e.Message); }
        }

        void Mesa()
        {
            var p = PartidaDiretor.Instance; var r = RecursosDiretor.Instance;
            Texto(raiz, "CIÊNCIA\nDELAS", 30, 22, 245, 85, 32, ouro);
            Recurso(300, arte?.saude, "SAÚDE", r.Atual.saude, r.Reservado.saude, new Color(1, .45f, .5f));
            Recurso(630, arte?.infraestrutura, "INFRAESTRUTURA", r.Atual.infraestrutura, r.Reservado.infraestrutura, new Color(.8f, .78f, 1));
            Recurso(960, arte?.pesquisa, "PESQUISA", r.Atual.pesquisa, r.Reservado.pesquisa, new Color(.4f, 1, .7f));
            Botao("Informações", 1320, 28, 68, 68, () => detalhes = true, sprite: arte?.livro); Botao("Pausa", 1425, 28, 68, 68, () => p.Pausar(), sprite: arte?.pausa);
            var am = AmeacasDiretor.Instance; Texto(raiz, $"RODADA {p.Rodada:00}  /  {ExibirFase(p.Fase)}", 30, 128, 850, 36, 24, ouro);
            Texto(raiz, $"OBJETIVO: {am.Cemiterio.Count}/{config.ameacas.Count} superadas  ·  {am.Ativas.Count} ativa{(am.Ativas.Count == 1 ? "" : "s")}  ·  {am.Restantes} a revelar", 890, 128, 670, 36, 21, null, TextAlignmentOptions.Right);
            if (alvo == null || !am.Contem(alvo)) alvo = am.Ativas.FirstOrDefault();
            paginaAmeacas = Mathf.Clamp(paginaAmeacas, 0, Math.Max(0, (am.Ativas.Count - 1) / 3)); posicoesAmeacas.Clear();
            for (int i = 0; i < 3; i++)
            {
                int n = paginaAmeacas * 3 + i; if (n >= am.Ativas.Count) break; var a = am.Ativas[n]; float x = 30 + i * 520;
                var b = Botao("", x, 175, 500, 240, () => alvo = a, true, alvo == a);
                b.GetComponent<Image>().sprite = null; b.GetComponent<Image>().color = painel;
                var imagem = config.Imagem(a.dados.id); Imagem(b.transform, imagem != null ? imagem : arte?.trofeuQuebrado, 12, 44, 125, 130);
                Texto(b.transform, a.dados.nome, 150, 10, 335, 48, 21, ouro);
                CondicaoAmeaca(b.transform, a);
                PenalidadeAmeaca(b.transform, a);
                bool atendida = am.PodeSuperar(a.dados);
                string situacao = (atendida ? "✓ Atendida" : "✗ Não atendida") + (a.dados.gastar ? " · gasta recursos" : "");
                Texto(b.transform, situacao, 150, 214, 335, 24, 16, atendida ? positivo : ouro);
                if (alvo == a) Texto(b.transform, "ALVO", 12, 200, 125, 25, 16, ouro, TextAlignmentOptions.Center);
                posicoesAmeacas[a.dados.id] = new Vector2(x + 250, 705);
            }
            Texto(raiz, OrientacaoRodada(), 30, 429, 1250, 29, 21, ouro);
            if (am.Ativas.Count > 3) Botao("Mais ameaças →", 1310, 433, 250, 38, () => paginaAmeacas = (paginaAmeacas + 1) % ((am.Ativas.Count + 2) / 3));
            imagemDetalhe = Imagem(raiz, null, 48, 497, 42, 66); imagemDetalhe.enabled = false;
            tituloDetalhe = Texto(raiz, "COMBINEM OS EFEITOS DAS CARTAS", 110, 494, 1435, 30, 23, ouro);
            textoDetalhe = Texto(raiz, "Passe o mouse sobre uma carta para ler. Clique numa ameaça antes de escolher uma carta que precise de alvo.", 110, 527, 1435, 43, 19);
            if (cartaInspecionada != null) Inspecionar(cartaInspecionada);
            posicoesCartas.Clear(); Mao(0, 30); Mao(1, 820);
            var atuais = new HashSet<string>(); foreach (var j in JogadoresDiretor.Instance.Jogadores) foreach (var c in j.Mao) atuais.Add(j.indice + ":" + c.id); maoAnterior.Clear(); maoAnterior.UnionWith(atuais);
            Texto(raiz, "ORDEM DAS CARTAS", 30, 878, 210, 50, 19, ouro);
            string fila = string.Join("  →  ", PlanejamentoDiretor.Instance.Fila.Select((j, n) => $"{n + 1}. J{j.jogador + 1} {j.carta.nome}"));
            var tf = Texto(raiz, fila.Length == 0 ? "Clique nas cartas para montar a fila. Clique novamente para retirar." : fila, 250, 879, 1300, 54, 20);
            tf.enableAutoSizing = true; tf.fontSizeMin = 14; tf.fontSizeMax = 20;
            Texto(raiz, p.Mensagem, 35, 959, 1520, 33, 22, new Color(.65f, 1, .85f));
        }

        void Recurso(float x, Sprite icone, string nome, int valor, int reservado, Color cor)
        {
            Imagem(raiz, icone, x + 12, 37, 47, 47);
            string linhaValor = reservado > 0 ? $"{valor} / {config.regras.maximoRecurso}  −{reservado}" : $"{valor} / {config.regras.maximoRecurso}";
            Texto(raiz, nome, x + 73, 28, 226, 25, 18, cor); Texto(raiz, linhaValor, x + 73, 62, 225, 30, 18);
        }

        void CondicaoAmeaca(Transform pai, AmeacaAtiva ameaca)
        {
            var a = ameaca.dados; var atual = RecursosDiretor.Instance.Atual;
            string verbo = a.gastar ? "GASTAR" : a.comparador == Comparador.Exatamente ? "TER EXATAMENTE" : a.comparador == Comparador.MaiorQue ? "TER MAIS DE" : "TER NO MÍNIMO";
            Texto(pai, "PARA SUPERAR: " + verbo, 150, 59, 335, 20, 14, ouro);
            int linha = 0;
            void Requisito(string nome, int valor, int disponivel)
            {
                if (valor == 0) return;
                bool suficiente = a.gastar || a.comparador == Comparador.PeloMenos ? disponivel >= valor : a.comparador == Comparador.MaiorQue ? disponivel > valor : disponivel == valor;
                Texto(pai, $"{valor} {nome}  ·  atual {disponivel}", 150, 81 + linha * 21, 335, 23, 18, suficiente ? positivo : Color.white);
                linha++;
            }
            Requisito("Saúde", a.requisito.saude, atual.saude);
            Requisito("Infraestrutura", a.requisito.infraestrutura, atual.infraestrutura);
            Requisito("Pesquisa", a.requisito.pesquisa, atual.pesquisa);
            if (linha == 0) Texto(pai, "Nenhum recurso exigido", 150, 81, 335, 23, 18, positivo);
        }

        void PenalidadeAmeaca(Transform pai, AmeacaAtiva a)
        {
            var p = PartidaDiretor.Instance; var efeitos = EfeitosCartasDiretor.Instance;
            var delta = efeitos.Penalidade(a);
            string titulo = "SE PERSISTIR: PERDA A CADA RODADA";
            string valor = ResumoRecursos(delta);
            Color cor = perda;
            if (a.bloqueadaAte >= p.Rodada)
            {
                titulo = "PENALIDADE BLOQUEADA ATÉ R" + a.bloqueadaAte;
                valor = "Sem perda nesta rodada"; cor = positivo;
            }
            else if (a.convertidaNaRodada == p.Rodada)
            {
                titulo = "SE PERSISTIR: CONVERSÃO NESTA RODADA";
                if (delta.Equals(default(Recursos))) valor = "Sem ganho nem perda";
                cor = positivo;
            }
            else if (efeitos.ReducaoPenalidade > 0)
            {
                titulo = "SE PERSISTIR: PERDA REDUZIDA AGORA";
                if (delta.Equals(default(Recursos))) { valor = "Sem perda nesta rodada"; cor = positivo; }
            }
            Texto(pai, titulo, 150, 150, 335, 20, 14, cor);
            Texto(pai, valor, 150, 172, 335, 42, 18, cor);
        }

        static string ResumoRecursos(Recursos r)
        {
            var partes = new List<string>();
            void Adicionar(string nome, int valor) { if (valor != 0) partes.Add((valor > 0 ? "+" : "−") + Math.Abs(valor) + " " + nome); }
            Adicionar("Saúde", r.saude); Adicionar("Infraestrutura", r.infraestrutura); Adicionar("Pesquisa", r.pesquisa);
            if (partes.Count == 3) return partes[0] + " · " + partes[1] + "\n" + partes[2];
            return partes.Count == 0 ? "Sem perda de recursos" : string.Join(" · ", partes);
        }

        string OrientacaoRodada()
        {
            var p = PartidaDiretor.Instance;
            if (p.Fase == Fase.Ameacas) return "Verificando ameaças — aguarde a resolução antes de planejar a próxima rodada.";
            if (p.Fase == Fase.EscolhaExtra) return $"J{p.JogadorExtra + 1}: escolha até {p.ExtrasRestantes} cartas extras ou clique em Concluir extras.";
            if (p.Fase != Fase.Planejamento) return "Aguarde: as cartas estão sendo executadas na ordem da fila.";
            var jogadores = JogadoresDiretor.Instance.Jogadores;
            if (jogadores.Any(j => j.confirmado)) return $"J{jogadores.First(j => !j.confirmado).indice + 1}: escolha suas cartas e confirme. A outra cientista já confirmou.";
            return $"1. Escolham até {p.Regras.limiteSelecao} cartas cada   →   2. Confirmem as duas mãos   →   3. Acompanhem a resolução";
        }

        static string ExibirFase(Fase f)
        {
            switch (f)
            {
                case Fase.Planejamento: return "PLANEJAMENTO";
                case Fase.Execucao: return "EXECUÇÃO";
                case Fase.Ameacas: return "AMEAÇAS";
                case Fase.EscolhaExtra: return "CARTA EXTRA";
                default: return f.ToString().ToUpperInvariant();
            }
        }

        void Mao(int indice, float x)
        {
            var p = PartidaDiretor.Instance; var j = JogadoresDiretor.Instance.Jogadores[indice]; var plan = PlanejamentoDiretor.Instance;
            Imagem(raiz, j.cientista == Cientista.MarieCurie ? arte?.retratoMarie : arte?.retratoJaqueline, x + 8, 577, 55, 62);
            Texto(raiz, $"J{indice + 1} · {Nome(j.cientista)}", x + 74, 583, 370, 30, 24, ouro);
            Texto(raiz, $"Conhecimento {j.conhecimento - plan.Reservado(indice)}/{p.Regras.conhecimento}  ·  Baralho {j.Baralho.Count}  ·  Descarte {j.Descarte.Count}", x + 74, 615, 650, 25, 17);
            bool extra = p.Fase == Fase.EscolhaExtra && p.JogadorExtra == indice;
            int selecionadas = plan.Fila.Count(c => c.jogador == indice);
            string confirmar = selecionadas == 0 ? "Passar sem jogar" : $"Confirmar {selecionadas} carta{(selecionadas == 1 ? "" : "s")}";
            Botao(extra ? $"Concluir extras ({p.ExtrasRestantes})" : j.confirmado ? "✓ Confirmado" : confirmar, x + 460, 582, 265, 35, () => { if (extra) p.FinalizarExtras(); else plan.Confirmar(indice); }, extra || (p.Fase == Fase.Planejamento && !j.confirmado));
            for (int n = 0; n < j.Mao.Count; n++)
            {
                var c = j.Mao[n]; int ordem = plan.Posicao(c, indice); float cx = x + 15 + n * 120; float cy = 655 - (ordem > 0 ? 8 : 0);
                var b = Botao((c.cientista != Cientista.Normal ? "★ " : "") + c.nome, cx, cy, 105, 157, () => { if (extra) p.EscolherExtra(c, alvo); else plan.Alternar(indice, c, alvo); }, (extra && ordem == 0) || (p.Fase == Fase.Planejamento && !j.confirmado), ordem > 0, () => { if (plan.Posicao(c, indice) > 0) plan.Alternar(indice, c, alvo); }, config.Imagem(c.id));
                var interacao = b.GetComponent<AnimacaoInteracao>(); interacao.carta = true; var focoAnterior = interacao.aoFocar; interacao.aoFocar = () => { focoAnterior?.Invoke(); Inspecionar(c); };
                if (ordem > 0)
                {
                    Imagem(b.transform, null, 77, 127, 26, 27, new Color(.07f, .08f, .18f, .96f));
                    Texto(b.transform, ordem.ToString(), 77, 127, 26, 27, 21, ouro, TextAlignmentOptions.Center);
                }
                Texto(raiz, c.nome, cx - 3, 817, 112, 30, 13, null, TextAlignmentOptions.Center);
                string chave = indice + ":" + c.id; posicoesCartas[chave] = new Vector2(cx + 52.5f, 1000 - cy - 78.5f);
                if (!maoAnterior.Contains(chave) && !EfeitosVisuaisDiretor.Instance.MovimentoReduzido)
                {
                    var rt = (RectTransform)b.transform; Vector2 fim = rt.anchoredPosition; rt.anchoredPosition = fim + new Vector2(0, -35);
                    rt.DOAnchorPos(fim, .28f).SetDelay(n * .035f).SetEase(Ease.OutCubic).SetUpdate(true).SetLink(b.gameObject);
                }
            }
        }

        void Inspecionar(CartaDados c)
        {
            cartaInspecionada = c; if (tituloDetalhe == null) return;
            tituloDetalhe.text = c.nome + "  ·  Conhecimento " + c.conhecimento + (c.cientista != Cientista.Normal ? "  ·  ESPECIAL" : ""); textoDetalhe.text = c.descricao;
            if (!c.custo.Equals(default(Recursos))) textoDetalhe.text += "  Custo adicional: " + c.custo;
            imagemDetalhe.sprite = config.Imagem(c.id); imagemDetalhe.enabled = imagemDetalhe.sprite != null;
        }

        public Vector2 PosicaoCarta(int j, string id) => posicoesCartas.TryGetValue(j + ":" + id, out var p) ? p : new Vector2(j == 0 ? 390 : 1180, 300);
        public Vector2 PosicaoAmeaca(string id) => posicoesAmeacas.TryGetValue(id, out var p) ? p : new Vector2(800, 730);

        void Cabecalho(string titulo)
        {
            Painel(170, 100, 1260, 790); Texto(raiz, titulo, 230, 145, 1140, 65, 40, ouro, TextAlignmentOptions.Center);
        }

        void PausaTela()
        {
            Cabecalho("PARTIDA PAUSADA"); Botao("Continuar", 430, 300, 740, 66, () => PartidaDiretor.Instance.Pausar()); Botao("Opções", 430, 410, 740, 66, () => opcoes = true); Botao("Reiniciar partida", 430, 520, 740, 66, Iniciar); Botao("Menu principal", 430, 630, 740, 66, () => PartidaDiretor.Instance.Menu());
        }

        void Fim()
        {
            var p = PartidaDiretor.Instance; bool venceu = p.Fase == Fase.Vitoria;
            Cabecalho(venceu ? "CRISE SUPERADA" : "A CRISE AVANÇOU"); Imagem(raiz, venceu ? arte?.vitoria : arte?.derrota, 540, 240, 520, 110); Imagem(raiz, venceu ? arte?.trofeu : arte?.trofeuQuebrado, 665, 365, 270, 270);
            Texto(raiz, $"{p.Mensagem}\nRodadas: {p.Rodada} · Ameaças superadas: {AmeacasDiretor.Instance.Cemiterio.Count}/15", 285, 665, 1030, 90, 25, null, TextAlignmentOptions.Center);
            Botao("Jogar novamente", 300, 790, 470, 55, Iniciar); Botao("Menu principal", 830, 790, 470, 55, () => p.Menu());
        }

        void Opcoes()
        {
            Cabecalho("OPÇÕES"); var a = AudioPartidaDiretor.Instance;
            Texto(raiz, $"Música: {a.VolumeMusica:P0}", 300, 285, 480, 50, 28); Botao("−", 910, 280, 120, 55, () => a.Ajustar(true, -.1f)); Botao("+", 1080, 280, 120, 55, () => a.Ajustar(true, .1f));
            Texto(raiz, $"Efeitos: {a.VolumeEfeitos:P0}", 300, 395, 480, 50, 28); Botao("−", 910, 390, 120, 55, () => a.Ajustar(false, -.1f)); Botao("+", 1080, 390, 120, 55, () => a.Ajustar(false, .1f));
            Botao("Tela cheia: " + (Screen.fullScreen ? "sim" : "não"), 300, 510, 1000, 55, () => Screen.fullScreen = !Screen.fullScreen);
            Botao("Movimento reduzido: " + (EfeitosVisuaisDiretor.Instance.MovimentoReduzido ? "sim" : "não"), 300, 610, 1000, 55, () => EfeitosVisuaisDiretor.Instance.AlternarMovimento());
            Botao("Voltar", 680, 740, 240, 65, () => opcoes = false, sprite: arte?.voltar);
        }

        void Tutorial()
        {
            Cabecalho("COMO JOGAR"); Texto(raiz, $"1. Vejam o requisito de cada ameaça. TER não consome recursos; GASTAR consome o valor indicado quando ela é superada.\n\n2. Cada cientista escolhe até {config.regras.limiteSelecao} cartas e confirma sua mão. Cliquem numa ameaça antes de escolher uma carta com alvo.\n\n3. Após a fila, as ameaças são verificadas automaticamente, da mais antiga à mais nova. Os recursos podem mudar entre elas.\n\n4. Cada ameaça que continuar ativa aplica sua penalidade por rodada. Superem todas sem deixar nenhum recurso zerar.\n\nMouse ou WASD/setas: navegar · Enter/Z/A: selecionar\nX/Backspace/B: cancelar · Tab/RB: informações · Esc/Menu: pausa", 260, 250, 1080, 510, 26);
            Botao("Voltar", 560, 780, 480, 60, () => tutorial = false);
        }

        void Detalhes()
        {
            Cabecalho("CADERNO DA PARTIDA"); Botao("Mãos / descarte", 220, 230, 360, 50, () => aba = 0, true, aba == 0); Botao("Ameaças superadas", 620, 230, 360, 50, () => aba = 1, true, aba == 1); Botao("Histórico", 1020, 230, 360, 50, () => aba = 2, true, aba == 2);
            string conteudo = "";
            if (aba == 0)
            {
                foreach (var j in JogadoresDiretor.Instance.Jogadores) conteudo += $"J{j.indice + 1} — {Nome(j.cientista)}\nMão: {string.Join(", ", j.Mao.Select(x => x.nome))}\nDescarte: {string.Join(", ", j.Descarte.Select(x => x.nome))}\nBaralho: {j.Baralho.Count} cartas (ordem oculta).\n\n";
                conteudo += "Próxima ameaça: " + (EfeitosCartasDiretor.Instance.Espiando ? (AmeacasDiretor.Instance.Proxima?.nome ?? "Baralho esgotado") : "Use Monitoramento de Casos para revelar.");
            }
            else if (aba == 1) conteudo = string.Join("\n", AmeacasDiretor.Instance.Cemiterio.Select((x, i) => (i + 1) + ". " + x.nome));
            else conteudo = string.Join("\n", PartidaDiretor.Instance.Historico.Skip(Math.Max(0, PartidaDiretor.Instance.Historico.Count - 15)));
            var t = Texto(raiz, conteudo, 230, 320, 1140, 445, 23);
            t.enableAutoSizing = true; t.fontSizeMin = 17; t.fontSizeMax = 23;
            Botao("Voltar à mesa", 560, 800, 480, 55, () => detalhes = false);
        }

        public void Navegar(int delta) { if (botoes.Count == 0) return; foco = (foco + delta % botoes.Count + botoes.Count) % botoes.Count; EventSystem.current.SetSelectedGameObject(botoes[foco].gameObject); }

        public void Acionar() { if (!suja && botoes.Count > 0) botoes[Mathf.Clamp(foco, 0, botoes.Count - 1)].onClick.Invoke(); }

        public void Cancelar()
        {
            if (opcoes) opcoes = false;
            else if (tutorial) tutorial = false;
            else if (detalhes) detalhes = false;
            else if (PartidaDiretor.Instance.Pausada) PartidaDiretor.Instance.Pausar();
            else if (botoes.Count > 0 && cancelamentos.TryGetValue(botoes[Mathf.Clamp(foco, 0, botoes.Count - 1)], out var acao)) acao(); suja = true;
        }

        public void Pausa() { if (opcoes) opcoes = false; else if (tutorial) tutorial = false; else if (detalhes) detalhes = false; else PartidaDiretor.Instance.Pausar(); suja = true; }

        public void Informacoes() { if (PartidaDiretor.Instance.Fase == Fase.Menu || PartidaDiretor.Instance.Pausada || PartidaDiretor.Instance.Terminou || opcoes) return; if (!detalhes) detalhes = true; else if (aba < 2) aba++; else { aba = 0; detalhes = false; } suja = true; }

        public void Especial() { int n = botoes.FindIndex(b => b.name.Contains("★")); if (n >= 0) { foco = n; EventSystem.current.SetSelectedGameObject(botoes[n].gameObject); } }

        static Cientista Outra(Cientista c) => c == Cientista.MarieCurie ? Cientista.JaquelineGoes : Cientista.MarieCurie;
        static string Nome(Cientista c) => c == Cientista.MarieCurie ? "Marie Curie" : "Jaqueline Goes";

        static void Sair()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}