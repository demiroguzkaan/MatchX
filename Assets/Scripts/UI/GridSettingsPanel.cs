using Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class GridSettingsPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_GridSizeField;
        [SerializeField] private Button m_RebuildGridButton;

        private int m_GridSize;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_GridSizeField.onValueChanged.AddListener(OnGridSizeFieldChanged);
            m_RebuildGridButton.onClick.AddListener(SetGridSize);
        }

        private void SetGridSize()
        {
            GridManager.Ins.gridSize = m_GridSize;
            GridManager.Ins.GenerateGrid();
            m_GridSizeField.text = string.Empty;
        }

        private void OnGridSizeFieldChanged(string value)
        {
            int.TryParse(value, out m_GridSize);
        }
    }
}
