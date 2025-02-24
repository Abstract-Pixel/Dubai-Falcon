using UnityEngine;
using System;

public class Hoop : MonoBehaviour
{
    public static Action OnHoopCollected;
    string hoopCollectedSoundKey = "HoopsCollected";
    string featherSoundKey = "Feathers";
    [SerializeField] ParticleSystem hoopCollectedParticles;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out AirMovement player))
        {
            OnHoopCollected?.Invoke();
            Destroy(gameObject);

            AudioManager.Instance.PlayAudio(featherSoundKey);
            AudioManager.Instance.PlayAudio(hoopCollectedSoundKey);
            ParticleSystem particle = Instantiate(hoopCollectedParticles,transform.position, Quaternion.identity,player.transform);
            Destroy(particle.gameObject, particle.main.duration +0.5f);
        }
    }

}
