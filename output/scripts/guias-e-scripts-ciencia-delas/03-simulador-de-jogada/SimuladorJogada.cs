using UnityEngine;

// GUIA 03 - Simulador de jogada
// Acompanha: 03-simulador-de-jogada.pdf
// Os métodos oferecem uma estrutura de trabalho, sem implementar as regras.
public class SimuladorJogada : MonoBehaviour
{
    // VARIÁVEIS QUE A EQUIPE DEVE CRIAR
    // Mão, seleção ordenada e descarte.
    // Conhecimento real, reservado e disponível; o saldo inicial é 3.
    // Estado da jogada: planejando, confirmada ou já executada.
    // Referências para cartas, numeração, textos e botões.

    private void Start()
    {
        // TODO: Preparar a mão usando cartas e custos do guia.
        // TODO: Começar com Conhecimento 3 e nenhuma reserva ou seleção.
        // TODO: Mostrar o estado inicial.
    }

    public void SelecionarCarta(int indiceCarta)
    {
        // TODO: Verificar se ainda está planejando e se a posição da mão existe.
        // TODO: Impedir carta repetida, quarta seleção ou custo acima do disponível.
        // TODO: Guardar a carta na ordem em que foi escolhida.
        // TODO: Recalcular reservas e atualizar a numeração, sem gastar o saldo real.
    }

    public void DesmarcarCarta(int indiceCarta)
    {
        // TODO: Permitir remoção somente antes de confirmar.
        // TODO: Remover apenas a carta indicada, preservando as demais seleções.
        // TODO: Liberar a reserva correspondente e renumerar a fila.
    }

    public void RecalcularReserva()
    {
        // TODO: Conferir os custos das cartas que continuam selecionadas.
        // TODO: Determinar o valor reservado e quanto ainda está disponível.
        // TODO: Manter o saldo real separado; reservar não significa consumir.
    }

    public void ConfirmarJogada()
    {
        // TODO: Bloquear mudanças na seleção, sem consumir os custos.
        // TODO: Aceitar seleção vazia como passagem de vez.
        // TODO: Não permitir que novas confirmações repitam uma operação.
    }

    public void ExecutarJogada()
    {
        // TODO: Exigir confirmação e impedir uma segunda execução.
        // TODO: Percorrer as cartas na ordem escolhida.
        // TODO: Cobrar cada custo somente agora, registrar o uso e mover ao descarte.
        // TODO: Evitar alterar uma lista durante uma repetição de forma insegura.
        // TODO: Encerrar a jogada, limpar reservas consumidas e atualizar a tela.
        // Neste exercício, este botão simula a liberação pelo outro jogador.
    }

    public void AtualizarInterface()
    {
        // TODO: Mostrar mão, fila numerada, saldo real, reserva e disponível.
        // TODO: Indicar se o jogador está planejando, confirmado ou já executou.
        // TODO: Bloquear botões incompatíveis com o estado atual.
    }

    public void Reiniciar()
    {
        // TODO: Restaurar mão e Conhecimento, limpar descarte, seleção e reservas.
        // TODO: Liberar o planejamento de uma nova demonstração.
    }

    // TESTES QUE A EQUIPE DEVE DEMONSTRAR
    // 1. Custos 2 e 1: saldo real 3 / reserva 3 / disponível 0.
    // 2. Cancelar o custo 2 mantém o custo 1 selecionado; quarta carta é recusada.
    // 3. Confirmar não cobra; Executar cobra uma vez, mesmo com cliques repetidos.
}
