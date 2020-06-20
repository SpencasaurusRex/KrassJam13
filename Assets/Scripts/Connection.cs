using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class Connection : MonoBehaviour
{
    public Node A;
    public Node B;
    public ConnectionBehaviour Behaviour;
    
    Polyline polyline;
    
    void Start()
    {
        A.AddConnection(this);
        B.AddConnection(this);
        
        polyline = GetComponent<Polyline>();
        
    }

    void DrawPoints()
    {
        const int Resolution = 100;
        List<Vector2> points = new List<Vector2>();
        List<Color> colors = new List<Color>();

        for (float i = 0; i <= Resolution; i++)
        {
            points.Add(Vector3.Lerp(A.transform.position, B.transform.position, i / Resolution));
            
            float amplitude = Mathf.Lerp(A.Amplitude, B.Amplitude, i / Resolution); 
            colors.Add(A.GetColor(amplitude));            
        }
        
        polyline.SetPoints(points, colors);
    }

    void Update()
    {
        DrawPoints();
    }

    public void Send(Node fromNode)
    {
        if (Behaviour == ConnectionBehaviour.TwoWay)
        {
            if (fromNode == A)
            {
                B.Amplitude++;
                A.Amplitude--;
            }
            else
            {
                A.Amplitude++;
                B.Amplitude--;
            }    
        }
        else if (Behaviour == ConnectionBehaviour.OneWay)
        {
            if (fromNode == A)
            {
                B.Amplitude++;
                A.Amplitude--;
            }
        }
    }

    public void Absorb(Node fromNode)
    {
        print("Absorb");
        if (Behaviour == ConnectionBehaviour.TwoWay)
        {
            if (fromNode == B)
            {
                B.Amplitude++;
                A.Amplitude--;
            }
            else
            {
                A.Amplitude++;
                B.Amplitude--;
            }
        }
        else if (Behaviour == ConnectionBehaviour.OneWay)
        {
            if (fromNode == B)
            {
                B.Amplitude++;
                A.Amplitude--;
            }
        }
    }
}

public enum ConnectionBehaviour
{
    TwoWay,
    OneWay,
}