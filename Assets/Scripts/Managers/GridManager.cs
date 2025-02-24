using UnityEngine;
using Scripts.Grid;

namespace Scripts.Managers
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private int m_GridSize;
        [SerializeField] private float m_GridSpacing = 0.5f;
        [SerializeField] private Transform m_GridsParent;
        [SerializeField] private GridPiece m_GridPiecePrefab;

        private Camera m_MainCamera;
        private GridPiece[,] m_Grids;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_MainCamera = Camera.main;
            GenerateGrid(m_GridSize);
        }

        public void GenerateGrid(int size)
        {
            ClearGrid();
            m_Grids = new GridPiece[size, size];

            var startX = -(size / 2f) * m_GridSpacing + 0.5f * m_GridSpacing;
            var startY = -(size / 2f) * m_GridSpacing + 0.5f * m_GridSpacing;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    var spawnPosition = new Vector2(startX + x * m_GridSpacing, startY + y * m_GridSpacing);
                    var gridPiece = Instantiate(m_GridPiecePrefab, spawnPosition, Quaternion.identity, m_GridsParent);
                    gridPiece.SetCoordinates(x, y);
                    m_Grids[x, y] = gridPiece;
                }
            }

            var minX = m_Grids[0, 0].transform.localPosition.x;
            var maxX = m_Grids[size - 1, 0].transform.localPosition.x;

            AdjustCamera(minX, maxX);
        }

        private void ClearGrid()
        {
            foreach (Transform child in m_GridsParent)
            {
                Destroy(child.gameObject);
            }
        }

        private void AdjustCamera(float minX, float maxX)
        {
            float gridWidth = maxX - minX + m_GridSpacing * 2;
            float requiredSize = gridWidth / (2f * m_MainCamera.aspect);
            m_MainCamera.orthographicSize = Mathf.Max(5, requiredSize);
        }
    }
}