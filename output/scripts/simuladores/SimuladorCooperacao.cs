using UnityEngine;

// GUIA 08 - Simulador de Cooperação
// Acompanha: 08-simulador-de-cooperacao.pdf
// Usar dois painéis no mesmo computador; não é necessário programar rede.
public class SimuladorCooperacao : MonoBehaviour
{
    // VARIÁVEIS QUE A EQUIPE DEVE CRIAR
    // Mãos e seleções de cada jogador, Conhecimento 3 por jogador e confirmações.
    // Saúde 10, Pesquisa 5 e Infraestrutura 5, compartilhadas.
    // Reservas de Conhecimento por jogador e reservas dos recursos compartilhados.
    // Fila global com identidade da carta, dono e ordem de seleção.
    // Estado que informa se a jogada já foi executada; referências da interface.

    private void Start()
    {
        // TODO: Preparar dois jogadores, os recursos e algumas cartas de teste.
        // TODO: Deixar ambos em planejamento, sem confirmações e sem reservas.
        // TODO: Exibir os dois painéis e a fila global vazia.
    }

    public void SelecionarCartaJogador1(int indiceCarta)
    {
        // TODO: Verificar se J1 ainda pode planejar, se a carta existe e se já foi escolhida.
        // TODO: Respeitar três seleções e os custos disponíveis.
        // TODO: Reservar os custos e registrar a escolha na fila global.
    }

    public void SelecionarCartaJogador2(int indiceCarta)
    {
        // TODO: Fazer as mesmas verificações usando a mão e o Conhecimento de J2.
        // TODO: Considerar também o que J1 já reservou dos recursos compartilhados.
        // TODO: Registrar na mesma fila global, preservando a ordem dos cliques.
    }

    public void DesmarcarCartaJogador1(int indiceCarta)
    {
        // TODO: Permitir apenas enquanto J1 não confirmou.
        // TODO: Remover sua carta, liberar somente seus custos e renumerar a fila global.
    }

    public void DesmarcarCartaJogador2(int indiceCarta)
    {
        // TODO: Permitir apenas enquanto J2 não confirmou.
        // TODO: Preservar as cartas e reservas restantes, inclusive as de J1.
    }

    public void RecalcularReservas()
    {
        // TODO: Separar a reserva de Conhecimento de cada jogador.
        // TODO: Somar os custos compartilhados escolhidos pelos dois.
        // TODO: Não descontar do saldo real durante o planejamento.
        // Para o teste de Infraestrutura 4, use dados identificados como exercício;
        // esse custo não representa uma nova carta oficial do GDD.
    }

    public void ConfirmarJogador1()
    {
        // TODO: Bloquear apenas a seleção de J1 e mostrar que ele está pronto.
        // TODO: Permitir confirmar vazio para passar.
        // TODO: Verificar se os dois já confirmaram; J2 pode continuar se ainda não confirmou.
    }

    public void ConfirmarJogador2()
    {
        // TODO: Bloquear apenas a seleção de J2 e aceitar passagem sem cartas.
        // TODO: Conferir as duas confirmações antes de liberar a execução.
    }

    public void ExecutarFilaGlobal()
    {
        // TODO: Exigir as duas confirmações e impedir uma segunda execução.
        // TODO: Executar na ordem global, cobrando cada custo de seu dono ou do recurso comum.
        // TODO: Simular os efeitos e registrar a ordem; não é preciso programar todos.
        // TODO: Consumir as reservas correspondentes e encerrar a jogada.
        // TODO: Se um recurso compartilhado zerar, interromper com derrota.
    }

    public void AtualizarInterface()
    {
        // TODO: Mostrar cada mão, Conhecimento e estado Pronto/Aguardando.
        // TODO: Mostrar recursos reais, reservas compartilhadas e fila numerada.
        // TODO: Uma remoção renumera a fila, sem permitir alterar a seleção já confirmada.
    }

    public void Reiniciar()
    {
        // TODO: Restaurar dados iniciais e limpar seleções, reservas e confirmações.
        // TODO: Limpar a fila, o histórico e qualquer bloqueio de execução.
    }

    // TESTES QUE A EQUIPE DEVE DEMONSTRAR
    // 1. Só J1 confirmado não executa; os dois confirmados liberam a execução.
    // 2. Selecionar J1-A, J2-B, J1-C gera A/B/C; remover B deixa A/C numeradas 1/2.
    // 3. Infraestrutura 5: reservar 4 para J1 impede reservar mais 4 para J2.
    //    Cancelar a reserva de J1 libera o valor para uma nova seleção de J2.
}
