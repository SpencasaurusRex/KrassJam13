using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class Connection : MonoBehaviour
{
    public Node A;
    public Node B;
    public ConnectionBehaviour Behaviour;
    public Signal SignalPrefab;
    public float CenterSize;
    
    public Color NegativeColor;
    public Color NeutralColor;
    public Color PositiveColor;
    
    Polyline polyline;
    
    HashSet<Signal> currentSignals = new HashSet<Signal>();
    
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

        for (float i = 0; i < Resolution; i++)
        {
            float t = i / Resolution;
            points.Add(Vector3.Lerp(A.transform.position, B.transform.position, t));
            colors.Add(NeutralColor);
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
                SendSignal(A, B, false);
            }
            else
            {
                SendSignal(B, A, false);
            }    
        }
        else if (Behaviour == ConnectionBehaviour.OneWay)
        {
            if (fromNode == A)
            {
                SendSignal(A, B, false);
            }
        }
    }

    public void Absorb(Node fromNode)
    {
        if (Behaviour == ConnectionBehaviour.TwoWay)
        {
            if (fromNode == B)
            {
                SendSignal(A, B, true);
            }
            else
            {
                SendSignal(B, A, true);
            }
        }
        else if (Behaviour == ConnectionBehaviour.OneWay)
        {
            if (fromNode == B)
            {
                SendSignal(A, B, true);
            }
        }
    }

    void SendSignal(Node fromNode, Node toNode, bool absorb)
    {
        var signal = Instantiate(SignalPrefab);
        signal.SetEndpoints(fromNode, toNode, this);
        fromNode.SendSignal();
        currentSignals.Add(signal);
    }

    public void RemoveSignal(Signal signal)
    {
        currentSignals.Remove(signal);
    }

    public bool HasActiveSignals()
    {
        return currentSignals.Count > 0;
    }
}

public enum ConnectionBehaviour
{
    TwoWay,
    OneWay,
}