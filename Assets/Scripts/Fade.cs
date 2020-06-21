using DG.Tweening;
using Shapes;
using UnityEngine;

public class Fade : MonoBehaviour
{
    Disc disc;
    public float Length = 1.0f;
    
    void Start()
    {
        disc = GetComponent<Disc>(); 
        
        Color innerClear = disc.ColorInner;
        innerClear.a = 0;
        
        Color outerClear = disc.ColorOuter;
        outerClear.a = 0;

        DOTween.To(() => disc.ColorInner, (Color value) => disc.ColorInner = value, 
            innerClear, Length).SetEase(Ease.InCubic);
        DOTween.To(() => disc.ColorOuter, (Color value) => disc.ColorOuter = value, 
            outerClear, Length).SetEase(Ease.InCubic).OnComplete(() => Destroy(gameObject));
    }
}
