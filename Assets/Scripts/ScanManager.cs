using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScanManager : MonoBehaviour
{
    public FaceTarget[] targets;       
    public Image[] uiSlots;              
    public Color lastScannedColor;       
    public int lastDarknessIndex;        

    public TextMeshProUGUI debugText;               
    private bool hasScan = false;

    public Image debugImage;

    public void Scan()
    {
        foreach (var t in targets)
        {
            if (t.isVisible)
            {
                lastScannedColor = t.unityColor;
                lastDarknessIndex = t.darknessIndex;
                hasScan = true;

                debugText.text = "Scanned: " + t.colorName;
                debugImage.color = t.unityColor;
                return;
            }
        }

        debugText.text = "No target detected!";
        debugImage.color = Color.red;
        hasScan = false;
    }

    public void SelectSlot(int slotIndex)
    {
        if (!hasScan) return;

        uiSlots[slotIndex].color = lastScannedColor;

        // Remove scan so user must scan again
        hasScan = false;
        debugText.text = "Placed color into slot " + slotIndex;
    }

    private void Update()
    {
        CheckOrder();
    }

    public void CheckOrder()
    {
        for (int i = 0; i < uiSlots.Length - 1; i++)
        {
            if (uiSlots[i].color == Color.clear) return;

            int a = GetDarknessByColor(uiSlots[i].color);
            int b = GetDarknessByColor(uiSlots[i + 1].color);

            if (a > b)
            {
                debugText.text = "Wrong order!";
                return;
            }
        }

        debugText.text = "Correct order!";
    }

    private int GetDarknessByColor(Color c)
    {
        foreach (var t in targets)
        {
            if (t.unityColor == c)
                return t.darknessIndex;
        }
        return -1;
    }
}
