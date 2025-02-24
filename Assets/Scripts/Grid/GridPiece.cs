using DG.Tweening;
using UnityEngine;
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

        private bool m_IsAnimating = false;
        private Sequence m_ScaleSequence;


        private void OnMouseDown()
        {
            if (!m_IsAnimating && !IsFilled)
            {
                OnClick();
            }
        }

        public void OnClick()
        {
            m_IsAnimating = true;
            IsFilled = true;

            m_ScaleSequence = DOTween.Sequence();
            m_ScaleSequence.Append(m_FillObject.transform.DOScale(1.25f, m_AnimationDuration / 2f));
            m_ScaleSequence.Append(m_FillObject.transform.DOScale(1f, m_AnimationDuration / 2f));

            m_ScaleSequence.OnKill(() => m_IsAnimating = false);
        }

        public void SetCoordinates(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
