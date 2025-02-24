using UnityEngine;
using Scripts.Managers;
using System.Collections.Generic;

namespace Scripts.Grid
{
    public class MatchManager : MonoBehaviour
    {
        [SerializeField] private int m_MinMatchLength = 3;

        private GridManager m_GridManager;
        private List<GridPiece> m_MatchedGrids = new();
        private List<GridPiece> m_CheckedGrids = new();
        private List<GridPiece> m_UncheckedGrids = new();
        private List<Vector2Int> m_Directions = new()
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_GridManager = GridManager.Ins;
        }

        public void CheckMatch(GridPiece clickedGrid)
        {
            m_MatchedGrids.Clear();
            m_CheckedGrids.Clear();
            m_UncheckedGrids.Clear();

            m_MatchedGrids.Add(clickedGrid);
            m_UncheckedGrids.Add(clickedGrid);

            while (m_UncheckedGrids.Count > 0)
            {
                CheckGrids();
            }

            if (m_MatchedGrids.Count >= m_MinMatchLength)
            {
                ResetGrids(m_MatchedGrids);
            }
        }

        private void CheckGrids()
        {
            var grid = m_UncheckedGrids[0];
            m_CheckedGrids.Add(grid);
            m_UncheckedGrids.RemoveAt(0);

            var gridPos = new Vector2Int(grid.Row, grid.Column);

            CheckDirection(gridPos);
        }

        private void CheckDirection(Vector2Int startPos)
        {
            var newX = 0;
            var newY = 0;

            for (int i = 0; i < m_Directions.Count; i++)
            {
                newX = startPos.x + m_Directions[i].x;
                newY = startPos.y + m_Directions[i].y;

                if (IsValidGridPosition(newX, newY) && m_GridManager.GetGridPiece(newX, newY).IsFilled)
                {
                    var gridPiece = m_GridManager.GetGridPiece(newX, newY);
                    if (!m_CheckedGrids.Contains(gridPiece))
                    {
                        m_UncheckedGrids.Add(gridPiece);
                        m_MatchedGrids.Add(gridPiece);
                    }
                }
            }
        }

        private bool IsValidGridPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < m_GridManager.gridSize && y < m_GridManager.gridSize;
        }

        private void ResetGrids(List<GridPiece> matchedGrids)
        {
            foreach (var gridPiece in matchedGrids)
            {
                gridPiece.ResetGrid();
            }
        }
    }
}
