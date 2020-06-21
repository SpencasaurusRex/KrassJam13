using System.Collections.Generic;
using Shapes;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Node : MonoBehaviour
{
    public int MaxAmplitude = 4;
    public int Amplitude;
    public float SmoothAmplitude;
    float ampVel;
    
    public int Frequency;
    
    public AudioClip[] FrequencyClips;
    
    public TextMeshPro Text;
    public TextMeshPro Text2;

    [Required] public Polyline SoundWave;
    
    float startingRadius;
    [Required]
    public Disc DiscBody;
    [Required]
    public Disc DiscRim;

    public Color PositiveColor;
    public Color NeutralColor;
    public Color NegativeColor;

    AudioSource audio;
    float targetVolume;
    List<Connection> connections = new List<Connection>();

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = FrequencyClips[Frequency - 1];
        audio.loop = true;
        
        Text.text = Mathf.Abs(Amplitude).ToString();
        Text.color = GetColor(Amplitude);
        
        startingRadius = DiscBody.Radius;
    }

    void DrawPoints()
    {
        const int Resolution = 100;
        const float inverseResolution = 1f / Resolution;

        List<Vector2> points = new List<Vector2>();
        Vector2 point = Vector2.zero;

        float r = DiscBody.Radius;
        for (int i = 0; i < Resolution; i++)
        {
            float theta = i * inverseResolution * Mathf.PI * 2;
            point.x = Util.Remap(0, Mathf.PI * 2, -r, r, theta);
            float tx = Util.Remap(-r, r, 0, 1, point.x);
            float sinOut = Mathf.Sin(Mathf.PI * tx) * SmoothAmplitude * Mathf.Sin(theta * Frequency);
            point.y = Util.Remap(-MaxAmplitude, MaxAmplitude, -r, r, sinOut);

            points.Add(point);
        }

        SoundWave.SetPoints(points);
    }

    public Color GetColor(float amplitude)
    {
        if (Amplitude < 0)
        {
            return NegativeColor;
        }
        if (Amplitude > 0)
        {
            return PositiveColor;
        }
        return NeutralColor;
    }

    public float SmoothTime = 0.1f;
    
    float volumeVel;
    
    void Update()
    {
        SmoothAmplitude = Mathf.SmoothDamp(SmoothAmplitude, Amplitude, ref ampVel, SmoothTime);
        DiscBody.Radius = startingRadius + radiusOffset;
        DiscRim.Radius = startingRadius + radiusOffset;
        DrawPoints();
        
        audio.volume = Mathf.SmoothDamp(audio.volume, targetVolume, ref volumeVel, 0.2f); 
    }

    public void AddConnection(Connection con)
    {
        connections.Add(con);
    }

    public void RequestSendAmplitude()
    {
        foreach (var con in connections)
        {
            con.Send(this);
        }
    }

    public void RequestAbsorbAmplitude()
    {
        foreach (var con in connections)
        {
            con.Absorb(this);
        }
    }
    
    public float RadiusAnimationLength = 0.2f;
    public float RadiusAnimationAmount = 0.5f;
    float radiusOffset;
    
    public void ReceiveSignal()
    {
        radiusOffset = RadiusAnimationAmount;
        DOTween.To(() => radiusOffset, (float value) => radiusOffset = value, 
            0, RadiusAnimationLength).SetEase(Ease.OutCubic);
        Amplitude++;
        UpdateText();
    }

    public void SendSignal()
    {
        radiusOffset = -RadiusAnimationAmount;
        DOTween.To(() => radiusOffset, (float value) => radiusOffset = value, 
            0, RadiusAnimationLength).SetEase(Ease.OutCubic);
        Amplitude--;
        UpdateText();
    }

    public void UpdateText()
    {
        TextMeshPro fadingText;
        TextMeshPro newText;
        if (Text.color.a > Text2.color.a)
        {
            fadingText = Text;
            newText = Text2;
        }
        else
        {
            fadingText = Text2;
            newText = Text;
        }

        var newColor = GetColor(SmoothAmplitude);
        newColor.a = 0;
        newText.color = newColor;
        newText.DOKill();
        newText.text = Mathf.Abs(Amplitude).ToString();
        newText.DOFade(1, 0.15f);
        
        fadingText.DOKill();
        fadingText.DOFade(0, 0.15f);
    }

    public void SetVolume(float amplitude)
    {
        targetVolume = Mathf.Clamp01(Mathf.Sqrt(amplitude) / 3.0f);
    }
}
