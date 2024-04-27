using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldValueValidator : MonoBehaviour
{
    [SerializeField] private int defaultValue;
    private TMP_InputField inputField;

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();

        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    void OnInputFieldValueChanged(string value)
    {
        if (value == null || value.Trim() == "" || int.Parse(value) <= 0) {
            inputField.text = defaultValue.ToString();
        }
    }
}
