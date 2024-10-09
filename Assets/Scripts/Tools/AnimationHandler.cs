using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AnimationHandler : MonoBehaviour
{
    public static Tween tween;
    //public static bool isTweening = tween.IsPlaying();
    public AnimationHandler()
    {
        DOTween.Init();
    }


    public static void FlipCardWithImageChange(Transform t, Image cardImage, Sprite newSprite, float speed = 0.5f, int direction = 1)
    {
        t.DOScaleX(0, speed / 2).OnComplete(() =>
        {
            cardImage.sprite = newSprite;
            t.DOScaleX(direction, speed / 2);
        });
    }
    public static void FlipVertical(Transform t, int direction, float speed = .5f)
    {

        t.DOScaleX(direction, speed);
    }
    public static void MoveToPosition(Transform t, Vector3 pos, float speed = .5f)
    {

        t.DOMove(pos, speed);
    }
    public static void MoveAllToSinglePoint(List<Transform> t, Vector3 pos, float speed = .5f, Ease easing = Ease.Linear)
    {

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < t.Count; i++)
        {
            sequence.Append(t[i].DOMove(pos, speed).SetEase(easing));
        }
    }
    public static void MoveAllToSpecifiedPoints(Transform[] t, Vector3[] pos, Transform parent, float speed = .5f, Ease easing = Ease.Linear)
    {

        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < t.Length; i++)
        {
            int index = i;
            t[index].SetParent(parent);

            sequence.Append(t[index].DOMove(pos[index], speed).SetEase(easing).OnComplete(() =>
            {
                Card c = t[index].GetComponent<Card>();
                c.FlipCard();
            }));
        }
    }
    public static void MoveToPositionWithCompletion(Transform t, Vector3 pos, float speed = .5f, Ease easing = Ease.Linear)
    {

        t.DOMove(pos, speed).SetEase(easing).OnComplete(() =>
        {
            Debug.Log("anim stop");
        });
    }

    public void MoveToPositionAndWait(Transform t, Vector3 pos, float speed = .5f, Ease easing = Ease.Linear)
    {
        StartCoroutine(MoveToPositionAndWaitC(t, pos, speed, easing));
    }

    public static void MoveAllToParent(Transform[] t, Vector3[] positions, Transform parent, float speed = .5f, Ease easing = Ease.Linear)
    {

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < t.Length; i++)
        {
            int index = i; // avoid loop closuse issue
            sequence.Append(t[i].DOMove(positions[index], speed).SetEase(easing)).OnComplete(() =>
            {
                t[index].SetParent(parent);
                t[index].GetComponent<Card>().FlipCard();
            });
        }
    }
    public static void MoveAllToParent(Transform[] t, Vector3[] positions, Transform parent)
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < t.Length; i++)
        {
            int index = i; // avoid loop closuse issue
            t[i].DOMove(positions[index], 0f).OnComplete(() =>
            {
                t[index].SetParent(parent);
            });
        }
    }
    private static IEnumerator MoveToPositionAndWaitC(Transform t, Vector3 pos, float speed = .5f, Ease easing = Ease.Linear)
    {
        Tween moveTween = t.DOMove(pos, speed).SetEase(easing);
        yield return moveTween.WaitForCompletion();
    }

    #region async
    public static async Task MoveToPositionAsync(Transform t, Vector3 pos, float speed = .5f, Ease easing = Ease.Linear)
    {
        await t.DOMove(pos, speed).SetEase(easing).AsyncWaitForCompletion();
    }

    public static async Task MoveAndFlipAsync(Transform t, Vector3 pos, float moveSpeed = .5f, float flipSpeed = .5f)
    {
        await t.DOMove(pos, moveSpeed).AsyncWaitForCompletion();
        await t.DOScaleX(-1f, flipSpeed).AsyncWaitForCompletion();
    }

    public static async Task MoveAllToSinglePointAsync(List<Transform> t, Vector3 pos, float speed = .5f, Ease easing = Ease.Linear)
    {
        for (int i = 0; i < t.Count; i++)
        {
            await t[i].DOMove(pos, speed).SetEase(easing).AsyncWaitForCompletion();
        }

    }


    #endregion

}
