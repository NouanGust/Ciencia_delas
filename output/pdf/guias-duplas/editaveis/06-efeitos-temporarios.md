# Guia 06 - Simulador de efeitos temporários

Unity + C# | Avançado | 2 a 4 integrantes | Entrega: 06/11/2026

## Objetivo

Controlar efeitos que duram rodadas ou aguardam um evento, mostrando quando atuam e quando expiram.

## Mínimo obrigatório

Saúde inicial 10 e Pesquisa 5, contador de rodadas, painel de efeitos ativos, histórico e Reiniciar. Use três efeitos: Pesquisa +1 no fim das próximas 3 rodadas; bloquear penalidades das próximas 2 rodadas; dobrar o próximo ganho direto de Pesquisa.

## Dados

Efeito: tipo e duração restante, quando aplicável. A duplicação é um efeito de uso único. Não permitir ativar novamente um efeito enquanto ele já estiver ativo.

## Etapas

1. **Definir ações:** Crie botões para ativar cada efeito, Ganho direto de Pesquisa +2 e Encerrar rodada. A penalidade padrão no fim de uma rodada é Saúde -2.
2. **Aplicar ganho periódico:** Ao encerrar a rodada, aplique +1 Pesquisa se o efeito estiver ativo. Só depois reduza sua duração.
3. **Aplicar bloqueio:** No mesmo encerramento, ignore a penalidade de Saúde se o bloqueio estiver ativo. Reduza sua duração após essa verificação.
4. **Aplicar duplicação:** O próximo clique em Ganho direto concede +4 em vez de +2 e consome o efeito. O ganho periódico não é duplicado.
5. **Expirar e registrar:** Remova efeitos com duração zero depois de processar seus eventos. Mostre a duração restante e registre o que aconteceu em cada rodada.
6. **Proteger o estado:** Limite recursos a 0–30; Saúde zero encerra a simulação. Reiniciar limpa todos os efeitos. Duplicação permanece até o uso ou o reinício.

## Testes obrigatórios

- [ ] **Ativar ganho periódico e encerrar quatro rodadas:** Pesquisa recebe +1 nas três primeiras e nada pela habilidade na quarta.
- [ ] **Ativar bloqueio e encerrar três rodadas:** Saúde não perde pontos nas duas primeiras e perde 2 na terceira.
- [ ] **Ativar duplicação e clicar duas vezes no ganho direto:** O primeiro ganho é +4; o segundo volta a ser +2.
- [ ] **Ativar duplicação e ganho periódico:** O periódico concede só +1; a duplicação permanece aguardando um ganho direto.
- [ ] **Reativar um efeito ainda ativo e reiniciar:** A reativação é recusada; Reiniciar restaura 10/5, rodada inicial e lista vazia.

## Divisão do trabalho

Uma pessoa implementa os efeitos e a ordem de processamento; outra implementa os controles e o histórico. Troquem para revisar expiração e interação entre efeitos.

## Limites

As regras acima foram simplificadas para o exercício. Não copiar automaticamente seus valores para as cartas oficiais. Explique a diferença entre duração por rodada e consumo por evento.

## Desafio opcional

Criar efeitos por dados configuráveis ou salvar e restaurar uma simulação em andamento.

## Entrega

Código-fonte, executável completo, README.md, testes.md e contribuicoes.md. Todos programam e explicam uma parte do código. Executar os cinco testes e solicitar teste de outra equipe.

## Marcos

- **Até 23/10:** Projeto criado e primeira interação funcionando.
- **Até 28/10:** Regra principal completa com interface provisória.
- **Até 30/10:** Primeiro executável entregue para outra equipe testar.
- **03–04/11:** Correções e testes dos casos extremos.
- **05/11:** Pacote final e ensaio da demonstração.
- **06/11:** Entrega e apresentação do programa.
