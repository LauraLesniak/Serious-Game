using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FullscreenExplosion : MonoBehaviour
{
    public static FullscreenExplosion Instance { get; private set; }

    [Header("UI Setup")]
    [Tooltip("Reference to the UI Image component on a Canvas for the explosion.")]
    [SerializeField] private Image explosionImage;

    [Header("Animation Frames")]
    [Tooltip("Array of frames (sprites) for the explosion animation.")]
    [SerializeField] private Sprite[] explosionFrames;

    [Header("Animation Timing")]
    [Tooltip("How many frames to play per second.")]
    [SerializeField] private float framesPerSecond = 10f;

    [Header("Behavior")]
    [Tooltip("Should the image be hidden automatically once the animation finishes?")]
    [SerializeField] private bool hideAfterPlay = true;

    private void Awake()
    {
        // Make this a singleton instance so it can be called from anywhere
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Optionally, you might want to hide the image at startup
        if (explosionImage != null)
        {
            explosionImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Call this method from any script: FullscreenExplosion.Instance.PlayExplosion();
    /// </summary>
    public void PlayExplosion()
    {
        // Make sure we have an Image and frames
        if (explosionImage == null || explosionFrames == null || explosionFrames.Length == 0)
        {
            Debug.LogWarning("FullscreenExplosion is missing required references or frames.");
            return;
        }

        // Start the animation coroutine
        StartCoroutine(PlayExplosionCoroutine());
    }

    private IEnumerator PlayExplosionCoroutine()
    {
        // Show the explosion image
        explosionImage.gameObject.SetActive(true);

        // Calculate the time between frames
        float frameDuration = 1f / framesPerSecond;

        // Iterate over all frames in the array
        for (int i = 0; i < explosionFrames.Length; i++)
        {
            explosionImage.sprite = explosionFrames[i];
            yield return new WaitForSeconds(frameDuration);
        }

        // Optionally hide the image after the animation completes
        if (hideAfterPlay)
        {
            explosionImage.gameObject.SetActive(false);
        }
    }
}
