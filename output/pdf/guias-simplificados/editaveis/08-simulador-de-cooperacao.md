# Simulador de Cooperação

Projeto independente em Unity e C#

Coordenar o planejamento dos dois jogadores com custos e confirmação compartilhados.

## O que desenvolver

1. **Separar os jogadores.** Criar dois painéis, um para Marie Curie e outro para Jaqueline Goes, com mãos próprias e Conhecimento individual 3/3. Exibir um único conjunto de Saúde, Pesquisa e Infraestrutura.
2. **Planejar em conjunto.** Permitir selecionar ou desmarcar cartas nos dois painéis antes das respectivas confirmações. Cada jogador escolhe até 3 cartas ou passa sem selecionar.
3. **Coordenar reservas.** Conhecimento é reservado por jogador. Custos em recursos compartilhados consideram a soma das reservas dos dois. Cancelar uma carta libera somente a reserva correspondente.
4. **Confirmar os dois.** Confirmar bloqueia apenas a seleção daquele jogador. O outro continua planejando. A execução só é liberada quando ambos confirmarem, inclusive quem passou.
5. **Executar uma fila global.** Registrar a ordem de seleção entre os dois painéis. Numerar e renumerar após remoções. Executar essa fila uma única vez; não executar todas as cartas de um jogador antes das do outro.

## Tela mínima

Dois painéis com Confirmar, estado Pronto/Aguardando, fila global e recursos compartilhados. Um mouse pode operar ambos no mesmo computador.

## Está pronto quando

- [ ] Confirmar só o jogador 1 não inicia a execução; confirmar o jogador 2 a libera.
- [ ] Selecionar J1-A, J2-B e J1-C produz a fila A, B, C. Remover B antes da confirmação resulta em A, C, numeradas 1 e 2.
- [ ] Com Infraestrutura 5, reservar 4 para J1 impede reservar mais 4 para J2. Cancelar a reserva de J1 libera o valor.
