using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScanManager : MonoBehaviour
{
    [Header("References")]
    public Transform ColorBar;
    public Transform Targets;

    [Header("Debug UI")]
    public TextMeshProUGUI debugText;
    public Image debugImage;

    public Color lastScannedColor;
    public int lastDarknessIndex;

    private Image[] colorbarSlots;
    private FaceTarget[] targets;
    private bool[] slotFilled;

    public static Action OnPlayerWin;

    private bool carryingColor = false;
    private Color carriedColor;
    private int carriedFromIndex = -1;

    public void OnSlotLongPressed(int index)
    {
        if (!slotFilled[index])
        {
            debugText.text = "Slot empty, nothing to pick up!";
            return;
        }

        carryingColor = true;
        carriedColor = colorbarSlots[index].color;
        carriedFromIndex = index;

        // Make original slot empty
        colorbarSlots[index].color = Color.white;
        slotFilled[index] = false;

        debugText.text = $"Picked up color from slot {index}";
    }


    private void Awake()
    {
        colorbarSlots = ColorBar.GetComponentsInChildren<Image>();
        targets = Targets.GetComponentsInChildren<FaceTarget>();

        if (colorbarSlots == null) Debug.Log("Uislots are null");
        if (targets == null) Debug.Log("Targets are null");

        slotFilled = new bool[colorbarSlots.Length];
        for (int i = 0; i < slotFilled.Length; i++)
            slotFilled[i] = false;

        //for (int i = 0; i < colorbarSlots.Length; i++)
        //{
        //    int slotIndex = i;
        //    var button = colorbarSlots[i].GetComponent<Button>();
        //    button.onClick.AddListener(() => OnSlotClicked(slotIndex));
        //}

        lastScannedColor = Color.white;
    }

    private void OnSlotClicked(int index)
    {
        // If carrying a color → drop it here
        if (carryingColor)
        {
            colorbarSlots[index].color = carriedColor;
            slotFilled[index] = true;

            carryingColor = false;
            carriedFromIndex = -1;

            debugText.text = $"Placed color in slot {index}";
            CheckOrder();
            return;
        }

        // Normal place mode (using scanned color)
        if (lastScannedColor != Color.white)
        {
            colorbarSlots[index].color = lastScannedColor;
            slotFilled[index] = true;

            debugText.text = "Placed scanned color";
            CheckOrder();
        }
    }

    public void OnSlotTapped(int index)
    {
        // If carrying → drop
        if (carryingColor)
        {
            colorbarSlots[index].color = carriedColor;
            slotFilled[index] = true;

            carryingColor = false;
            carriedFromIndex = -1;

            debugText.text = $"Dropped color into slot {index}";
            CheckOrder();
            return;
        }

        // No carrying → place scanned color
        if (lastScannedColor != Color.white)
        {
            colorbarSlots[index].color = lastScannedColor;
            slotFilled[index] = true;
            debugText.text = $"Placed scanned color";
            CheckOrder();
            return;
        }

        debugText.text = "Tap does nothing (no scanned color, not carrying any)";
    }



    public void Scan()
    {
        foreach (var t in targets)
        {
            if (t.isVisible)
            {
                lastScannedColor = t.unityColor;
                lastDarknessIndex = t.darknessIndex;

                debugText.text = "Scanned: " + t.colorName;
                debugImage.color = t.unityColor;
                return;
            }
        }

        debugText.text = "No target detected!";
        debugImage.color = Color.red;
    }

    public void CheckOrder()
    {
        for (int i = 0; i < slotFilled.Length; i++)
        {
            if (!slotFilled[i])
            {
                debugText.text = "Fill all slots first!";
                return;
            }
        }

        for (int i = 0; i < colorbarSlots.Length - 1; i++)
        {
            int a = GetDarknessByColor(colorbarSlots[i].color);
            int b = GetDarknessByColor(colorbarSlots[i + 1].color);

            if (a > b)
            {
                debugText.text = "Wrong order!";
                return;
            }
        }

        debugText.text = "Correct order!";
        OnPlayerWin?.Invoke();
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

    public void ResetColorbar()
    {
        for (int i = 0; i < colorbarSlots.Length; i++)
        {
            colorbarSlots[i].color = Color.white; 
            slotFilled[i] = false;
        }

        debugText.text = "Puzzle reset.";
        debugImage.color = Color.white;

        lastScannedColor = Color.white;
        lastDarknessIndex = -1;
    }
}
