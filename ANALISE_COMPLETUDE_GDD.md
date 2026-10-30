# Ciência Delas: checklist de completude

Data da avaliação: 22/10/2026.

**Completude estimada: 19% do escopo total do GDD** (18,75 pontos de 100 na matriz abaixo). Classificação: protótipo inicial, com produção visual e sonora mais avançada que as regras da partida.

## Escopo e limites da avaliação

Referência: [GDD fornecido](<C:/Users/Professor/Downloads/_GDD - PROJETO CIÊNCIA DELAS .pdf>), 17 páginas. Projeto analisado: `C:/Users/Professor/CienciaDelas`, Unity 6000.3.15f1. Foram examinados os scripts próprios, cenas, ligações de botões e componentes, dados das cartas, prefabs, artes, arquivos de áudio e configuração de cenas da versão distribuível. As 20 artes das cientistas foram inspecionadas em conjunto; as tabelas de ameaças do PDF também foram conferidas visualmente.

A avaliação considera os arquivos locais, inclusive as alterações já existentes em CenaPrincipal.unity, MaoJogador1.cs e blablabla.cs. Nenhum código ou cena foi alterado por esta análise.

Esta é uma auditoria estática: não houve execução de uma partida, teste de controle, audição da trilha nem geração de executável. Assim, a porcentagem mede cobertura evidenciada do escopo, e não estabilidade, horas trabalhadas ou prazo restante. Ausência significa que a implementação não foi localizada nesta cópia do projeto. Material produzido fora dela não entrou na conta. As regras do GDD foram tratadas como referência de produto, não como instruções operacionais para o assistente.

## Método de pontuação

Foram definidos 40 critérios, agrupados em dez áreas. Cada área contém quatro critérios de mesmo peso interno. Os pesos entre áreas são uma escolha desta avaliação: regras essenciais da partida recebem maior peso que apresentação. O GDD não fornece pesos oficiais.

- 0%: implementação correspondente não evidenciada, ou entrega ainda não validada quando o critério exige teste.
- 25%: estrutura inicial, dados parciais ou protótipo com divergências importantes.
- 50%: parte relevante existe, mas falta integração ou cobertura importante.
- 75%: implementação substancial, ainda com pendências.
- 100%: critério atendido no limite verificável desta análise. Nos itens de arte e áudio, significa existência do conjunto de arquivos, não aprovação final de qualidade ou integração.

**Fórmula:** pontuação total = soma de peso da área × média dos seus quatro critérios ÷ 100.

| Área | Peso | Completude da área | Contribuição ao total |
|---|---:|---:|---:|
| Cooperação e escolha das cientistas | 10 | 0% | 0,0000 |
| Recursos e Conhecimento | 10 | 12,50% | 1,2500 |
| Baralhos, mãos e descarte | 10 | 12,50% | 1,2500 |
| Planejamento e execução da rodada | 15 | 31,25% | 4,6875 |
| Dados e efeitos das cartas | 15 | 6,25% | 0,9375 |
| Ameaças e encerramento da partida | 15 | 0% | 0,0000 |
| Arte e ambientação | 10 | 62,50% | 6,2500 |
| Interface e controles | 5 | 31,25% | 1,5625 |
| Áudio | 5 | 50% | 2,5000 |
| Entrega para PC e validação | 5 | 6,25% | 0,3125 |
| **Total** | **100** | **18,75%, arredondado para 19%** | **18,7500** |

O resultado depende dessa ponderação; deve ser usado como estimativa de acompanhamento, não como uma medição exata de esforço. As ambiguidades do GDD listadas adiante podem mudar os critérios de aceite.

## Checklist detalhado

Caixa marcada significa que aquele critério específico está atendido no escopo declarado. Itens parciais continuam desmarcados. A nota registra o crédito parcial e permite recalcular a porcentagem após cada entrega.

### 1. Cooperação e cientistas — GDD, páginas 3–4 e 17

- [ ] **C01 — Dois jogadores com estados independentes (0%).** A lógica ativa de seleção depende de `MaoJogador1.Instance`. Há áreas visuais para mãos em cenas, mas não foi encontrada lógica equivalente do segundo jogador.
- [ ] **C02 — Escolha entre Marie Curie e Jaqueline Goes (0%).** Não foi localizado fluxo de escolha com atribuição da cientista ao jogador.
- [ ] **C03 — Dois baralhos corretos de 15 cartas (0%).** Cada um deve ter 10 normais e 5 especiais da respectiva cientista. A configuração de Setup em CenaPrincipal contém dez entradas, sem montagem dos dois conjuntos completos.
- [ ] **C04 — Coordenação cooperativa da partida (0%).** Não há estado por participante que coordene planejamento, confirmação e execução conjunta.

### 2. Recursos e Conhecimento — GDD, páginas 4–7

- [ ] **C05 — Saúde compartilhada, inicial 10 e limitada a 0–30 (25%).** Existe Saúde e atualização textual, mas DiretorGame inicia em 5, usa máximo 10 e pode subtrair até valores negativos.
- [ ] **C06 — Infraestrutura compartilhada, inicial 5 e limitada a 0–30 (0%).** Existe o texto `Infra: 0/0` na cena, sem implementação de recurso correspondente encontrada.
- [ ] **C07 — Pesquisa compartilhada, inicial 5 e limitada a 0–30 (0%).** Existe o texto `Pesq: 0/0`, sem implementação de recurso correspondente encontrada.
- [ ] **C08 — Conhecimento individual 3/3 a cada rodada (25%).** MaoJogador1 inicializa três pontos e controla gasto, mas faltam o segundo jogador e a restauração por rodada.

### 3. Baralhos, mãos e descarte — GDD, páginas 4–7

- [ ] **C09 — Modelo de baralho e compra inicial (25%).** Existe sorteio sem reposição e lista de cartas, mas são sorteadas cinco, em vez das quatro previstas, e a instanciação contém falhas.
- [ ] **C10 — Exibição da mão com limite de seis e reposição por rodada (25%).** Há slots e instanciação visual; CenaPrincipal configura cinco slots e não há o ciclo de reposição definido no GDD.
- [ ] **C11 — Descarte após a execução (0%).** ConfirmarJogada não remove as cartas executadas nem as envia ao descarte de cada jogador.
- [ ] **C12 — Reembaralhamento do descarte quando o baralho acaba (0%).** Não localizado.

### 4. Planejamento e execução — GDD, páginas 6–7

- [ ] **C13 — Selecionar e desmarcar cartas com feedback (75%).** Há clique, elevação da carta e retorno visual. Desmarcar uma carta zera o dano acumulado de toda a seleção, mesmo que outras permaneçam escolhidas.
- [ ] **C14 — Reservar e liberar custos (50%).** Existe redução e devolução do Conhecimento disponível de um jogador. Faltam reservas dos três recursos compartilhados, coordenação entre jogadores e separação explícita entre reserva e consumo na execução.
- [ ] **C15 — Limite de três cartas, passar e bloquear após dupla confirmação (0%).** Há um botão de confirmar, mas essas regras não foram implementadas. Cartas de custo zero também não encontram limite explícito de quantidade.
- [ ] **C16 — Fila global numerada e ciclo completo de rodada (0%).** Não há execução sequencial pela ordem de seleção, renumeração, fases nem avanço automático completo. A confirmação aplica uma soma de dano; o turno inimigo é chamado pela barra de espaço.

### 5. Dados e efeitos das cartas — GDD, páginas 8–10

- [ ] **C17 — Dados funcionais de todas as cartas, com custos corretos (25%).** Existem Carta, ScriptableObjects e prefabs. Os dez assets em CartasSO usam nomes genéricos e custo/dano zero; os modelos têm nome, custo, dano e imagem, sem representação dos efeitos do GDD.
- [ ] **C18 — Efeitos das dez cartas normais (0%).** Não foram encontrados ganho de Pesquisa/Infraestrutura/Saúde nem visualização da próxima ameaça. A base atual trata cartas como dano.
- [ ] **C19 — Cinco efeitos exclusivos de Marie Curie (0%).** Faltam bloqueio temporário de ameaça, duplicação do próximo efeito e benefícios com duração em rodadas, além dos ganhos diretos previstos.
- [ ] **C20 — Cinco efeitos exclusivos de Jaqueline Goes (0%).** Faltam anulação, cartas adicionais gratuitas, redução de penalidades e conversão de efeitos, além da recuperação dos recursos.

### 6. Ameaças e encerramento — GDD, páginas 4, 7 e 10–11

- [ ] **C21 — Baralho com 15 desafios e revelação por rodada (0%).** A classe Ameaca representa um alvo com vida/dano. Não há baralho de desafios com os requisitos descritos.
- [ ] **C22 — Resolver “Possuir” e “Gastar” por antiguidade, com reavaliação (0%).** Não localizado. Dano contra vida não satisfaz esse critério.
- [ ] **C23 — Múltiplas ameaças persistentes, penalidades e cemitério definitivo (0%).** Não localizado; a referência principal é uma única Ameaca.
- [ ] **C24 — Vitória ao superar todas e derrota imediata se qualquer recurso zerar (0%).** Não há vitória implementada. O protótipo possui FimDeJogo, mas não cumpre a regra dos três recursos, falha no caso de igualdade do dano com a Saúde e tenta sair em vez de apresentar um encerramento completo.

### 7. Arte e ambientação — GDD, páginas 8–12 e 14

- [x] **C25 — Arquivos das dez artes normais produzidos (100%).** Os dez arquivos correspondentes estão em Assets/Art/Cartas. Este item avalia produção dos arquivos; efeitos e integração são avaliados separadamente.
- [x] **C26 — Arquivos das dez artes especiais produzidos (100%).** Cinco de cada cientista em Assets/Art/Cartas/Especiais. Ainda cabe revisão editorial e de consistência com os valores finais.
- [ ] **C27 — Conjunto visual das 15 ameaças (25%).** Foi localizada uma arte identificável de “Fracasso no Desenvolvimento da Vacina”; não foi localizado o conjunto completo. O crédito de 25% representa início da entrega, não 25% das 15 imagens.
- [ ] **C28 — Laboratório, mesa e evolução visual da crise (25%).** Existem fundo, arte de tela inicial e composição de interface. Não foi encontrado sistema de alteração visual conforme a crise nem comprovação de toda a ambientação integrada.

### 8. Interface e controles — GDD, páginas 12, 14–15

- [ ] **C29 — Menu principal, início, opções e saída (50%).** Menu inicio tem botões ligados; Opcoes apenas registra mensagem e toca som. Há problemas no fluxo de entrada e na configuração para executável.
- [ ] **C30 — Mesa e indicadores completos da partida (50%).** Existem mão, textos, ameaça e confirmação, mas faltam indicadores funcionais dos recursos, segundo jogador, fila, fases e múltiplas ameaças.
- [ ] **C31 — Teclado/mouse e pausa conforme o GDD (25%).** Clique e Esc têm código; não foi localizado o conjunto completo de navegação e atalhos. Pausar o tempo não bloqueia explicitamente o clique nas cartas.
- [ ] **C32 — Gamepad completo conforme o GDD (0%).** O projeto contém ações de entrada e módulo de UI genéricos; isso não comprova integração das ações específicas das cartas, especiais e abas. Não há teste realizado.

### 9. Áudio — GDD, página 13

- [x] **C33 — Biblioteca de músicas e efeitos básicos disponível (100%).** Há arquivos para menu, fase, batalha, vitória, derrota e ações. A nota é de disponibilidade, sem julgamento auditivo de qualidade ou direitos de uso.
- [ ] **C34 — Gerenciador, fontes e transição entre músicas (75%).** DiretorAudio.cs contém ScriptSound; DiretorAudio.prefab possui fontes e clipes configurados e é referenciado pelo menu. Falta validar execução e integração de todos os estados.
- [ ] **C35 — Sons cobrindo as ações do jogo (25%).** Há chamadas para botões e confirmação; algumas chamadas de seleção estão comentadas e os eventos ainda inexistentes do jogo não têm cobertura integrada.
- [ ] **C36 — Música dinâmica da ameaça, vitória/derrota e ambiente de laboratório (0%).** Existe método de música da ameaça final, mas não foi localizado seu acionamento pelo estado da crise, nem integração completa dos encerramentos e ambiente.

### 10. Entrega para PC e validação — GDD, páginas 16–17

- [ ] **C37 — Entrada e sequência de cenas configuradas para distribuição (25%).** Há quatro cenas habilitadas; Main é a primeira, antes de MainMenu e Menu inicio. Não foi localizado encaminhamento funcional de Main para o menu inicial. GameOver não consta da lista.
- [ ] **C38 — Executável PC compilado e validado (0%, não comprovado).** Não foi gerado nesta análise. Há uso de UnityEditor em script de execução sem proteção, incompatível com compilação para jogador.
- [ ] **C39 — Partida completa verificada do início à vitória e à derrota (0%, não comprovado).** As regras ausentes impedem tratar o protótipo como uma partida completa do GDD.
- [ ] **C40 — Balanceamento e aceite com dois jogadores (0%, não comprovado).** Não foram encontrados resultados de testes. Critério de validação derivado da experiência cooperativa e estratégica definida no GDD, não uma exigência documental adicional de testes automatizados.

## Problemas prioritários encontrados

As constatações abaixo resultam da leitura do código e das configurações. Os sintomas em execução ainda precisam ser reproduzidos.

| Prioridade | Evidência | Consequência e correção necessária |
|---|---|---|
| P0 | [blablabla.cs, linha 16](C:/Users/Professor/CienciaDelas/Assets/_scripts/blablabla.cs:16) instancia card_inst, mas preenche e adiciona card_prefab nas linhas seguintes. | Os dados são escritos no modelo e a lista recebe a mesma referência, em vez das instâncias sorteadas. Preencher a instância correta e definir uma única etapa de montagem visual da mão. |
| P0 | [MaoJogador1.cs, linha 41](C:/Users/Professor/CienciaDelas/Assets/_scripts/MaoJogador1.cs:41) acessa cartas[i]; [CenaPrincipal, linha 942](C:/Users/Professor/CienciaDelas/Assets/Scenes/CenaPrincipal.unity:942) salva a lista vazia. | O preenchimento depende de outro Start, sem ordem explicitamente definida nos scripts/metadados inspecionados. Há risco de acesso fora da lista. Inicializar os dados antes de renderizar a mão. |
| P0 | [Menu inicio, linha 1044](<C:/Users/Professor/CienciaDelas/Assets/Scenes/Menu inicio.unity:1044>) contém blablabla habilitado sem card_prefab serializado. | O código atual tenta instanciar esse campo ao abrir o menu. Remover o componente de teste do menu ou configurá-lo no fluxo correto. |
| P0 | [Ameaca.cs, linha 12](C:/Users/Professor/CienciaDelas/Assets/_scripts/Ameaca.cs:12). | A condição de dano está invertida: dano menor que a vida destrói o alvo; dano maior ou igual apenas subtrai. Além de corrigir o protótipo, adequar a ameaça às regras de recursos do GDD. |
| P0 | [DiretorCenas.cs, linha 1](C:/Users/Professor/CienciaDelas/Assets/_scripts/DiretorCenas.cs:1) e linha 51 usam UnityEditor/EditorApplication sem proteção. | Separar o código exclusivo do editor para permitir gerar o jogo para PC. A análise não executou uma compilação para confirmar o diagnóstico em log. |
| P0 | [DiretorGame.cs, linha 95](C:/Users/Professor/CienciaDelas/Assets/_scripts/DiretorGame.cs:95) compara dano > Saúde. | Se o dano for exatamente igual, a Saúde chega a zero sem entrar nessa derrota. Centralizar a aplicação dos recursos e verificar zero após cada alteração. |
| P0 | [DiretorGame.cs, linha 112](C:/Users/Professor/CienciaDelas/Assets/_scripts/DiretorGame.cs:112), [DiretorCenas.cs, linha 46](C:/Users/Professor/CienciaDelas/Assets/_scripts/DiretorCenas.cs:46) e [PausaDiretor.cs](C:/Users/Professor/CienciaDelas/Assets/_scripts/PausaDiretor.cs). | FimDeJogo zera o tempo, Sair espera com tempo escalado e PausaDiretor reescreve a escala a cada quadro. O encerramento pode ficar bloqueado ou ter sua pausa sobrescrita. Criar um estado de fim de jogo separado da pausa. |
| P1 | [MaoJogador1.cs, linha 75](C:/Users/Professor/CienciaDelas/Assets/_scripts/MaoJogador1.cs:75). | Ao desmarcar uma carta, danoTotal vira zero, invalidando outras cartas ainda selecionadas. Recalcular a seleção restante. |
| P1 | [DiretorGame.cs, linha 73](C:/Users/Professor/CienciaDelas/Assets/_scripts/DiretorGame.cs:73). | ConfirmarJogada não bloqueia nova confirmação, não descarta cartas e não encerra a rodada. O mesmo total pode ser reaplicado enquanto o alvo existir. |

## Divergências e decisões pendentes no GDD

Estas são dúvidas de especificação, não funcionalidades que deveriam ser inventadas durante a implementação:

1. **Compra por rodada:** a página 4 manda repor até quatro cartas e comprar mais uma se já houver quatro ou mais; as páginas 5 e 7 dizem comprar apenas uma. A mão inicial de quatro e o máximo de seis são consistentes, mas a reposição precisa ser unificada.
2. **Conhecimento:** “recuperam 3 pontos” na página 4 pode sugerir acréscimo; a página 7 estabelece 3/3. Esta avaliação adotou restauração para 3/3 como referência, sem alterar o projeto.
3. **Recompensa por superar ameaça:** o texto das páginas 4 e 7 prevê efeitos positivos, mas as tabelas das páginas 10–11 mostram somente nome, requisito e falha. Faltam os valores de sucesso, se forem parte efetiva da regra.
4. **Comparadores dos requisitos:** “Pesquisa +7”, “Saúde +10” e “todos os recursos =14/16” precisam indicar claramente se significam pelo menos, mais que ou exatamente o valor.
5. **Controle Epidemiológico:** o exemplo de transformar -3 em +1 não define uma regra geral para todas as penalidades, alvos e durações.
6. **Diagnóstico Rápido:** explicitar como duas cartas extras entram na fila depois de confirmada a seleção, qual jogador as escolhe e como essa exceção interage com o limite de três.
7. **Efeitos temporários e cooperação:** explicitar os alvos de Radioatividade/Sequenciamento Genético, a quem pertence a “próxima carta” duplicada e como dois participantes fornecem entradas simultâneas no PC. O GDD não exige explicitamente multiplayer online; sua ausência não foi penalizada como requisito de rede.
8. **Arte e regra:** a arte de Radioatividade exibe -1 Saúde, enquanto o texto da página 9 descreve -2 por dois turnos. Revisar o conjunto após fixar os valores oficiais; existência de imagem não equivale à sua aprovação final.

## Ordem sugerida para concluir

1. **Estabilizar a entrada e a mão:** corrigir inicialização, referências, dados das cartas, eventos do menu e uso de APIs do editor. Aceite: abrir pelo ponto de entrada previsto e chegar à mesa sem erros.
2. **Fechar as regras centrais:** resolver ambiguidades do GDD; implementar jogadores, baralhos, Saúde/Infraestrutura/Pesquisa, Conhecimento, compra, descarte e reposição. Aceite: dois jogadores possuem estados corretos e recursos respeitam 0–30.
3. **Completar uma rodada:** planejamento, limite, reserva, confirmação dupla, bloqueio, fila numerada, execução e descarte. Aceite: uma rodada termina e a seguinte começa sem intervenção de depuração.
4. **Implementar o conteúdo funcional:** dez normais, dez especiais, efeitos temporários e quinze ameaças, incluindo reavaliação e persistência. Aceite: cada carta cumpre o efeito documentado e cada ameaça tem requisito verificável.
5. **Fechar a partida e a distribuição:** vitória, derrota, reinício, menus e executável PC. Aceite: concluir partidas pelos dois resultados e reiniciar sem estado residual.
6. **Integrar e validar apresentação:** artes restantes, textos explicativos, áudio por evento, controles completos, legibilidade e testes de balanceamento em dupla.

A cena de pacotes e DiretorPacote foram identificados, mas abertura de pacotes não é um requisito da versão inicial descrita no GDD. Essa funcionalidade não recebeu peso nem compensou regras ausentes.

**Conclusão:** há uma base útil de apresentação e interação. O maior trabalho restante é implementar e integrar o funcionamento cooperativo descrito no GDD. A marca de 19% reconhece o material existente sem contar imagens, protótipos ou componentes isolados como regras concluídas.
