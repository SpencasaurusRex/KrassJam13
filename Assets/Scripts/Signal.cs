using UnityEngine;

public class Signal : MonoBehaviour
{
    public float TimePerUnit = 0.5f;
    
    public AudioClip High;
    public AudioClip Low;
    
    Node startNode;
    Node endNode;
    float totalTime;
    float currentTime;
    Connection connection;
    bool high;
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
            if (endNode.TryReceiveSignal())
            {
                connection.RemoveSignal(this);
                Destroy(gameObject);    
            }
            else
            {
                var end = startNode;
                startNode = endNode;
                endNode = end;
                currentTime = 0;

                high = !high;
                Factory.Instance.OneShotSound(Vector3.zero, high ? High : Low, 0.5f);
            }
        }
    }
}
