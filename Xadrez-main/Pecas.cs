using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Xadrez;

public abstract class Pecas : Form, ICount
{
    public string cor { get; protected set; } = "cor" ?? string.Empty; // precaução para que não seja null
    internal int linha { get; set; }
    internal int coluna { get; set; }
    public int line { get; set; }

    public PictureBox pictureBox = new PictureBox();

    public char[]? characterColumn { get; set; } = new char[1] ?? null;
    public char c { get; private set; }

    public Pecas() { }
    public Pecas(string Cor, int Linha, int Coluna)
    {
        cor = Cor;
        linha = Linha;
        coluna = Coluna;
        line = Linha;
    }
    public abstract bool MovimentoValido(int linhaDestino, int colunaDestino, Pecas pecaDestino);

    public abstract bool Xeque(Pecas rei, Pecas pecaAtacante, Pecas[,] tabuleiro);

    public void CountC(int c)
    {
        characterColumn = new char[8]
        {
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',
            'g',
            'h',
        };

        if (c >= 0 && c <= 8)
        {
            this.c = characterColumn[c];
        }
    }

    public int revertLine(int l)
    {
        if (l < 0 || l > 7) return -1;
        return 7 - l + 1;
    }
}
