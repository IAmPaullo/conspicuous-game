using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public AnimationHandler()
    {
        DOTween.Init();
    }


    public static void FlipHorizontal(Transform t, int direction, float speed = .5f)
    {
        DOTween.Init();
        t.DOScaleX(direction, speed);
    }
    public static void FlipVertical(Transform t, int direction, float speed = .5f)
    {
        DOTween.Init();
        t.DOScaleX(direction, speed);
    }
    public static void MoveToPosition(Transform t, Vector3 pos, float speed = .5f)
    {
        DOTween.Init();
        t.DOMove(pos, speed);
    }
    public static void MoveAllToSinglePoint(List<Transform> t, Vector3 pos, float speed = .5f, Ease easing = Ease.Linear)
    {
        DOTween.Init();
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < t.Count; i++)
        {
            sequence.Append(t[i].DOMove(pos, speed).SetEase(easing));
        }
    }
    public static void MoveAllToSpecifiedPoints(Transform[] t, Vector3[] pos, float speed = .5f, Ease easing = Ease.Linear)
    {
        DOTween.Init();
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < t.Length; i++)
        {
            sequence.Append(t[i].DOMove(pos[i], speed).SetEase(easing));
        }
    }


}
