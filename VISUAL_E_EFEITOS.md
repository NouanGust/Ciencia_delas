# Visual e efeitos

A cena `Assets/Scenes/CienciaDelas.unity` usa os assets existentes em `Assets/Art`. O menu, as duas mãos, os recursos, as ameaças, a pausa e os encerramentos compartilham a apresentação em pixel art. As cartas mantêm a proporção original; seus efeitos e custos atuais aparecem na área de leitura ao receberem foco por mouse, teclado ou controle.

## Onde alterar

- `Assets/Resources/CienciaDelasAparencia.asset`: fundos, cientistas, botões, ícones, troféus, sequências de slash e intensidade dos efeitos.
- `Assets/Resources/CienciaDelasConfiguracao.asset`: relação entre IDs das cartas/ameaças e suas imagens, além das regras e áudio.
- `Assets/CienciaDelas/Editor/MapeamentoArte.json`: caminhos das artes usadas. O menu **Ciência Delas > Visual > Reaplicar assets da pasta Art** reaplica esse mapeamento. Isso substitui as referências visuais configuradas manualmente; edite o mapeamento também se quiser manter as trocas em uma reaplicação.
- **Ciência Delas > Visual > Verificar referências** valida as 35 imagens de cartas/ameaças, os fundos, cientistas e limites de recorte das sequências.

As imagens originais não foram redesenhadas. A importação das imagens utilizadas usa filtro Point, sem mipmaps e sem compressão, para preservar a pixel art. Os IDs dos sprites existentes foram preservados.

## Clareza da mesa

Os painéis semitransparentes atrás das mãos, recursos, leitura de cartas, fila e botões do menu foram removidos, assim como a camada que escurecia toda a arte de fundo. As ameaças e telas de pausa/informações mantêm superfícies opacas para leitura; os textos sobre o cenário usam uma sombra discreta.

Cada ameaça separa **para superar** de **perda a cada rodada**. Só aparecem os recursos realmente exigidos, com seus totais atuais. TER não consome recursos; GASTAR consome o requisito. A indicação de condição atendida considera o estado atual, sem prever os efeitos da fila ou o consumo por outras ameaças. As ameaças são verificadas automaticamente depois da fila, começando pela mais antiga.

As penalidades mostram perdas com sinal negativo e refletem bloqueio, redução e conversão já aplicados. A orientação permanente muda entre planejamento, espera pela outra cientista, execução e escolha de extras. Os botões distinguem confirmar cartas de passar sem jogar. Essas alterações são apenas de apresentação; as regras permanecem iguais.

## Efeitos integrados

`EfeitosVisuaisDiretor` é o singleton de feedback visual. Escuta eventos tipados da partida e dos recursos, sem decidir regras:

- DOTween no foco e pressão dos botões, entrada das cartas compradas, transição entre telas e voo da carta executada.
- Carta selecionada elevada e marcada com sua posição na fila global.
- Slash em especiais e em eventos de ameaças; partículas na execução, resolução e vitória.
- Valores flutuantes para ganhos e perdas de recursos.
- Tremor de câmera e do conteúdo da UI nas penalidades, especiais e derrota. O tremor da UI torna o efeito perceptível também em Canvas Overlay.
- **Movimento reduzido**, salvo nas opções, desativa tremor, partículas e animações de deslocamento/escala. Feedback de texto, recursos e seleção permanece disponível.

`ParticulasUIDiretor` usa um pool de até 96 imagens animadas no Canvas. São partículas de interface, com movimento, rotação, escala e fade via DOTween. Isso mantém os efeitos à frente da mesa sem exigir materiais de ParticleSystem compatíveis com uma render pipeline específica. Cada emissão tem quantidade configurável, limitada a 40.

`AnimacaoInteracao` é uma vista por botão/carta. Os diretores continuam singletons. Tweens são associados aos objetos e encerrados ao destruí-los; pausa, reinício e troca para o menu limpam efeitos transitórios e restauram a posição da câmera/mesa.

## Slash_shadder

A pasta contém folhas de sprites, e não arquivos de shader. Foram usadas `Part 1/01.png` e `Part 4/41.png`. Cada célula tem 48×48 pixels; o padrão usa dez quadros da faixa ciano, a partir da coluna 10 e da primeira linha, a 24 quadros por segundo. O recorte acontece em memória e os sprites são reutilizados. Para trocar a animação, altere a textura e os parâmetros em Aparência.

## Artes reaproveitadas

Há imagens específicas para 11 das 15 ameaças. Para as quatro sem correspondência nominal, foram usadas variantes disponíveis:

| Ameaça | Imagem provisória |
|---|---|
| Colapso na Produção de Vacinas | Contágio Fora de Controle (2) |
| Caos na Distribuição de Vacinas | Desinformação da População (2) |
| Crise Sanitária Internacional | Contágio Fora de Controle (3) |
| Resistência à Imunização | Risco nos testes clínicos (2) |

Essas referências podem ser substituídas individualmente. As imagens de Diagnóstico Rápido e Mapeamento Viral utilizam o sprite importado dos GIFs. Alguns ícones/valores embutidos nas artes divergem do GDD; o painel textual usa os dados atuais do jogo. Por exemplo, Radioatividade mantém a regra de -2 Saúde, embora a imagem mostre -1.

## Validação reproduzível

`CienciaDelas.Editor.FerramentasProjeto.GerarValidacaoVisual` cria um build de desenvolvimento em `Builds/Visual`. Ao executá-lo com `--validar-visual`, a rotina captura menu, mesa com seis cartas por mão, seleção, efeitos, Diagnóstico, pausa, opções e encerramentos. O teste visual usa uma mão controlada para exercitar os especiais. As capturas e o relatório ficam em `output/validacao/visual`.

O código de captura só está presente no editor e em builds de desenvolvimento. Não interfere nas partidas normais. Em execução oculta, a captura usa uma textura de renderização da própria UI, pois a janela oculta não apresenta o backbuffer. O modo de Canvas é restaurado após cada captura.

A revisão de clareza acrescenta capturas de três ameaças com requisitos e perdas compostos (`10-condicoes-e-penalidades.png`), bloqueio/redução/conversão (`11-penalidades-modificadas.png`), espera pela segunda confirmação (`12-aguardando-confirmacao.png`) e tutorial (`13-como-jogar.png`).

### Resultado — 30/10/2026

- Referências importadas e verificadas no Unity 6000.3.15f1.
- 16 cenários de regras e 100 simulações aprovados dentro do editor.
- Teste do executável de desenvolvimento concluído sem erros de execução, com inspeção das capturas de menu, mãos cheias, seleção, efeitos, diagnóstico, pausa, opções, várias ameaças e encerramentos.
- Build Windows normal, sem o código de captura: `Builds/Windows/CienciaDelas.exe`. Compilação concluída com zero erros e zero avisos (relatório `output/validacao/build.txt`).
- Teste físico de controle e avaliação de conforto/legibilidade em outros monitores permanecem para o aceite humano. O teste automatizado usa os comandos da interface para navegação e seleção; não simula um gamepad físico.

### Ajustes de Juice e Visual — 30/10/2026

**Pacing da resolução de turno**

`Fase.Ameacas` agora é uma fase visível: `AvancarExecucao` para nesta fase quando a fila esvazia. `InicializadorJogo` aguarda `intervaloResolucao` (padrão 1,8 s) antes de chamar `PartidaDiretor.FinalizarRodada()`, que executa `AmeacasDiretor.Resolver()` + início de rodada. O intervalo entre cartas (`intervaloExecucao`, padrão 1,0 s) também foi aumentado. Ambos são editáveis no Inspector do asset `CienciaDelasConfiguracao`. Testes (`VerificacaoRegras.Rodada`, `VerificacaoVisual`) chamam `FinalizarRodada()` diretamente, sem depender do timer.

**Limpeza da mesa**

- Linha de instrução estática "Após a fila…" removida da mesa — o texto de orientação dinâmica já cobre essa informação.
- Campo `−0 reservado` suprimido no topo quando não há reserva ativa: o número só aparece quando `reservado > 0`.
- Nome da fase usa acentuação correta: EXECUÇÃO, AMEAÇAS, PLANEJAMENTO, CARTA EXTRA (helper `ExibirFase`).
- Status de ameaça simplificado: `✓ Atendida` / `✗ Não atendida`, omitindo " · sem gastar" quando não se aplica.

**Troca de cientistas no menu**

Os sprites de Marie Curie e Jaqueline Goes agora acompanham a seleção de J1/J2. Ao clicar no botão de troca, as imagens e os nomes nas laterais da tela se invertem imediatamente. Rótulos "J1" e "J2" foram adicionados sobre cada cientista para reforçar a associação visual.
