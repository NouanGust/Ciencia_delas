# Modelos de scripts - Ciência Delas

Cada arquivo acompanha um dos nove PDFs simplificados. Os modelos seguem a ideia dos exemplos da turma: uma classe MonoBehaviour, funções com nomes em português e comentários explicando o trabalho.

**Não há lógica pronta.** Os corpos dos métodos contêm somente comentários. Importar o arquivo não faz o simulador funcionar: a equipe deve criar os dados, programar as regras, chamar as funções necessárias e montar a interface.

## Correspondência

| Guia | Script |
|---|---|
| 01 - Simulador de baralho | SimuladorBaralho.cs |
| 02 - Simulador de compra e descarte | SimuladorCompraDescarte.cs |
| 03 - Simulador de jogada | SimuladorJogada.cs |
| 04 - Simulador de Efeitos | SimuladorEfeitos.cs |
| 05 - Simulador de Combo | SimuladorCombo.cs |
| 06 - Simulador de ameaças | SimuladorAmeacas.cs |
| 07 - Simulador de Recursos | SimuladorRecursos.cs |
| 08 - Simulador de Cooperação | SimuladorCooperacao.cs |
| 09 - Simulador de Turnos | SimuladorTurnos.cs |

## Como começar

1. Criar um projeto Unity separado, fora da pasta do jogo principal, usando a versão adotada pela turma.
2. Copiar somente o script da atividade para Assets/Scripts desse novo projeto. Manter o nome do arquivo igual ao da classe.
3. Criar um GameObject vazio chamado DiretorSimulador e adicionar o componente correspondente.
4. Ler primeiro a seção de variáveis sugeridas. A equipe deve declarar seus campos e, se necessário, criar classes auxiliares para cartas, ameaças e reservas. Esses dados não dependem das classes Carta ou blabla do jogo principal.
5. Implementar uma função por vez. Start é chamado pela Unity; as demais funções precisam ser chamadas pelo código da equipe ou ligadas aos botões. Os comentários não executam ações.
6. Montar a interface descrita no PDF. No OnClick de cada botão, arrastar o GameObject do simulador e escolher a função adequada. Funções com parâmetro inteiro precisam de um índice ou valor configurado; validar o parâmetro antes de acessar uma lista. Índices de listas começam em zero.
7. Funções de efeito ou verificação são etapas internas: não ligar todas diretamente aos botões. As regras também devem ser verificadas dentro do código, mesmo quando um botão estiver desabilitado.
8. Demonstrar os três testes do PDF e registrar os resultados. Todos os integrantes devem programar e explicar sua contribuição.

## Como interpretar os comentários

- `TODO`: tarefa a ser programada pela equipe.
- `VARIÁVEIS QUE A EQUIPE DEVE CRIAR`: sugestão de dados, ainda sem declarações prontas.
- `TESTES QUE A EQUIPE DEVE DEMONSTRAR`: comportamentos esperados, não testes automatizados já implementados.
- Os nomes e assinaturas são um ponto de partida. A equipe pode criar funções auxiliares, alterar parâmetros ou transformar uma verificação em uma função que retorna um resultado, explicando a escolha.
- Não é necessário usar Update para regras acionadas por botões. Não é necessário importar UnityEditor ou copiar dependências do projeto principal.

## Entrega até 06/11/2026

Projeto independente, executável, demonstração dos três testes e indicação do que cada integrante implementou. As convenções e limites de cada exercício continuam sendo os do PDF correspondente.

Os scripts originais DiretorCenas.cs e blablabla.cs foram usados somente como referência de estilo e não foram alterados.
