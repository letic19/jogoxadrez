using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Xadrez;

public class Torre : Pecas
{
    // public PictureBox torreImagem { get; private set; }
    public override bool MovimentoValido(int LinhaDestino, int ColunaDestino, Pecas pecaDestino)
    {
        if (LinhaDestino < 0 || LinhaDestino > 7 || ColunaDestino < 0 || ColunaDestino > 7)
        {
            return false;
        }
        
        if (LinhaDestino == linha && ColunaDestino == coluna)
            return false;

        // Movimento horizontal ou vertical
        return (LinhaDestino == linha) || (ColunaDestino == coluna);
    }
    public Torre(string cor, int linha, int coluna) : base(cor, linha, coluna)
    {
        pictureBox = new PictureBox
        {
            Location = new Point(coluna * 50, linha * 50),
            Size = new Size(50, 50),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Parent = this,
            
        };

        pictureBox.BackColor = (linha+coluna)%2==0 ? Color.White : Color.Black;

        try
        {
           string path = Path.Combine($"{Application.StartupPath}", "imagens", $"torre_{cor}.png"); // Se estiver dando erro, edite o valor da variável 'disk' para "D"
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
