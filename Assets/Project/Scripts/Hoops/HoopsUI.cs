using CustomInspector;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class HoopsUI : MonoBehaviour
{
    int hoopsCollected;
    [SerializeField] TextMeshProUGUI hoopsText;
    [SelfFill][SerializeField]MMF_Player hoopsTextFeedback;
    [SerializeField] string extraText;


    private void OnEnable()
    {
        Hoop.OnHoopCollected+= UpdateHoopsCollectedText;
    }

    private void OnDisable()
    {
        Hoop.OnHoopCollected -= UpdateHoopsCollectedText;
    }

    private void Start()
    {
        hoopsText.text = "Hoops: " + hoopsCollected +extraText;
    }


    void UpdateHoopsCollectedText()
    {
        hoopsCollected++;
        hoopsText.text = "Hoops: " + hoopsCollected;
        hoopsTextFeedback?.PlayFeedbacks();
    }
}
