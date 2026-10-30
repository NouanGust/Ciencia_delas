# Simulador de baralho

Projeto independente em Unity e C#

Montar e organizar os três baralhos previstos no jogo.

## O que desenvolver

1. **Cadastrar cartas.** Cada carta deve ter identificador, nome, tipo, cientista quando aplicável e custo de Conhecimento. Use os nomes e custos das páginas 8-10 do GDD; efeitos não precisam funcionar neste simulador.
2. **Montar os dois baralhos jogáveis.** Marie Curie recebe 10 cartas normais e suas 5 especiais. Jaqueline Goes recebe cópias das mesmas 10 normais e suas próprias 5 especiais. Cada baralho deve ter 15 cartas.
3. **Montar o baralho da Ameaça.** Cadastrar os 15 desafios das páginas 10-11 em uma lista separada. Não misturar desafios com cartas das cientistas.
4. **Embaralhar.** Criar um botão para embaralhar cada baralho, alterando somente a ordem. Não acrescentar, perder ou duplicar cartas.
5. **Validar e restaurar.** Exibir quantidade e composição de cada baralho. Um botão Reiniciar deve recuperar as listas originais.

## Tela mínima

Três listas com nomes, tipos e contadores; botões Embaralhar e Reiniciar.

## Está pronto quando

- [ ] Os baralhos mostram 15 / 15 / 15 cartas; cada cientista possui 10 normais e 5 especiais corretas.
- [ ] Após vários embaralhamentos, os mesmos identificadores continuam presentes uma vez em cada baralho.
- [ ] Alterar a ordem de um baralho não altera os outros. Reiniciar restaura a composição inicial.
