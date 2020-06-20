using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [Required]
    public Camera Camera;

    Node[] nodes;
    Connection[] connections;
    
    void Start()
    {
        nodes = FindObjectsOfType<Node>();
        connections = FindObjectsOfType<Connection>();
    }

    void Update()
    {
        CheckClick();
        print(CheckIfSolved());
    }

    void CheckClick()
    {
        bool left = false;
        bool right = false;
        if (Input.GetMouseButtonDown(0))
        {
            left = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            right = true;
        }

        if (left != right)
        {
            var collider = Physics2D.OverlapPoint(Camera.ScreenToWorldPoint(Input.mousePosition));
            if (collider == null) return;
            
            if (collider.gameObject.TryGetComponent<Node>(out var node))
            {
                if (left)
                {
                    node.SendAmplitude();
                }
                else
                {
                    node.AbsorbAmplitude();
                }
            }
        }
    }

    bool CheckIfSolved()
    {
        Dictionary<int, int> amplitudes = new Dictionary<int, int>();
        foreach (var node in nodes)
        {
            if (!amplitudes.ContainsKey(node.Frequency))
            {
                amplitudes.Add(node.Frequency, 0);
            }
            amplitudes[node.Frequency] += node.Amplitude;
        }
        
        foreach (var amp in amplitudes.Values)
        {
            if (amp != 0)
            {
                return false;                
            }
        }
        
        return true;
        // TODO: Check if connections are currently sending something
    }
}
