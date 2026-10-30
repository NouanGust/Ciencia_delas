using UnityEngine;

// GUIA 01 - Simulador de baralho
// Acompanha: 01-simulador-de-baralho.pdf
// Este arquivo é um roteiro: os métodos estão vazios de propósito.
// Crie um projeto separado e coloque este componente em um GameObject.
public class SimuladorBaralho : MonoBehaviour
{
    // VARIÁVEIS QUE A EQUIPE DEVE CRIAR
    // Dados de uma carta: identificador, nome, tipo, cientista e custo.
    // Lista de cartas normais: 10 modelos usados pelas duas cientistas.
    // Listas de especiais: 5 de Marie Curie e 5 de Jaqueline Goes.
    // Listas dos baralhos em uso: Marie, Jaqueline e as 15 ameaças.
    // Guarde os dados originais separadamente para permitir o reinício.
    // Crie referências para os textos e áreas de exibição na interface.

    private void Start()
    {
        // TODO: Decidir como cadastrar os dados: Inspector ou dados criados pela equipe.
        // TODO: Preparar as cartas e montar os três baralhos.
        // TODO: Mostrar a composição e a quantidade de cada baralho.
    }

    public void CadastrarCartas()
    {
        // TODO: Consultar os nomes e custos nas páginas 8 a 11 do GDD.
        // TODO: Criar os dados das normais, especiais e ameaças.
        // TODO: Não programar os efeitos aqui; este exercício organiza as cartas.
    }

    public void MontarBaralhos()
    {
        // TODO: Montar 10 normais + 5 especiais para cada cientista.
        // TODO: Criar uma identidade própria para cada cópia das cartas normais.
        // TODO: Manter as 15 ameaças em outro baralho.
        // TODO: Evitar que as duas cientistas compartilhem a mesma lista em uso.
    }

    public void EmbaralharMarie()
    {
        // TODO: Alterar a ordem das cartas do baralho de Marie.
        // TODO: Preservar a quantidade e os identificadores existentes.
        // TODO: Atualizar a interface depois de terminar.
    }

    public void EmbaralharJaqueline()
    {
        // TODO: Alterar somente a ordem do baralho de Jaqueline.
        // TODO: Pensar em como reaproveitar a lógica de embaralhamento.
    }

    public void EmbaralharAmeacas()
    {
        // TODO: Embaralhar as ameaças sem misturá-las às cartas das cientistas.
        // TODO: Atualizar a lista que aparece na tela.
    }

    public void ValidarBaralhos()
    {
        // TODO: Conferir 15 cartas em cada um dos três baralhos.
        // TODO: Conferir as 10 normais e as 5 especiais corretas de cada cientista.
        // TODO: Informar erros de composição, identificadores repetidos ou cartas ausentes.
    }

    public void AtualizarInterface()
    {
        // TODO: Mostrar nome, tipo, ordem e quantidade de cartas em cada lista.
        // TODO: Atualizar os elementos existentes sem duplicar a exibição a cada clique.
    }

    public void Reiniciar()
    {
        // TODO: Recuperar a composição original dos três baralhos.
        // TODO: Limpar o estado anterior antes de montar novamente.
        // TODO: Atualizar a tela.
    }

    // TESTES QUE A EQUIPE DEVE DEMONSTRAR
    // 1. Os três baralhos têm 15 cartas e a composição correta.
    // 2. Embaralhar várias vezes não perde nem duplica identificadores.
    // 3. Embaralhar uma cientista não altera a outra; Reiniciar restaura as listas.
}
