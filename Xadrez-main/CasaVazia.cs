using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xadrez;

public class CasaVazia : Pecas
{
    public CasaVazia() : base() { }

    public CasaVazia(string nome, int linha, int coluna) : base(nome, linha, coluna)
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
            string path = Path.Combine($"{Application.StartupPath}", "imagens", $"{nome}.png"); // Se estiver dando erro, edite o valor da variável 'disk' para "D"
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
        return true;
    }

    public override bool Xeque(Pecas rei, Pecas pecaAtacante, Pecas[,] tb)
    {
        return false;
    }
}
