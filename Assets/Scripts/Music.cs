using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;

    private AudioSource audiosource;

    void Start()
    {
        audiosource = FindObjectOfType<AudioSource>();
        audiosource.loop = false;
    }

    void Update()
    {
        if (!audiosource.isPlaying)
        {
            audiosource.clip = GetRandomClip();
            audiosource.Play();
        }
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
