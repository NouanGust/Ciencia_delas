# Simulador de ameaças

Projeto independente em Unity e C#

Revelar desafios, verificar requisitos e manter as ameaças não superadas entre rodadas.

## O que desenvolver

1. **Cadastrar.** Preparar as 15 ameaças das páginas 10-11, com nome, requisito e penalidade. Começar os testes com Primeiros Casos do Surto, Falha no Isolamento e Origem Desconhecida do Vírus.
2. **Revelar e manter.** Revelar uma ameaça por rodada. Manter as anteriores não resolvidas em uma lista de ativas, ordenada da mais antiga à mais recente.
3. **Verificar requisitos.** Possuir verifica sem consumir. Gastar exige saldo suficiente e desconta o valor. Se qualquer recurso chegar a zero, encerrar imediatamente. Separar requisito e penalidade nos dados.
4. **Resolver e reavaliar.** Mover ameaças superadas para um cemitério definitivo. Aplicar recompensa configurada, quando houver, e verificar novamente as ativas. Uma ameaça não pode ser recompensada duas vezes.
5. **Aplicar falhas e encerrar.** Depois das resoluções, aplicar uma vez por rodada as penalidades das ameaças restantes. Vitória exige baralho vazio e nenhuma ameaça ativa; derrota ocorre se qualquer recurso zerar.

## Tela mínima

Baralho restante, ameaças ativas, superadas, recursos e histórico; botões Revelar, Resolver e Reiniciar.

## Está pronto quando

- [ ] Com Pesquisa 7, Primeiros Casos é superada sem gastar Pesquisa, usando a convenção de limite mínimo.
- [ ] Com Pesquisa 3, Origem Desconhecida gasta 2 e deixa 1. Com apenas 1, permanece ativa e aplica Saúde -1.
- [ ] Baralho vazio com ameaça ativa não gera vitória. Uma ameaça superada nunca retorna ao monte.
