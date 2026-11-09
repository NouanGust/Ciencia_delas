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
        [SerializeField] private ConfiguracaoJogo config;
        [SerializeField] private Canvas canvas;
        [SerializeField] private TooltipDiretor dicas;
        [SerializeField] private RectTransform conteudo, camadaEfeitos;
        [SerializeField] private List<TelaEditavel> telas = new List<TelaEditavel>();
        [Serializable] public class TelaEditavel { public string nome; public RectTransform raiz; }
        private readonly Dictionary<string, ElementoInterface> elementos = new Dictionary<string, ElementoInterface>();
        private readonly HashSet<ElementoInterface> usados = new HashSet<ElementoInterface>();
        private bool construindo, reposicionarMesa;
        private string telaAtual;
        private bool Reduzido => !Application.isPlaying || EfeitosVisuaisDiretor.Instance == null || EfeitosVisuaisDiretor.Instance.MovimentoReduzido;
        public IReadOnlyList<TelaEditavel> Telas => telas;
        public RectTransform Conteudo => conteudo;
        private AparenciaJogo arte => config.aparencia;
        private RectTransform raiz;
        private TMP_FontAsset fonte;
        private bool suja = true, opcoes, detalhes, tutorial;
        private int aba, foco;
        private Cientista primeira = Cientista.MarieCurie;
        private AmeacaAtiva alvo;
        private readonly List<Button> botoes = new List<Button>();
        private readonly List<Button>[] botoesJogador = { new List<Button>(), new List<Button>() };
        private readonly Button[] focosJogador = new Button[2];
        private readonly Dictionary<Button, Image> cursoresJogador = new Dictionary<Button, Image>();
        public bool MesaAberta => telaAtual == "Mesa" && !opcoes && !detalhes && !tutorial &&
            !PartidaDiretor.Instance.Pausada && !PartidaDiretor.Instance.Terminou && PartidaDiretor.Instance.Fase != Fase.Menu;
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
            PrepararEstrutura(c);
            var eventos = GetComponentInChildren<EventSystem>(true);
            if (eventos != null) eventos.sendNavigationEvents = false;
            var efeitos = GetComponent<EfeitosVisuaisDiretor>();
            if (efeitos == null) efeitos = gameObject.AddComponent<EfeitosVisuaisDiretor>();
            efeitos.Inicializar(c, conteudo, camadaEfeitos);
            PartidaDiretor.Instance.Alterada -= Atualizar;
            PartidaDiretor.Instance.Alterada += Atualizar;
            telaAnterior = "";
            suja = true;
        }

        void PrepararEstrutura(ConfiguracaoJogo c)
        {
            config = c; fonte = c.fonte;
            if (canvas == null)
            {
                var go = new GameObject("Interface - Ciencia Delas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                go.transform.SetParent(transform, false);
                canvas = go.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                var scaler = go.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1600, 1000);
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                conteudo = Centro("Telas", canvas.transform);
                camadaEfeitos = Centro("Efeitos da partida", canvas.transform);
            }
            if (GetComponentInChildren<EventSystem>(true) == null && FindFirstObjectByType<EventSystem>() == null)
            {
                var entrada = new GameObject("Entrada UI", typeof(EventSystem), typeof(InputSystemUIInputModule));
                entrada.transform.SetParent(transform, false);
                entrada.GetComponent<EventSystem>().sendNavigationEvents = false;
                entrada.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();
            }
            elementos.Clear();
            foreach (var e in conteudo.GetComponentsInChildren<ElementoInterface>(true))
                if (!string.IsNullOrEmpty(e.Chave)) elementos[e.Chave] = e;
            PrepararTooltips();
        }

        void PrepararTooltips()
        {
            if (dicas != null) return;
            var camada = new GameObject("Tooltips", typeof(RectTransform));
            camada.transform.SetParent(canvas.transform, false);
            var area = (RectTransform)camada.transform;
            area.anchorMin = Vector2.zero; area.anchorMax = Vector2.one;
            area.offsetMin = area.offsetMax = Vector2.zero;
            dicas = camada.AddComponent<TooltipDiretor>();
            var fundo = new GameObject("Fundo da dica", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
            fundo.transform.SetParent(area, false);
            dicas.painel = (RectTransform)fundo.transform;
            dicas.painel.anchorMin = dicas.painel.anchorMax = new Vector2(.5f, .5f);
            dicas.painel.pivot = new Vector2(0, 1);
            fundo.GetComponent<Image>().color = new Color(.025f, .04f, .085f, .98f);
            fundo.GetComponent<Image>().raycastTarget = false;
            fundo.GetComponent<CanvasGroup>().blocksRaycasts = false;
            var label = new GameObject("Nome", typeof(RectTransform), typeof(TextMeshProUGUI));
            label.transform.SetParent(fundo.transform, false);
            dicas.texto = label.GetComponent<TextMeshProUGUI>();
            dicas.texto.font = fonte; dicas.texto.fontSize = 28; dicas.texto.color = Color.white;
            dicas.texto.raycastTarget = false; dicas.texto.richText = false;
            var r = dicas.texto.rectTransform; r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one;
            r.offsetMin = new Vector2(16, 12); r.offsetMax = new Vector2(-16, -12);
            dicas.Ocultar();
        }

        void PrepararDica(Graphic visual, string nome = "")
        {
            var dica = visual.GetComponent<DicaInterface>();
            if (dica == null)
            {
                dica = visual.gameObject.AddComponent<DicaInterface>();
                visual.raycastTarget = true;
            }
            dica.nome = nome;
            dica.tooltip = dicas;
        }

        string IconeRecurso(string nome)
            => config.iconesRecursos == null ? nome : $"<link=\"{nome}\"><sprite name=\"{nome}\" tint=0></link>";

        RectTransform Centro(string nome, Transform pai)
        {
            var r = new GameObject(nome, typeof(RectTransform)).GetComponent<RectTransform>();
            r.SetParent(pai, false); r.anchorMin = r.anchorMax = r.pivot = new Vector2(.5f, .5f);
            r.sizeDelta = new Vector2(1600, 1000); return r;
        }

        public void Atualizar() { suja = true; }
        void LateUpdate() { if (suja && config != null) { suja = false; Desenhar(); } }
        protected override void OnDestroy() { PartidaDiretor.Instance.Alterada -= Atualizar; base.OnDestroy(); }

        RectTransform Objeto(string nome, Transform pai, float x, float y, float w, float h, out bool novo)
        {
            var elementoPai = pai.GetComponent<ElementoInterface>();
            string chave = (elementoPai != null ? elementoPai.Chave : telaAtual) + "/" + nome;
            novo = !elementos.TryGetValue(chave, out var elemento) || elemento == null;
            if (novo)
            {
                var r = new GameObject(nome, typeof(RectTransform)).GetComponent<RectTransform>();
                r.SetParent(pai, false); r.anchorMin = r.anchorMax = new Vector2(0, 1);
                r.pivot = new Vector2(.5f, .5f); r.anchoredPosition = new Vector2(x + w / 2, -y - h / 2);
                r.sizeDelta = new Vector2(w, h);
                elemento = r.gameObject.AddComponent<ElementoInterface>(); elemento.Configurar(chave);
                elementos[chave] = elemento;
            }
            usados.Add(elemento);
            if (reposicionarMesa && telaAtual == "Mesa")
            {
                var r = (RectTransform)elemento.transform;
                r.anchorMin = r.anchorMax = new Vector2(0, 1); r.pivot = new Vector2(.5f, .5f);
                r.anchoredPosition = new Vector2(x + w / 2, -y - h / 2);
                r.sizeDelta = new Vector2(w, h);
            }
            return (RectTransform)elemento.transform;
        }

        Image Imagem(string nome, Transform pai, Sprite sprite, float x, float y, float w, float h, Color? cor = null, bool proporcao = true, bool dinamica = false)
        {
            var r = Objeto(nome, pai, x, y, w, h, out var novo);
            var e = r.GetComponent<ElementoInterface>();
            var i = r.GetComponent<Image>();
            if (i == null) i = r.gameObject.AddComponent<Image>();
            if (novo)
            {
                e.atualizarImagem = dinamica; i.sprite = sprite; i.color = cor ?? Color.white;
                i.preserveAspect = proporcao; i.raycastTarget = false;
            }
            if (e.atualizarImagem) i.sprite = sprite;
            return i;
        }
        Image ImagemVivo(string nome, Transform pai, Sprite sprite, float x, float y, float w, float h, Color? cor = null, bool proporcao = true)
            => Imagem(nome, pai, sprite, x, y, w, h, cor, proporcao, true);

        void Painel(string nome, float x, float y, float w, float h, Color? cor = null)
        {
            var i = Imagem(nome, raiz, null, x, y, w, h, cor ?? painel);
            if (i.GetComponent<Outline>() == null)
            {
                var borda = i.gameObject.AddComponent<Outline>();
                borda.effectColor = new Color(.48f, .55f, .8f, .65f); borda.effectDistance = new Vector2(1, -1);
            }
        }

        TextMeshProUGUI Texto(string nome, Transform pai, string valor, float x, float y, float w, float h, float tamanho = 22, Color? cor = null, TextAlignmentOptions alinhamento = TextAlignmentOptions.TopLeft, bool dinamico = false)
        {
            var r = Objeto(nome, pai, x, y, w, h, out var novo);
            var e = r.GetComponent<ElementoInterface>();
            var t = r.GetComponent<TextMeshProUGUI>();
            if (t == null) t = r.gameObject.AddComponent<TextMeshProUGUI>();
            if (novo || (reposicionarMesa && telaAtual == "Mesa"))
            {
                e.atualizarTexto = dinamico; e.atualizarCor = dinamico;
                t.text = valor; if (fonte != null) t.font = fonte;
                t.spriteAsset = config.iconesRecursos;
                t.fontSize = tamanho; t.color = cor ?? Color.white;
                t.raycastTarget = false; t.alignment = alinhamento;
                t.textWrappingMode = TextWrappingModes.Normal; t.overflowMode = TextOverflowModes.Truncate;
            }
            if (e.atualizarTexto) t.text = valor;
            if (e.atualizarCor) t.color = cor ?? Color.white;
            if (valor.Contains("<link=")) PrepararDica(t);
            return t;
        }
        TextMeshProUGUI TextoVivo(string nome, Transform pai, string valor, float x, float y, float w, float h, float tamanho = 22, Color? cor = null, TextAlignmentOptions alinhamento = TextAlignmentOptions.TopLeft)
            => Texto(nome, pai, valor, x, y, w, h, tamanho, cor, alinhamento, true);

        Button Botao(string nome, string texto, float x, float y, float w, float h, Action acao, bool ativo = true, bool marcado = false, Action cancelar = null, Sprite sprite = null, bool dinamico = false)
        {
            var r = Objeto(nome, raiz, x, y, w, h, out var novo);
            var e = r.GetComponent<ElementoInterface>();
            var i = r.GetComponent<Image>();
            if (i == null) i = r.gameObject.AddComponent<Image>();
            var b = r.GetComponent<Button>();
            if (b == null) b = r.gameObject.AddComponent<Button>();
            var anim = r.GetComponent<AnimacaoInteracao>();
            if (anim == null) anim = r.gameObject.AddComponent<AnimacaoInteracao>();
            if (novo)
            {
                e.atualizarImagem = nome.StartsWith("Carta J", StringComparison.Ordinal);
                i.sprite = sprite != null ? sprite : arte?.botaoNormal;
                i.preserveAspect = sprite != null;
                i.color = sprite != null ? Color.white : new Color(.38f, .43f, .68f);
                if (nome.StartsWith("Ameaca ", StringComparison.Ordinal)) { i.sprite = null; i.color = painel; }
                b.targetGraphic = i;
                var cores = b.colors; cores.highlightedColor = new Color(1, 1, .8f); cores.selectedColor = new Color(1, 1, .8f);
                cores.disabledColor = new Color(.65f, .65f, .7f, .9f); b.colors = cores;
                anim.duracao = arte != null ? arte.duracaoHover : .16f;
            }
            if (e.atualizarImagem) i.sprite = sprite;
            b.interactable = ativo;
            var legenda = Texto("Legenda", r, texto, 8, 5, w - 16, h - 10, 20, null, TextAlignmentOptions.Center, dinamico);
            var el = legenda.GetComponent<ElementoInterface>();
            if (novo) el.ocultar = sprite != null && !e.atualizarImagem;
            if (e.atualizarImagem && sprite != null) usados.Remove(el);
            var outline = r.GetComponent<Outline>();
            if (outline == null)
            {
                outline = r.gameObject.AddComponent<Outline>(); outline.effectColor = ouro; outline.effectDistance = new Vector2(3, -3);
            }
            outline.enabled = marcado;
            if (e.clique != null) b.onClick.RemoveListener(e.clique);
            e.clique = () => { AudioPartidaDiretor.Instance?.Som(); acao(); suja = true; };
            b.onClick.AddListener(e.clique);
            anim.aoFocar = null;
            if (ativo && !e.ocultar)
            {
                botoes.Add(b); if (cancelar != null) cancelamentos[b] = cancelar;
                anim.aoFocar = () => { int index = botoes.IndexOf(b); if (index >= 0) foco = index; };
            }
            return b;
        }
        Button BotaoVivo(string nome, string texto, float x, float y, float w, float h, Action acao, bool ativo = true, bool marcado = false, Action cancelar = null, Sprite sprite = null)
            => Botao(nome, texto, x, y, w, h, acao, ativo, marcado, cancelar, sprite, true);

        Vector2 PosicaoNoPalco(RectTransform r)
        {
            Vector3 local = camadaEfeitos.InverseTransformPoint(r.TransformPoint(r.rect.center));
            return (Vector2)local - camadaEfeitos.rect.min;
        }

#if UNITY_EDITOR
        public void AplicarLayoutCarrossel(ConfiguracaoJogo c)
        {
            reposicionarMesa = true;
            try { ConstruirCenaEditavel(c); }
            finally { reposicionarMesa = false; }
        }

        // Explicit editor command: never reconstructs the saved interface on entering Play.
        public void ConstruirCenaEditavel(ConfiguracaoJogo c)
        {
            if (Application.isPlaying) throw new InvalidOperationException("Saia do modo de jogo antes de preparar a cena.");
            PrepararEstrutura(c); construindo = true;
            alvo = null; maoAnterior.Clear();
            var p = PartidaDiretor.Instance;
            try
            {
                p.Iniciar(primeira, c.regras, c.cartas, c.ameacas, 123);
                foreach (var j in JogadoresDiretor.Instance.Jogadores)
                    JogadoresDiretor.Instance.Comprar(j, Math.Max(0, c.regras.limiteMao - j.Mao.Count));
                AmeacasDiretor.Instance.Revelar(); AmeacasDiretor.Instance.Revelar();
                foreach (string tela in new[] { "Menu", "Mesa", "Pausa", "Opcoes", "Como jogar", "Caderno", "Vitoria", "Derrota" }) DesenharTela(tela);
                construindo = false;
                DesenharTela("Mesa");
            }
            finally
            {
                construindo = false; p.Menu(); suja = true; telaAnterior = "";
                MostrarTelaNoEditor("Mesa");
            }
        }

        public void MostrarTelaNoEditor(string nome)
        {
            if (Application.isPlaying) return;
            foreach (var t in telas) if (t.raiz != null) t.raiz.gameObject.SetActive(t.nome == nome);
        }
#endif

        void Fundo(bool menu)
        {
            var sprite = arte != null ? (menu ? arte.fundoMenu : arte.fundoMesa) : config.fundo;
            Imagem("Cor do fundo", raiz, null, 0, 0, 1600, 1000, config.fundoCalmo);
            if (sprite != null) { float largura = Mathf.Max(1600, 1000 * sprite.rect.width / sprite.rect.height); Imagem("Cenario", raiz, sprite, (1600 - largura) / 2, 0, largura, 1000, Color.white); }
        }

        void Desenhar()
        {
            var p = PartidaDiretor.Instance;
            string tela = opcoes ? "Opcoes" : tutorial ? "Como jogar" : p.Terminou ? (p.Fase == Fase.Vitoria ? "Vitoria" : "Derrota") : p.Pausada ? "Pausa" : detalhes ? "Caderno" : p.Fase == Fase.Menu ? "Menu" : "Mesa";
            DesenharTela(tela);
        }

        void DesenharTela(string tela)
        {
            bool mesmaTela = tela == telaAnterior;
            var botaoEmFoco = mesmaTela && botoes.Count > 0 ? botoes[Mathf.Clamp(foco, 0, botoes.Count - 1)] : null;
            telaAtual = tela;
            var vista = telas.Find(t => t.nome == tela);
            if (vista == null)
            {
                vista = new TelaEditavel { nome = tela, raiz = Centro(tela, conteudo) };
                telas.Add(vista);
            }
            raiz = vista.raiz;
            if (tela != telaAnterior) dicas?.Ocultar();
            foreach (var t in telas) t.raiz.gameObject.SetActive(t == vista);
            usados.Clear(); botoes.Clear(); cancelamentos.Clear();
            foreach (var lista in botoesJogador) lista.Clear();
            cursoresJogador.Clear();
            Fundo(tela == "Menu");
            switch (tela)
            {
                case "Opcoes": Opcoes(); break;
                case "Como jogar": Tutorial(); break;
                case "Vitoria": case "Derrota": Fim(); break;
                case "Pausa": PausaTela(); break;
                case "Caderno": Detalhes(); break;
                case "Menu": Menu(); break;
                default: Mesa(); break;
            }
            foreach (var e in elementos.Values)
                if (e != null && e.Chave.StartsWith(tela + "/", StringComparison.Ordinal))
                    e.gameObject.SetActive(usados.Contains(e) && !e.ocultar);
            if (tela != telaAnterior) foco = 0;
            telaAnterior = tela;
            if (tela == "Mesa") AtualizarFocosJogadores();
            if (Application.isPlaying && botoes.Count > 0 && EventSystem.current != null)
            {
                // Disabled/removed controls must not shift focus into the other player's hand.
                if (mesmaTela && botaoEmFoco != null)
                {
                    int indice = botoes.IndexOf(botaoEmFoco);
                    if (indice < 0) { EventSystem.current.SetSelectedGameObject(null); foco = 0; return; }
                    foco = indice;
                }
                foco = Mathf.Clamp(foco, 0, botoes.Count - 1);
                EventSystem.current.SetSelectedGameObject(botoes[foco].gameObject);
            }
        }

        void Menu()
        {
            Texto("Titulo", raiz, "CIÊNCIA DELAS", 120, 65, 1360, 90, 64, ouro, TextAlignmentOptions.Center);
            bool marieEsq = primeira == Cientista.MarieCurie;
            ImagemVivo("Cientista J1", raiz, marieEsq ? arte?.marie : arte?.jaqueline, 60, 260, 440, 490);
            ImagemVivo("Cientista J2", raiz, marieEsq ? arte?.jaqueline : arte?.marie, 1100, 260, 440, 490);
            Texto("Rotulo J1", raiz, "J1", 110, 268, 340, 30, 22, new Color(.6f, .85f, 1f), TextAlignmentOptions.Center);
            Texto("Rotulo J2", raiz, "J2", 1150, 268, 340, 30, 22, new Color(.6f, .85f, 1f), TextAlignmentOptions.Center);
            TextoVivo("Nome J1", raiz, marieEsq ? "Marie Curie" : "Jaqueline Goes", 110, 770, 340, 45, 30, ouro, TextAlignmentOptions.Center);
            TextoVivo("Nome J2", raiz, marieEsq ? "Jaqueline Goes" : "Marie Curie", 1150, 770, 340, 45, 30, ouro, TextAlignmentOptions.Center);
            Botao("Iniciar partida", "Iniciar partida", 605, 280, 390, 108, Iniciar, sprite: arte?.iniciar);
            Botao("Opcoes", "Opções", 605, 415, 390, 100, () => opcoes = true, sprite: arte?.opcoes);
            Botao("Como jogar", "Como jogar", 605, 545, 390, 85, () => tutorial = true, sprite: arte?.tutorial);
            Botao("Sair", "Sair", 605, 670, 390, 100, Sair, sprite: arte?.sair);
            BotaoVivo("Trocar cientistas", $"J1: {Nome(primeira)}    ↔    J2: {Nome(Outra(primeira))}", 300, 860, 1000, 56, () => primeira = Outra(primeira));
        }

        void Iniciar()
        {
            opcoes = detalhes = tutorial = false; alvo = null; foco = 0; maoAnterior.Clear();
            focosJogador[0] = focosJogador[1] = null;
            try { PartidaDiretor.Instance.Iniciar(primeira, config.regras, config.cartas, config.ameacas, Environment.TickCount); }
            catch (Exception e) { Debug.LogException(e); PartidaDiretor.Instance.Notificar(e.Message); }
        }

        void Mesa()
        {
            var p = PartidaDiretor.Instance; var r = RecursosDiretor.Instance;
            Recurso(445, arte?.saude, "SAÚDE", r.Atual.saude, r.Reservado.saude, new Color(1, .45f, .5f));
            Recurso(595, arte?.pesquisa, "PESQUISA", r.Atual.pesquisa, r.Reservado.pesquisa, new Color(.4f, 1, .7f));
            Recurso(745, arte?.infraestrutura, "INFRAESTRUTURA", r.Atual.infraestrutura, r.Reservado.infraestrutura, new Color(.8f, .78f, 1));
            var pausa = Botao("Pausa", "Pausa", 1485, 32, 78, 78, () => p.Pausar(), sprite: arte?.pausa);
            var livro = Botao("Informacoes", "Caderno", 1485, 140, 78, 78, () => detalhes = true, sprite: arte?.livro);
            PrepararDica(pausa.image, "Pausar partida"); PrepararDica(livro.image, "Caderno da partida");
            Tecla("Pausa", "ESC", 1405, 56);
            Tecla("Caderno", "TAB", 1405, 164);
            var am = AmeacasDiretor.Instance;
            TextoVivo("Rodada e fase", raiz, $"RODADA {p.Rodada:00}  ·  {ExibirFase(p.Fase)}", 450, 30, 700, 50, 32, ouro, TextAlignmentOptions.Center);
            TextoVivo("Objetivo", raiz, $"{am.Cemiterio.Count}/{config.ameacas.Count}\nsuperadas", 1445, 260, 150, 86, 26, null, TextAlignmentOptions.Center);
            CarrosselAmeacas();

            Painel("Painel da fila", 210, 452, 1180, 155);
            elementos["Mesa/Painel da fila"].transform.SetSiblingIndex(3);
            Texto("Rotulo da fila", raiz, "FILA DE CARTAS", 235, 465, 1125, 38, 26, ouro, TextAlignmentOptions.Center);
            string fila = string.Join("  →  ", PlanejamentoDiretor.Instance.Fila.Select((j, n) => $"{n + 1}. J{j.jogador + 1} {j.carta.nome}"));
            var tf = TextoVivo("Fila de cartas", raiz, fila.Length == 0 ? "Escolha as cartas para montar a fila. Escolha novamente para retirar." : fila, 240, 505, 1120, 60, 24, null, TextAlignmentOptions.Center);
            if (construindo) { tf.enableAutoSizing = true; tf.fontSizeMin = 19; tf.fontSizeMax = 24; }
            TextoVivo("Orientacao da rodada", raiz, OrientacaoRodada(), 240, 575, 1120, 28, 18, ouro, TextAlignmentOptions.Center);
            ComandosJogador(0, 195); ComandosJogador(1, 895);
            Painel("Painel das maos", 170, 665, 1395, 294);
            elementos["Mesa/Painel das maos"].transform.SetSiblingIndex(3);
            posicoesCartas.Clear(); Mao(0, 180); Mao(1, 880);
            var atuais = new HashSet<string>(); foreach (var j in JogadoresDiretor.Instance.Jogadores) foreach (var c in j.Mao) atuais.Add(j.indice + ":" + c.id); maoAnterior.Clear(); maoAnterior.UnionWith(atuais);
            TextoVivo("Mensagem da partida", raiz, p.Mensagem, 180, 961, 1380, 36, 22, new Color(.65f, 1, .85f));
        }

        void CarrosselAmeacas()
        {
            var am = AmeacasDiretor.Instance;
            posicoesAmeacas.Clear();
            if (alvo == null || !am.Contem(alvo)) alvo = am.Ativas.FirstOrDefault();
            int indice = alvo == null ? 0 : am.Ativas.ToList().IndexOf(alvo);
            if (alvo != null)
            {
                var b = Botao("Ameaca 1", "", 550, 100, 500, 280, () => { }, true, true);
                var imagem = config.Imagem(alvo.dados.id);
                ImagemVivo("Ilustracao", b.transform, imagem != null ? imagem : arte?.trofeuQuebrado, 12, 44, 125, 150);
                var nome = TextoVivo("Nome", b.transform, alvo.dados.nome, 150, 8, 335, 72, 25, ouro);
                if (construindo) { nome.enableAutoSizing = true; nome.fontSizeMin = 18; nome.fontSizeMax = 25; }
                CondicaoAmeaca(b.transform, alvo); PenalidadeAmeaca(b.transform, alvo);
                bool atendida = am.PodeSuperar(alvo.dados);
                TextoVivo("Situacao", b.transform, (atendida ? "Atendida" : "Não atendida") + (alvo.dados.gastar ? " · consome" : ""), 150, 242, 335, 32, 22, atendida ? positivo : ouro);
                Texto("Indicador de alvo", b.transform, "ALVO", 12, 242, 125, 32, 22, ouro, TextAlignmentOptions.Center);
                posicoesAmeacas[alvo.dados.id] = PosicaoNoPalco((RectTransform)b.transform);
                if (am.Ativas.Count > 2) PreviaAmeaca("Ameaca 2", am.Ativas[(indice + am.Ativas.Count - 1) % am.Ativas.Count], 260, -1);
                if (am.Ativas.Count > 1) PreviaAmeaca("Ameaca 3", am.Ativas[(indice + 1) % am.Ativas.Count], 1090, 1);
            }
            if (construindo || am.Ativas.Count == 0)
                Texto("Sem ameacas", raiz, "Nenhuma ameaça ativa", 550, 210, 500, 60, 30, ouro, TextAlignmentOptions.Center);
            Botao("Ameaca anterior", "←", 630, 396, 64, 42, () => MudarAmeaca(-1), am.Ativas.Count > 1);
            TextoVivo("Posicao no carrossel", raiz, am.Ativas.Count == 0 ? "0 / 0" : $"{indice + 1} / {am.Ativas.Count} ativas", 704, 396, 192, 42, 23, null, TextAlignmentOptions.Center);
            Botao("Mais ameacas", "→", 906, 396, 64, 42, () => MudarAmeaca(1), am.Ativas.Count > 1);
            Tecla("Ameaca anterior", "Q", 586, 401);
            Tecla("Proxima ameaca", "E", 982, 401);
        }

        void PreviaAmeaca(string nome, AmeacaAtiva a, float x, int direcao)
        {
            var b = Botao(nome, "", x, 130, 250, 240, () => MudarAmeaca(direcao));
            var imagem = config.Imagem(a.dados.id);
            ImagemVivo("Ilustracao", b.transform, imagem != null ? imagem : arte?.trofeuQuebrado, 12, 20, 60, 70);
            var titulo = TextoVivo("Nome", b.transform, a.dados.nome, 82, 10, 156, 96, 20, ouro, TextAlignmentOptions.Left);
            if (construindo) { titulo.enableAutoSizing = true; titulo.fontSizeMin = 16; titulo.fontSizeMax = 20; }
            CondicaoAmeaca(b.transform, a, true);
            bool atendida = AmeacasDiretor.Instance.PodeSuperar(a.dados);
            TextoVivo("Situacao", b.transform, atendida ? "Atendida" : "Não atendida", 12, 198, 226, 32, 20, atendida ? positivo : ouro, TextAlignmentOptions.Center);
            posicoesAmeacas[a.dados.id] = PosicaoNoPalco((RectTransform)b.transform);
        }

        void MudarAmeaca(int delta)
        {
            var ativas = AmeacasDiretor.Instance.Ativas;
            if (ativas.Count == 0) { alvo = null; return; }
            int indice = Mathf.Max(0, ativas.ToList().IndexOf(alvo));
            alvo = ativas[(indice + delta % ativas.Count + ativas.Count) % ativas.Count];
            dicas?.Ocultar();
            suja = true;
        }

        void Recurso(float y, Sprite icone, string nome, int valor, int reservado, Color cor)
        {
            var imagem = Imagem("Icone " + nome, raiz, icone, 44, y, 72, 72);
            PrepararDica(imagem, nome == "SAÚDE" ? "Saúde" : nome == "INFRAESTRUTURA" ? "Infraestrutura" : "Pesquisa");
            string linhaValor = $"{valor}/{config.regras.maximoRecurso}" + (reservado > 0 ? $"  −{reservado}" : "");
            TextoVivo("Valor " + nome, raiz, linhaValor, 10, y + 76, 140, 42, 27, cor, TextAlignmentOptions.Center);
        }

        void Tecla(string nome, string tecla, float x, float y)
        {
            var sprite = config.IconeTecla(tecla);
            Imagem("Tecla " + nome, raiz, sprite, x, y, LarguraTecla(tecla), 32);
            if (sprite == null) Texto("Tecla alternativa " + nome, raiz, tecla, x, y, 32, 32, 16, null, TextAlignmentOptions.Center);
        }

        float LarguraTecla(string tecla)
        {
            var sprite = config.IconeTecla(tecla);
            return sprite != null ? 32 * sprite.rect.width / sprite.rect.height : 32;
        }

        void ComandosJogador(int indice, float x)
        {
            string j = "J" + (indice + 1);
            Tecla(j + " anterior", indice == 0 ? "A" : "LEFT ARROW", x, 623);
            Tecla(j + " proxima", indice == 0 ? "D" : "RIGHT ARROW", x + 36, 623);
            Texto("Comando navegar " + j, raiz, "Navegar", x + 80, 624, 112, 30, 20);
            string selecionar = indice == 0 ? "Z" : "ENTER";
            Tecla(j + " selecionar", selecionar, x + 200, 623);
            float legenda = 210 + LarguraTecla(selecionar);
            Texto("Comando selecionar " + j, raiz, "Selecionar / confirmar", x + legenda, 624, 630 - legenda, 30, 20);
        }

        void CondicaoAmeaca(Transform pai, AmeacaAtiva ameaca, bool compacta = false)
        {
            var a = ameaca.dados; var atual = RecursosDiretor.Instance.Atual;
            string verbo = a.gastar ? "GASTAR" : a.comparador == Comparador.Exatamente ? "TER EXATAMENTE" : a.comparador == Comparador.MaiorQue ? "TER MAIS DE" : "TER NO MÍNIMO";
            float x = compacta ? 12 : 150, y = compacta ? 150 : 118;
            float largura = compacta ? 226 : 335, passo = compacta ? 76 : 110;
            TextoVivo("Tipo de requisito", pai, verbo, x, compacta ? 112 : 82, largura, 32, compacta ? 18 : 20, ouro);
            int linha = 0;
            void Requisito(string nome, int valor, int disponivel)
            {
                if (valor == 0 && !construindo) return;
                bool suficiente = a.gastar || a.comparador == Comparador.PeloMenos ? disponivel >= valor : a.comparador == Comparador.MaiorQue ? disponivel > valor : disponivel == valor;
                TextoVivo("Requisito " + linha, pai, $"{IconeRecurso(nome)} {valor}", x + linha * passo, y, passo, 40, compacta ? 23 : 28, suficiente ? positivo : Color.white);
                linha++;
            }
            Requisito("Saúde", a.requisito.saude, atual.saude);
            Requisito("Infraestrutura", a.requisito.infraestrutura, atual.infraestrutura);
            Requisito("Pesquisa", a.requisito.pesquisa, atual.pesquisa);
            if (construindo || linha == 0) Texto("Sem requisito", pai, "Nenhum recurso exigido", x, y, largura, 40, compacta ? 18 : 22, positivo);
        }

        void PenalidadeAmeaca(Transform pai, AmeacaAtiva a)
        {
            var p = PartidaDiretor.Instance; var efeitos = EfeitosCartasDiretor.Instance;
            var delta = efeitos.Penalidade(a);
            string titulo = "PERDA POR RODADA";
            string valor = ResumoRecursos(delta);
            Color cor = perda;
            if (a.bloqueadaAte >= p.Rodada)
            {
                titulo = "BLOQUEADA ATÉ R" + a.bloqueadaAte;
                valor = "Sem perda nesta rodada"; cor = positivo;
            }
            else if (a.convertidaNaRodada == p.Rodada)
            {
                titulo = "CONVERSÃO NESTA RODADA";
                if (delta.Equals(default(Recursos))) valor = "Sem ganho nem perda";
                cor = positivo;
            }
            else if (efeitos.ReducaoPenalidade > 0)
            {
                titulo = "PERDA REDUZIDA";
                if (delta.Equals(default(Recursos))) { valor = "Sem perda nesta rodada"; cor = positivo; }
            }
            TextoVivo("Tipo de penalidade", pai, titulo, 150, 163, 335, 30, 20, cor);
            TextoVivo("Penalidade", pai, valor, 150, 197, 335, 40, 26, cor);
        }

        string ResumoRecursos(Recursos r)
        {
            var partes = new List<string>();
            void Adicionar(string nome, int valor) { if (valor != 0) partes.Add(IconeRecurso(nome) + " " + (valor > 0 ? "+" : "−") + Math.Abs(valor)); }
            Adicionar("Saúde", r.saude); Adicionar("Infraestrutura", r.infraestrutura); Adicionar("Pesquisa", r.pesquisa);
            return partes.Count == 0 ? "Sem perda de recursos" : string.Join("  ", partes);
        }

        string OrientacaoRodada()
        {
            var p = PartidaDiretor.Instance;
            if (p.Fase == Fase.Ameacas) return "Verificando ameaças — aguarde a resolução antes de planejar a próxima rodada.";
            if (p.Fase == Fase.EscolhaExtra) return $"J{p.JogadorExtra + 1}: escolha até {p.ExtrasRestantes} cartas extras ou clique em Concluir extras.";
            if (p.Fase != Fase.Planejamento) return "Aguarde: as cartas estão sendo executadas na ordem da fila.";
            var jogadores = JogadoresDiretor.Instance.Jogadores;
            if (jogadores.Any(j => j.confirmado)) return $"J{jogadores.First(j => !j.confirmado).indice + 1}: escolha suas cartas e confirme. A outra cientista já confirmou.";
            return $"Escolham até {p.Regras.limiteSelecao} cartas cada  →  Confirmem as mãos  →  Acompanhem a resolução";
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
            ImagemVivo("Retrato J" + (indice + 1), raiz, j.cientista == Cientista.MarieCurie ? arte?.retratoMarie : arte?.retratoJaqueline, x + 8, 676, 46, 55);
            TextoVivo("Nome J" + (indice + 1), raiz, $"J{indice + 1} · {Nome(j.cientista)}", x + 62, 674, 350, 36, 24, ouro);
            TextoVivo("Baralho J" + (indice + 1), raiz, $"{IconeRecurso("Conhecimento")} {j.conhecimento - plan.Reservado(indice)}/{p.Regras.conhecimento}  ·  Baralho {j.Baralho.Count}  ·  Descarte {j.Descarte.Count}", x + 62, 716, 610, 34, 22);
            bool extra = p.Fase == Fase.EscolhaExtra && p.JogadorExtra == indice;
            int selecionadas = plan.Fila.Count(c => c.jogador == indice);
            string confirmar = selecionadas == 0 ? "Passar sem jogar" : $"Confirmar {selecionadas} carta{(selecionadas == 1 ? "" : "s")}";
            var confirmarMao = BotaoVivo("Confirmar J" + (indice + 1), extra ? $"Concluir extras ({p.ExtrasRestantes})" : j.confirmado ? "Confirmado" : confirmar, x + 420, 674, 245, 44, () => { if (extra) p.FinalizarExtras(); else plan.Confirmar(indice); }, extra || (p.Fase == Fase.Planejamento && !j.confirmado));
            for (int n = 0; n < j.Mao.Count; n++)
            {
                var c = j.Mao[n]; int ordem = plan.Posicao(c, indice); float cx = x + 15 + n * 109; float cy = 752;
                var b = BotaoVivo("Carta J" + (indice + 1) + " espaco " + (n + 1), (c.cientista != Cientista.Normal ? "★ " : "") + c.nome, cx, cy, 94, 141, () => { if (extra) p.EscolherExtra(c, alvo); else plan.Alternar(indice, c, alvo); }, (extra && ordem == 0) || (p.Fase == Fase.Planejamento && !j.confirmado), ordem > 0, () => { if (plan.Posicao(c, indice) > 0) plan.Alternar(indice, c, alvo); }, config.Imagem(c.id));
                var interacao = b.GetComponent<AnimacaoInteracao>(); interacao.carta = true; interacao.especial = c.cientista != Cientista.Normal; PrepararDica(b.image, DescricaoCarta(c)); b.GetComponent<DicaInterface>().mostrarAoSelecionar = true;
                RegistrarControleJogador(indice, b);
                if (construindo || ordem > 0)
                {
                    Imagem("Fundo da ordem", b.transform, null, 66, 112, 26, 27, new Color(.07f, .08f, .18f, .96f));
                    TextoVivo("Ordem", b.transform, ordem.ToString(), 66, 112, 26, 27, 20, ouro, TextAlignmentOptions.Center);
                }
                var legenda = TextoVivo("Legenda J" + (indice + 1) + " espaco " + (n + 1), raiz, c.nome, cx - 3, 899, 100, 56, 16, null, TextAlignmentOptions.Top);
                if (construindo) { legenda.enableAutoSizing = true; legenda.fontSizeMin = 12; legenda.fontSizeMax = 16; }
                string chave = indice + ":" + c.id; posicoesCartas[chave] = PosicaoNoPalco((RectTransform)b.transform);
                if (!maoAnterior.Contains(chave) && !Reduzido)
                {
                    var rt = (RectTransform)b.transform; Vector2 fim = rt.anchoredPosition; rt.anchoredPosition = fim + new Vector2(0, -35);
                    rt.DOAnchorPos(fim, .28f).SetDelay(n * .035f).SetEase(Ease.OutCubic).SetUpdate(true).SetLink(b.gameObject);
                }
            }
            RegistrarControleJogador(indice, confirmarMao);
        }

        void RegistrarControleJogador(int jogador, Button botao)
        {
            var r = (RectTransform)botao.transform;
            var cursor = Imagem("Cursor do jogador", r, null, 4, 4, r.rect.width - 8, r.rect.height - 8, Color.clear);
            var antigo = cursor.GetComponent<Outline>();
            if (antigo != null) antigo.enabled = false;
            var contorno = cursor.GetComponent<MolduraFoco>();
            if (contorno == null)
            {
                contorno = cursor.gameObject.AddComponent<MolduraFoco>();
                contorno.cor = jogador == 0 ? new Color(.3f, .85f, 1) : new Color(.85f, .6f, 1);
            }
            cursoresJogador[botao] = cursor;
            if (!botao.interactable || botao.GetComponent<ElementoInterface>().ocultar) return;
            botoesJogador[jogador].Add(botao);
            var interacao = botao.GetComponent<AnimacaoInteracao>(); var anterior = interacao.aoFocar;
            interacao.aoFocar = () => { anterior?.Invoke(); FocarJogador(jogador, botao, false); };
        }

        void AtualizarFocosJogadores()
        {
            for (int j = 0; j < 2; j++)
                if (!botoesJogador[j].Contains(focosJogador[j])) focosJogador[j] = botoesJogador[j].FirstOrDefault();
            AtualizarCursoresJogadores();
        }

        void AtualizarCursoresJogadores()
        {
            foreach (var par in cursoresJogador)
                par.Value.enabled = MesaAberta && (par.Key == focosJogador[0] || par.Key == focosJogador[1]);
        }

        bool PodeControlarJogador(int jogador)
        {
            if (!MesaAberta || jogador < 0 || jogador > 1) return false;
            var p = PartidaDiretor.Instance;
            return p.Fase == Fase.Planejamento ? !JogadoresDiretor.Instance.Jogadores[jogador].confirmado :
                p.Fase == Fase.EscolhaExtra && p.JogadorExtra == jogador;
        }

        void FocarJogador(int jogador, Button botao, bool selecionar)
        {
            if (!PodeControlarJogador(jogador) || !botoesJogador[jogador].Contains(botao)) return;
            focosJogador[jogador] = botao;
            AtualizarCursoresJogadores();
            if (selecionar && EventSystem.current != null) EventSystem.current.SetSelectedGameObject(botao.gameObject);
        }

        public void NavegarJogador(int jogador, int delta)
        {
            if (!PodeControlarJogador(jogador)) return;
            var lista = botoesJogador[jogador]; if (lista.Count == 0) return;
            int indice = Mathf.Max(0, lista.IndexOf(focosJogador[jogador]));
            FocarJogador(jogador, lista[(indice + delta % lista.Count + lista.Count) % lista.Count], true);
        }

        public void AcionarJogador(int jogador)
        {
            if (!PodeControlarJogador(jogador)) return;
            var botao = focosJogador[jogador];
            if (botao == null || !botao.IsActive() || !botao.IsInteractable()) return;
            FocarJogador(jogador, botao, true);
            botao.onClick.Invoke();
        }

        public void CancelarJogador(int jogador)
        {
            if (!PodeControlarJogador(jogador)) return;
            var botao = focosJogador[jogador];
            if (botao != null && cancelamentos.TryGetValue(botao, out var cancelar)) { cancelar(); suja = true; }
        }

        public void NavegarAmeacas(int delta) { if (MesaAberta) MudarAmeaca(delta); }

        string DescricaoCarta(CartaDados c)
        {
            string texto = c.nome + "  ·  Conhecimento: " + c.conhecimento + (c.cientista != Cientista.Normal ? "  ·  ESPECIAL" : "") + "\n\n" + c.descricao;
            if (!c.custo.Equals(default(Recursos)))
            {
                var custos = new List<string>();
                if (c.custo.saude != 0) custos.Add("Saúde " + c.custo.saude);
                if (c.custo.infraestrutura != 0) custos.Add("Infraestrutura " + c.custo.infraestrutura);
                if (c.custo.pesquisa != 0) custos.Add("Pesquisa " + c.custo.pesquisa);
                texto += "\n\nCusto: " + string.Join(" · ", custos);
            }
            return texto;
        }

        public Vector2 PosicaoCarta(int j, string id) => posicoesCartas.TryGetValue(j + ":" + id, out var p) ? p : new Vector2(j == 0 ? 390 : 1180, 300);
        public Vector2 PosicaoAmeaca(string id) => posicoesAmeacas.TryGetValue(id, out var p) ? p : new Vector2(800, 730);

        void Cabecalho(string titulo)
        {
            Painel("Painel", 170, 100, 1260, 790); Texto("Titulo", raiz, titulo, 230, 145, 1140, 65, 40, ouro, TextAlignmentOptions.Center);
        }

        void PausaTela()
        {
            Cabecalho("PARTIDA PAUSADA"); Botao("Continuar", "Continuar", 430, 300, 740, 66, () => PartidaDiretor.Instance.Pausar()); Botao("Opcoes", "Opções", 430, 410, 740, 66, () => opcoes = true); Botao("Reiniciar partida", "Reiniciar partida", 430, 520, 740, 66, Iniciar); Botao("Menu principal", "Menu principal", 430, 630, 740, 66, () => PartidaDiretor.Instance.Menu());
        }

        void Fim()
        {
            var p = PartidaDiretor.Instance; bool venceu = telaAtual == "Vitoria";
            Cabecalho(venceu ? "CRISE SUPERADA" : "A CRISE AVANÇOU"); Imagem("Resultado", raiz, venceu ? arte?.vitoria : arte?.derrota, 540, 240, 520, 110); Imagem("Trofeu", raiz, venceu ? arte?.trofeu : arte?.trofeuQuebrado, 665, 365, 270, 270);
            TextoVivo("Resumo", raiz, $"{p.Mensagem}\nRodadas: {p.Rodada} · Ameaças superadas: {AmeacasDiretor.Instance.Cemiterio.Count}/15", 285, 665, 1030, 90, 25, null, TextAlignmentOptions.Center);
            Botao("Jogar novamente", "Jogar novamente", 300, 790, 470, 55, Iniciar); Botao("Menu principal", "Menu principal", 830, 790, 470, 55, () => p.Menu());
        }

        void Opcoes()
        {
            Cabecalho("OPÇÕES"); var a = AudioPartidaDiretor.Instance;
            TextoVivo("Volume da musica", raiz, $"Música: {(a != null ? a.VolumeMusica : .65f):P0}", 300, 285, 480, 50, 28); Botao("Diminuir musica", "−", 910, 280, 120, 55, () => a.Ajustar(true, -.1f)); Botao("Aumentar musica", "+", 1080, 280, 120, 55, () => a.Ajustar(true, .1f));
            TextoVivo("Volume dos efeitos", raiz, $"Efeitos: {(a != null ? a.VolumeEfeitos : .8f):P0}", 300, 395, 480, 50, 28); Botao("Diminuir efeitos", "−", 910, 390, 120, 55, () => a.Ajustar(false, -.1f)); Botao("Aumentar efeitos", "+", 1080, 390, 120, 55, () => a.Ajustar(false, .1f));
            BotaoVivo("Tela cheia", "Tela cheia: " + (Screen.fullScreen ? "sim" : "não"), 300, 510, 1000, 55, () => Screen.fullScreen = !Screen.fullScreen);
            BotaoVivo("Movimento reduzido", "Movimento reduzido: " + (Reduzido ? "sim" : "não"), 300, 610, 1000, 55, () => EfeitosVisuaisDiretor.Instance.AlternarMovimento());
            Botao("Voltar", "Voltar", 680, 740, 240, 65, () => opcoes = false, sprite: arte?.voltar);
        }

        void Tutorial()
        {
            Cabecalho("COMO JOGAR"); Texto("Instrucoes", raiz, $"1. Vejam o requisito de cada ameaça. TER não consome recursos; GASTAR consome o valor indicado quando ela é superada.\n\n2. Cada cientista escolhe até {config.regras.limiteSelecao} cartas e confirma sua mão. Cliquem numa ameaça antes de escolher uma carta com alvo.\n\n3. Após a fila, as ameaças são verificadas automaticamente, da mais antiga à mais nova. Os recursos podem mudar entre elas.\n\n4. Cada ameaça que continuar ativa aplica sua penalidade por rodada. Superem todas sem deixar nenhum recurso zerar.\n\nJ1: A/D navegam na própria mão; Z seleciona ou confirma.\nJ2: ←/→ navegam na própria mão; Enter seleciona ou confirma.\nNaveguem até Confirmar para encerrar a seleção de cada mão.\nQ/E: ameaças · Tab: caderno · Esc: pausa. Mouse também funciona.", 260, 250, 1080, 510, 26);
            Botao("Voltar", "Voltar", 560, 780, 480, 60, () => tutorial = false);
        }

        void Detalhes()
        {
            Cabecalho("CADERNO DA PARTIDA"); Botao("Maos e descarte", "Mãos / descarte", 220, 230, 360, 50, () => aba = 0, true, aba == 0); Botao("Ameacas superadas", "Ameaças superadas", 620, 230, 360, 50, () => aba = 1, true, aba == 1); Botao("Historico", "Histórico", 1020, 230, 360, 50, () => aba = 2, true, aba == 2);
            string conteudo = "";
            if (aba == 0)
            {
                foreach (var j in JogadoresDiretor.Instance.Jogadores) conteudo += $"J{j.indice + 1} — {Nome(j.cientista)}\nMão: {string.Join(", ", j.Mao.Select(x => x.nome))}\nDescarte: {string.Join(", ", j.Descarte.Select(x => x.nome))}\nBaralho: {j.Baralho.Count} cartas (ordem oculta).\n\n";
                conteudo += "Próxima ameaça: " + (EfeitosCartasDiretor.Instance.Espiando ? (AmeacasDiretor.Instance.Proxima?.nome ?? "Baralho esgotado") : "Use Monitoramento de Casos para revelar.");
            }
            else if (aba == 1) conteudo = string.Join("\n", AmeacasDiretor.Instance.Cemiterio.Select((x, i) => (i + 1) + ". " + x.nome));
            else conteudo = string.Join("\n", PartidaDiretor.Instance.Historico.Skip(Math.Max(0, PartidaDiretor.Instance.Historico.Count - 15)));
            var t = TextoVivo("Conteudo do caderno", raiz, conteudo, 230, 320, 1140, 445, 23);
            if (construindo) { t.enableAutoSizing = true; t.fontSizeMin = 17; t.fontSizeMax = 23; }
            Botao("Voltar a mesa", "Voltar à mesa", 560, 800, 480, 55, () => detalhes = false);
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

        public void Especial() { int n = botoes.FindIndex(b => b.GetComponent<AnimacaoInteracao>().especial); if (n >= 0) { foco = n; EventSystem.current.SetSelectedGameObject(botoes[n].gameObject); } }

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
