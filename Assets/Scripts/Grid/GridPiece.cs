using UnityEngine;
using DG.Tweening;
using Scripts.Managers;
using Scripts.Interfaces;

namespace Scripts.Grid
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GridPiece : MonoBehaviour, IClickable
    {
        [SerializeField] private float m_AnimationDuration = 0.5f;
        [SerializeField] private GameObject m_FillObject;
        [field: SerializeField] public bool IsFilled { get; private set; }
        [field: SerializeField] public int Row { get; private set; }
        [field: SerializeField] public int Column { get; private set; }

        private Sequence m_ScaleSequence;
        private GridManager m_GridManager;

        private void Start()
        {
            Initialize();
        }

        private void OnMouseDown()
        {
            if (m_GridManager.canClick && !IsFilled)
            {
                OnClick();
            }
        }

        public void SetCoordinates(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public void ResetGrid()
        {
            m_GridManager.canClick = false;
            m_ScaleSequence = DOTween.Sequence();
            m_ScaleSequence.Append(m_FillObject.transform.DOScale(0f, m_AnimationDuration / 2f));

            m_ScaleSequence.OnKill(() =>
            {
                m_GridManager.canClick = true;
                IsFilled = false;
            });
        }

        public void OnClick()
        {
            m_GridManager.canClick = false;
            IsFilled = true;

            m_ScaleSequence = DOTween.Sequence();
            m_ScaleSequence.Append(m_FillObject.transform.DOScale(1.25f, m_AnimationDuration / 2f));
            m_ScaleSequence.Append(m_FillObject.transform.DOScale(1f, m_AnimationDuration / 2f));

            m_ScaleSequence.OnKill(() =>
            {
                m_GridManager.canClick = true;
                GridManager.Ins.OnGridPieceClicked(this);
            });
        }

        private void Initialize()
        {
            m_GridManager = GridManager.Ins;
        }
    }
}
