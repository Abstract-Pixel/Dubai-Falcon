using UnityEngine;
using TMPro;

public class NameEntryTMP : MonoBehaviour
{
    public TMP_InputField nameInputField;
    [SerializeField] KeyCode enter;

    void Update()
    {
        if(Input.GetKeyDown(enter))
        {
            ConfirmName();
        }
    }

    public void ConfirmName()
    {
        string enteredName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(enteredName))
        {
            enteredName = "Falco";
        }
        PlayerData.Instance.playerName = enteredName;
        GameManger.instance.LoadGame();
    }
}