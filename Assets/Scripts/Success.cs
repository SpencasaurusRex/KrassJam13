using DG.Tweening;
using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;

public class Success : MonoBehaviour
{
    [Required]
    public Disc Rim;
    
    Disc disc;
    void Start()
    {
        disc = GetComponent<Disc>();
    }

    float discAngle;
    void Update()
    {
        disc.AngRadiansEnd = Mathf.Deg2Rad * discAngle;
    }

    public void Trigger()
    {
        DOTween.To(() => discAngle, (float value) => discAngle = value,
            360, 1).SetEase(Ease.InOutCubic).OnComplete(Expand);
    }

    void Expand()
    {
        DOTween.To(() => disc.Radius, (float value) => disc.Radius = Rim.Radius = value,
            20, 1.5f).SetEase(Ease.InCubic);
    }
}
