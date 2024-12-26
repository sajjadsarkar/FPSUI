using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Make sure you have DOTween imported

public class LogoAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Rotate and move the logo
        AnimateLogo();
    }

    void AnimateLogo()
    {
        // Rotate 15 degrees to the right over 1 second, then 15 degrees to the left over 1 second, and loop
        transform.DORotate(new Vector3(0, 0, 15), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

    }
}