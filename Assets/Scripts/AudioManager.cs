using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;

    [Header("sound clips")]
    [SerializeField] AudioClip enterSound;
    [SerializeField] AudioClip cancelSound;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEnterBtnSound()
    {
        m_AudioSource.clip = enterSound;
        m_AudioSource.Play();
    }

    public void PlayCancelBtnSound()
    {
        m_AudioSource.clip = cancelSound;
        m_AudioSource.Play();
    }
}
