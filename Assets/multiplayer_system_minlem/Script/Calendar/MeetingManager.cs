using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

[Serializable]
public class MeetingData
{
    public string title;
    public string date;
    public string startTime;
    public string endTime;
    public string creator;
    public string description;
    public string link;
}

public class MeetingManager : MonoBehaviour
{
    [Header("Popup References")]
    public GameObject popupNewMeeting;
    public GameObject popupReviewMeeting;

    [Header("Input Fields (New Meeting)")]
    public TMP_InputField titleInput;
    public TMP_InputField dateInput;
    public TMP_InputField startTimeInput;
    public TMP_InputField endTimeInput;
    public TMP_InputField creatorInput;
    public TMP_InputField descriptionInput;

    [Header("Buttons (New Meeting)")]
    public Button saveButton;
    public Button cancelButton;
    public Button addMeetingButton;

    [Header("Review Meeting UI")]
    public TMP_Text titleText;
    public TMP_Text timeText;
    public TMP_Text creatorText;
    public TMP_Text descriptionText;

    [Header("Buttons (Review)")]
    public Button joinButton;
    public Button editButton;
    public Button deleteButton;
    public Button closeButton;

    private List<MeetingData> meetings = new List<MeetingData>();

    // daftar tanggal yang punya meeting (dot)
    public List<DateTime> meetingDates = new List<DateTime>();

    [Header("References")]
    public CalendarManager_New calendarManager;

    private MeetingData currentMeeting;
    private bool isEditing = false;

    void Start()
    {
        popupNewMeeting.SetActive(false);
        popupReviewMeeting.SetActive(false);

        // hubungkan tombol
        if (addMeetingButton != null)
            addMeetingButton.onClick.AddListener(OpenNewMeetingPopup);
        if (saveButton != null)
            saveButton.onClick.AddListener(SaveMeeting);
        if (cancelButton != null)
            cancelButton.onClick.AddListener(CloseNewMeetingPopup);
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseReviewPopup);

        if (joinButton != null)
            joinButton.onClick.AddListener(OnJoinClicked);
        if (editButton != null)
            editButton.onClick.AddListener(OnEditClicked);
        if (deleteButton != null)
            deleteButton.onClick.AddListener(OnDeleteClicked);
    }

    // dibuka saat klik tombol "+ Add Meeting" (tanggal sekarang)
    public void OpenNewMeetingPopup()
    {
        isEditing = false;
        ClearInputs();
        popupNewMeeting.SetActive(true);
    }

    // dipanggil dari CalendarManager_New saat klik tanggal
    public void OpenNewMeeting(DateTime clickedDate)
    {
        // cek apakah sudah ada meeting di tanggal ini
        MeetingData existingMeeting = meetings.Find(m =>
            DateTime.TryParse(m.date, out DateTime d) && d.Date == clickedDate.Date);

        if (existingMeeting != null)
        {
            // kalau sudah ada meeting di tanggal ini → tampilkan review
            currentMeeting = existingMeeting;
            ShowMeetingReview(existingMeeting);
        }
        else
        {
            // kalau belum ada → buka form Add Meeting
            isEditing = false;
            ClearInputs();
            dateInput.text = clickedDate.ToString("dd/MM/yyyy");
            popupNewMeeting.SetActive(true);
        }
    }


    // simpan meeting baru / hasil edit
    void SaveMeeting()
    {
        if (string.IsNullOrWhiteSpace(titleInput.text))
        {
            Debug.LogWarning("Meeting title tidak boleh kosong!");
            return;
        }

        if (isEditing && currentMeeting != null)
        {
            // update data lama
            currentMeeting.title = titleInput.text;
            currentMeeting.date = dateInput.text;
            currentMeeting.startTime = startTimeInput.text;
            currentMeeting.endTime = endTimeInput.text;
            currentMeeting.creator = creatorInput.text;
            currentMeeting.description = descriptionInput.text;
        }
        else
        {
            // buat data baru
            MeetingData newMeeting = new MeetingData()
            {
                title = titleInput.text,
                date = dateInput.text,
                startTime = startTimeInput.text,
                endTime = endTimeInput.text,
                creator = creatorInput.text,
                description = descriptionInput.text,
                link = "https://example.com/meeting"
            };

            meetings.Add(newMeeting);
            currentMeeting = newMeeting;

            // dot simpan tanggalnya
            DateTime parsedDate;
            if (DateTime.TryParseExact(dateInput.text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                if (!meetingDates.Exists(d => d.Date == parsedDate.Date))
                    meetingDates.Add(parsedDate);
            }
            else
            {
                Debug.LogWarning($"Gagal parse date dari input: {dateInput.text}");
            }
        }

        popupNewMeeting.SetActive(false);

        // dot update tampilan kalender setelah meeting disimpan
        if (calendarManager != null)
            calendarManager.RefreshDots();

        foreach (var d in meetingDates)
        {
            Debug.Log($"[MeetingManager] Stored date: {d.ToString("dd/MM/yyyy")}");
        }

    }

    // dipanggil saat klik tanggal yang ada meetingnya
    public void OpenReviewMeeting(DateTime clickedDate)
    {
        // cari meeting yang sesuai tanggal
        MeetingData existingMeeting = meetings.Find(m =>
            DateTime.TryParseExact(m.date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d)
            && d.Date == clickedDate.Date);

        if (existingMeeting != null)
        {
            currentMeeting = existingMeeting;
            ShowMeetingReview(existingMeeting);
            Debug.Log($"Buka review meeting untuk tanggal {clickedDate:dd/MM/yyyy}");
        }
        else
        {
            Debug.Log($"Tidak ditemukan meeting di tanggal {clickedDate:dd/MM/yyyy}");
        }
    }

    // tampilkan detail meeting
    void ShowMeetingReview(MeetingData meeting)
    {
        if (meeting == null) return;

        titleText.text = meeting.title;
        timeText.text = $"{meeting.date} | {meeting.startTime} - {meeting.endTime}";
        creatorText.text = $"By: {meeting.creator}";
        descriptionText.text = meeting.description;

        popupReviewMeeting.SetActive(true);
    }

    void OnJoinClicked()
    {
        if (currentMeeting != null)
            Application.OpenURL(currentMeeting.link);
    }

    void OnEditClicked()
    {
        if (currentMeeting == null) return;

        isEditing = true;
        popupReviewMeeting.SetActive(false);
        popupNewMeeting.SetActive(true);

        titleInput.text = currentMeeting.title;
        dateInput.text = currentMeeting.date;
        startTimeInput.text = currentMeeting.startTime;
        endTimeInput.text = currentMeeting.endTime;
        creatorInput.text = currentMeeting.creator;
        descriptionInput.text = currentMeeting.description;
    }

    void OnDeleteClicked()
    {
        if (currentMeeting != null)
        {
            // hapus tanggal dari daftar dot
            if (DateTime.TryParseExact(currentMeeting.date, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime parsed))
            {
                meetingDates.RemoveAll(d => d.Date == parsed.Date);
            }

            // hapus data meeting-nya dari list utama
            meetings.Remove(currentMeeting);

            // hapus referensi agar tidak ada meeting aktif
            currentMeeting = null;

            // tutup popup
            popupReviewMeeting.SetActive(false);

            // refresh kalender supaya dot hilang
            if (calendarManager != null)
            {
                calendarManager.RefreshDots();
            }

            popupNewMeeting.SetActive(false);

            Debug.Log("Meeting berhasil dihapus dan kalender di-refresh.");
        }
        else
        {
            Debug.LogWarning("Tidak ada meeting yang sedang dipilih untuk dihapus.");
        }
    }


    void CloseNewMeetingPopup()
    {
        popupNewMeeting.SetActive(false);
    }

    void CloseReviewPopup()
    {
        popupReviewMeeting.SetActive(false);
    }

    void ClearInputs()
    {
        titleInput.text = "";
        dateInput.text = DateTime.Now.ToString("dd/MM/yyyy");
        startTimeInput.text = "";
        endTimeInput.text = "";
        creatorInput.text = "";
        descriptionInput.text = "";
    }
}
