using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Success : MonoBehaviour
{
    [Required]
    public Disc Rim;
    public Color EndColorInner;
    public Color EndColorOuter;
    
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
            15, 1.5f).SetEase(Ease.InCubic).OnComplete(() => StartCoroutine(NextScene()));
        DOTween.To(() => disc.ColorInner, (Color value) => disc.ColorInner = value,
            EndColorInner, 1.0f).SetEase(Ease.InCubic);
        DOTween.To(() => disc.ColorOuter, (Color value) => disc.ColorOuter = value,
            EndColorOuter, 1.0f).SetEase(Ease.InCubic);
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
