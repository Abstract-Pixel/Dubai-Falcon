using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    [SerializeField] string soundKey;

    int counter;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "Player")
        {
            if (counter == 8) return;
            counter++;
            AudioManager.Instance.PlayAudio(soundKey);
        }
    }
}
