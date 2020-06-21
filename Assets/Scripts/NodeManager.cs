using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [Required]
    public Camera Camera;
    
    [Required]
    public Transform BackgroundTransform;
    public float SpinSpeed;
    
    [Required]
    public Success SuccessAnim;
    
    Node[] nodes;
    Connection[] connections;
    
    Dictionary<int, List<Node>> nodeLookup = new Dictionary<int, List<Node>>();
    
    float extraSpinSpeed;
    float decayRate = 50;
    
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

    bool solved;
    
    void Update()
    {
        CheckClick();
        
        
        extraSpinSpeed = Mathf.Max(0, extraSpinSpeed - Time.deltaTime * decayRate);
        BackgroundTransform.rotation *= Quaternion.AngleAxis((SpinSpeed + extraSpinSpeed) * Time.deltaTime, Vector3.forward);

        if (CheckIfSolved() && !solved)
        {
            solved = true;
            StartCoroutine(Success());
        }
        
    }

    IEnumerator Success()
    {
        yield return new WaitForSeconds(0.8f);
        extraSpinSpeed += 200;
        SuccessAnim.Trigger();
    }

    void CheckClick()
    {
        bool left = false;
        bool right = false;
        if (Input.GetMouseButtonDown(0))
        {
            left = true;
            extraSpinSpeed += 50;
        }

        if (Input.GetMouseButtonDown(1))
        {
            right = true;
            extraSpinSpeed += 50;
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
        
        print(connections.FirstOrDefault(c => c.HasActiveSignals()));
        return solved && !connections.Any(c => c.HasActiveSignals());
    }
}
