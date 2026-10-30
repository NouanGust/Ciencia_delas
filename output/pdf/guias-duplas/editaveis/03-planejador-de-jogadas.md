# Guia 03 - Planejador de jogadas

Unity + C# | Intermediário | 2 a 4 integrantes | Entrega: 06/11/2026

## Objetivo

Permitir planejar uma jogada, reservar Conhecimento e confirmar a seleção sem cobranças duplicadas.

## Mínimo obrigatório

Uma mão fixa de seis cartas, três pontos de Conhecimento, limite de três cartas selecionadas, ordem numerada, Confirmar e Reiniciar.

## Dados

Custos das seis cartas de teste: 0, 0, 1, 1, 2 e 3. Guarde a mão, uma lista ordenada de seleção e um estado Planejando/Confirmado.

## Etapas

1. **Montar a mão:** Crie seis cartas identificadas e mostre nome e custo. Inicie com saldo 3 e reserva 0.
2. **Selecionar:** Verifique se a carta não está selecionada, se há espaço entre as três seleções e se o custo cabe no disponível.
3. **Reservar:** Calcule reserva pela soma dos custos selecionados. Disponível = saldo - reserva. Não reduza o saldo real durante o planejamento.
4. **Cancelar:** Remova somente a carta clicada, recalcule a reserva e renumere a ordem. As outras seleções devem permanecer válidas.
5. **Confirmar:** Desconte a reserva uma única vez, registre a ordem final e bloqueie alterações. Confirmar sem cartas significa passar.
6. **Reiniciar:** Restaure a mão, o saldo, a seleção e o estado inicial. Este protótipo não precisa simular efeitos ou rodadas seguintes.

## Testes obrigatórios

- [ ] **Selecionar custos 2 e 1:** Reserva 3, disponível 0 e saldo real ainda 3 antes de confirmar.
- [ ] **Tentar selecionar custo 1 quando o disponível é 0:** A seleção é recusada sem alterar as outras cartas.
- [ ] **Selecionar três cartas, incluindo as duas de custo 0, e tentar uma quarta:** A quarta é recusada pelo limite de quantidade.
- [ ] **Selecionar 2 e 1 e cancelar a de custo 2:** A carta de custo 1 continua selecionada na posição 1; reserva 1 e disponível 2.
- [ ] **Confirmar duas vezes ou confirmar vazio:** A segunda confirmação não cobra de novo. Uma seleção vazia confirma a passagem sem gastar Conhecimento.

## Divisão do trabalho

Uma pessoa implementa seleção e reserva; outra implementa cartas e numeração. Troquem para adicionar bloqueio e validar cancelamento.

## Limites

O mínimo é um planejador de um jogador. Não aplicar dano, não consumir recursos compartilhados e não conectar ao projeto principal.

## Desafio opcional

Adicionar um segundo jogador com saldo independente e confirmação conjunta.

## Entrega

Código-fonte, executável completo, README.md, testes.md e contribuicoes.md. Todos programam e explicam uma parte do código. Executar os cinco testes e solicitar teste de outra equipe.

## Marcos

- **Até 23/10:** Projeto criado e primeira interação funcionando.
- **Até 28/10:** Regra principal completa com interface provisória.
- **Até 30/10:** Primeiro executável entregue para outra equipe testar.
- **03–04/11:** Correções e testes dos casos extremos.
- **05/11:** Pacote final e ensaio da demonstração.
- **06/11:** Entrega e apresentação do programa.
