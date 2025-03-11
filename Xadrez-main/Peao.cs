using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

namespace Xadrez;

public class Peao : Pecas
{
    bool primeiroMovimento = true;
    public Peao() : base() { }

    public Peao(string Cor, int Linha, int Coluna) : base(Cor, Linha, Coluna)
    {
        pictureBox = new PictureBox
        {
            Location = new Point(coluna * 50, linha * 50),
            Size = new Size(50, 50),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Parent = this,
        };

        pictureBox.BackColor = (linha + coluna) % 2 == 0 ? Color.White : Color.Black;

        try
        {
            string path = Path.Combine($"{Application.StartupPath}", "imagens", $"peao_{cor}.png"); // Se estiver dando erro, edite o valor da variável 'disk' para "D"
            // MessageBox.Show("Tentando carregar: " + path);
            pictureBox.Image = Image.FromFile(path);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao carregar imagem: " + ex.Message);
        }
    }
    public override bool MovimentoValido(int LinhaDestino, int ColunaDestino, Pecas pecaDestino)
    {
        if (LinhaDestino < 0 || LinhaDestino > 7 || ColunaDestino < 0 || ColunaDestino > 7)
            return false;

        int direcao = (cor == "branco") ? 1 : -1;

        int difLinha = linha - LinhaDestino;
        int difColuna = coluna - ColunaDestino;

        if (difColuna == 0 && (difLinha == direcao || (difLinha == 2 * direcao && primeiroMovimento)) && pecaDestino is CasaVazia)
        {
            primeiroMovimento = false;
            return true;
        }

        if (difColuna == 0 && difLinha == direcao)
        {
            primeiroMovimento = false;
            return true;
        }

        if (difColuna == 0 && difLinha == 2 * direcao && primeiroMovimento)
        {
            primeiroMovimento = false;
            return true;
        }

        if (Math.Abs(difColuna) == 1 && difLinha == direcao)
        {
            if (!(pecaDestino is CasaVazia) && pecaDestino.cor != cor)
            {
                primeiroMovimento = false;
                return true;
            }
        }

        return false;
    }

    public override bool Xeque(Pecas rei, Pecas pecaAtacante, Pecas[,] tb)
    {

        if (rei.cor != cor)
        {
            if (pecaAtacante.MovimentoValido(rei.linha, rei.coluna, rei))
            {
                MessageBox.Show($"O rei está em Xeque");
                return true;
            }
        }
        return false;
    }
}
