using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Xadrez;

public class Tabuleiro
{
    public Pecas[,] Matriz { get; private set; }

    // public static Pecas[,] TabuleiroGlobal { get; private set; }

    private List<PictureBox> pb = new List<PictureBox>();

    Torre[] torres;
    Cavalo[] cavalos;
    Bispo[] bispos;
    Rei[] reis;
    Rainha[] damas;
    Peao[] peoesPretos;
    Peao[] peoesBrancos;


    List<Pecas> listPecas = new List<Pecas>();

    public Tabuleiro()
    {
        Matriz = new Pecas[8, 8];
        // TabuleiroGlobal = Matriz;

        torres = new Torre[4];
        cavalos = new Cavalo[4];
        bispos = new Bispo[4];
        reis = new Rei[2];
        damas = new Rainha[2];
        peoesPretos = new Peao[8];
        peoesBrancos = new Peao[8];

        torres[0] = new Torre("preta", 0, 0);
        cavalos[0] = new Cavalo("preto", 0, 1);
        bispos[0] = new Bispo("preto", 0, 2);
        damas[0] = new Rainha("preta", 0, 3);
        reis[0] = new Rei("preto", 0, 4);
        bispos[1] = new Bispo("preto", 0, 5);
        cavalos[1] = new Cavalo("preto", 0, 6);
        torres[1] = new Torre("preta", 0, 7);
        for (int i = 0; i < 8; i++)
        {
            peoesPretos[i] = new Peao("preto", 1, i);
        }

        torres[2] = new Torre("branca", 7, 0);
        cavalos[2] = new Cavalo("branco", 7, 1);
        bispos[2] = new Bispo("branco", 7, 2);
        damas[1] = new Rainha("branca", 7, 3);
        reis[1] = new Rei("branco", 7, 4);
        bispos[3] = new Bispo("branco", 7, 5);
        cavalos[3] = new Cavalo("branco", 7, 6);
        torres[3] = new Torre("branca", 7, 7);
        for (int i = 0; i < 8; i++)
        {
            peoesBrancos[i] = new Peao("branco", 6, i);
        }

        listPecas.AddRange(new Pecas[] { torres[0], cavalos[0], bispos[0], damas[0], reis[0], bispos[1], cavalos[1], torres[1] });
        listPecas.AddRange(peoesPretos);
        listPecas.AddRange(peoesBrancos);
        listPecas.AddRange(new Pecas[] { torres[2], cavalos[2], bispos[2], damas[1], reis[1], bispos[3], cavalos[3], torres[3] });

        foreach (Pecas p in listPecas)
        {
            pb.Add(p.pictureBox);
        }
    }

    public void InicializarTabuleiro(Form1 arg)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                CasaVazia casa = new CasaVazia("casavazia", i, j);
                // Configura o PictureBox da casa vazia
                casa.pictureBox.Location = new Point(j * 50, i * 50);
                casa.pictureBox.Size = new Size(48, 48);
                casa.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                casa.pictureBox.BackColor = ((i + j) % 2 == 0) ? Color.White : Color.Black;
                casa.pictureBox.Visible = true;
                // Configura o evento de clique para a casa vazia
                casa.pictureBox.Click += (sender, args) => { arg.ClickNoTabuleiro(casa); };

                Matriz[i, j] = casa;
            }
        }

        foreach (Pecas p in listPecas)
        {
            Matriz[p.linha, p.coluna] = p;
            if (!arg.Controls.Contains(p.pictureBox))
            {
                arg.Controls.Add(p.pictureBox);
            }
            p.pictureBox.Click += (sender, args) => { arg.ClickNoTabuleiro(p); };
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (!arg.Controls.Contains(Matriz[i, j].pictureBox))
                {
                    arg.Controls.Add(Matriz[i, j].pictureBox);
                }
            }
        }
    }
}
