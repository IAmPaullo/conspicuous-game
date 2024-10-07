using UnityEditor;
using UnityEngine;

public class CardSO : ScriptableObject
{
    public int ID;
    public string cardName;
    [TextArea(1, 5)]
    public string description = "\n\n\n";

    [Space]
    public Sprite sprite;

}
#if UNITY_EDITOR
[CustomEditor(typeof(CardSO))]
public class SpriteDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CardSO spriteData = (CardSO)target;

        if (spriteData.sprite != null)
        {
            Rect rect = GUILayoutUtility.GetRect(100, 100, GUILayout.ExpandWidth(false));
            EditorGUI.DrawPreviewTexture(rect, spriteData.sprite.texture);
        }
    }
}
#endif