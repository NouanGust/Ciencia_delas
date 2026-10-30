# Verificação — 30/10/2026

Unity 6000.3.15f1: importação, compilação e verificação de referências concluídas.

Regras: 16 cenários e 100 simulações aprovados dentro do Unity. Relatório: regras.txt.

Visual: 35 referências de cartas/ameaças válidas, com fundos, cientistas, botões e duas sequências de efeitos. Quatro ameaças reutilizam variantes disponíveis, conforme VISUAL_E_EFEITOS.md.

Execução: build de desenvolvimento testado sem erros de execução. Foram capturados e inspecionados menu, mesa com seis cartas por mão, seleção, execução com efeitos, Diagnóstico, pausa, opções, múltiplas ameaças, vitória e derrota. As capturas usam uma textura de renderização da UI para permitir inspeção com a janela oculta. Relatório: visual/resultado.txt; capturas: visual/*.png.

Distribuição: build normal em Builds/Windows/CienciaDelas.exe, compilado com zero erros e zero avisos. A pasta inteira deve acompanhar o executável. Relatório: build.txt.

Ainda sujeito a aceite humano: gamepad físico, conforto visual, diferentes monitores e balanceamento em dupla. As partidas controladas de teste não comprovam balanceamento.

A indisponibilidade dos serviços do editor e as expirações de revisão de permissão registradas em 29/10 foram superadas nesta execução autorizada.
