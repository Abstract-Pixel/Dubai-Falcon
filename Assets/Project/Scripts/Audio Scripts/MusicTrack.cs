using System.Collections.Generic;
using UnityEngine;

public class MusicTrack : MonoBehaviour
{
    [SerializeField] List<SoundOptions> MusicSequences;
    [SerializeField] int currentSequenceCounter;

    public static MusicTrack Instance;
    public AudioSource currentSource; 
    AudioSource previousSource;

    SoundOptions previousMusicSequence;
    SoundOptions currentMusicSequence;

    bool isFading = false;
    float fadeDuration;
    float fadeTimer;
    float initialVolume;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        PlayAudio();
    }

    protected void PlayAudio()
    {
        previousMusicSequence = currentMusicSequence;
        currentMusicSequence = MusicSequences[currentSequenceCounter];
        previousSource = currentSource;
        currentSource = AudioManager.Instance.PlayAudio(currentMusicSequence);
    }

    void Update()
    {
        if (isFading)
        {
            fadeTimer += Time.unscaledDeltaTime;
            if (fadeTimer < fadeDuration)
            {
                float newVolume = Mathf.Lerp(initialVolume, 0f, fadeTimer / fadeDuration);
                previousSource.volume = newVolume;
            }
            else
            {
                EndFade();
            }
        }
    }

    private void EndFade()
    {
        previousSource.volume = 0f;
        AudioManager.Instance.StopAudio(previousMusicSequence);
        fadeTimer = 0f;
        isFading = false;
    }

    public void CrossFade()
    {
        if (isFading)
        {
            EndFade();
        }

        currentSequenceCounter++;
        if(currentSequenceCounter>MusicSequences.Count-1)
        {
            currentSequenceCounter = 0;
        }
        PlayAudio();

        if (previousSource != null)
        {
            fadeDuration = previousMusicSequence.soundPlayOptions.FadeDuration;
            fadeTimer = 0f;
            initialVolume = previousSource.volume;
            isFading = true;
        }
    }

}