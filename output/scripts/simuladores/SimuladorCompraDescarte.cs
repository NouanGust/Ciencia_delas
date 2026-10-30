using UnityEngine;

// GUIA 02 - Simulador de compra e descarte
// Acompanha: 02-simulador-de-compra-e-descarte.pdf
// Este arquivo não contém a lógica pronta. Implemente os comentários TODO.
public class SimuladorCompraDescarte : MonoBehaviour
{
    // VARIÁVEIS QUE A EQUIPE DEVE CRIAR
    // Dados originais das 15 cartas e listas de baralho, mão e cemitério.
    // Identificação da carta selecionada e referências da interface.
    // Valores de referência: mão inicial 4, limite da mão 6.
    // Cada carta deve estar em apenas uma lista por vez.

    private void Start()
    {
        // TODO: Preparar as 15 cartas, embaralhar e distribuir a mão inicial.
        // TODO: Exibir as três áreas com seus contadores.
        // TODO: Não comprar uma carta extra junto da distribuição inicial.
    }

    public void PrepararBaralho()
    {
        // TODO: Construir o baralho a partir dos dados originais.
        // TODO: Limpar mão, cemitério e seleção de uma partida anterior.
        // TODO: Embaralhar sem alterar os dados originais.
    }

    public void DistribuirMaoInicial()
    {
        // TODO: Retirar quatro cartas do baralho e colocá-las na mão.
        // TODO: Mover as cartas; não manter a mesma carta em duas listas.
    }

    public void NovaRodada()
    {
        // TODO: Solicitar a compra de uma carta e atualizar a tela.
        // TODO: Respeitar o limite de seis cartas na mão.
        // REGRA DO GUIA: compra +1, conforme páginas 5 e 7 do GDD.
        // A página 4 traz outra regra; não misturar as duas neste exercício.
    }

    public void ComprarCarta()
    {
        // TODO: Verificar espaço na mão antes de retirar qualquer carta.
        // TODO: Se o baralho estiver vazio, tentar repô-lo com o cemitério.
        // TODO: Se não houver carta disponível, informar sem acessar uma lista vazia.
        // TODO: Mover uma carta do baralho para a mão.
    }

    public void SelecionarCarta(int indiceCarta)
    {
        // indiceCarta representa uma posição da mão; a primeira posição é zero.
        // TODO: Verificar se a posição existe e guardar qual carta foi escolhida.
        // TODO: Mostrar visualmente a seleção. Selecionar ainda não descarta.
    }

    public void ExecutarCartaSelecionada()
    {
        // TODO: Verificar se existe uma seleção válida.
        // TODO: Simular a execução e depois mover essa carta da mão ao cemitério.
        // TODO: Limpar a seleção para que outro clique não execute a mesma carta.
        // TODO: Atualizar a tela; não é necessário aplicar o efeito real.
    }

    public void ReporBaralho()
    {
        // TODO: Quando o monte acabar, transferir o cemitério para o baralho.
        // TODO: Embaralhar o novo monte e deixar o cemitério vazio.
        // TODO: Tratar o caso de ambos estarem vazios sem inventar cartas.
    }

    public void AtualizarInterface()
    {
        // TODO: Mostrar cartas, contadores e seleção nas três áreas.
        // TODO: Se usar prefab, preencher os dados da instância criada, não do modelo.
        // TODO: A exibição não deve adicionar cartas às listas de dados novamente.
    }

    public void Reiniciar()
    {
        // TODO: Recuperar as 15 cartas originais e distribuir quatro novamente.
        // TODO: Limpar seleção, objetos visuais antigos e cemitério.
    }

    // TESTES QUE A EQUIPE DEVE DEMONSTRAR
    // 1. Inicialização: mão 4 / baralho 11 / cemitério 0. Mão 6 bloqueia compra.
    // 2. Executar move uma carta para o cemitério; a soma das três áreas continua 15.
    // 3. Com mão 4 / baralho 0 / cemitério 11, comprar resulta em 5 / 10 / 0.
}
