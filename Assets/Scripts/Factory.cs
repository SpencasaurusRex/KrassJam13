using Sirenix.OdinInspector;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public static Factory Instance;
    
    [Required]
    public AudioSource OneShotSoundPrefab;
    
    void OnEnable()
    {
        Instance = this;
    }

    public void OneShotSound(Vector3 position, AudioClip clip, float volume)
    {
        var sound = Instantiate(OneShotSoundPrefab);
        sound.transform.position = position;
        sound.clip = clip;
        sound.loop = false;
        sound.volume = volume;
        sound.Play();
    }
}
