using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalendarManager_New : MonoBehaviour
{
    [Header("UI References")]
    public GameObject monthYearText;
    public GameObject dayCellPrefab;       // prefab untuk tiap tanggal
    public Transform gridParent;           // grid parent (tempat spawn tanggal)
    public Button prevMonthButton;
    public Button nextMonthButton;

    [Header("Popup & Reminder (Optional)")]
    public GameObject popupNewMeeting;
    public TextMeshProUGUI popupNewMeetingDateField;
    public GameObject popupReviewMeeting;
    public GameObject reminderManager;

    private DateTime currentDate;
    private List<GameObject> spawnedDays = new List<GameObject>();

    public MeetingManager meetingManager;

    void Start()
    {
        currentDate = DateTime.Now;
        GenerateCalendar(currentDate);

        // pasang event tombol
        if (prevMonthButton != null)
            prevMonthButton.onClick.AddListener(() => ChangeMonth(-1));
        if (nextMonthButton != null)
            nextMonthButton.onClick.AddListener(() => ChangeMonth(1));
    }

    void GenerateCalendar(DateTime date)
    {
        // hapus tanggal lama
        foreach (var day in spawnedDays)
            Destroy(day);
        spawnedDays.Clear();

        // tulis bulan & tahun ke teks
        if (monthYearText != null)
        {
            var tmp = monthYearText.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = date.ToString("MMMM yyyy");
            else
                Debug.LogWarning("TextMeshProUGUI belum ke-attach di MonthYearText!");
        }

        // hitung awal bulan & total hari
        DateTime firstDay = new DateTime(date.Year, date.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

        // dapatkan offset (minggu di awal)
        int startDay = (int)firstDay.DayOfWeek;

        // kosongin beberapa cell biar tanggal pas di posisi
        for (int i = 0; i < startDay; i++)
        {
            GameObject emptyCell = Instantiate(dayCellPrefab, gridParent);
            var tmp = emptyCell.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = "";
            spawnedDays.Add(emptyCell);
        }

        // isi tanggal
        for (int day = 1; day <= daysInMonth; day++)
        {
            GameObject newCell = Instantiate(dayCellPrefab, gridParent);

            // setup tampilan cell via komponen DayCellUI
            var cellUI = newCell.GetComponent<DayCellUI>();
            if (cellUI != null)
            {
                cellUI.Setup(day, new DateTime(date.Year, date.Month, day), this);
            }
            else
            {
                var tmp = newCell.GetComponentInChildren<TextMeshProUGUI>();
                if (tmp != null)
                    tmp.text = day.ToString();
            }

            spawnedDays.Add(newCell);
        }
    }

    void ChangeMonth(int delta)
    {
        currentDate = currentDate.AddMonths(delta);
        GenerateCalendar(currentDate);
    }

    // dipanggil saat klik tanggal
    public void OnDayClicked(DayCellUI cell)
    {
        Debug.Log($"Kamu klik tanggal: {cell.dayNumber} ({cell.date:dd MMMM yyyy})");

        if (meetingManager == null)
        {
            Debug.LogWarning("MeetingManager belum di-assign ke CalendarManager_New!");
            return;
        }

        // cek apakah tanggal ini ada meeting
        bool hasMeeting = meetingManager.meetingDates.Exists(d => d.Date == cell.date.Date);

        if (hasMeeting)
        {
            // kalau ADA meeting → buka popup review
            meetingManager.OpenReviewMeeting(cell.date);
            Debug.Log($"Tanggal {cell.date:dd/MM/yyyy} ada meeting → buka review popup");
        }
        else
        {
            // kalau BELUM ada meeting → buka popup tambah meeting
            meetingManager.OpenNewMeeting(cell.date);
            Debug.Log($"Tanggal {cell.date:dd/MM/yyyy} belum ada meeting → buka add meeting popup");
        }
    }


    public void OnDayClicked(int day)
    {
        DateTime clickedDate = new DateTime(currentDate.Year, currentDate.Month, day);
        meetingManager.OpenNewMeeting(clickedDate);
    }

    public void RefreshDots()
    {
        Debug.Log("Refreshing dots...");

        if (meetingManager == null) return;

        foreach (var dayObj in spawnedDays)
        {
            var cell = dayObj.GetComponent<DayCellUI>();
            if (cell == null) continue;

            // cek apakah ada tanggal meeting yang sama harinya
            bool hasMeeting = meetingManager.meetingDates.Exists(
                d => d.Date == cell.date.Date
            );

            cell.ShowDot(hasMeeting);
            Debug.Log($"Checking cell {cell.date:dd/MM/yyyy}");
        }
    }




}
