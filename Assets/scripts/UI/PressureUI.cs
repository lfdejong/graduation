using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PressureUI : MonoBehaviour
{
    [SerializeField] private Image neutralUI;
    [SerializeField] private Image leftUI;
    [SerializeField] private Image rightUI;
    [SerializeField] private Image upUI;
    [SerializeField] private Image downUI;

    private Vector3 originalScale;       // Original scale for UI images
    private Vector3 enlargedScale;       // Enlarged scale for feedback

    private void Start()
    {
        originalScale = upUI.transform.localScale;
        enlargedScale = originalScale * 1.5f;
    }

    // Update the UI feedback based on the active sensor direction
    public void UpdateUIFeedback(int activeSensor)
    {
        // Reset all UI elements to their original scale
        ResetUI();

        // Enlarge the appropriate UI image based on the active sensor direction
        switch (activeSensor)
        {
            case 0:
                downUI.transform.localScale = enlargedScale;
                break;
            case 1:
                upUI.transform.localScale = enlargedScale;
                break;
            case 2:
                leftUI.transform.localScale = enlargedScale;
                break;
            case 3:
                rightUI.transform.localScale = enlargedScale;
                break;
            default:
                neutralUI.transform.localScale = enlargedScale;
                break;
        }
    }

    // Reset all UI images to the original scale
    private void ResetUI()
    {
        upUI.transform.localScale = originalScale;
        downUI.transform.localScale = originalScale;
        leftUI.transform.localScale = originalScale;
        rightUI.transform.localScale = originalScale;
        neutralUI.transform.localScale = originalScale;
    }
}
