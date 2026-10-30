# Guia 04 - Resolvedor de ameaças

Unity + C# | Intermediário a avançado | 2 a 4 integrantes | Entrega: 06/11/2026

## Objetivo

Programar desafios que verificam recursos, persistem entre rodadas e são superados quando seus requisitos são atendidos.

## Mínimo obrigatório

Cinco ameaças, fila de revelação, lista de ativas, lista de superadas, recursos visíveis e botão Resolver rodada. Permita ajustar recursos antes da resolução para testar cenários.

## Dados

Ameaça: id, nome, tipo Possuir/Gastar, recurso exigido, quantidade e penalidade. Use: A Possuir Pesquisa 7 / Saúde -2; B Possuir Infraestrutura 7 / Infraestrutura -1; C Gastar Pesquisa 2 / Saúde -1; D Possuir Saúde 10 / Saúde -1; E Gastar Infraestrutura 3 / Pesquisa -1.

## Etapas

1. **Cadastrar os desafios:** Use a tabela didática acima, na ordem A até E. Recursos iniciais: Saúde 10, Pesquisa 5 e Infraestrutura 5; limites 0 a 30.
2. **Revelar:** No início de cada rodada, mova uma ameaça do monte para as ativas. Depois da quinta, continue resolvendo apenas as que restarem.
3. **Verificar por ordem:** Percorra as ativas da mais antiga à mais recente. Possuir exige valor maior ou igual e não consome; Gastar exige saldo suficiente e desconta o valor.
4. **Superar ou manter:** Mova as aprovadas para superadas. As demais continuam ativas. Nesta versão didática, superar não concede recompensa.
5. **Aplicar penalidades:** Após as verificações, aplique uma vez a penalidade de cada ameaça restante. Se qualquer recurso chegar a zero, encerre imediatamente.
6. **Encerrar e reiniciar:** Vitória exige monte vazio e nenhuma ameaça ativa. Reiniciar restaura recursos e as cinco ameaças.

## Testes obrigatórios

- [ ] **A ativa e Pesquisa em 7:** A é superada e a Pesquisa continua em 7.
- [ ] **C ativa e Pesquisa em 3:** C é superada e a Pesquisa termina em 1.
- [ ] **C ativa e Pesquisa em 1:** C permanece ativa; Pesquisa não é gasta e Saúde perde 1.
- [ ] **A e B ativas com requisitos não atendidos por duas rodadas:** Cada penalidade acontece uma vez por rodada; ameaças não desaparecem sozinhas.
- [ ] **Preparar os estados finais:** Monte vazio com ameaça ativa não é vitória. Monte e ativas vazios geram vitória. Qualquer recurso em zero gera derrota e bloqueia novas rodadas.

## Divisão do trabalho

Uma pessoa implementa requisitos e recursos; outra implementa listas e histórico. Troquem para testar persistência e condições finais.

## Limites

A tabela é uma especificação didática fechada, não a lista oficial completa do GDD. A dificuldade não é o foco; os ajustes manuais servem para testar a lógica.

## Desafio opcional

Adicionar recompensas e reavaliar as ameaças até que nenhuma nova resolução seja possível, sem resolver a mesma ameaça duas vezes.

## Entrega

Código-fonte, executável completo, README.md, testes.md e contribuicoes.md. Todos programam e explicam uma parte do código. Executar os cinco testes e solicitar teste de outra equipe.

## Marcos

- **Até 23/10:** Projeto criado e primeira interação funcionando.
- **Até 28/10:** Regra principal completa com interface provisória.
- **Até 30/10:** Primeiro executável entregue para outra equipe testar.
- **03–04/11:** Correções e testes dos casos extremos.
- **05/11:** Pacote final e ensaio da demonstração.
- **06/11:** Entrega e apresentação do programa.
