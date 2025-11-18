using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class Test : MonoBehaviour
{
    public bool IsVisible;

    public Button button;

    private ObserverBehaviour observer;

    private void Awake()
    {
        observer = GetComponent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += OnStatusChanged;
        ColorBlock colorblock = button.colors;
        colorblock.normalColor = Color.red;
        button.colors = colorblock;
    }

    private void OnDestroy()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnStatusChanged;
    }

    private void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        IsVisible = status.Status == Status.TRACKED;

        if (IsVisible)
        {
            ColorBlock colorblock = button.colors;
            colorblock.normalColor = Color.green;
            button.colors = colorblock;
        }
        else
        {
            ColorBlock colorblock = button.colors;
            colorblock.normalColor = Color.red;
            button.colors = colorblock;
        }
    }
}
