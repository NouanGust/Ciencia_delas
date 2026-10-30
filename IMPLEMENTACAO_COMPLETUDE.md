# Implementação da partida cooperativa

**Atualização de 30/10/2026:** os assets e efeitos visuais foram integrados. A validação pelo Unity e a geração do executável Windows, anteriormente bloqueadas, foram concluídas. Consulte `VISUAL_E_EFEITOS.md` para configuração, imagens reaproveitadas, animações e resultados dos testes visuais. O registro de 29/10 abaixo descreve a etapa anterior.

## Abrir e jogar

A cena de entrada é `Assets/Scenes/CienciaDelas.unity`. Abra essa cena e pressione Play. O menu permite trocar as cientistas entre J1 e J2, iniciar, ajustar volume e sair. As cenas antigas permanecem como protótipos e não são usadas na versão distribuível.

Os dois participantes compartilham a tela e o mouse/teclado ou controles conectados. Cada mão tem estado e confirmação próprios. Selecione até três cartas por mão e confirme as duas jogadas. Confirmar sem cartas passa a vez. A fila executa automaticamente, resolve as ameaças e inicia a próxima rodada. Não há rede ou pareamento individual de dispositivos.

Mouse: clique seleciona/desmarca; botão direito cancela a carta em foco. Setas/WASD navegam pelos botões; Enter/Z confirmam; X/Backspace cancelam; Tab alterna informações; Esc pausa. No controle: direcional/analógico navegam; A seleciona; B cancela; LB foca uma carta especial disponível; RB/RT alternam informações; Menu pausa. A habilidade especial é uma carta e respeita seu custo.

## Organização por singletons

Os diretores de regras são singletons C# com `Instance`, sem dependências de objetos da cena. A inicialização e o reinício são explícitos; não dependem da ordem de `Start` das mãos. Os diretores de apresentação herdam `SingletonComponente<T>` e pertencem à cena.

| Script em Assets/CienciaDelas | Responsabilidade |
|---|---|
| Core/PartidaDiretor.cs | Fases, rodada, pausa, confirmação conjunta, execução, extras, vitória e derrota |
| Core/JogadoresDiretor.cs | Cientistas, baralhos, mãos, compra, descarte e reembaralhamento |
| Core/RecursosDiretor.cs | Limites, reservas compartilhadas e derrota imediata |
| Core/PlanejamentoDiretor.cs | Seleção, custos individuais, ordem global e bloqueio após confirmar |
| Core/EfeitosCartasDiretor.cs | Efeitos diretos, duplicação, durações, anulação e conversão |
| Core/AmeacasDiretor.cs | Revelação, requisitos, resolução por antiguidade, reavaliação e cemitério |
| Runtime/InicializadorJogo.cs | Inicialização dos componentes e intervalo entre cartas |
| Runtime/InterfaceDiretor.cs | Menu, mesa, duas mãos, fila, ameaças, informações, pausa e encerramento |
| Runtime/EntradaDiretor.cs | Mouse, teclado e gamepads usando o Input System instalado |
| Runtime/AudioPartidaDiretor.cs | Música por estado/crise, transição, eventos, ambiente opcional e volumes |
| Runtime/ConfiguracaoJogo.cs | Catálogo e referências visuais/sonoras editáveis no Inspector |

`Modelos.cs` contém somente dados. Cartas e jogadores são instâncias de dados, não singletons: cada participante precisa ter seu próprio estado. `PartidaDiretor.Alterada` informa mudanças para interfaces alternativas; `Evento` informa eventos sonoros. Uma futura interface pode chamar os mesmos diretores e substituir somente a apresentação.

## Ajustar regras, arte e áudio

Selecione `Assets/Resources/CienciaDelasConfiguracao.asset` no Inspector. Há 20 cartas (10 normais e 5 por cientista), 15 ameaças, regras, cores, fonte, fundo e clipes. As artes são opcionais: cada entrada relaciona o ID da carta/ameaça a um Sprite. Sem imagem, a apresentação usa painéis de cor e texto. Não é necessário editar a regra para substituir imagem ou música.

Os custos de Saúde/Infraestrutura/Pesquisa estão disponíveis em cada carta, embora o catálogo original cobre apenas Conhecimento. Os ganhos e custos são separados. Penalidades das ameaças são magnitudes positivas; o diretor aplica a perda. Recompensas estão disponíveis e começam em zero porque o GDD não fornece valores.

Faça alterações de balanceamento antes de iniciar o Play. Atualize também a descrição da carta quando mudar um efeito ou uma duração.

O áudio usa os clipes já existentes no projeto. O campo `ambiente` é opcional e fica disponível para um loop de laboratório posterior. Não foi produzido novo material visual ou sonoro.

## Decisões para ambiguidades do GDD

- Compra: decisão do usuário — repor até quatro; com quatro ou mais, comprar uma; máximo seis. A mão inicial tem quatro, sem compra adicional na primeira rodada. `reporAteQuatro` permite alternar a regra.
- Conhecimento: restaurar para 3/3 em cada rodada; não acumula.
- Requisitos `+N` e `=N`: padrão `PeloMenos`. Cada ameaça permite escolher `MaiorQue` ou `Exatamente` no Inspector. Recursos com requisito zero não participam da comparação.
- Recompensas: zero até existirem valores aprovados. O sistema já suporta ganhos e reavaliação após cada sucesso.
- Radioatividade: alvo escolhido antes de selecionar a carta, com fallback para a ameaça mais antiga; bloqueia a rodada atual e a próxima. Saúde -2 no final de ambas. O bloqueio não impede superar a ameaça por requisitos.
- Sequenciamento Genético: afeta a ameaça revelada na rodada atual. Não escolhe uma ameaça antiga caso o baralho já tenha acabado.
- Descoberta Científica: dobra a próxima carta da fila global, inclusive do outro jogador. A carga permanece entre rodadas até ser usada. Custos não são duplicados. Ganhos/reduções/quantidade de extras são multiplicados; bloqueios têm duração multiplicada. Espiar é idempotente. Uma Descoberta duplicada prepara multiplicador 4 para a próxima carta.
- Experimento Avançado: três finais de rodada, incluindo a atual, antes de resolver ameaças. Legado: início das duas próximas rodadas. Benefícios temporários e efeitos de Radioatividade são aplicados na ordem em que foram criados; zero continua sendo derrota imediata.
- Diagnóstico Rápido: suspende a execução para o dono escolher até duas cartas adicionais da própria mão. Elas entram no fim da fila, na ordem de escolha, excedem o limite normal de três e dispensam somente Conhecimento. É possível concluir a escolha sem gastar todas as vagas. Cartas já na fila não podem ser escolhidas novamente.
- Controle Epidemiológico: uma ameaça escolhida, na rodada atual; cada perda vira ganho `floor(perda / 3)`. Assim, -3 vira +1 e -1/-2 viram 0. O divisor é configurável. Conversão usa a perda original; não soma Mapeamento sobre um ganho convertido. Bloqueio tem prioridade sobre conversão.
- Quando um efeito anterior inviabiliza um custo já reservado, a carta seguinte é cancelada sem consumo e permanece na mão. Custos nunca são financiados antecipadamente por ganhos ainda não executados.
- Derrota tem prioridade sobre cura/recompensa: custo ou perda que zere qualquer recurso encerra imediatamente. Ameaças gastam automaticamente quando cumprem seus requisitos.

Essas decisões são padrões de implementação para permitir uma partida completa; ainda precisam de avaliação de balanceamento pela equipe.

## Verificação e manutenção

No Unity, use **Ciência Delas > Verificar regras**. Os testes estão em `Assets/CienciaDelas/Tests/VerificacaoRegras.cs`; executam 16 cenários determinísticos e 100 simulações, incluindo seleção/reserva, descarte, todos os efeitos, expiração, comparadores, reavaliação, pausa, reinício, vitória e derrota. Partidas de vitória controlada verificam o fluxo, não comprovam balanceamento do catálogo.

**Ciência Delas > Preparar cena e configuração** cria a configuração/cena somente se não existirem e configura a entrada da versão distribuível. Não sobrescreve o catálogo já editado. **Ciência Delas > Gerar jogo para Windows** executa as verificações e gera `Builds/Windows/CienciaDelas.exe`. Relatórios ficam em `output/validacao`.

### Resultado desta implementação — 29/10/2026

- 16 cenários e 100 simulações aprovados, executados com o compilador C# e o Mono incluídos no Unity instalado.
- Scripts novos compilados com e sem `UNITY_EDITOR`, contra as bibliotecas reais do projeto. Scripts de execução preexistentes também compilaram após as correções; permanecem avisos de campos serializados e variável não utilizada do protótipo.
- Sintaxe da cena/configuração conferida com o parser YAML disponível no projeto; catálogo contém 20 cartas e 15 ameaças. Referências usam metadados locais.
- A importação/execução pelo editor e a compilação do executável **não foram concluídas**: a tentativa restrita falhou na comunicação com serviços locais do Unity, e as duas revisões automáticas de permissão para execução ampliada expiraram. A compilação isolada dos scripts não substitui teste em Play Mode nem validação visual/gamepad.

Os scripts do protótipo receberam correções pontuais de inicialização de mão, uso da instância sorteada, desmarcação, pausa, dano e isolamento de APIs do editor. Não misture os diretores do protótipo com a nova cena. O novo fluxo substitui as regras antigas de dano/vida por requisitos e recursos.

## Pendências de produção e aceite

Arte final das ameaças, composição de laboratório, ambiente sonoro definitivo, teste físico de gamepads, legibilidade em diferentes monitores e balanceamento/aceite com duas pessoas continuam sendo validações humanas. A implementação e os testes automáticos não equivalem a aprovação desses critérios. A análise original de completude foi preservada como registro da auditoria anterior.
