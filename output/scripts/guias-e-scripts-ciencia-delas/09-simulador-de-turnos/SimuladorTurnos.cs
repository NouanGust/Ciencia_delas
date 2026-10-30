using UnityEngine;

// GUIA 09 - Simulador de Turnos
// Acompanha: 09-simulador-de-turnos.pdf
// Programar o controle das fases. Os demais sistemas podem usar dados de teste.
public class SimuladorTurnos : MonoBehaviour
{
    // VARIÁVEIS QUE A EQUIPE DEVE CRIAR
    // Número da rodada e fase atual: início, planejamento, execução, ameaças e fim.
    // Confirmação de cada jogador, Conhecimento, recursos e estado da partida.
    // Mãos, baralhos, descartes e fila global com dados simples de teste.
    // Ameaças no monte, ativas e superadas; histórico e referências da interface.
    // Use uma única representação de fase para evitar estados incompatíveis.

    private void Start()
    {
        // TODO: Preparar a partida e entrar na primeira rodada.
        // TODO: Mostrar fase, rodada, recursos e estado dos jogadores.
    }

    public void PrepararPartida()
    {
        // TODO: Inicializar Saúde 10, Pesquisa 5, Infraestrutura 5 e os dois jogadores.
        // TODO: Distribuir quatro cartas a cada jogador e limpar os descartes.
        // TODO: Preparar ameaças e zerar os registros de confirmações e ações anteriores.
        // Convenção do guia: as quatro cartas iniciais são a compra da primeira rodada.
    }

    public void IniciarRodada()
    {
        // TODO: Só iniciar se a partida não terminou e a fase permitir o avanço.
        // TODO: Restaurar o Conhecimento de cada jogador para 3/3.
        // TODO: A partir da segunda rodada, comprar uma carta por jogador, até o limite 6.
        // TODO: Revelar uma nova ameaça se ainda houver cartas no monte.
        // TODO: Limpar confirmações da rodada anterior e liberar o planejamento.
        // TODO: Impedir que outro clique repita compras ou revelação na mesma rodada.
    }

    public void SelecionarCartaJogador1(int indiceCarta)
    {
        // TODO: Aceitar somente durante o planejamento e antes da confirmação de J1.
        // TODO: Verificar carta, limite de três seleções e disponibilidade de custos.
        // TODO: Reservar e registrar a escolha na fila global.
    }

    public void SelecionarCartaJogador2(int indiceCarta)
    {
        // TODO: Aplicar as mesmas regras para J2, mantendo a ordem global.
        // TODO: Considerar as reservas compartilhadas feitas pelos dois jogadores.
    }

    public void DesmarcarCartaJogador1(int indiceCarta)
    {
        // TODO: Permitir somente no planejamento e antes da confirmação de J1.
        // TODO: Liberar a reserva da carta removida e renumerar a fila.
    }

    public void DesmarcarCartaJogador2(int indiceCarta)
    {
        // TODO: Permitir somente no planejamento e antes da confirmação de J2.
        // TODO: Preservar as demais cartas e reservas ao remover a escolha.
    }

    public void ConfirmarJogador1()
    {
        // TODO: Registrar que J1 está pronto, inclusive com seleção vazia.
        // TODO: Bloquear novas alterações de J1 e verificar a confirmação de J2.
    }

    public void ConfirmarJogador2()
    {
        // TODO: Registrar que J2 está pronto e bloquear suas alterações.
        // TODO: Liberar a execução somente quando ambos estiverem prontos.
    }

    public void ExecutarJogadas()
    {
        // TODO: Exigir a fase correta e as duas confirmações.
        // TODO: Mudar para execução e impedir novos cliques de planejamento.
        // TODO: Percorrer a fila global, cobrar cada custo e aplicar o efeito de teste.
        // TODO: Verificar derrota após cada alteração; se ocorrer, parar imediatamente.
        // TODO: Enviar as cartas usadas aos descartes de seus donos.
        // TODO: Quando terminar, liberar a fase de ameaças sem repetir a fila.
    }

    public void ResolverAmeacas()
    {
        // TODO: Executar somente depois de todas as cartas da fila.
        // TODO: Verificar ameaças por antiguidade, aplicar sucessos e reavaliar as ativas.
        // TODO: Aplicar uma vez as penalidades restantes, verificando derrota a cada mudança.
        // TODO: Dados e recompensas de teste devem ser identificados como simplificações.
        // TODO: Ao terminar, liberar a verificação final da rodada.
    }

    public void FinalizarRodada()
    {
        // TODO: Exigir que as fases anteriores tenham terminado.
        // TODO: Verificar vitória: nenhuma ameaça no monte nem entre as ativas.
        // TODO: Se ainda puder continuar, avançar o número da rodada e preparar a próxima.
        // TODO: Evitar que um clique repetido pule uma rodada ou execute outra fase.
    }

    public void VerificarDerrota()
    {
        // TODO: Conferir se Saúde, Pesquisa ou Infraestrutura chegou a zero.
        // TODO: Encerrar a partida imediatamente, independentemente da fase.
        // TODO: Bloquear avanço e ações; manter o botão Reiniciar disponível.
    }

    public void AtualizarInterface()
    {
        // TODO: Exibir rodada, fase atual, recursos e confirmações.
        // TODO: Habilitar apenas ações permitidas e registrar as transições no histórico.
    }

    public void Reiniciar()
    {
        // TODO: Limpar todas as fases, listas de teste, confirmações e bloqueios anteriores.
        // TODO: Voltar à preparação inicial e abrir a primeira rodada uma única vez.
    }

    // TESTES QUE A EQUIPE DEVE DEMONSTRAR
    // 1. Não executar antes de duas confirmações nem permitir seleção após confirmar.
    // 2. Não repetir compra, revelação ou cobrança; Conhecimento volta para 3/3.
    // 3. Recurso zero interrompe qualquer fase; vitória bloqueia avanço; reinício restaura tudo.
    // O exercício não depende dos scripts das outras equipes nem do jogo principal.
}
