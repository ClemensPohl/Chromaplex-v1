using UnityEngine;
using Vuforia;

public class FaceTarget : MonoBehaviour
{
    public string colorName;      
    public Color unityColor;      
    public int darknessIndex;     // 0 = light, 1 = medium, 2 = dark
    public bool isVisible;

    private ObserverBehaviour observer;

    private void Awake()
    {
        observer = GetComponent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += OnStatusChanged;
    }

    private void OnDestroy()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnStatusChanged;
    }

    private void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        isVisible = status.Status == Status.TRACKED;
    }
}
