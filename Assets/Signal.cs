using UnityEngine;

public class Signal : MonoBehaviour
{
    public float TimePerUnit = 0.5f;
    
    Node StartNode;
    Node EndNode;
    float totalTime;
    float currentTime;
    
    public void SetEndpoints(Node startNode, Node endNode)
    {
        StartNode = startNode;
        EndNode = endNode;
        var distance = (endNode.transform.position - startNode.transform.position).magnitude;
        totalTime = distance * TimePerUnit;
        
        transform.position = startNode.transform.position;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        transform.position = Vector3.Lerp(StartNode.transform.position, EndNode.transform.position,
            currentTime / totalTime);

        if (currentTime >= totalTime)
        {
            EndNode.ReceiveSignal();
            Destroy(gameObject);
        }
    }
}
