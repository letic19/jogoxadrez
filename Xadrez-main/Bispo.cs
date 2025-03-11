using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xadrez;

public class Bispo : Pecas
{
    public override bool MovimentoValido(int LinhaDestino, int ColunaDestino, Pecas pecaDestino)
    {
        if (LinhaDestino < 0 || LinhaDestino > 7 || ColunaDestino < 0 || ColunaDestino > 7)
        {
            return false;
        }
        int difLinha = Math.Abs(linha - LinhaDestino);
        int difColuna = Math.Abs(coluna - ColunaDestino);

        return (difLinha == difColuna);
    }
    public Bispo(string cor, int linha, int coluna) : base(cor, linha, coluna)
    {
        pictureBox = new PictureBox
        {
            Location = new Point(coluna * 50, linha * 50),
            Size = new Size(48, 48),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Parent = this,
        };

        pictureBox.BackColor = (linha + coluna) % 2 == 0 ? Color.White : Color.Black;

        try
        {
            string path = Path.Combine($"{Application.StartupPath}", "imagens", $"bispo_{cor}.png"); // Se estiver dando erro, edite o valor da variável 'disk' para "D"
            // MessageBox.Show("Tentando carregar: " + path);
            pictureBox.Image = Image.FromFile(path);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao carregar imagem: " + ex.Message);
        }
    }

    public override bool Xeque(Pecas rei, Pecas pecaAtacante, Pecas[,] tb)
    {
        ref Pecas[,] t = ref tb;
        // Math.Abs(pecaAtacante.linha);
        // Math.Abs(pecaAtacante.coluna);
        MessageBox.Show($"{pecaAtacante.GetType()}");

        try
        {
            MessageBox.Show($"{tb[pecaAtacante.linha, pecaAtacante.coluna].Name}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}: {e.StackTrace}");
        }

        if (rei.cor != cor)
        {
            if (pecaAtacante.MovimentoValido(rei.linha, rei.coluna, rei) && tb[pecaAtacante.linha - 1, pecaAtacante.coluna + 1] is CasaVazia)
            {
                MessageBox.Show($"{tb[pecaAtacante.linha + 1, pecaAtacante.coluna + 1].Name}");
                // MessageBox.Show($"O rei está em Xeque");
                return true;
            }
        }
        return false;
    }
}
