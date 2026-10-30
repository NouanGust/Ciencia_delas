# Guia 01 - Simulador de baralho

Unity + C# | Básico a intermediário | 2 a 4 integrantes | Entrega: 06/11/2026

## Objetivo

Construir um programa que movimente cartas entre baralho, mão e descarte, sem perder nem duplicar nenhuma carta.

## Mínimo obrigatório

Uma cena com três áreas, contadores, botão Comprar, seleção de carta para descarte e botão Reiniciar. Use 15 cartas identificadas, quatro na mão inicial e limite de seis na mão.

## Dados

Carta: identificador e nome. Estado: listas baralho, mão e descarte. Mesmo cartas com nomes iguais precisam de identificadores diferentes.

## Etapas

1. **Criar os dados:** Monte 15 cartas de teste com identificadores de 1 a 15. Comece exibindo os nomes em textos ou botões.
2. **Preparar a partida:** Coloque todas as cartas no baralho, embaralhe e mova quatro para a mão. Atualize os três contadores.
3. **Comprar uma carta:** Antes de comprar, verifique o limite da mão. Retire a carta do baralho e adicione a mesma carta à mão.
4. **Descartar:** Ao selecionar uma carta da mão, mova-a para o descarte e atualize a tela. Uma carta já descartada não pode ser descartada novamente.
5. **Repor o baralho:** Se uma compra for solicitada com o baralho vazio, embaralhe o descarte para formar um novo baralho. Se ambos estiverem vazios, informe que não há cartas.
6. **Reiniciar:** Reconstrua o estado inicial. Depois de cada ação, confira: baralho + mão + descarte = 15.

## Testes obrigatórios

- [ ] **Iniciar:** Mão com 4, baralho com 11 e descarte com 0.
- [ ] **Comprar três vezes:** A mão chega a 6; a terceira compra é recusada e não altera as listas.
- [ ] **Descartar uma carta da mão:** A mão diminui em 1, o descarte aumenta em 1 e o identificador não aparece em duas áreas.
- [ ] **Preparar mão com 4, baralho vazio e descarte com 11; comprar:** O descarte é reembaralhado; uma carta vai à mão. Resultado: mão 5, baralho 10 e descarte 0.
- [ ] **Reiniciar após várias ações:** Voltam os totais 4/11/0, sem identificadores repetidos ou ausentes.

## Divisão do trabalho

Uma pessoa começa pelas listas e operações; a outra, pela interface e contadores. Depois, troquem de função para implementar reposição e reinício.

## Limites

Não implementar combate, efeitos de cartas ou dois jogadores. A compra por botão é uma regra didática; não resolve a divergência de compra por rodada do GDD.

## Desafio opcional

Permitir importar uma lista de cartas e escolher o tamanho do baralho.

## Entrega

Código-fonte, executável completo, README.md, testes.md e contribuicoes.md. Todos programam e explicam uma parte do código. Executar os cinco testes e solicitar teste de outra equipe.

## Marcos

- **Até 23/10:** Projeto criado e primeira interação funcionando.
- **Até 28/10:** Regra principal completa com interface provisória.
- **Até 30/10:** Primeiro executável entregue para outra equipe testar.
- **03–04/11:** Correções e testes dos casos extremos.
- **05/11:** Pacote final e ensaio da demonstração.
- **06/11:** Entrega e apresentação do programa.
