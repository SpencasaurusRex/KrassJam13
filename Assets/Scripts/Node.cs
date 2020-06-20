using System.Collections.Generic;
using Shapes;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int MaxAmplitude = 4;
    public int Amplitude;
    public int Frequency;
    [Required]
    public Polyline SoundWave;
    TextMeshPro text;
    Disc disc;
    
    public Color PositiveColor;
    public Color NeutralColor;
    public Color NegativeColor;
    
    List<Connection> connections = new List<Connection>();
    
    void Start()
    {
        disc = GetComponent<Disc>();
        text = GetComponentInChildren<TextMeshPro>();
    }

    void DrawPoints()
    {
        const int Resolution = 100;
        const float inverseResolution = 1f / Resolution;
        
        List<Vector2> points = new List<Vector2>();
        Vector2 point = Vector2.zero;

        float r = disc.Radius;
        for (int i = 0; i < Resolution; i++)
        {
            float theta = i * inverseResolution * Mathf.PI * 2; 
            point.x = Util.Remap(0, Mathf.PI * 2, -r, r, theta);
            float sinOut = Amplitude * Mathf.Sin(theta * Frequency); 
            point.y = Util.Remap(-MaxAmplitude, MaxAmplitude, -r, r, sinOut);
            
            points.Add(point);
        }
        
        SoundWave.SetPoints(points);
    }

    public Color GetColor(float amplitude)
    {
        float t = Mathf.InverseLerp(-MaxAmplitude, MaxAmplitude, amplitude);
        if (t < 0.5f)
        {
            return Util.Lerp(NegativeColor, NeutralColor, t * 2);
        }
        return Util.Lerp(NeutralColor, PositiveColor, t * 2 - 1);
    }

    void Update()
    {
        DrawPoints();
        text.text = Mathf.Abs(Amplitude).ToString();
        text.color = GetColor(Amplitude);
    }

    public void AddConnection(Connection con)
    {
        connections.Add(con);
    }

    public void SendAmplitude()
    {
        foreach (var con in connections)
        {
            con.Send(this);
        }
    }

    public void AbsorbAmplitude()
    {
        foreach (var con in connections)
        {
            con.Absorb(this);
        }
    }
}
