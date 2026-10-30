# Simulador de Combo

Projeto independente em Unity e C#

Demonstrar como a ordem das cartas pode modificar o resultado de uma jogada.

## O que desenvolver

1. **Preparar as cartas.** Usar Descoberta Científica (custo 3, dobra os efeitos da próxima carta), Análise Genética (custo 0, Pesquisa +1) e Distribuição de Insumos (custo 0, Infraestrutura +1).
2. **Montar a sequência.** Começar com Conhecimento 3, Pesquisa 5 e Infraestrutura 5. Permitir selecionar até 3 cartas e exibir a ordem. A soma de custos precisa caber no Conhecimento disponível.
3. **Ativar o efeito.** Ao executar Descoberta Científica, marcar que o próximo efeito de carta será dobrado. A própria Descoberta não aumenta recursos.
4. **Consumir uma vez.** A próxima carta aplica o dobro de seu efeito e consome a duplicação. Dobrar o benefício não dobra o custo nem cria uma segunda execução.
5. **Comparar.** Mostrar os valores antes e depois de cada carta. Permitir reiniciar e testar outra ordem com as mesmas cartas.

## Tela mínima

Cartas, fila numerada, Conhecimento, Pesquisa, Infraestrutura e indicação Duplicação ativa.

## Está pronto quando

- [ ] Descoberta + Análise: Pesquisa vai de 5 para 7, consumindo 3 de Conhecimento no total.
- [ ] Descoberta + Análise + Distribuição: Pesquisa termina em 7 e Infraestrutura em 6. Só a primeira carta após Descoberta foi dobrada.
- [ ] Análise + Descoberta: Pesquisa termina em 6; a duplicação fica aguardando a próxima carta. Reiniciar remove o efeito.
