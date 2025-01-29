using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string key;       // Unique identifier for this sound
    public AudioClip clip;   // The actual audio clip
}

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager Instance { get; private set; }

    [Header("List of Sound Effects")]
    public List<SoundEffect> soundEffects = new List<SoundEffect>();

    private AudioSource audioSource;
    
    // A dictionary to quickly look up clips by key
    private Dictionary<string, AudioClip> soundDict;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure an AudioSource is on this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Initialize the dictionary from the list
        soundDict = new Dictionary<string, AudioClip>();
        foreach (var soundEffect in soundEffects)
        {
            if (!soundDict.ContainsKey(soundEffect.key) && soundEffect.clip != null)
            {
                soundDict.Add(soundEffect.key, soundEffect.clip);
            }
        }
    }

    /// <summary>
    /// Plays a sound effect based on the given key.
    /// </summary>
    public void Play(string key)
    {
        if (!soundDict.ContainsKey(key))
        {
            Debug.LogWarning($"SoundManager: No sound found with key '{key}'");
            return;
        }

        AudioClip clip = soundDict[key];
        audioSource.PlayOneShot(clip);
    }
}
