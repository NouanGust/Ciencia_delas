# Guia 08 - Laboratório de sequências

Unity + C# | Intermediário | 2 a 4 integrantes | Entrega: 06/11/2026

## Objetivo

Criar um minijogo em que o jogador reorganiza amostras fictícias para reproduzir uma sequência de referência.

## Mínimo obrigatório

Três desafios com 4, 5 e 6 peças, referência visível, peças clicáveis, botão Conferir, contador de tentativas e Reiniciar. Use letras A, B, C e D; repetições são permitidas.

## Dados

Desafio: sequência-alvo e sequência inicial com os mesmos símbolos em outra ordem. Cada peça tem um identificador próprio e um símbolo; a comparação da resposta usa os símbolos.

## Etapas

1. **Criar desafios:** Cadastre três pares de sequências. Exemplo: alvo A-B-C-D e início C-A-D-B. Confira se cada início contém exatamente os símbolos do seu alvo.
2. **Mostrar a tarefa:** Exiba referência e peças com texto legível. Explique: clique em uma peça e depois em outra para trocar suas posições.
3. **Trocar peças:** O primeiro clique seleciona e destaca; o segundo troca as posições e limpa a seleção. Clicar duas vezes na mesma peça apenas cancela a seleção.
4. **Conferir:** Apenas o botão Conferir aumenta as tentativas. Compare tamanho e cada posição; informe sucesso ou peça outra tentativa sem revelar uma solução automática.
5. **Avançar:** Quando estiver correto, bloqueie a edição e habilite Próximo. Após o terceiro desafio, mostre a conclusão e o total de tentativas.
6. **Reiniciar:** Volte ao primeiro desafio com sequência inicial, nenhuma seleção e tentativas zeradas. Use símbolos ou texto além de cores.

## Testes obrigatórios

- [ ] **Abrir cada desafio:** Referência e peças têm o mesmo tamanho e a mesma quantidade de cada símbolo.
- [ ] **Trocar duas posições e clicar duas vezes na mesma peça:** A primeira ação troca corretamente; a segunda cancela sem duplicar nem apagar peças.
- [ ] **Conferir uma ordem incorreta:** A tentativa aumenta em 1 e o desafio permanece aberto.
- [ ] **Conferir uma ordem correta e tentar editar:** O desafio é concluído; as peças ficam bloqueadas e Próximo é habilitado.
- [ ] **Resolver os três desafios e reiniciar:** A tela final aparece; Reiniciar restaura o primeiro estado e zera as tentativas.

## Divisão do trabalho

Uma pessoa implementa listas, troca e comparação; outra implementa seleção e telas. Troquem para adicionar progressão e reinício.

## Limites

Trata-se de uma analogia fictícia de organização de amostras. Não apresenta um procedimento real de sequenciamento genético e não deve ser descrito como tal.

## Desafio opcional

Gerar sequências aleatórias solucionáveis, adicionar mais desafios ou permitir arrastar as peças.

## Entrega

Código-fonte, executável completo, README.md, testes.md e contribuicoes.md. Todos programam e explicam uma parte do código. Executar os cinco testes e solicitar teste de outra equipe.

## Marcos

- **Até 23/10:** Projeto criado e primeira interação funcionando.
- **Até 28/10:** Regra principal completa com interface provisória.
- **Até 30/10:** Primeiro executável entregue para outra equipe testar.
- **03–04/11:** Correções e testes dos casos extremos.
- **05/11:** Pacote final e ensaio da demonstração.
- **06/11:** Entrega e apresentação do programa.
