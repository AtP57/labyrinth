using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown algorithmDropdown;
    [SerializeField] private TMP_Dropdown sizeDropdown;
    [SerializeField] private GameObject customSizeSelector;
    [SerializeField] private TMP_InputField customWidthInputField;
    [SerializeField] private TMP_InputField customHeightInputField;

    private void Start()
    {
        sizeDropdown.onValueChanged.AddListener(OnSizeDropdownValueChanged);
    }

    public void Play()
    {
        selectAlgorithm(algorithmDropdown.value);
        selectSize(sizeDropdown.value);
        SceneManager.LoadScene("GameScene");
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void selectAlgorithm(int algorithm) 
    { 
        GlobalVariables.selectedAlgorithm = algorithm+1;
    }

    public void selectSize(int size)
    {
        switch (size)
        {
            case 0:
                selectSizeSmall();
                break;
            case 1:
                selectSizeMedium();
                break; 
            case 2:
                selectSizeLarge();
                break;
            case 3:
                selectCustomSize();
                break;
        }
    }

    public void selectSizeSmall()
    {
        GlobalVariables.selectedWidthNodes = 5;
        GlobalVariables.selectedHeightNodes = 3;
    }
    public void selectSizeMedium()
    {
        GlobalVariables.selectedWidthNodes = 7;
        GlobalVariables.selectedHeightNodes = 5;
    }
    public void selectSizeLarge()
    {
        GlobalVariables.selectedWidthNodes = 10;
        GlobalVariables.selectedHeightNodes = 7;
    }


    public void selectCustomSize()
    {
        GlobalVariables.selectedWidthNodes = int.Parse(customWidthInputField.text);
        GlobalVariables.selectedHeightNodes = int.Parse(customHeightInputField.text);
    }

    public void OnSizeDropdownValueChanged(int value)
    {
        switch (value)
        {
            case 3:
                customSizeSelector.SetActive(true);
                break;
            default: 
                customSizeSelector.SetActive(false); 
                break;
        }
    }
}
