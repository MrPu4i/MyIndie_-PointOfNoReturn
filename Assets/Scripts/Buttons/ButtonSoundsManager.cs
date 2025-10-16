using UnityEngine;

public class ButtonSoundsManager : MonoBehaviour
{
    [SerializeField] private AudioClip hoverEnterSound;
    [SerializeField] private AudioClip hoverExitSound;
    [SerializeField] private AudioClip clickSound;
    
    private AudioSource audioSource;
    
    public static ButtonSoundsManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlayHoverEnter() => PlaySound(hoverEnterSound);
    public void PlayHoverExit() => PlaySound(hoverExitSound);
    public void PlayClick() => PlaySound(clickSound);
    
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
