# Simulador de Turnos

Projeto independente em Unity e C#

Controlar as fases de uma rodada e impedir ações fora da etapa correta.

## O que desenvolver

1. **Abrir a partida.** Inicializar dois jogadores, recursos 10/5/5 e quatro cartas em cada mão. Para este exercício, a distribuição inicial é a compra da primeira rodada; a compra adicional começa na segunda.
2. **Iniciar a rodada.** Restaurar Conhecimento para 3/3. A partir da segunda rodada, comprar 1 carta por jogador, respeitando o limite de 6. Revelar uma nova ameaça, se houver.
3. **Planejar e confirmar.** Liberar seleção de até 3 cartas por jogador, reservar custos e aguardar as duas confirmações. Quem confirma fica bloqueado; selecionar zero cartas permite passar.
4. **Executar.** Percorrer a fila global na ordem de seleção. Cobrar custos, aplicar cada efeito e enviar as cartas usadas ao cemitério de seus donos. Não aceitar novas seleções nesta etapa.
5. **Resolver e avançar.** Resolver ameaças por antiguidade, aplicar sucessos e reavaliar. Aplicar penalidades das restantes; verificar vitória ou derrota. Se a partida continuar, iniciar outra rodada.

## Tela mínima

Rodada atual, fase atual, confirmação dos dois jogadores e histórico. Use botões e dados simples para simular cartas e ameaças; não dependa das entregas de outras equipes.

## Está pronto quando

- [ ] Executar antes das duas confirmações é bloqueado. Após confirmar, o jogador não altera sua seleção.
- [ ] Uma rodada não compra, revela ameaça ou cobra a mesma carta duas vezes. Conhecimento volta para 3/3 na seguinte.
- [ ] Se um recurso zerar durante qualquer fase, a partida para imediatamente. Vitória também bloqueia o avanço; Reiniciar restaura o estado inicial.
