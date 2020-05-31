namespace B20_Ex02
{
    public struct Coordinate
    {
        private readonly int r_Row;
        private readonly int r_Col;

        public Coordinate(string move)
        {
            r_Col = move[0] - UI.sr_ColSymbol[0];
            r_Row = move[1] - UI.sr_RowSymbol[0];
        }

        public int Row
        {
            get { return r_Row; }
        }

        public int Col
        {
            get { return r_Col; }
        }

        internal static Coordinate FromPresentationBoardCoordinateToGameBoardCoordinate(int io_CurrRowCoordinateOfPresentationBoard, int io_CurrColCoordinateOfPresentationBoard)
        {
            return new Coordinate(io_CurrRowCoordinateOfPresentationBoard, io_CurrColCoordinateOfPresentationBoard);
        }

        private Coordinate(int io_CurrRowCoordinateOfPresentationBoard, int io_CurrColCoordinateOfPresentationBoard)
        {
            r_Row = (io_CurrRowCoordinateOfPresentationBoard / UI.sr_SpaceForSingleCubeRows) - 1;
            r_Col = (io_CurrColCoordinateOfPresentationBoard / UI.sr_SpaceForSingleCubeCols) - 1;
        }
    }
}
