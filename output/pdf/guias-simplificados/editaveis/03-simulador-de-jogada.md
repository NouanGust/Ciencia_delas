# Simulador de jogada

Projeto independente em Unity e C#

Selecionar cartas, reservar Conhecimento e executar uma jogada na ordem escolhida.

## O que desenvolver

1. **Preparar a mão.** Exibir cartas com custos do GDD e iniciar com 3 pontos de Conhecimento. Usar, por exemplo, Análise Genética (0), Distribuição de Insumos (0), Sequenciamento Rápido (1) e Coleta de Amostras (2).
2. **Selecionar.** Permitir até 3 cartas, inclusive as de custo zero. Numerar a seleção pela ordem dos cliques. Impedir selecionar a mesma carta duas vezes ou reservar mais Conhecimento que o disponível.
3. **Reservar e cancelar.** Ao selecionar, reservar o custo sem consumir o saldo real. Ao desmarcar, liberar somente aquela reserva e renumerar as cartas restantes.
4. **Confirmar.** Bloquear alterações após confirmar. Confirmar sem cartas significa passar. Neste recorte de um jogador, um botão separado Executar simula a liberação pela confirmação do outro jogador.
5. **Executar.** Percorrer a fila uma vez, cobrar cada custo somente na execução e registrar a carta usada. Enviar as executadas ao descarte. Reiniciar limpa o estado.

## Tela mínima

Mão, fila numerada, saldo real, valor reservado e disponível; botões Confirmar, Executar e Reiniciar.

## Está pronto quando

- [ ] Selecionar custos 2 e 1 deixa saldo real 3, reserva 3 e disponível 0.
- [ ] Cancelar a carta de custo 2 mantém a outra selecionada, com reserva 1 e disponível 2. Uma quarta seleção é recusada.
- [ ] Confirmar não cobra custos. Executar cobra uma vez; clicar novamente não repete a jogada.
