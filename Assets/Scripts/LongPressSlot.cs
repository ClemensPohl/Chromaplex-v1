using UnityEngine;
using UnityEngine.EventSystems;

public class SlotHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public int slotIndex;

    private ScanManager scanManager;
    private float pointerDownTime;
    private bool longPressTriggered;

    private const float holdDuration = 0.35f; // shorter feels better

    private void Start()
    {
        scanManager = FindAnyObjectByType<ScanManager>();
    }

    private void Update()
    {
        if (pointerDownTime > 0 && !longPressTriggered)
        {
            if (Time.time - pointerDownTime >= holdDuration)
            {
                longPressTriggered = true;
                scanManager.OnSlotLongPressed(slotIndex);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDownTime = Time.time;
        longPressTriggered = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDownTime = 0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!longPressTriggered)
        {
            scanManager.OnSlotTapped(slotIndex);
        }
    }
}
