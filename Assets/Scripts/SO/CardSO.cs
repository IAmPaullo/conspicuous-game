using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Card ", menuName = "Cards/New Card", order = 1)]
public class CardSO : ScriptableObject
{
    public int ID;
    public string cardName;
    [TextArea(1, 5)]
    public string description = "\n\n\n";

    [Space]
    public Sprite sprite;

    public Color debugColor;

    public void RandomColor()
    {
        debugColor = new(Random.value, Random.value, Random.value);
    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(CardSO))]
public class CardSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CardSO cardSO = (CardSO)target;


        if (cardSO.sprite != null)
        {
            Rect rect = GUILayoutUtility.GetRect(100, 100, GUILayout.ExpandWidth(false));
            EditorGUI.DrawPreviewTexture(rect, cardSO.sprite.texture);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Generate Random Color"))
        {
            cardSO.RandomColor();

            EditorUtility.SetDirty(cardSO);
        }
    }
}
#endif