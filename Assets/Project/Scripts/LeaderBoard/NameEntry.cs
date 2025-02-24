using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameEntry : MonoBehaviour
{
    public InputField nameInputField;

    public void ConfirmName()
    {
        string enteredName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(enteredName))
        {
            enteredName = "Falco"; // Default if no name entered
        }
        PlayerData.Instance.playerName = enteredName;
        SceneManager.LoadScene("Level 1"); // Change to your leaderboard scene name
    }
}