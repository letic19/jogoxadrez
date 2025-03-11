using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xadrez;

public class Rainha : Pecas
{
    public override bool MovimentoValido(int LinhaDestino, int ColunaDestino, Pecas pecaDestino)
    {

        if (LinhaDestino < 0 || LinhaDestino > 7 || ColunaDestino < 0 || ColunaDestino > 7)
            return false;

        int difLinha = Math.Abs(LinhaDestino - linha);
        int difColuna = Math.Abs(ColunaDestino - coluna);

        // Não permite não mover
        if (LinhaDestino == linha && ColunaDestino == coluna)
            return false;

        // Movimento horizontal ou vertical
        if (LinhaDestino == linha || ColunaDestino == coluna)
            return true;

        // Movimento diagonal
        if (difLinha == difColuna)
            return true;

        return false;
    }
    public Rainha(string cor, int linha, int coluna) : base(cor, linha, coluna)
    {
        pictureBox = new PictureBox
        {
            Location = new Point(coluna * 50, linha * 50),
            Size = new Size(50, 50),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Parent = this,
        };
        
        pictureBox.BackColor = (linha+coluna)%2==0 ? Color.White : Color.Black;
        this.BringToFront();
        
        try
        {
            string path = Path.Combine($"{Application.StartupPath}", "imagens", $"dama_{cor}.png"); // Se estiver dando erro, edite o valor da variável 'disk' para "D"
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
