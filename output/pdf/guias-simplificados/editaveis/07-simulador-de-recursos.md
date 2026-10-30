# Simulador de Recursos

Projeto independente em Unity e C#

Controlar os recursos compartilhados e separar o Conhecimento de cada jogador.

## O que desenvolver

1. **Inicializar.** Saúde começa em 10, Infraestrutura em 5 e Pesquisa em 5. Esses três valores pertencem à equipe. Cada jogador possui seu próprio Conhecimento, inicialmente 3/3.
2. **Alterar valores.** Criar ações de ganho e perda. Limitar Saúde, Pesquisa e Infraestrutura ao intervalo de 0 a 30. Mostrar o valor real, a reserva e o disponível.
3. **Reservar.** Separar reserva de consumo. Reservar torna o valor indisponível; cancelar libera a reserva; executar efetiva o gasto. O total de reservas nunca pode exceder o saldo.
4. **Reconhecer derrota.** Após qualquer consumo ou perda efetiva, verificar se algum recurso compartilhado chegou a zero. Encerrar imediatamente. Conhecimento zero, sozinho, não causa derrota.
5. **Restaurar.** No início de cada rodada, restaurar o Conhecimento de ambos para 3/3, sem acumular. Reiniciar deve recuperar todos os valores iniciais e limpar reservas.

## Tela mínima

Painel dos três recursos, Conhecimento dos dois jogadores e botões Ganhar, Perder, Reservar, Cancelar, Executar, Nova rodada e Reiniciar.

## Está pronto quando

- [ ] Um ganho de 5 sobre Saúde 29 resulta em 30. Uma perda de 4 sobre Saúde 2 resulta em 0 e derrota.
- [ ] Reservar toda a Pesquisa inicial deixa saldo real 5 e disponível 0, sem derrota. Executar esse gasto leva o saldo a zero e encerra.
- [ ] Gastar Conhecimento do jogador 1 não altera o do jogador 2. Nova rodada restaura ambos para 3.
