using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputValidator : MonoBehaviour
{
    [SerializeField] Button startButton;
    /// <summary>
    /// Check if the name was inputted (and not removed)
    /// </summary>
    public void CheckNamePresence()
    {
        string inputFieldContnent = gameObject.GetComponent<TMP_InputField>().text;
        MainManager.Instance.playerName = inputFieldContnent;//save the name between scenes
        if (inputFieldContnent != string.Empty)
        {
            startButton.interactable = true;
        }
        else if (inputFieldContnent == string.Empty)
        {
            startButton.interactable = false;
        }
    }
}
