# Guia 07 - Quiz: mulheres na ciência

Unity + C# | Básico a intermediário | 2 a 4 integrantes | Entrega: 06/11/2026

## Objetivo

Criar um quiz que ensine por meio de perguntas, explicações e fontes verificáveis.

## Mínimo obrigatório

Dez perguntas com quatro alternativas e uma resposta correta. Exibir uma pergunta por vez, pontuação, explicação após responder, resultado e Reiniciar.

## Dados

Pergunta: enunciado, quatro alternativas, índice correto, explicação e fonte. Use pelo menos três perguntas sobre cada uma das duas cientistas; as demais podem abordar ciência e pesquisa.

## Etapas

1. **Pesquisar e revisar:** Escolha fontes institucionais e registre título e endereço. Entregue perguntas e respostas para revisão do professor antes de fechar o conteúdo.
2. **Separar conteúdo e lógica:** Guarde as perguntas em uma coleção de dados. Comece programando com duas perguntas provisórias, claramente marcadas para revisão.
3. **Apresentar:** Mostre enunciado, alternativas e progresso, como Pergunta 3 de 10. Limpe os destaques da pergunta anterior.
4. **Responder uma vez:** No primeiro clique, registre a resposta, some um ponto se correta e bloqueie todas as alternativas dessa pergunta.
5. **Explicar e avançar:** Mostre a resposta correta, uma explicação curta e a fonte. O usuário avança quando terminar de ler; não use cronômetro obrigatório.
6. **Finalizar:** Após a décima resposta, mostre o total de 0 a 10. Reiniciar deve limpar respostas, pontuação e progresso.

## Testes obrigatórios

- [ ] **Responder uma questão corretamente:** A pontuação aumenta exatamente 1 e aparece a explicação correspondente.
- [ ] **Responder incorretamente:** Não soma ponto; a resposta correta e a explicação são exibidas.
- [ ] **Clicar repetidamente ou em outra alternativa após responder:** A pontuação e a resposta registrada não mudam.
- [ ] **Responder todas as perguntas:** O resultado aparece depois da décima, sem tentar acessar uma décima primeira.
- [ ] **Reiniciar e usar uma pergunta com texto longo:** A pontuação volta a zero; enunciado e alternativas continuam legíveis e sem cortes.

## Divisão do trabalho

As duas pessoas pesquisam e revisam perguntas. Uma começa pela navegação e outra pela validação de respostas; depois troquem para implementar resultado e reinício.

## Limites

O professor deve validar o conteúdo científico. Não confundir as habilidades fictícias das cartas com fatos sobre a trajetória das cientistas. Não exigir cadastro ou dados pessoais.

## Desafio opcional

Embaralhar perguntas e alternativas preservando a associação com a resposta correta; adicionar revisão dos erros ao final.

## Entrega

Código-fonte, executável completo, README.md, testes.md e contribuicoes.md. Todos programam e explicam uma parte do código. Executar os cinco testes e solicitar teste de outra equipe.

## Marcos

- **Até 23/10:** Projeto criado e primeira interação funcionando.
- **Até 28/10:** Regra principal completa com interface provisória.
- **Até 30/10:** Primeiro executável entregue para outra equipe testar.
- **03–04/11:** Correções e testes dos casos extremos.
- **05/11:** Pacote final e ensaio da demonstração.
- **06/11:** Entrega e apresentação do programa.
