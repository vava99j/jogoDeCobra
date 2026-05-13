using Timer = System.Windows.Forms.Timer;

namespace JogoDaCobrinha
{

    // classe principal da janela do jogo
    public partial class Form1 : Form
    {
        // Timer responsável por atualizar o jogo
        // funciona como um loop
        Timer temporizadorDoJogo = new Timer();

        // Cria o objeto da cobrinha
        Cobrinha cobrinha = new Cobrinha();

        // variavel que armazena a posição atual da comida
        Point posicaoDaComida;

        // Gera números aleatórios
        // usamos para criar a comida em posições diferentes
        Random geradorAleatorio = new Random();

        // tamanho de cada quadrado do jogo
        int tamanhoDoBloco = 20;

        // quantidade de colunas do mapa
        int quantidadeDeColunas = 30;

        // quantidade de linhas do mapa
        int quantidadeDeLinhas = 22;

        // guardar a pontuação do jogador
        int pontuacao = 0;

        // construtor da janela do jogo
        // executa automaticamente quando o jogo inicia
        public Form1()
        {
            // Inicializa os componentes do Windows Forms
            InitializeComponent();

            // Configura a janela
            ConfigurarJanela();

            // Criar a primeira comida
            CriarNovaComida();

            // Tempo entre cada atualização do jogo
            temporizadorDoJogo.Interval = 120;

            // evento executado a cada "tick" do timer
            // a cada 120 milisegundos o timer dispara um evento.
            // Esse evento é o tick
            // quando acontecer um tick, executa o evento "AtualizarJogo".
            temporizadorDoJogo.Tick += AtualizarJogo;

            // inicia o timer
            temporizadorDoJogo.Start();

            // evento responsável por desenhar na tela
            this.Paint += DesenharJogo;

            // evento responsável por detectar teclas
            this.KeyDown += DetectarTeclaPressionada;
        }

        // configurações da janela
        private void ConfigurarJanela()
        {
            // Título da Janela
            this.Text = "Jogo da Cobrinha";

            // Largura da janela
            this.Width = 650;

            // Altura da janela
            this.Height = 550;

            // Centraliza a janela na tela
            this.StartPosition = FormStartPosition.CenterScreen;

            // cor de fundo da janela
            this.BackColor = Color.Black; // preto

            // Evita piscadas durante a renderização (renderizar -> mostrar na tela)
            this.DoubleBuffered = true;

            // Permite capturar as teclas do teclado
            this.KeyPreview = true;
        }

        // método é chamado sempre pelo Timer
        // sender -> quem disparou o evento -> Timer
        // e -> informações extras do evento, tick não envia quase nenhuma informação extra
        // entao ele usa: EventArgs.Empty
        private void AtualizarJogo(object sender, EventArgs e)
        {
            // Calcular a próxima posição da cabeça
            Point novaPosicaoDaCabeca = cobrinha.CalcularNovaPosicaoDaCabeca();

            // Verifica se existe colisão na parede
            bool bateuNaParede =
                novaPosicaoDaCabeca.X < 0 ||
                novaPosicaoDaCabeca.X >= quantidadeDeColunas ||
                novaPosicaoDaCabeca.Y < 0 ||
                novaPosicaoDaCabeca.Y >= quantidadeDeLinhas;

            // verifica colisão com o próprio corpo
            bool bateuNoCorpo = cobrinha.BateuNoProprioCorpo(novaPosicaoDaCabeca);

            // se bateu em algo
            if (bateuNaParede || bateuNoCorpo)
            {
                // Finaliza o jogo
                FinalizarJogo();

                // Interrompe o método
                return;
            }

            // verifica se a cobrinha comeu a comida
            bool comeuComida = novaPosicaoDaCabeca == posicaoDaComida;

            // move a cobrinha
            cobrinha.Mover(novaPosicaoDaCabeca, comeuComida);

            // se comeu a comida
            if (comeuComida)
            {
                // aumenta a pontuação
                pontuacao++;

                // Cria a nova comida
                CriarNovaComida();
            }

            // Solicita redesenho da tela
            this.Invalidate();
        }

        // método responsável por desenhar tudo na tela
        private void DesenharJogo(object sender, PaintEventArgs e)
        {
            // objeto gráfico usado para desenhar
            Graphics tela = e.Graphics;

            // desenha os elementos do jogo
            // METODO DE DESENHO DE PONTUAÇÃO
            DesenharPontuacao(tela);

            // METODO DE DESENHO DA COBRINHA
            DesenharCobrinha(tela);

            // METODO DE DESENHO DA COMIDA
            DesenharComida(tela);
        }

        // Desenhar pontuação
        private void DesenharPontuacao(Graphics tela)
        {
            tela.DrawString(
                // texto exibido
                $"Pontuação: {pontuacao}",

                // fonte do texto
                new Font("Arial", 16, FontStyle.Bold),

                // cor do texto
                Brushes.White, // branco

                // Posição X
                10,

                // Posição Y
                10
                );
        }

        // desenhar cobrinha
        private void DesenharCobrinha(Graphics tela)
        {
            // percorre todas as partes do corpo
            foreach (Point parteDoCorpo in cobrinha.PartesDoCorpo)
            {
                // desenha um retângulo para cada parte do corpo
                tela.FillRectangle(

                    // cor da cobrinha
                    Brushes.LimeGreen,

                    // Posição X
                    parteDoCorpo.X * tamanhoDoBloco,

                    //Posição Y
                    parteDoCorpo.Y * tamanhoDoBloco + 50,

                    // Largura do bloco
                    tamanhoDoBloco - 2,

                    // Altura do bloco
                    tamanhoDoBloco - 2
                );
            }
        }

        // desenhar comida
        private void DesenharComida(Graphics tela)
        {
            // desenhar um círculo vermelho
            tela.FillEllipse(
                // cor da comida
                Brushes.Red,

                // posição X
                posicaoDaComida.X * tamanhoDoBloco,

                // Posição Y
                posicaoDaComida.Y * tamanhoDoBloco + 50,

                // largura
                tamanhoDoBloco - 2,

                // altura
                tamanhoDoBloco - 2
            );
        }

        // detectar teclas pressionadas
        private void DetectarTeclaPressionada(object sender, KeyEventArgs e)
        {
            // seta pra cima
            if (e.KeyCode == Keys.Up)
            {
                // 0 horizontal e -1 vertical
                cobrinha.MudarDirecao(0, -1);
            }

            // seta para baixo
            else if (e.KeyCode == Keys.Down)
            {
                // 0 horizontal e 1 vertical
                cobrinha.MudarDirecao(0, 1);
            }

            // seta para esquerda
            else if (e.KeyCode == Keys.Left)
            {
                // -1 horizontal e 0 vertical
                cobrinha.MudarDirecao(-1, 0);
            }

            // seta para direita
            else if (e.KeyCode == Keys.Right)
            {
                // 1 horizontal e 0 vertical
                cobrinha.MudarDirecao(1, 0);
            }
        }

        // criar comida em posição aleatória
        private void CriarNovaComida()
        {
            // gera posição X aleatória
            int posicaoX = geradorAleatorio.Next(0, quantidadeDeColunas);

            // gera posição Y aleatória
            int posicaoY = geradorAleatorio.Next(0, quantidadeDeLinhas);

            // define a posição da comida
            posicaoDaComida = new Point(posicaoX, posicaoY);
        }

        // finaliza o jogo
        private void FinalizarJogo()
        {
            // parar o timer
            temporizadorDoJogo.Stop();

            // mostra mensagem final
            MessageBox.Show(
                $"Fim de jogo!\nPontuação: {pontuacao}"
            );

            // Reinicia o jogo
            ReiniciarJogo();
        }

        // reinicia todas as informações do jogo
        private void ReiniciarJogo()
        {
            // reiniciar a cobrinha
            cobrinha.Reiniciar();

            // zerar pontuação
            pontuacao = 0;

            // Criar uma nova comida
            CriarNovaComida();

            // inicia o timer novamente
            temporizadorDoJogo.Start();

        }
    }
}