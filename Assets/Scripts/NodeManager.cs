using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
        
        StartCoroutine(RevealNodes());
    }

    IEnumerator RevealNodes()
    {
        foreach (var node in nodes)
        {
            yield return new WaitForSeconds(1f);
            node.Reveal();
        }
    }

    bool solved;
    
    void Update()
    {
        extraSpinSpeed = Mathf.Max(0, Mathf.Min(extraSpinSpeed - Time.deltaTime * decayRate, 150));
        BackgroundTransform.rotation *= Quaternion.AngleAxis((SpinSpeed + extraSpinSpeed) * Time.deltaTime, Vector3.forward);

        if (CheckIfSolved() && !solved)
        {
            solved = true;
            StartCoroutine(Success());
        }

        if (!solved)
        {
            CheckClick();
        }
    }

    IEnumerator Success()
    {
        DOTween.To(() => extraSpinSpeed, (float value) => extraSpinSpeed = value, 200, 0.8f);
        yield return new WaitForSeconds(0.8f);
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
        
        return solved && !connections.Any(c => c.HasActiveSignals());
    }
}
