using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalCustomer;
    [SerializeField] private TextMeshProUGUI currentCustomer;
    [SerializeField] private TextMeshProUGUI arrivalTime;
    [SerializeField] private TextMeshProUGUI clockTime;
    [SerializeField] private TextMeshProUGUI serviceTime;
    [SerializeField] private TextMeshProUGUI waitingTime;

    [SerializeField] private ArrivalProcess arrivalProcess;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        arrivalProcess = GameObject.Find("CustomerArrival").GetComponent<ArrivalProcess>();
    }

    // Update is called once per frame
    void Update()
    {
        totalCustomer.text = "Total Customer: " + arrivalProcess.customerCount;
        arrivalTime.text = "Arrival Time: " + arrivalProcess.interArrivalTime.ToString("F2") + "mins";

        int totalSeconds = Mathf.FloorToInt(arrivalProcess.startTime * 360);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        clockTime.text = string.Format("Clock Time: {0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

    }
}
