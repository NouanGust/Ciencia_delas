using UnityEngine;

// GUIA 07 - Simulador de Recursos
// Acompanha: 07-simulador-de-recursos.pdf
// Defina os dados e implemente a lógica a partir dos comentários.
public class SimuladorRecursos : MonoBehaviour
{
    // VARIÁVEIS QUE A EQUIPE DEVE CRIAR
    // Saúde inicial 10, Pesquisa 5 e Infraestrutura 5, compartilhadas pela equipe.
    // Conhecimento individual de dois jogadores: 3 para cada um.
    // Reservas identificadas: dono, recurso e quantidade, permitindo cancelar uma só.
    // Valores para os controles de teste: recurso, quantidade e jogador escolhidos.
    // Estado da partida, referências dos textos e histórico.

    private void Start()
    {
        // TODO: Preparar os recursos, os dois Conhecimentos e as reservas vazias.
        // TODO: Mostrar saldo real, reservado e disponível de cada recurso.
    }

    public void GanharRecurso()
    {
        // TODO: Ler qual recurso e qual quantidade foram escolhidos na interface.
        // TODO: Validar a entrada, aplicar o ganho e respeitar o teto de 30.
        // TODO: Atualizar os valores exibidos.
    }

    public void PerderRecurso()
    {
        // TODO: Validar o recurso e a quantidade escolhidos.
        // TODO: Aplicar a perda, impedir resultado negativo e verificar derrota.
        // TODO: Para este teste isolado, não aplicar perdas avulsas durante uma reserva
        // pendente; concluir ou cancelar a reserva primeiro para não criar conflito.
    }

    public void ReservarRecurso()
    {
        // TODO: Verificar a quantidade disponível considerando as reservas existentes.
        // TODO: Registrar a reserva com sua identificação sem descontar do saldo real.
        // TODO: Permitir reservar o saldo inteiro, mesmo deixando disponível zero.
        // TODO: Não declarar derrota apenas por não haver valor disponível.
    }

    public void CancelarReserva(int indiceReserva)
    {
        // TODO: Conferir se a reserva existe e remover somente a indicada.
        // TODO: Liberar o valor reservado sem acrescentá-lo ao saldo real.
        // TODO: Atualizar a interface; cancelar não gera recursos novos.
    }

    public void ExecutarReserva(int indiceReserva)
    {
        // TODO: Cobrar efetivamente o valor e remover a reserva consumida.
        // TODO: Impedir executar a mesma reserva novamente.
        // TODO: Se o gasto zerar um recurso compartilhado, registrar derrota imediata.
        // O GDD permite um gasto fatal; não recusar só porque o resultado será zero.
    }

    public void GastarConhecimentoJogador1(int quantidade)
    {
        // TODO: Validar a quantidade e o saldo do jogador 1 antes de gastar.
        // TODO: Não alterar o Conhecimento do jogador 2.
        // TODO: Conhecimento zero não causa derrota.
    }

    public void GastarConhecimentoJogador2(int quantidade)
    {
        // TODO: Aplicar as mesmas verificações, usando o saldo do jogador 2.
        // TODO: Manter os dois saldos independentes e atualizar a tela.
    }

    public void NovaRodada()
    {
        // TODO: Restaurar ambos os Conhecimentos para 3, sem somar sobre o saldo anterior.
        // TODO: Não restaurar automaticamente Saúde, Pesquisa ou Infraestrutura.
        // TODO: Exigir reservas resolvidas ou canceladas antes de abrir outra rodada.
    }

    public void VerificarDerrota()
    {
        // TODO: Conferir os valores reais dos três recursos compartilhados.
        // TODO: Encerrar se algum for zero, informar o motivo e bloquear ações.
        // TODO: Reiniciar deve continuar disponível após a derrota.
    }

    public void AtualizarInterface()
    {
        // TODO: Exibir saldos reais, reservas, disponíveis e Conhecimento por jogador.
        // TODO: Mostrar mensagens para entradas inválidas e para o fim da partida.
    }

    public void Reiniciar()
    {
        // TODO: Restaurar 10/5/5, Conhecimento 3/3 e remover todas as reservas.
        // TODO: Limpar o resultado anterior e liberar as ações de teste.
    }

    // TESTES QUE A EQUIPE DEVE DEMONSTRAR
    // 1. Saúde 29 com ganho 5 resulta em 30; Saúde 2 com perda 4 resulta em 0 e derrota.
    // 2. Reservar Pesquisa 5 não derrota; executar esse gasto zera o saldo e derrota.
    // 3. Conhecimentos são independentes; NovaRodada restaura cada um para 3.
}
