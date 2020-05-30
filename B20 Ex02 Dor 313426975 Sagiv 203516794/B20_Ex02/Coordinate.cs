using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public struct Coordinate
    {
        private readonly int r_Row;
        private readonly int r_Col;

        public Coordinate(string move)
        {
            r_Col = move[0] - UI.r_ColSymbol[0];
            r_Row = move[1] - UI.r_RowSymbol[0];
        }

        public int Row
        {
            get { return r_Row; }
        }

        public int Col
        {
            get { return r_Col; }
        }
    }
}
