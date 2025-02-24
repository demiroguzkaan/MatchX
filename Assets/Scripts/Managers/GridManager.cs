using UnityEngine;
using Scripts.Grid;

namespace Scripts.Managers
{
    public class GridManager : MonoBehaviour
    {
        private static GridManager m_Ins;
        public static GridManager Ins
        {
            get
            {
                if (!m_Ins)
                    m_Ins = FindObjectOfType<GridManager>();
                return m_Ins;
            }
        }

        public int gridSize;
        public bool canClick;

        [SerializeField] private float m_GridSpacing = 0.5f;
        [SerializeField] private Transform m_GridsParent;
        [SerializeField] private GridPiece m_GridPiecePrefab;
        [SerializeField] private MatchManager m_GridMatchChecker;

        private Camera m_MainCamera;
        private GridPiece[,] m_Grids;


        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_MainCamera = Camera.main;
            GenerateGrid();
        }

        public void GenerateGrid()
        {
            if (!canClick || gridSize <= 0) return;

            ClearGrid();
            m_Grids = new GridPiece[gridSize, gridSize];

            var startX = -(gridSize / 2f) * m_GridSpacing + 0.5f * m_GridSpacing;
            var startY = -(gridSize / 2f) * m_GridSpacing + 0.5f * m_GridSpacing;

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    var spawnPosition = new Vector2(startX + x * m_GridSpacing, startY + y * m_GridSpacing);
                    var gridPiece = Instantiate(m_GridPiecePrefab, spawnPosition, Quaternion.identity, m_GridsParent);
                    gridPiece.SetCoordinates(x, y);
                    m_Grids[x, y] = gridPiece;
                }
            }

            var minX = m_Grids[0, 0].transform.localPosition.x;
            var maxX = m_Grids[gridSize - 1, 0].transform.localPosition.x;

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
            var gridWidth = maxX - minX + m_GridSpacing * 2;
            var requiredSize = gridWidth / (2f * m_MainCamera.aspect);
            m_MainCamera.orthographicSize = Mathf.Max(5, requiredSize);
        }

        public void OnGridPieceClicked(GridPiece clickedGrid)
        {
            m_GridMatchChecker.CheckMatch(clickedGrid);
        }

        public GridPiece GetGridPiece(int x, int y)
        {
            return m_Grids[x, y];
        }
    }
}
