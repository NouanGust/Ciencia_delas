using UnityEngine;

// GUIA 04 - Simulador de Efeitos
// Acompanha: 04-simulador-de-efeitos.pdf
// Cada botão abaixo deve representar uma carta ou uma mudança de fase.
public class SimuladorEfeitos : MonoBehaviour
{
    // VARIÁVEIS QUE A EQUIPE DEVE CRIAR
    // Saúde, Pesquisa, Infraestrutura, rodada e fase atual.
    // Dados dos efeitos ativos, duração restante e momento de aplicação.
    // Identificação da ameaça revelada e da anulação válida nesta rodada.
    // Histórico e referências para os textos da interface.
    // Valores iniciais: Saúde 10 / Pesquisa 5 / Infraestrutura 5; limites 0 a 30.

    private void Start()
    {
        // TODO: Preparar os valores iniciais e uma ameaça de teste identificada.
        // TODO: Começar com a rodada inicial aberta e sem efeitos ativos.
        // TODO: Exibir recursos, fase e histórico vazio.
    }

    public void IntervencaoEmergencial()
    {
        // TODO: Aplicar Saúde +2 e respeitar o limite de 30.
        // TODO: Registrar a mudança e atualizar a interface.
    }

    public void EnergiaNuclear()
    {
        // TODO: Aplicar Pesquisa +2 e Infraestrutura +1.
        // TODO: Atualizar os dois valores e mostrar o que mudou.
    }

    public void ExperimentoAvancado()
    {
        // TODO: Registrar um efeito para o fim de três rodadas.
        // TODO: Em cada aplicação, dar +1 em Saúde, Pesquisa e Infraestrutura.
        // TODO: Não aplicar os três ganhos de duração de uma só vez ao ativar.
        // Convenção do guia: o fim da rodada de ativação já conta como uma aplicação.
        // Não permitir reativar este efeito enquanto ainda estiver ativo.
    }

    public void LegadoCientifico()
    {
        // TODO: Registrar Pesquisa +1 no início das próximas duas rodadas.
        // TODO: Não conceder o benefício no clique de ativação.
        // TODO: Impedir reativação enquanto o efeito ainda estiver ativo.
    }

    public void SequenciamentoGenetico()
    {
        // TODO: Marcar a ameaça revelada como anulada na rodada atual.
        // TODO: Não anular automaticamente todas as ameaças nem todas as rodadas.
    }

    public void IniciarRodada()
    {
        // TODO: Exigir que a rodada anterior tenha terminado antes de abrir outra.
        // TODO: Avançar a rodada e expirar a anulação da rodada passada.
        // TODO: Aplicar uma vez os efeitos de início, incluindo Legado Científico.
        // TODO: Atualizar suas durações depois da aplicação e remover os expirados.
    }

    public void EncerrarRodada()
    {
        // TODO: Impedir que cliques repetidos encerrem a mesma rodada várias vezes.
        // TODO: Aplicar uma vez os efeitos de fim, incluindo Experimento Avançado.
        // TODO: Atualizar durações e resolver a penalidade de teste da ameaça.
        // TODO: Manter a anulação válida até a resolução dessa rodada terminar.
    }

    public void ResolverPenalidadeDaAmeaca()
    {
        // TODO: Verificar se esta ameaça está anulada nesta rodada.
        // TODO: Aplicar ou bloquear a penalidade de teste e registrar o motivo.
        // TODO: Respeitar os limites dos recursos e reconhecer zero como derrota.
    }

    public void AtualizarInterface()
    {
        // TODO: Mostrar recursos, rodada, fase, efeitos ativos e duração restante.
        // TODO: Informar no histórico qual efeito atuou ou expirou.
    }

    public void Reiniciar()
    {
        // TODO: Restaurar os valores, a fase inicial e a ameaça de teste.
        // TODO: Limpar histórico, durações, anulações e qualquer estado de derrota.
    }

    // TESTES QUE A EQUIPE DEVE DEMONSTRAR
    // 1. Energia Nuclear: Pesquisa 5 vira 7 e Infraestrutura 5 vira 6; teto é 30.
    // 2. Experimento atua três vezes; Legado atua duas; os efeitos então expiram.
    // 3. Sequenciamento bloqueia a ameaça revelada nesta rodada, não na seguinte.
}
