# Simulador de Efeitos

Projeto independente em Unity e C#

Aplicar efeitos imediatos, temporários e de anulação usando exemplos reais das cartas.

## O que desenvolver

1. **Preparar o painel.** Mostrar Saúde, Pesquisa, Infraestrutura, rodada atual e efeitos ativos. Usar valores iniciais 10/5/5 e limites de 0 a 30. Botões representam a execução das cartas; compra e seleção ficam fora deste recorte.
2. **Efeitos imediatos.** Implementar Intervenção Emergencial: Saúde +2; e Energia Nuclear: Pesquisa +2 e Infraestrutura +1.
3. **Efeitos no fim da rodada.** Implementar Experimento Avançado: Saúde, Pesquisa e Infraestrutura +1 ao final de cada rodada, durante 3 rodadas. Depois, remover o efeito.
4. **Efeitos no início da rodada.** Implementar Legado Científico: Pesquisa +1 no início das próximas 2 rodadas. Não aplicar o ganho imediatamente ao ativar.
5. **Anulação.** Implementar Sequenciamento Genético: anular os efeitos da ameaça revelada somente na rodada atual. Usar uma penalidade de teste e mostrar quando ela foi bloqueada.

## Tela mínima

Recursos, efeitos com duração restante, histórico e botões para ativar cartas, iniciar/encerrar rodada e reiniciar.

## Está pronto quando

- [ ] Energia Nuclear leva Pesquisa 5 para 7 e Infraestrutura 5 para 6; ganhos acima de 30 param em 30.
- [ ] Experimento Avançado atua 3 vezes e Legado Científico 2 vezes; novas rodadas não mantêm efeitos expirados.
- [ ] A ameaça revelada não aplica sua penalidade na rodada anulada; na seguinte, a anulação acabou.
