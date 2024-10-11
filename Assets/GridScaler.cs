using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridScaler : MonoBehaviour
{
    public float cellWidthRatio = 0.25f;
    public float cellHeightRatio = 0.3f;

    private GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        AdjustCellSize();
    }

    void AdjustCellSize()
    {
        RectTransform parentRect = GetComponentInParent<RectTransform>();

        float cellWidth = parentRect.rect.width * cellWidthRatio;
        float cellHeight = parentRect.rect.height * cellHeightRatio;

        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
    }

    void Update()
    {
        AdjustCellSize();
    }
}
