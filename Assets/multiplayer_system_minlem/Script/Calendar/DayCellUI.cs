using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DayCellUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI dayNumberText;
    public GameObject dot;

    [HideInInspector] public int dayNumber;
    [HideInInspector] public System.DateTime date;

    private Button button;
    private CalendarManager_New calendarManager;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void Setup(int day, System.DateTime dateRef, CalendarManager_New manager)
    {
        dayNumber = day;
        date = dateRef;
        calendarManager = manager;

        if (dayNumberText != null)
            dayNumberText.text = day.ToString();

        // default: sembunyikan dot
        if (dot != null)
            dot.SetActive(false);
    }

    public void ShowDot(bool show)
    {
        dot.SetActive(show);
        Debug.Log($"Dot for {dayNumber} set to {show}");
    }


    private void OnClick()
    {
        if (calendarManager != null)
            calendarManager.OnDayClicked(this);
        else
            Debug.LogWarning("CalendarManager belum diassign ke DayCellUI!");
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnClick);
    }
}
