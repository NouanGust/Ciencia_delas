# Guia 05 - Editor e validador de cartas

Unity + C# | Intermediário | 2 a 4 integrantes | Entrega: 06/11/2026

## Objetivo

Criar uma ferramenta para cadastrar cartas e detectar erros antes que os dados sejam usados em um jogo.

## Mínimo obrigatório

Formulário, lista de cartas, edição, exclusão, exportação e importação JSON. Entregar um arquivo com dez cartas válidas e outro com erros de teste.

## Dados

Campos: id único, nome, descrição, custo inteiro de 0 a 3, efeito (GanharSaude/GanharPesquisa/GanharInfraestrutura) e quantidade inteira de 1 a 30. O JSON deve guardar uma coleção de cartas.

## Etapas

1. **Definir o modelo:** Escreva um exemplo de registro e escolha como mostrar cada campo no formulário. Não misture dados da carta com referências a objetos da cena.
2. **Cadastrar:** Ao salvar, rejeite id ou nome vazios, id repetido, descrição vazia, custo inválido, efeito desconhecido e quantidade fora do intervalo.
3. **Editar e excluir:** Permita selecionar uma carta para edição. Ao verificar id duplicado, ignore o próprio registro editado. Peça confirmação antes de excluir.
4. **Exportar:** Salve as dez cartas em JSON e mostre ao usuário onde o arquivo foi gravado. Use uma pasta permitida para dados do aplicativo.
5. **Importar com segurança:** Leia e valide todo o arquivo antes de substituir a lista atual. Em erro de leitura, estrutura ou conteúdo, preserve os dados anteriores e explique o problema.
6. **Conferir a ida e volta:** Exporte, reinicie ou limpe a lista, importe e compare todos os campos. Escreva instruções para localizar os arquivos.

## Testes obrigatórios

- [ ] **Cadastrar dez cartas válidas:** Todas aparecem na lista com os campos corretos.
- [ ] **Informar custo -1, custo 4 ou nome vazio:** O registro é recusado com uma mensagem específica.
- [ ] **Cadastrar id já existente e editar o mesmo registro sem mudar seu id:** O novo duplicado é recusado; a edição do próprio registro é aceita.
- [ ] **Importar JSON quebrado ou efeito desconhecido:** O erro é informado e a lista anterior permanece intacta.
- [ ] **Exportar e importar novamente:** Quantidade de cartas, ids, acentos, custos e efeitos permanecem iguais.

## Divisão do trabalho

Uma pessoa implementa dados e validação; outra implementa formulário e lista. Troquem para integrar importação e exportação.

## Limites

Não implementar os efeitos das cartas nem carregar o arquivo no jogo principal. O formato é próprio deste exercício e precisa ser documentado.

## Desafio opcional

Adicionar busca, filtros ou um relatório que reúna todos os erros de um arquivo.

## Entrega

Código-fonte, executável completo, README.md, testes.md e contribuicoes.md. Todos programam e explicam uma parte do código. Executar os cinco testes e solicitar teste de outra equipe.

## Marcos

- **Até 23/10:** Projeto criado e primeira interação funcionando.
- **Até 28/10:** Regra principal completa com interface provisória.
- **Até 30/10:** Primeiro executável entregue para outra equipe testar.
- **03–04/11:** Correções e testes dos casos extremos.
- **05/11:** Pacote final e ensaio da demonstração.
- **06/11:** Entrega e apresentação do programa.
