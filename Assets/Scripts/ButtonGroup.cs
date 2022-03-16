using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonGroup : MonoBehaviour
{
    [SerializeField]
    public Button[] buttons;

    private string value = null;
    public string Value { get { return value; } }

    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    public void OnButtonSelect(Button button)
    {
        value = button.name;
        Debug.Log($"Button group value is { value }");
        SendMessageUpwards("SetType", value);
    }
}
