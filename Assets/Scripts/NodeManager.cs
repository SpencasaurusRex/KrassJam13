using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [Required]
    public Camera Camera;
    
    [Required]
    public Transform BackgroundTransform;
    public float SpinSpeed;
    
    Node[] nodes;
    Connection[] connections;
    
    Dictionary<int, List<Node>> nodeLookup = new Dictionary<int, List<Node>>();
    
    void Start()
    {
        nodes = FindObjectsOfType<Node>();
        connections = FindObjectsOfType<Connection>();

        foreach (var node in nodes)
        {
            if (!nodeLookup.ContainsKey(node.Frequency))
            {
                nodeLookup.Add(node.Frequency, new List<Node>());    
            }
            nodeLookup[node.Frequency].Add(node);
        }
    }

    void Update()
    {
        CheckClick();
        print(CheckIfSolved());
        
        BackgroundTransform.rotation *= Quaternion.AngleAxis(SpinSpeed * Time.deltaTime, Vector3.forward);
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
                    node.RequestSendAmplitude();
                }
                else
                {
                    node.RequestAbsorbAmplitude();
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
        
        bool solved = true;
        foreach (var freq in amplitudes.Keys)
        {
            var amp = amplitudes[freq];
            List<Node> frequencyNodes = nodeLookup[freq];
            var ampSplit = Mathf.Abs(amp) / (float)frequencyNodes.Count;
            foreach (var node in frequencyNodes)
            {
                node.SetVolume(ampSplit);
            }

            if (amp != 0)
            {
                solved = false;
            }
        }
        
        return solved;
        // TODO: Check if connections are currently sending something
    }
}
