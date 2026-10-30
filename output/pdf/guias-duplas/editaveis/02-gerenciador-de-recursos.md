# Guia 02 - Gerenciador de recursos

Unity + C# | Básico | 2 a 4 integrantes | Entrega: 06/11/2026

## Objetivo

Programar um painel de Saúde, Pesquisa e Infraestrutura com limites, ações e encerramento consistente.

## Mínimo obrigatório

Exibir Saúde 10, Pesquisa 5 e Infraestrutura 5. Todos os recursos ficam entre 0 e 30. Criar botões de ações, histórico, mensagem de derrota e Reiniciar.

## Dados

Estado: três valores inteiros e um indicador de partida encerrada. Ação: nome e alterações dos três recursos, como Saúde +2 e Pesquisa -1.

## Etapas

1. **Exibir o estado:** Crie os três valores iniciais e mostre-os em textos. Faça a interface ler os valores de uma única fonte.
2. **Cadastrar ações:** Prepare pelo menos quatro ações: ganho de Saúde, ganho de Pesquisa, ganho de Infraestrutura e uma ação que afete dois recursos.
3. **Aplicar uma ação:** Calcule as alterações como uma única ação e limite cada resultado ao intervalo de 0 a 30. Atualize todos os indicadores juntos.
4. **Reconhecer derrota:** Após cada ação, se qualquer recurso for zero, encerre a partida e informe qual recurso acabou.
5. **Bloquear e registrar:** Bloqueie ações após a derrota. Registre o nome da ação e os valores antes/depois para facilitar a conferência.
6. **Reiniciar:** Restaure 10/5/5, limpe o histórico e libere as ações.

## Testes obrigatórios

- [ ] **Abrir o programa:** Valores iniciais: Saúde 10, Pesquisa 5, Infraestrutura 5.
- [ ] **Adicionar 5 a um recurso que está em 29:** O resultado é 30, nunca 34.
- [ ] **Retirar 4 de um recurso que está em 2:** O resultado é 0 e a derrota é exibida.
- [ ] **Retirar exatamente 5 da Pesquisa inicial:** Pesquisa chega a 0 e a derrota ocorre imediatamente, sem esperar outra ação.
- [ ] **Tentar agir após derrota e depois reiniciar:** A ação é bloqueada; Reiniciar restaura os valores e permite jogar novamente.

## Divisão do trabalho

Uma pessoa implementa regras e limites; outra implementa botões e histórico. Troquem para revisar derrota e reinício.

## Limites

Não implementar cartas ou ameaças. O histórico pode permanecer apenas na memória. As ações são exemplos de teste, não um balanceamento oficial.

## Desafio opcional

Adicionar um gráfico do histórico ou permitir configurar as ações por arquivo.

## Entrega

Código-fonte, executável completo, README.md, testes.md e contribuicoes.md. Todos programam e explicam uma parte do código. Executar os cinco testes e solicitar teste de outra equipe.

## Marcos

- **Até 23/10:** Projeto criado e primeira interação funcionando.
- **Até 28/10:** Regra principal completa com interface provisória.
- **Até 30/10:** Primeiro executável entregue para outra equipe testar.
- **03–04/11:** Correções e testes dos casos extremos.
- **05/11:** Pacote final e ensaio da demonstração.
- **06/11:** Entrega e apresentação do programa.
