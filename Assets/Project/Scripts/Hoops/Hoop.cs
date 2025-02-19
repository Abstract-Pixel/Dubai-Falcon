using UnityEngine;
using System;

public class Hoop : MonoBehaviour
{
    public static Action OnHoopCollected;
    string hoopCollectedSoundKey = "HoopsCollected";
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out AirMovement player))
        {
            OnHoopCollected?.Invoke();
            Destroy(gameObject);
            AudioManager.Instance.PlayAudio(hoopCollectedSoundKey);
        }
    }
}
