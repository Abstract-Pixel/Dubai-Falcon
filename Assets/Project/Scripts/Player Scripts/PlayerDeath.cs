using CustomInspector;
using System;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public static Action OnFalconDie;
    [SelfFill][SerializeField] AirMovement player;
    [SelfFill][SerializeField] Rigidbody rb;
    private void OnCollisionEnter(Collision collision)
    {
        OnFalconDie?.Invoke();
        player.enabled = true;
        rb.useGravity = true;
    }
}
