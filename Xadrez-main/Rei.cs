using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Xadrez;

public class Rei : Pecas
{
    public Rei(string cor, int linha, int coluna) : base(cor, linha, coluna)
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
            string path = Path.Combine($"{Application.StartupPath}", "imagens", $"rei_{cor}.png"); // Se estiver dando erro, edite o valor da variável 'disk' para "D"
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
        int difLinha = LinhaDestino - linha;
        int difColuna = ColunaDestino - coluna;

        return difLinha <= 1 || difColuna <= 1 || difColuna >= -1 || difLinha >= -1;
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
