using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class AudioManager : Singleton<AudioManager>
{
    GameManager _gM;

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip backgroundMusic;

    [Header("General SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip reloadSFX;
    [SerializeField] private AudioClip weaponSwitchSFX;

    [Header("Weapon SFX")]
    public AudioClip pistolShot;
    public AudioClip shotgunShot;
    public AudioClip rifleShot;

    [Header("Pickup SFX")]
    [SerializeField] public AudioClip pickupSFX;


    [Header("Game Over")]
    [SerializeField] private AudioClip gameOverSFX;

    [Header("Volumes")]
    [SerializeField] private float bgmVolume = 0.6f;
    [SerializeField] private float gameoverVolume = 0.6f;

    [SerializeField] private float reloadVolume = 0.6f;
    [SerializeField] private float switchVolume = 0.8f;
    [SerializeField] private float pistolVolume = 1.0f;
    [SerializeField] private float shotgunVolume = 0.7f;
    [SerializeField] private float rifleVolume = 0.9f;
    [SerializeField] public float pickupVolume = 1f;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        _gM = GameManager.Instance;

        if (_gM != null && _gM.player != null)
        {
            _gM.player.OnDeath.AddListener(OnPlayerDeath);
        }

        if (scene.name == "Gameplay" && !bgmSource.isPlaying)
            PlayBGM();
    }

    private void Awake()
    {
        if (Instance != this) // Already an instance exists
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);


        _gM = GameManager.Instance;

        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.playOnAwake = false;
            bgmSource.loop = true;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
        }
    }

    private void OnPlayerDeath()
    {
        bgmSource.Stop();

        if (gameOverSFX != null)
            sfxSource.PlayOneShot(gameOverSFX, gameoverVolume);
    }

    public void PlayBGM()
    {
        if (bgmSource == null || backgroundMusic == null) return;

        bgmSource.clip = backgroundMusic;
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }
    public void PlayReloadSFX()
    {
        var player = _gM.player;

        if (player == null || player.ActiveWeapon == null)
            return;

        WeaponBase w = player.ActiveWeapon;

        bool canReload = 
            w.CurrentClip < w.MaxClipSize && 
            player.HasAmmoFor(w);
        
        if (canReload && reloadSFX != null)
            sfxSource.PlayOneShot(reloadSFX, reloadVolume);
    }

    public void PlayWeaponSwitchSFX()
    {
        var player = _gM.player;

        if (player == null)
            return;

        WeaponBase active = player.ActiveWeapon;
        WeaponBase primary = player.PrimaryWeapon;
        WeaponBase secondary = player.SecondaryWeapon;

        bool canSwitch =
            primary != null &&
            secondary != null &&
            active != null &&
            (active == primary || active == secondary);

        if (canSwitch && weaponSwitchSFX != null)
            sfxSource.PlayOneShot(weaponSwitchSFX, switchVolume);
    }

    public void PlayWeaponShotSFX(WeaponBase weapon)
    {
        if (weapon == null) return;

        AudioClip clip = null;
        float volume = 1f;

        switch (weapon.AmmoType)
        {
            case WeaponType.Pistol:
                clip = pistolShot;
                volume = pistolVolume;
                break;
            case WeaponType.Shotgun:
                clip = shotgunShot;
                volume = shotgunVolume;
                break;
            case WeaponType.Rifle:
                clip = rifleShot;
                volume = rifleVolume;
                break;
        }

        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    public void StopGameOver()
    {
        sfxSource.Stop();
    }
}