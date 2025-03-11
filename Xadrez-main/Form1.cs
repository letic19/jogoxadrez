using System.CodeDom;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;

namespace Xadrez;

public partial class Form1 : Form
{
#pragma warning disable CS8632 // A anotação para tipos de referência anuláveis deve ser usada apenas em código em um contexto de anotações '#nullable'.
    private PictureBox? pecaSelecionada = null; // Armazena a peça selecionada
#pragma warning restore CS8632 // A anotação para tipos de referência anuláveis deve ser usada apenas em código em um contexto de anotações '#nullable'.

    private int origemX = -1, origemY = -1; // Armazena a posição da peça
    public Tabuleiro tb = new Tabuleiro();
    bool __mv = false;

    private bool vez_das_brancas = true;

    public Pecas[,] tabuleiro;

    public Form1()
    {
        tabuleiro = tb.Matriz;
        
        try
        {
            tb.InicializarTabuleiro(this);
        }
        catch (Exception e)
        {
            MessageBox.Show($"{e.Message}: {e.StackTrace}");
        }

        InitializeComponent();
        MessageBox.Show($"{Application.StartupPath}");
    }

    public void ClickNoTabuleiro(Pecas peca)
    {
        Pecas pecaRei;
        peca.CountC(peca.coluna);
        peca.line = peca.revertLine(peca.line);

        if (origemX == -1 && origemY == -1) // Primeiro clique: seleciona a peça
        {
            MessageBox.Show($"Peça selecionada: {peca.GetType().Name} {peca.c}{peca.line}");
            if (peca is not CasaVazia)
            {
                pecaSelecionada = peca.pictureBox;
                origemX = peca.linha;
                origemY = peca.coluna;
            }

            if ((peca.cor == "preto" || peca.cor == "preta") && !__mv)
            {
                MessageBox.Show("As peças brancas que começam.");
                origemX = -1;
                origemY = -1;
                return;
            }

            if ((peca.cor == "preto" || peca.cor == "preta") && vez_das_brancas)
            {
                MessageBox.Show($"Vez das brancas");
                origemX = -1;
                origemY = -1;
                return;
            }
            else if ((peca.cor == "branco" || peca.cor == "branca") && !vez_das_brancas)
            {
                MessageBox.Show($"Vez das pretas");
                origemX = -1;
                origemY = -1;
                return;
            }

        }
        else // Segundo clique: tenta mover a peça
        {
            Pecas pecaOrigem = tabuleiro[origemX, origemY];
            Pecas pecaDestino = tabuleiro[peca.linha, peca.coluna];

            __mv = true;
            vez_das_brancas = !vez_das_brancas;

            MessageBox.Show($"Peça destino: {pecaDestino.GetType().Name} {pecaDestino.c}{pecaDestino.line}");

            if (peca.linha == origemX && peca.coluna == origemY)
            {
                pecaSelecionada = null;
                origemX = -1;
                origemY = -1;
                return;
            }
            pecaRei = ((pecaOrigem.cor == "branco" || pecaOrigem.cor == "branca") && (pecaDestino is not CasaVazia || pecaDestino is CasaVazia)) ? tabuleiro[0, 4] : tabuleiro[7, 4];

            Pecas[,] _clone_t = tabuleiro;
            if (!pecaOrigem.MovimentoValido(peca.linha, peca.coluna, pecaDestino))
            {
                MessageBox.Show($"Movimento inválido");
                vez_das_brancas = !vez_das_brancas;
                pecaSelecionada = null;
                origemX = -1;
                origemY = -1;
                return;
            }


            else if (pecaDestino is CasaVazia) // Se o destino estiver vazio, apenas move a peça
            {
                int xOrigem = origemX, yOrigem = origemY;
                CasaVazia novaCasaVazia = CriarCasaVazia(xOrigem, yOrigem);
                tabuleiro[xOrigem, yOrigem] = novaCasaVazia;
                tabuleiro[peca.linha, peca.coluna] = pecaOrigem;

                pecaOrigem.linha = pecaDestino.linha;
                pecaOrigem.coluna = pecaDestino.coluna;
                this.Controls.Remove(pecaOrigem);

                peca.pictureBox.Location = new Point(origemY * 50, origemX * 50);
                peca.pictureBox.BackColor = (origemX + origemY) % 2 == 0 ? Color.White : Color.Black;
                peca.pictureBox.Visible = true;
                pecaOrigem.pictureBox.Location = new Point(pecaOrigem.coluna * 50, pecaOrigem.linha * 50); // Eu juro que daqui pra frente, eu nunca vou esquecer dessa desgraça, isso vai ser meu pesadelo de todas as noites ╰（‵□′）╯
                pecaOrigem.pictureBox.BackColor = (pecaOrigem.linha + peca.coluna) % 2 == 0 ? Color.White : Color.Black;
                pecaOrigem.pictureBox.Visible = true;
                if ((pecaOrigem.cor.StartsWith("b") && peca.cor.StartsWith("p") || pecaOrigem.cor.StartsWith("p") && peca.cor.StartsWith("b")) && peca is not CasaVazia)
                {
                    MessageBox.Show("Movimento inválido");
                    pecaSelecionada = null;
                    origemX = -1;
                    origemY = -1;
                    return;
                }

                if (!this.Controls.Contains(pecaOrigem.pictureBox))
                {
                    MessageBox.Show("PictureBox NÃO encontrado no Controls! Adicionando...");
                    pecaOrigem.pictureBox.BringToFront();
                    this.Controls.SetChildIndex(pecaOrigem.pictureBox, 0);
                }


                if (!this.Controls.Contains(novaCasaVazia.pictureBox))
                {
                    novaCasaVazia.pictureBox.BringToFront();
                    this.Controls.Add(novaCasaVazia.pictureBox);
                }

                novaCasaVazia.pictureBox.BringToFront();
                this.Controls.SetChildIndex(novaCasaVazia.pictureBox, 0);
                pecaOrigem.pictureBox.BringToFront();
                this.Controls.SetChildIndex(pecaOrigem.pictureBox, 0);



                if (canXeque(pecaOrigem.Xeque(pecaRei, pecaOrigem, this.tabuleiro) && ((tabuleiro[pecaRei.linha + 2, pecaRei.coluna - 2] is CasaVazia && tabuleiro[pecaRei.linha + 1, pecaRei.coluna - 1] is CasaVazia) || (tabuleiro[pecaRei.linha + 1, pecaRei.coluna] is CasaVazia) && (tabuleiro[pecaRei.linha + 2, pecaRei.coluna + 2] is CasaVazia || tabuleiro[pecaRei.linha + 1, pecaRei.coluna + 1] is CasaVazia))))
                {
                    MessageBox.Show($"Xeque");
                }

            }
            else // Se houver outra peça, troca as posições
            {
                if (!pecaOrigem.MovimentoValido(peca.linha, peca.coluna, pecaDestino))
                {
                    pecaSelecionada = null;
                    origemX = -1;
                    origemY = -1;
                    return;
                }

                CasaVazia novaCasaVazia = CriarCasaVazia(origemX, origemY);
                if (this.Controls.Contains(tabuleiro[peca.linha, peca.coluna].pictureBox))
                {
                    this.Controls.Remove(tabuleiro[peca.linha, peca.coluna].pictureBox);
                    this.Controls.Remove(tabuleiro[peca.linha, peca.coluna]);
                    tabuleiro[peca.linha, peca.coluna] = novaCasaVazia;
                    tabuleiro[peca.linha, peca.coluna].pictureBox = novaCasaVazia.pictureBox;
                }

                tabuleiro[peca.linha, peca.coluna] = pecaOrigem;
                tabuleiro[origemX, origemY] = novaCasaVazia;
                this.Controls.Add(novaCasaVazia.pictureBox);

                pecaOrigem.linha = peca.linha;
                pecaOrigem.coluna = peca.coluna;
                pecaOrigem.pictureBox.Location = new Point(pecaOrigem.coluna * 50, pecaOrigem.linha * 50); // Eu juro que daqui pra frente, eu nunca vou esquecer dessa desgraça, isso vai ser meu pesadelo de todas as noites ╰（‵□′）╯
                pecaOrigem.pictureBox.BackColor = (pecaOrigem.linha + pecaOrigem.coluna) % 2 == 0 ? Color.White : Color.Black;

                novaCasaVazia.pictureBox.BringToFront();
                this.Controls.SetChildIndex(novaCasaVazia.pictureBox, 0);
                pecaOrigem.pictureBox.BringToFront();
                this.Controls.SetChildIndex(pecaOrigem.pictureBox, 0);
            }

            this.Refresh();

            pecaSelecionada = null;
            origemX = -1;
            origemY = -1;
        }
    }

    private CasaVazia CriarCasaVazia(int linha, int coluna)
    {
        CasaVazia casa = new CasaVazia("casavazia", linha, coluna);
        casa.pictureBox.Visible = true;
        casa.pictureBox.Click += (sender, args) => { ClickNoTabuleiro(casa); };

        return casa;
    }

    private bool canXeque(bool arg)
    {
        return arg;
    }
}
