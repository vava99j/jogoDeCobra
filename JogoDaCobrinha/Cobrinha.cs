using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoDaCobrinha
{
    public class Cobrinha
    {
        // Lista que armazena todas as partes do corpo da cobrinha
        // Cada Point representa uma posição no jogo (coordenadas x e y)
        public List<Point> PartesDoCorpo { get; private set; }

        // Direção horizontal -> 
        // 1 = direita
        // -1 = esquerda
        // 0 = sem movimento horizontal
        public int DirecaoHorizontal { get; private set; }

        // Direção vertical ->
        // 1 = baixo
        // -1 = cima
        // 0 = sem movimento vertical
        public int DirecaoVertical { get; private set; }

        // Construtor da classe
        // Executa automaticamente quando criamos um objeto cobrinha
        public Cobrinha()
        {
            // Cria a lista vazia
            PartesDoCorpo = new List<Point>();

            // Inicializar a cobrinha
            Reiniciar();
        }

        // método responsável por reiniciar o jogo
        public void Reiniciar()
        {
            // Limpa as partes do corpo da cobrinha
            PartesDoCorpo.Clear();

            // Adiciona a cabeça da cobrinha 
            // define a posição 
            // a cobrinha começa na posição x=5 e y=5
            PartesDoCorpo.Add(new Point(5, 5));

            // Define a direção inicial para a direita
            DirecaoHorizontal = 1;

            // Sem movimento vertical
            DirecaoVertical = 0;
        }

        // retornar a cabeça da cobrinha
        // a cabeça sempre fica na posição 0 da lista
        public Point ObterCabeca()
        {
            return PartesDoCorpo[0];
        }

        // Calcula a próxima posição da cabeça
        public Point CalcularNovaPosicaoDaCabeca()
        {
            // obter a posição atual da cabeça
            Point cabecaAtual = ObterCabeca();

            //Retorna uma nova posição
            // soma a direção atual nas coordenadas x e y
            return new Point(
                cabecaAtual.X + DirecaoHorizontal,
                cabecaAtual.Y + DirecaoVertical
             );
        }

        // Mover a cobrinha
        public void Mover(Point novaPosicaoDaCabeca, bool comeuComida)
        {
            // Adiciona a nova cabeça no inicio da lista
            PartesDoCorpo.Insert(0, novaPosicaoDaCabeca);

            if (!comeuComida)
            {
                // remover a última parte do corpo
                // isso cria o efeito de movimento
                PartesDoCorpo.RemoveAt(PartesDoCorpo.Count - 1);
            }

            // se comeu comida:
            // NAO remove o último bloco
            // Assim a cobrinha cresce
        }

        // alterar a direção da cobrinha
        public void MudarDirecao(int novaDirecaoHorizontal, int novaDirecaoVertical)
        {
            // verifica se o jogador está tentando voltar pra tras
            // exemplo:
            // se estiver indo pra DIREITA: DirecaoHorizontal = 1
            // e tentar ir pra ESQUERDA: novaDirecaoHorizontal = -1
            // a soma fica: 1 + (-1) = 0
            bool estaTentandoVoltar = DirecaoHorizontal + novaDirecaoHorizontal == 0 && DirecaoVertical + novaDirecaoVertical == 0;

            // só muda a direção se NÃO estiver tentando voltar
            if (!estaTentandoVoltar)
            {
                DirecaoHorizontal = novaDirecaoHorizontal;
                DirecaoVertical = novaDirecaoVertical;
            }
        }

        // verifica se a cobrinha bateu no próprio corpo
        public bool BateuNoProprioCorpo(Point novaPosicaoDaCabeca)
        {
            // Retorna TRUE se a posição já existir no corpo
            // Retorna FALSE se não existir
            // "Essa posição já existe dentro da lista?" se sim, então a cobrinha bateu nela mesma
            return PartesDoCorpo.Contains(novaPosicaoDaCabeca);
        }
    }
    }
