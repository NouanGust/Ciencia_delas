using UnityEngine;

// GUIA 05 - Simulador de Combo
// Acompanha: 05-simulador-de-combo.pdf
// Combo aqui é a combinação e a ordem dos efeitos, sem pontuação extra inventada.
public class SimuladorCombo : MonoBehaviour
{
    // VARIÁVEIS QUE A EQUIPE DEVE CRIAR
    // Dados das três cartas: Descoberta Científica, Análise Genética e Distribuição.
    // Lista da sequência selecionada e estado de execução.
    // Conhecimento inicial 3, Pesquisa 5 e Infraestrutura 5.
    // Indicador de duplicação aguardando a próxima carta e histórico de efeitos.

    private void Start()
    {
        // TODO: Preparar as três cartas e os valores iniciais.
        // TODO: Exibir uma sequência vazia e duplicação desativada.
    }

    public void SelecionarCarta(int indiceCarta)
    {
        // TODO: Adicionar a carta na ordem de escolha, respeitando até três cartas.
        // TODO: Não selecionar a mesma carta duas vezes.
        // TODO: Conferir se a soma dos custos cabe nos três pontos disponíveis.
        // Custos do guia: Descoberta 3, Análise 0 e Distribuição 0.
    }

    public void DesmarcarCarta(int indiceCarta)
    {
        // TODO: Remover somente a carta escolhida enquanto a sequência não executou.
        // TODO: Atualizar a ordem e o custo total mostrado ao jogador.
    }

    public void ExecutarSequencia()
    {
        // TODO: Validar a seleção e impedir a repetição da mesma sequência.
        // TODO: Percorrer na ordem escolhida, cobrando cada custo na execução.
        // TODO: Acionar o efeito correspondente à carta atual.
        // TODO: Registrar valores antes e depois, preservando os limites dos recursos.
    }

    public void AplicarDescobertaCientifica()
    {
        // TODO: Ativar a duplicação para a próxima carta de efeito.
        // TODO: Não aumentar os recursos apenas por executar Descoberta.
        // Esta função é uma etapa da execução, não um botão de ativação gratuita.
    }

    public void AplicarAnaliseGenetica()
    {
        // TODO: Preparar o ganho normal de Pesquisa +1.
        // TODO: Verificar se a duplicação está ativa e ajustar somente este efeito.
        // TODO: Aplicar o ganho e consumir a duplicação, caso tenha sido usada.
    }

    public void AplicarDistribuicaoDeInsumos()
    {
        // TODO: Preparar o ganho normal de Infraestrutura +1.
        // TODO: Tratar a duplicação pendente da mesma maneira, sem repetir a carta.
        // TODO: Não dobrar o custo da carta nem beneficiar todas as cartas seguintes.
    }

    public void AtualizarInterface()
    {
        // TODO: Mostrar sequência numerada, custos, recursos e duplicação ativa.
        // TODO: Mostrar cada resultado no histórico para comparar ordens diferentes.
    }

    public void Reiniciar()
    {
        // TODO: Restaurar Conhecimento 3, Pesquisa 5 e Infraestrutura 5.
        // TODO: Limpar sequência, histórico, duplicação e bloqueio de execução.
    }

    // TESTES QUE A EQUIPE DEVE DEMONSTRAR
    // 1. Descoberta + Análise: Pesquisa termina em 7, com custo total 3.
    // 2. Descoberta + Análise + Distribuição: Pesquisa 7 e Infraestrutura 6.
    // 3. Análise + Descoberta: Pesquisa 6; duplicação aguarda outra carta.
    // O comportamento entre rodadas ou entre dois jogadores não faz parte deste recorte.
}
