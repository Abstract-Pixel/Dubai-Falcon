using CustomInspector;
using MoreMountains.Tools;
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
    [SerializeField] float startMaskSize = 0.7f;
    [SerializeField] AnimationCurve transitionCurve;

    [ReadOnly][SerializeField] float timer;
    [ReadOnly][SerializeField] float currentMaskSize;

    private float initialMaskSize;
    private bool isTransitioning;
    private bool isReturningToZero;

    private void OnEnable()
    {
        if (player != null)
        {
            player.OnFalconDiving += HandleOnFalconDiving;
            player.OnFalconFlyingUp += HandleOnFalconFlyingUp;
            player.OnFalconIdle += HandleOnFalconIdle;
        }
        else
        {
            Debug.LogError("AirMovement player is not assigned. Please assign it in the inspector.");
        }
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.OnFalconDiving -= HandleOnFalconDiving;
            player.OnFalconFlyingUp -= HandleOnFalconFlyingUp;
            player.OnFalconIdle -= HandleOnFalconIdle;
        }
    }

    void Start()
    {
        speedLinesMaterial.SetFloat(maskSizeProperty, startMaskSize);
        currentMaskSize = startMaskSize;
    }

    void HandleOnFalconDiving()
    {
        StartTransition(divingTargetMaskSize);
    }

    void HandleOnFalconFlyingUp()
    {
        StartTransition(divingTargetMaskSize);
    }

    void HandleOnFalconIdle()
    {
        
        if(!isReturningToZero && isTransitioning)
        {
            isTransitioning = false;
            isReturningToZero = true;
            timer = 0f;
        }
    }

    void StartTransition(float targetMaskSize)
    {
        if (!isTransitioning)
        {
            initialMaskSize = speedLinesMaterial.GetFloat(maskSizeProperty);
            timer = 0f;
            isTransitioning = true;
            isReturningToZero = false;
        }
    }

    void Update()
    {
        if (isTransitioning)
        {
            timer += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(timer / transitionDuration);
            float curveValue = transitionCurve.Evaluate(normalizedTime);
            currentMaskSize = Mathf.Lerp(initialMaskSize, divingTargetMaskSize, curveValue);
            speedLinesMaterial.SetFloat(maskSizeProperty, currentMaskSize);

            if (timer >= transitionDuration)
            {
                isTransitioning = false;
                isReturningToZero = true;
                initialMaskSize = currentMaskSize;
                timer = 0f;
            }
        }
        else if (isReturningToZero)
        {
            timer += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(timer / transitionDuration);
            float curveValue = transitionCurve.Evaluate(normalizedTime);
            currentMaskSize = Mathf.Lerp(initialMaskSize, startMaskSize, curveValue);
            speedLinesMaterial.SetFloat(maskSizeProperty, currentMaskSize);

            if (timer >= transitionDuration)
            {
                isReturningToZero = false;
            }
        }
    }
}