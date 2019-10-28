using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource = default;
    [SerializeField] private AudioSource musicSource = default;

    [SerializeField] private AudioClip backgroundMusic = default;
    [SerializeField] private AudioClip explosionSound = default;
    [SerializeField] private AudioClip laserSound = default;
    [SerializeField] private AudioClip teleportSound = default;
    [SerializeField] private AudioClip coinSound = default;
    [SerializeField] private AudioClip shotSound = default;

    private void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = backgroundMusic;
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float val)
    {
        musicSource.volume = val;
    }

    public void SetSfxVolume(float val)
    {
        sfxSource.volume = val;
    }

    public void PlayExplosion()
    {
        sfxSource.PlayOneShot(explosionSound);
    }

    public void PlayLaser()
    {
        sfxSource.PlayOneShot(laserSound);
    }

    public void PlayTeleport()
    {
        sfxSource.PlayOneShot(teleportSound);
    }

    public void PlayCoin()
    {
        sfxSource.PlayOneShot(coinSound);
    }

    public void PlayShot()
    {
        sfxSource.PlayOneShot(shotSound);
    }
}
