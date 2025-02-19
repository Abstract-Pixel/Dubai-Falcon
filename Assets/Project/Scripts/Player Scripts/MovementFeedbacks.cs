using CustomInspector;
using UnityEngine;

public class MovementFeedbacks : MonoBehaviour
{
    [SelfFill][SerializeField] AirMovement player;
    [Header("SpeedLines References")]
    [SerializeField] Material speedLinesMaterial;
    [SerializeField] string maskSizeProperty = "_Centre_Mask_Size";

    [Header("Diving Transition Settings")]
    [SerializeField] float transitionDuration;
    [SerializeField] float divingTargetMaskSize;
    [SerializeField] AnimationCurve transitionCurve;

    [ReadOnly][SerializeField] float timer;


    void HandleOnFalconDiving()
    {

    }

    void HandleOnFalconFlyingUp()
    {

    }

}
