using Microsoft.Extensions.Logging;

namespace RegularForestLibrary
{
    /// <summary>
    /// Forest class represents forest of numbers loaded from file.
    /// </summary>
    public class RegularForest
    {
        private readonly ILogger<RegularForest> _logger;
        private int _rows;
        private int _cols;
        private byte[,] _trees = null!;
        private byte[,] _treeVisibilityMatrix = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegularForest" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public RegularForest(ILogger<RegularForest> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// Counts visible trees.
        /// </summary>
        /// <returns></returns>
        public int CountVisibleTreesInFile(string path)
        {
            try
            {
                var fileLinesList = File.ReadLines(path).ToList();
                _rows = fileLinesList.Count;
                _cols = fileLinesList.First().Length;
                _trees = new byte[_rows, _cols];

                for (var rowIndex = 0; rowIndex < _rows; rowIndex++)
                {
                    for (var colIndex = 0; colIndex < _cols; colIndex++)
                    {
                        _trees[rowIndex, colIndex] = byte.Parse(fileLinesList[rowIndex][colIndex].ToString());
                    }
                }

                _logger.LogInformation("Forest loaded.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while reading file.");
                throw;
            }

            _treeVisibilityMatrix = new byte[_rows, _cols];
            FillVisibilityMatrix();
            return _treeVisibilityMatrix.Cast<byte>().Count(visibilityValue => visibilityValue > 0);
        }

        /// <summary>
        /// Creates the visibility matrix.
        /// </summary>
        private void FillVisibilityMatrix()
        {
            _logger.LogInformation("Creating visibility matrix.");
            Enumerable.Range(0, _rows).ToList().ForEach(CreateVisibilityRow);
            Enumerable.Range(0, _cols).ToList().ForEach(CreateVisibilityCol);
            _logger.LogInformation("Visibility matrix has been created.");
        }


        /// <summary>
        /// Creates the visibility row.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        private void CreateVisibilityRow(int rowIndex)
        {
            var maximalTree = _trees[rowIndex, 0];
            SetTreeVisible(rowIndex, colIndex: 0, Direction.Left);

            for (var colIndex = 1; colIndex < _cols; colIndex++)
            {
                if (_trees[rowIndex, colIndex] > maximalTree)
                {
                    SetTreeVisible(rowIndex, colIndex, Direction.Left);
                    maximalTree = _trees[rowIndex, colIndex];
                }
            }

            maximalTree = _trees[rowIndex, _cols - 1];
            SetTreeVisible(rowIndex, colIndex: _cols - 1, Direction.Right);

            for (var colIndex = _cols - 2; colIndex >= 0; colIndex--)
            {
                if (_trees[rowIndex, colIndex] > maximalTree)
                {
                    SetTreeVisible(rowIndex, colIndex, Direction.Right);
                    maximalTree = _trees[rowIndex, colIndex];
                }
            }
        }

        /// <summary>
        /// Creates the visibility col.
        /// </summary>
        /// <param name="colIndex">Index of the col.</param>
        private void CreateVisibilityCol(int colIndex)
        {
            var maximalTree = _trees[0, colIndex];
            SetTreeVisible(rowIndex: 0, colIndex, Direction.Top);

            for (var rowIndex = 1; rowIndex < _rows; rowIndex++)
            {
                if (_trees[rowIndex, colIndex] > maximalTree)
                {
                    SetTreeVisible(rowIndex, colIndex, Direction.Top);
                    maximalTree = _trees[rowIndex, colIndex];
                }
            }

            maximalTree = _trees[_rows - 1, colIndex];
            SetTreeVisible(_rows - 1, colIndex, Direction.Bottom);

            for (var rowIndex = _rows - 2; rowIndex >= 0; rowIndex--)
            {
                if (_trees[rowIndex, colIndex] > maximalTree)
                {
                    SetTreeVisible(rowIndex, colIndex, Direction.Bottom);
                    maximalTree = _trees[rowIndex, colIndex];
                }
            }
        }

        /// <summary>
        /// Sets the tree visible in visibility matrix from 'direction'.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="direction">The direction.</param>
        private void SetTreeVisible(int rowIndex, int colIndex, Direction direction)
        {
            _treeVisibilityMatrix[rowIndex, colIndex] = (byte)(_treeVisibilityMatrix[rowIndex, colIndex] | (byte)direction);
        }
    }
}
