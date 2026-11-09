# Editar a cena Ciência Delas

Abra `Assets/Scenes/CienciaDelas.unity`, com o botão Play desligado.

1. Expanda **Diretores Ciencia Delas > Interface - Ciencia Delas > Telas** na Hierarchy.
2. Selecione **Diretores Ciencia Delas**. No componente **Interface Diretor**, use os botões **Mostrar tela para edição**: Menu, Mesa, Pausa, Opcoes, Como jogar, Caderno, Vitoria e Derrota.
3. Selecione um objeto da tela. Edite sua posição e tamanho em **Rect Transform**, sua imagem e cor em **Image**, ou sua escrita e fonte em **TextMeshPro**. Use a ferramenta Rect (T) para mover e redimensionar visualmente.
4. Salve a cena com **Ctrl+S**. Pressione Play para testar. O jogo inicia no menu, independentemente da tela escolhida para edição.

Na Mesa, os grupos **Recursos**, **Ameacas**, **Jogadora 1**, **Jogadora 2** e **Inspecao da carta** facilitam a seleção. Cada mão tem seis espaços já preparados, de acordo com a configuração atual. As três ameaças visíveis são espaços reutilizados pelo carrossel: uma ameaça central (o alvo atual) e até duas prévias. As setas percorrem todas as ameaças ativas; clicar numa prévia a traz para o centro. Com duas ameaças, apenas uma prévia é exibida, sem duplicações. Os dados exibidos fora do Play são exemplos para orientar a edição.

## Textos, imagens e dados da partida

Os textos fixos, as imagens de fundo e os botões comuns são lidos da própria cena. O jogo preserva suas mudanças de posição, tamanho, fonte e aparência.

Valores de recursos, nomes de cartas, cientistas, requisitos e mensagens acompanham a partida. O componente **Elemento Interface** identifica essa ligação e oferece:

- **Atualizar Texto**: desative se quiser substituir um conteúdo variável por uma frase fixa.
- **Atualizar Imagem**: desative se quiser uma ilustração fixa no lugar da carta ou cientista atual.
- **Atualizar Cor**: desative para usar sua própria cor em um indicador que muda conforme a partida.
- **Ocultar**: marque para retirar um elemento da apresentação durante o jogo. A ativação normal de cartas e telas é controlada pela partida.

Para mudar os dados de todas as cartas, regras, tempos e sons, selecione o campo **Configuracao** do componente **Inicializador Jogo**. As ilustrações variáveis vêm da lista **Artes** desse arquivo. As alterações nesse arquivo valem para todas as cenas que o utilizam. Fundos, textos fixos e estilos que já estão na cena são editados diretamente nos objetos.

Os botões mantêm suas ações do jogo. Você pode acrescentar ações no evento **On Click()** do componente Button. Objetos decorativos novos, sem Elemento Interface, também são preservados. Ao duplicar um elemento para criar uma decoração, remova a cópia de Elemento Interface para não duplicar sua ligação com o jogo.

## Cuidados ao experimentar

Mudanças feitas durante Play são temporárias: saia do Play antes de editar algo que deseja salvar. Alterar números de cartas, regras ou textos pode exigir ajustar o layout para manter a legibilidade.

O menu **Ciência Delas > Preparar interface editável na cena aberta** completa a estrutura de uma cena com InicializadorJogo. Ele preserva os estilos existentes e pode adicionar elementos que estejam faltando; não é necessário usá-lo para editar a aparência. Salve a cena depois de utilizá-lo. Partículas e efeitos de curta duração continuam sendo gerados apenas durante a partida.

## Fonte e dicas dos ícones

A interface usa a fonte **Xmind SDF**, gerada a partir da Xmind que já estava no projeto. Os tamanhos continuam editáveis nos componentes TextMeshPro de cada objeto. O material dessa fonte não usa o contorno preto pesado da apresentação anterior.

Saúde, infraestrutura, pesquisa e conhecimento aparecem como ícones. Passe o mouse sobre eles para consultar o nome. Nos ícones do cabeçalho, o campo **Nome** do componente **Dica Interface** permite editar a dica. Nos textos, a marcação `<link="Infraestrutura"><sprite name="Infraestrutura" tint=0></link>` associa o ícone ao nome da dica.

O painel das dicas está em **Interface - Ciencia Delas > Tooltips > Fundo da dica**. Sua fonte, cores e espaçamento são editáveis ali; **Tooltip Diretor** controla o afastamento do cursor e a largura máxima. O painel acompanha o mouse, permanece dentro da tela e não bloqueia os cliques.

O comando **Ciência Delas > Aplicar fonte e ícones legíveis** reaplica a proposta de tipografia e disposição dos elementos. Use-o apenas se quiser voltar a essa proposta, pois ele substitui ajustes de tamanho, fonte e posição feitos depois dela.

## Mesa com carrossel

A rodada aparece acima das ameaças; a fila fica no centro e as duas mãos na parte inferior. Saúde, pesquisa e infraestrutura ficam à esquerda. Pausa, caderno e progresso ficam à direita.

A descrição da carta abre em um tooltip ao passar o mouse ou selecioná-la pelo teclado, com nome, conhecimento e custos. Ela fecha ao sair, perder o foco ou trocar de tela, sem bloquear cliques. O painel fixo de inspeção anterior fica inativo.

O comando **Ciência Delas > Aplicar mesa com carrossel** aplica esta disposição aos objetos da Mesa. Ele substitui as posições e tamanhos dessa tela, mantendo as demais telas. Use-o somente para reaplicar a proposta; alterações manuais posteriores continuam preservadas durante o jogo. Salve a cena depois de aplicá-lo.

## Teclado para dois jogadores

- J1: A/D percorrem apenas as cartas e o botão de confirmação de J1; Z seleciona, retira a carta já selecionada ou aciona a confirmação em foco.
- J2: setas esquerda/direita percorrem apenas as cartas e o botão de confirmação de J2; Enter seleciona, retira ou confirma.
- Cada mão mantém seu próprio foco, inclusive ao alternar entre teclado e mouse. X cancela a carta em foco de J1; Backspace cancela a de J2.
- Q/E percorrem as ameaças. Tab abre o caderno; Esc pausa ou volta à mesa. Nos menus, WASD/setas e Z/Enter mantêm a navegação comum.

Os ícones vêm de **Art > UI > KEYBOARD KEY (16X16)**. O comando **Ciência Delas > Preparar ícones e controles dos jogadores** associa os sprites à configuração e prepara as legendas editáveis da cena. A fila mostra as orientações da rodada; cada mão tem sua legenda de comandos logo acima.
