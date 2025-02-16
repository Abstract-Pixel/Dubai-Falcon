using CustomInspector;
using System;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] int hoopsRequiredToWin;
    [ReadOnly][SerializeField] int currentHoopsCollected;

    public static Action OnAllHoopsCollected;
    void Start()
    {
        Hoop.OnHoopCollected +=handleHoopCollection;
    }

    private void OnDisable()
    {
        Hoop.OnHoopCollected -=handleHoopCollection;
    }

    public void handleHoopCollection()
    {
        currentHoopsCollected++;
        if(currentHoopsCollected >= hoopsRequiredToWin)
        {
               OnAllHoopsCollected?.Invoke();
        }
    }
}
