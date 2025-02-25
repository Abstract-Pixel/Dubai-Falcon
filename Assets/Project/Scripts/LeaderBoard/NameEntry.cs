using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NameEntryTMP : MonoBehaviour
{
    public TMP_InputField nameInputField;

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