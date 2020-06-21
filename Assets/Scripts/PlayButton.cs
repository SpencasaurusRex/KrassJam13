using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Shapes;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [Required]
    public Camera Cam;
    
    [Required]
    public Spin spin;
    
    [Required]
    public Disc CenterDisk;
    
    [Required]
    public Disc RimDisk;

    public float Length;
    
    Polyline line;
    TextMeshPro text;
    
    float rot;
    bool play;
    TweenerCore<float, float, FloatOptions> rotTween;
    TweenerCore<Color, Color, ColorOptions> textTween;
    TweenerCore<Color, Color, ColorOptions> lineTween;
    
    public Color EndColorInner;
    public Color EndColorOuter;
    
    Color clearGreen;
    Color green;
    
    public AudioClip PlayButtonSound;
    
    void Start()
    {
        line = GetComponentInChildren<Polyline>();
        text = GetComponent<TextMeshPro>();
        
        green = text.color;
        clearGreen = text.color;
        clearGreen.a = 0;
    }

    float spinVel;
    bool transition;
    
    void Update()
    {
        if (transition) return;
        if (Physics2D.OverlapPoint(Cam.ScreenToWorldPoint(Input.mousePosition)))
        {
            spin.Rate = Mathf.SmoothDamp(spin.Rate, 100f, ref spinVel, 0.5f);
            
            if (Input.GetMouseButtonDown(0))
            {
                Transition();
                transition = true;
                return;
            }

            if (!play)
            {
                play = true;
                rotTween?.Kill();
                textTween?.Kill();
                lineTween?.Kill();

                rotTween = DOTween.To(() => rot, (float value) => rot = value, 180, Length)
                    .SetEase(Ease.InOutCubic);
                textTween = DOTween.To(() => text.color, (Color value) => text.color = value, clearGreen, Length)
                    .SetEase(Ease.InOutCubic);
                lineTween = DOTween.To(() => line.Color, (Color value) => line.Color = value, green, Length)
                    .SetEase(Ease.InOutCubic);
            }
        }
        else
        {
            spin.Rate = Mathf.SmoothDamp(spin.Rate, 10f, ref spinVel, 0.5f);
            
            if (play)
            {
                play = false;
                rotTween?.Kill();
                textTween?.Kill();
                lineTween?.Kill();

                rotTween = DOTween.To(() => rot, (float value) => rot = value, 0, Length)
                    .SetEase(Ease.InOutCubic);
                textTween = DOTween.To(() => text.color, (Color value) => text.color = value, green, Length)
                    .SetEase(Ease.InOutCubic);
                lineTween = DOTween.To(() => line.Color, (Color value) => line.Color = value, clearGreen, Length)
                    .SetEase(Ease.InOutCubic);
            }
        }

        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    void Transition()
    {
        Factory.Instance.OneShotSound(Vector3.zero, PlayButtonSound, 0.5f);
        rotTween?.Kill();
        textTween?.Kill();
        lineTween?.Kill();
        
        DOTween.To(() => text.color, (Color value) => text.color = value, clearGreen, Length)
            .SetEase(Ease.InOutCubic);
        DOTween.To(() => line.Color, (Color value) => line.Color = value, clearGreen, Length)
            .SetEase(Ease.InOutCubic);
        DOTween.To(() => CenterDisk.Radius, (float value) => CenterDisk.Radius = RimDisk.Radius = value,
            15, 1.5f).SetEase(Ease.InCubic).OnComplete(() => StartCoroutine(NextScene()));
        DOTween.To(() => CenterDisk.ColorInner, (Color value) => CenterDisk.ColorInner = value,
            EndColorInner, 1.0f).SetEase(Ease.InCubic);
        DOTween.To(() => CenterDisk.ColorOuter, (Color value) => CenterDisk.ColorOuter = value,
            EndColorOuter, 1.0f).SetEase(Ease.InCubic);
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
