using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xadrez
{
    public interface ICount
    {
        public void CountC(int c);
        public int revertLine(int l);
    }
}