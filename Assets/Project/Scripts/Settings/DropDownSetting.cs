using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[Serializable]
public abstract class DropDownSetting<OptionDataType, SettingsDataType>
{
    [SerializeField] public string name;
    [SerializeField] protected TMP_Dropdown dropDownUI;
    [SerializeField] protected OptionDataType[ ] optionsArray;

    public virtual void InitializeSetting()
    {
        InitializeDropDownUiOptions();
        GetSettingData(out SettingsDataType currentSettingValue);
        dropDownUI.onValueChanged.AddListener(SetSettingData);
        dropDownUI.value = (int)(object)currentSettingValue;
        dropDownUI.RefreshShownValue();
    }

    /// 
    /// Needs to be Overridden when the SettingsDataType is more complex than primary types 
    /// like the type Resolution, or if drop down text needs to be displayed differently like a resolution setting
    ///
    public virtual void InitializeDropDownUiOptions()
    {
        dropDownUI.options= new List<TMP_Dropdown.OptionData>();
        foreach (var option in optionsArray)
        {
            dropDownUI.options.Add(new TMP_Dropdown.OptionData(option.ToString()));
        }
    }

    protected abstract void GetSettingData(out SettingsDataType data);

    protected abstract void SetSettingData(int data);
}
