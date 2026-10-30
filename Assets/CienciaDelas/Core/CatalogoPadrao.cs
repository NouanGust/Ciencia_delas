using System.Collections.Generic;

namespace CienciaDelas
{
    // Os valores são copiados para um asset editável; não é necessário editar este arquivo para balancear.
    public static class CatalogoPadrao
    {
        public static List<CartaDados> Cartas()
        {
            var c = new List<CartaDados>();
            Adicionar(c,"analise","Análise Genética",0,new Recursos(0,0,1));
            Adicionar(c,"rapido","Sequenciamento Rápido",1,new Recursos(0,0,2));
            Adicionar(c,"resposta","Resposta Epidemiológica",2,new Recursos(0,3,0));
            Adicionar(c,"monitoramento","Monitoramento de Casos",0,default,Efeito.Espiar,descricao:"Revela a próxima ameaça até ela entrar em jogo.");
            Adicionar(c,"insumos","Distribuição de Insumos",0,new Recursos(0,1,0));
            Adicionar(c,"amostras","Coleta de Amostras",2,new Recursos(2,0,1));
            Adicionar(c,"controlado","Experimento Controlado",0,new Recursos(0,0,1));
            Adicionar(c,"campo","Ação em Campo",2,new Recursos(1,1,1));
            Adicionar(c,"intervencao","Intervenção Emergencial",2,new Recursos(2,0,0));
            Adicionar(c,"vacinas","Desenvolvimento de Vacinas",2,new Recursos(1,0,2));
            Adicionar(c,"radioatividade","Radioatividade",2,default,Efeito.Radioatividade,Cientista.MarieCurie,"Bloqueia a ameaça escolhida por 2 rodadas, incluindo esta. Saúde -2 ao final de cada uma.");
            Adicionar(c,"descoberta","Descoberta Científica",3,default,Efeito.Duplicar,Cientista.MarieCurie,"Dobra o efeito da próxima carta da fila global, inclusive de outra cientista. Não dobra custos.");
            Adicionar(c,"energia","Energia Nuclear",3,new Recursos(0,1,2),Efeito.Recursos,Cientista.MarieCurie);
            Adicionar(c,"avancado","Experimento Avançado",3,default,Efeito.Experimento,Cientista.MarieCurie,"+1 em cada recurso ao final de 3 rodadas, incluindo esta.");
            Adicionar(c,"legado","Legado Científico",3,default,Efeito.Legado,Cientista.MarieCurie,"+1 Pesquisa no início das próximas 2 rodadas.");
            Adicionar(c,"genetico","Sequenciamento Genético",3,default,Efeito.Anular,Cientista.JaquelineGoes,"Anula a penalidade da ameaça recém-revelada nesta rodada.");
            Adicionar(c,"diagnostico","Diagnóstico Rápido",3,default,Efeito.Diagnostico,Cientista.JaquelineGoes,"Escolha até 2 cartas adicionais da sua mão, sem custo de Conhecimento. Custos de recursos continuam valendo.");
            Adicionar(c,"mapeamento","Mapeamento Viral",2,default,Efeito.Mapear,Cientista.JaquelineGoes,"Reduz em 1 cada perda de recurso de cada ameaça nesta rodada.");
            Adicionar(c,"imediata","Resposta Imediata",3,new Recursos(1,1,1),Efeito.Recursos,Cientista.JaquelineGoes);
            Adicionar(c,"controle","Controle Epidemiológico",3,default,Efeito.Converter,Cientista.JaquelineGoes,"Nesta rodada, cada perda da ameaça escolhida vira ganho de um terço, arredondado para baixo.");
            return c;
        }
        static void Adicionar(List<CartaDados> c,string id,string nome,int k,Recursos ganho,Efeito e=Efeito.Recursos,Cientista cientista=Cientista.Normal,string descricao=null)
        {
            c.Add(new CartaDados { id=id,nome=nome,conhecimento=k,ganho=ganho,efeito=e,cientista=cientista,descricao=descricao??("Ganho: "+ganho) });
        }
        public static List<AmeacaDados> Ameacas()
        {
            return new List<AmeacaDados> {
                A("casos","Primeiros Casos do Surto",0,0,7,2,0,0),
                A("isolamento","Falha no Isolamento da Área Afetada",0,7,0,0,1,0),
                A("origem","Origem Desconhecida do Vírus",0,0,2,1,0,0,true),
                A("variante","Nova Variante Detectada",0,0,7,0,1,0),
                A("hospitais","Superlotação dos Hospitais",0,9,0,1,1,0),
                A("contagio","Contágio Fora de Controle",10,0,0,1,0,0),
                A("desinformacao","Desinformação da População",11,0,11,1,0,0),
                A("mutacao","Mutação Inesperada do Vírus",0,0,8,1,1,1,true),
                A("fracasso","Fracasso no Desenvolvimento da Vacina",0,0,6,0,0,2),
                A("testes","Risco nos Testes Clínicos",0,7,7,0,2,1),
                A("colapso","Colapso na Produção de Vacinas",0,10,0,0,2,0),
                A("caos","Caos na Distribuição de Vacinas",12,12,0,2,2,0),
                A("crise","Crise Sanitária Internacional",14,14,14,2,2,2),
                A("resistencia","Resistência à Imunização",8,0,0,2,0,0,true),
                A("global","Ameaça Global Iminente",16,16,16,3,3,3)
            };
        }
        static AmeacaDados A(string id,string nome,int s,int i,int p,int ds,int di,int dp,bool gastar=false) => new AmeacaDados {
            id=id,nome=nome,requisito=new Recursos(s,i,p),penalidade=new Recursos(ds,di,dp),gastar=gastar,comparador=Comparador.PeloMenos
        };
    }
}
