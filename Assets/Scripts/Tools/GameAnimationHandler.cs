using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class GameAnimationHandler : MonoBehaviour
{
    public static Tween tween;
    //public static bool isTweening = tween.IsPlaying();
    public GameAnimationHandler()
    {
        DOTween.Init();
    }

    public static void ColorObject(Image image, Color color, float endValue)
    {
        image.DOColor(color, endValue);
    }

    public static void FlipCardWithImageChange(Transform t, Image cardImage, Sprite newSprite, float speed = 0.5f, int direction = 1)
    {
        t.DOKill(true);
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

    public static void StartHoverAnimation(Transform t, float scaleAmount = 1.1f, float duration = 0.5f)
    {

        t.DOScale(scaleAmount, duration)
                             .SetLoops(-1, LoopType.Yoyo)
                             .SetEase(Ease.InOutSine);
    }


    public static void StopHoverAnimation(Transform t, float duration = 0.3f)
    {

        t.DOKill();
        t.DOScale(1f, duration).SetEase(Ease.OutSine);
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
    public static void MoveAllToSpecifiedPoints(Transform[] t, Vector3[] pos, Transform parent, float speed = .5f, Ease easing = Ease.Linear, Action onComplete = null)
    {

        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < t.Length; i++)
        {
            int index = i;
            t[index].SetParent(parent);

            sequence.Append(t[index].DOMove(pos[index], speed).SetEase(easing).OnComplete(() =>
            {
                Card c = t[index].GetComponent<Card>();
                c.Flip(true);
            }));
        }
        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
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
                t[index].GetComponent<Card>().Flip(true);
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

    public static void MoveTogether(Transform t1, Transform t2, Transform finalPos, float speed = 0.5f, Ease easing = Ease.Linear)
    {

        Vector3 middlePoint = (t1.position + t2.position) / 2;


        Sequence moveToMiddle = DOTween.Sequence();

        moveToMiddle.Join(t1.DOMove(middlePoint, speed).SetEase(easing));
        moveToMiddle.Join(t2.DOMove(middlePoint, speed).SetEase(easing));


        moveToMiddle.OnComplete(() =>
        {
            t1.DOMove(finalPos.position, speed).SetEase(easing)
            .OnComplete(() => t1.SetParent(finalPos));
            t2.DOMove(finalPos.position, speed).SetEase(easing)
            .OnComplete(() => t2.SetParent(finalPos)); ;
        });
    }
    public static void MatchSuccessAnimation(Transform t1, Transform t2, float speed = 0.5f, Ease easing = Ease.InOutBack)
    {
        Sequence matchSequence = DOTween.Sequence();
        matchSequence.Join(t1.DOScale(Vector3.zero, speed).SetEase(easing));
        matchSequence.Join(t2.DOScale(Vector3.zero, speed).SetEase(easing));
        matchSequence.OnComplete(() =>
        {
            t1.gameObject.SetActive(false);
            t2.gameObject.SetActive(false);
        });
    }

    public static void MatchFailAnimation(Transform t1, Transform t2, float speed = 0.2f)
    {
        Sequence mismatchSequence = DOTween.Sequence();
        mismatchSequence.Join(t1.DOShakePosition(speed, strength: new Vector3(0.2f, 0.2f, 0)));
        mismatchSequence.Join(t2.DOShakePosition(speed, strength: new Vector3(0.2f, 0.2f, 0)));
        mismatchSequence.OnComplete(() =>
        {
            t1.DOScale(Vector3.one, 0.2f);
            t2.DOScale(Vector3.one, 0.2f);
        });
    }


    public static void ExitCard(Transform t, Vector3 exitPosition, Vector3 scale, float speed = 0.5f, Ease easing = Ease.Linear)
    {

        Sequence sequence = DOTween.Sequence();


        sequence.Join(t.DOMove(exitPosition, speed).SetEase(Ease.InOutQuad));

        sequence.Join(t.DOScale(scale, speed).SetEase(Ease.InOutQuad));

        sequence.Play();
    }

    internal static void HoverCard(Transform t, bool scaleUp, float speed = 0.5f, Ease easing = Ease.OutQuad)
    {
        float scale = scaleUp ? 1.2f : 1f;
        t.DOScale(scale, speed).SetEase(easing);
        if (scaleUp)
        {

            t.DORotate(new Vector3(0f, 0f, 10f), 0.6f, RotateMode.LocalAxisAdd)
                .From(new Vector3(0f, 0f, -10f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);


            t.DOLocalMoveY(t.localPosition.y + 0.2f, 0.3f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
        else
        {

            ResetCard(t, speed);
        }

    }

    public static void ResetCard(Transform t, float speed = 0.3f)
    {
        t.DOKill(true);

        t.DOScale(1f, speed).SetEase(Ease.OutQuad);

        t.DORotate(Vector3.zero, speed).SetEase(Ease.OutQuad);
    }

    public static void ResetCard(Transform t, Vector3 originalScale, float speed = 0f)
    {
        t.DOKill();
        t.DOScale(originalScale, speed).SetEase(Ease.OutQuad);
        t.DORotate(Vector3.zero, speed).SetEase(Ease.OutQuad);
    }


    #endregion

}
