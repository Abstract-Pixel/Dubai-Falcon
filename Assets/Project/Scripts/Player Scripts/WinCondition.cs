using CustomInspector;
using System;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] int hoopsRequiredToWin;
    [ReadOnly][SerializeField] int currentHoopsCollected;

    public static Action OnAllHoopsCollected;

    string gameMusicSoundKey = "GameMusic";
    string windDivingSoundKey = "WindDiving";
    string wingsFlappingSoundKey = "WingsFlap";
    string falconFlyingUpSoundKey = "FalconUp";
    string FalconDivingSoundKey = "FalconDiving";
    void Start()
    {
        Hoop.OnHoopCollected +=handleHoopCollection;
    }

    private void OnDisable()
    {
        Hoop.OnHoopCollected -=handleHoopCollection;
    }

    public void handleHoopCollection()
    {
        currentHoopsCollected++;
        if(currentHoopsCollected >= hoopsRequiredToWin)
        {
               OnAllHoopsCollected?.Invoke();
            AudioManager.Instance.StopAudio(falconFlyingUpSoundKey);
            AudioManager.Instance.StopAudio(gameMusicSoundKey);
            AudioManager.Instance.StopAudio(windDivingSoundKey);
            AudioManager.Instance.StopAudio(wingsFlappingSoundKey);
            AudioManager.Instance.StopAudio(FalconDivingSoundKey);
        }
    }
}
