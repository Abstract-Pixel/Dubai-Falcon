using CustomInspector;
using System;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public static Action OnFalconDie;
    [SelfFill][SerializeField] AirMovement player;
    [SelfFill][SerializeField] Rigidbody rb;

    string falconDeadSoundKey = "FalconDead";

    string gameMusicSoundKey= "GameMusic";
    string windDivingSoundKey= "WindDiving";
    string wingsFlappingSoundKey= "WingsFlap";
    string falconFlyingUpSoundKey= "FalconUp";
    string FalconDivingSoundKey = "FalconDiving";
    private void OnCollisionEnter(Collision collision)
    {
        OnFalconDie?.Invoke();
        player.enabled = false;
        rb.useGravity = true;
        AudioManager.Instance.PlayAudio(falconDeadSoundKey);
        AudioManager.Instance.StopAudio(falconFlyingUpSoundKey);
        AudioManager.Instance.StopAudio(gameMusicSoundKey);
        AudioManager.Instance.StopAudio(windDivingSoundKey);
        AudioManager.Instance.StopAudio(wingsFlappingSoundKey);
        AudioManager.Instance.StopAudio(FalconDivingSoundKey);
    }
}
