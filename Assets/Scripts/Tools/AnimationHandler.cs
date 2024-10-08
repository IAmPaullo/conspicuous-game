using DG.Tweening;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public AnimationHandler()
    {
        DOTween.Init();
    }


    public static void FlipHorizontal(Transform t, int direction, float speed)
    {
        DOTween.Init();
        t.DOScaleX(direction, speed);
    }
}
