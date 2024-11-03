using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource musicSource;
    public static SoundController instance = null;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        if (musicSource != null)
        {
            musicSource.Play();
        }
    }
}
