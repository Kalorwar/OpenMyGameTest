using UnityEngine;

namespace Project.Scripts.Grid
{
    public class GridLayoutCalculator
    {
        private readonly float _fixedBottomGridY;
        private readonly float _screenFillAmount;
        private Vector3 _gridStartPosition;

        public GridLayoutCalculator(float screenFillAmount, float fixedBottomGridY)
        {
            _screenFillAmount = screenFillAmount;
            _fixedBottomGridY = fixedBottomGridY;
        }

        public float CellSize { get; private set; }

        public void Calculate(Camera camera, int width, int height)
        {
            var screenHeight = camera.orthographicSize * 2;
            var screenWidth = screenHeight * camera.aspect;

            var availableWidth = screenWidth * _screenFillAmount;
            var availableHeight = screenHeight * _screenFillAmount;

            var cellSizeByWidth = availableWidth / width;
            var cellSizeByHeight = availableHeight / height;

            CellSize = Mathf.Min(cellSizeByWidth, cellSizeByHeight);

            var totalGridWidth = width * CellSize;

            _gridStartPosition = new Vector3(
                -totalGridWidth / 2,
                _fixedBottomGridY - CellSize / 2,
                0
            );
        }

        public Vector3 GetCellCenter(int x, int y)
        {
            return new Vector3(
                _gridStartPosition.x + x * CellSize + CellSize / 2,
                _gridStartPosition.y + y * CellSize + CellSize / 2,
                0
            );
        }
    }
}