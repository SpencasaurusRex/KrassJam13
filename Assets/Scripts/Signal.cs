using UnityEngine;

public class Signal : MonoBehaviour
{
    public float TimePerUnit = 0.5f;
    
    Node startNode;
    Node endNode;
    float totalTime;
    float currentTime;
    Connection connection;
    
    public void SetEndpoints(Node startNode, Node endNode, Connection connection)
    {
        this.startNode = startNode;
        this.endNode = endNode;
        this.connection = connection;
        
        var distance = (endNode.transform.position - startNode.transform.position).magnitude;
        totalTime = distance * TimePerUnit;
        
        transform.position = startNode.transform.position;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        transform.position = Vector3.Lerp(startNode.transform.position, endNode.transform.position,
            currentTime / totalTime);

        if (currentTime >= totalTime)
        {
            endNode.ReceiveSignal();
            connection.RemoveSignal(this);
            Destroy(gameObject);
        }
    }
}
