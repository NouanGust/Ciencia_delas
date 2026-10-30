# Simulador de compra e descarte

Projeto independente em Unity e C#

Movimentar as cartas de um jogador entre baralho, mão e cemitério, respeitando os limites.

## O que desenvolver

1. **Preparar.** Usar 15 cartas identificadas e embaralhadas. Na abertura da partida, comprar 4 cartas. Mostrar separadamente baralho, mão e cemitério.
2. **Comprar por rodada.** Criar um botão Nova rodada que compra uma carta se houver espaço. Nunca ultrapassar 6 cartas na mão. Para o exercício, seguir a regra das páginas 5 e 7.
3. **Simular o uso.** Selecionar uma carta da mão e clicar Executar. Ao terminar essa execução simulada, retirar a carta da mão e movê-la para o cemitério. Não é preciso aplicar seu efeito.
4. **Repor o baralho.** Quando acabar o monte de compra, embaralhar as cartas do cemitério para formar outro. Se não houver cartas disponíveis, informar isso sem causar erro.
5. **Conservar as cartas.** Uma carta só pode estar em uma área. Reiniciar deve recuperar as 15 cartas e distribuir novamente a mão inicial.

## Tela mínima

Três áreas com cartas e contadores; botões Nova rodada, Executar e Reiniciar.

## Está pronto quando

- [ ] Ao iniciar: mão 4, baralho 11, cemitério 0. Com mão de 6, a compra é bloqueada.
- [ ] Executar uma carta diminui a mão em 1 e aumenta o cemitério em 1. A soma das três áreas permanece 15.
- [ ] Prepare mão 4, baralho 0 e cemitério 11. Após comprar: mão 5, baralho 10 e cemitério 0.
