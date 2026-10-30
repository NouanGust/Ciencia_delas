using UnityEngine;

// GUIA 06 - Simulador de ameaças
// Acompanha: 06-simulador-de-ameacas.pdf
// As regras devem ser implementadas pela equipe nos espaços abaixo.
public class SimuladorAmeacas : MonoBehaviour
{
    // VARIÁVEIS QUE A EQUIPE DEVE CRIAR
    // Dados da ameaça: id, nome, requisitos, comparadores, penalidades e recompensa.
    // Alguns desafios exigem mais de um recurso: não limitar os dados a um único campo.
    // Listas do baralho, das ativas por antiguidade e do cemitério definitivo.
    // Recursos iniciais: Saúde 10, Pesquisa 5 e Infraestrutura 5.
    // Estado da rodada: revelação disponível, resolução pendente ou já concluída.
    // Estado da partida e referências da interface.

    private void Start()
    {
        // TODO: Cadastrar os desafios das páginas 10 e 11 do GDD.
        // TODO: Preparar recursos, baralho e listas vazias de ativas e superadas.
        // TODO: Liberar a primeira revelação.
    }

    public void CadastrarAmeacas()
    {
        // TODO: Preparar as 15 ameaças; iniciar o desenvolvimento com as três do guia.
        // TODO: Diferenciar Possuir, Gastar e o comparador de cada requisito.
        // TODO: Guardar requisitos e penalidades em campos separados.
        // O GDD não define recompensas: deixar configuráveis, inicialmente em zero.
        // Convenção do guia: +7 significa pelo menos 7; o sinal = permanece igualdade.
    }

    public void RevelarAmeaca()
    {
        // TODO: Impedir mais de uma revelação na mesma rodada ou após fim de jogo.
        // TODO: Mover uma carta do baralho para o fim da lista de ativas.
        // TODO: Se o monte acabou, permitir resolver as ativas restantes sem criar carta.
        // TODO: Marcar a rodada como aguardando resolução.
    }

    public void ResolverRodada()
    {
        // TODO: Exigir uma rodada pendente e impedir resolver a mesma rodada duas vezes.
        // TODO: Verificar as ativas da mais antiga para a mais recente.
        // TODO: Possuir apenas confere; Gastar consome somente quando pode ser atendido.
        // TODO: Ao superar, retirar das ativas, aplicar recompensa e guardar no cemitério.
        // TODO: Reavaliar as restantes após um sucesso, pois os recursos podem ter mudado.
        // TODO: Terminar as reavaliações quando não houver novos sucessos.
        // TODO: Aplicar as penalidades restantes uma vez e verificar o resultado da partida.
        // TODO: Se a partida continuar, liberar a rodada seguinte.
    }

    public void VerificarAmeaca(int indiceAmeaca)
    {
        // TODO: Conferir se a posição existe e examinar todos os requisitos dessa ameaça.
        // TODO: Decidir como devolver a conclusão à resolução: este método é só um esqueleto.
        // TODO: Ao consumir recursos, verificar imediatamente se algum chegou a zero.
        // TODO: Não consumir um requisito Possuir nem cobrar duas vezes uma ameaça.
    }

    public void AplicarPenalidades()
    {
        // TODO: Aplicar apenas as penalidades das ameaças que continuam ativas.
        // TODO: Respeitar o intervalo 0 a 30 e parar se ocorrer derrota.
        // TODO: Não reaplicar a penalidade por causa de um clique repetido em Resolver.
    }

    public void VerificarFimDeJogo()
    {
        // TODO: Derrota quando qualquer recurso compartilhado chegar a zero.
        // TODO: Vitória apenas quando o baralho e a lista de ativas estiverem vazios.
        // TODO: Bloquear novas ações após qualquer encerramento.
    }

    public void AtualizarInterface()
    {
        // TODO: Mostrar as três listas, recursos, rodada, requisitos e histórico.
        // TODO: Indicar o que foi superado, o que permanece ativo e o resultado final.
    }

    public void Reiniciar()
    {
        // TODO: Restaurar os recursos e as 15 ameaças originais.
        // TODO: Limpar ativas, cemitério, histórico e bloqueios da rodada e da partida.
    }

    // TESTES QUE A EQUIPE DEVE DEMONSTRAR
    // 1. Pesquisa 7 supera Primeiros Casos sem gastar Pesquisa.
    // 2. Origem Desconhecida: Pesquisa 3 vira 1; com Pesquisa 1, falha e perde Saúde 1.
    // 3. Monte vazio com ameaça ativa não é vitória; superadas não voltam ao monte.
}
