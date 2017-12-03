using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] helloList;
    [SerializeField] private AudioClip[] yeahList;
    [SerializeField] private AudioSource sound;

    public static SoundManager Instance;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void PlayHello()
    {
        if (helloList.Length <= 0) return;

        sound.PlayOneShot(helloList[Random.Range(0,helloList.Length)]);
    }

    public void PlayYeah()
    {
        if (yeahList.Length <= 0) return;

        sound.PlayOneShot(yeahList[Random.Range(0, yeahList.Length)]);
    }
}
