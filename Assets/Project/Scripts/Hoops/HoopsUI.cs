using TMPro;
using UnityEngine;

public class HoopsUI : MonoBehaviour
{
    int hoopsCollected;
    [SerializeField] TextMeshProUGUI hoopsText;


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
        hoopsText.text = "Hoops: " + hoopsCollected;
    }


    void UpdateHoopsCollectedText()
    {
        hoopsCollected++;
        hoopsText.text = "Hoops: " + hoopsCollected;
    }
}
