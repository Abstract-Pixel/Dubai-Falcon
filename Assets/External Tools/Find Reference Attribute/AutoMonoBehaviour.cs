using UnityEngine;
using System.Reflection;

public class AutoMonoBehaviour : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        AssignFindReferences();
    }

    protected void AssignFindReferences()
    {
        FieldInfo[] fields = GetType().GetFields(
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance
        );

        foreach (FieldInfo field in fields)
        {
            if (field.GetCustomAttribute<FindReferenceAttribute>() != null)
            {
                if (typeof(Component).IsAssignableFrom(field.FieldType))
                {
                    // Explicitly cast to Component
                    Component found = FindAnyObjectByType(field.FieldType) as Component;

                    if (found != null)
                    {
                        field.SetValue(this, found);
                    }
                }
            }
        }
    }
}