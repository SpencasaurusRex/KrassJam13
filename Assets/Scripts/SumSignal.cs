using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class SumSignal : MonoBehaviour
{
    public float MaxAmplitude = 6;
    Node[] nodes;
    Polyline polyline;
    void Start()
    {
        nodes = FindObjectsOfType<Node>();
        polyline = GetComponent<Polyline>();
    }
    
    void Update()
    {
        const int Resolution = 100;
        const float inverseResolution = 1f / Resolution;

        List<Vector2> points = new List<Vector2>();
        Vector2 point = Vector2.zero;

        float r = 1;
        for (int i = 0; i < Resolution; i++)
        {
            float theta = i * inverseResolution * Mathf.PI * 2;
            point.x = Util.Remap(0, Mathf.PI * 2, -r, r, theta);
            float tx = Util.Remap(-r, r, 0, 1, point.x);

            float totalSin = 0;
            for (int n = 0; n < nodes.Length; n++)
            {
                if (!nodes[n].Revealed) continue;
                totalSin += nodes[n].SmoothAmplitude * Mathf.Sin(theta * nodes[n].Frequency); 
            }

            float modifier = Mathf.Sqrt(Mathf.Sin(Mathf.PI * tx)); 
            point.y = Util.Remap(-MaxAmplitude, MaxAmplitude, -r, r, totalSin * modifier);

            points.Add(point);
            
        }
        
        polyline.SetPoints(points);
    }
}
