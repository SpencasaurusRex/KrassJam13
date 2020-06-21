using UnityEngine;

public class OneShotSound : MonoBehaviour
{
    AudioSource source;
    
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        // if (!source.isPlaying)
        // {
        //     Destroy(gameObject);
        // }
    }
}