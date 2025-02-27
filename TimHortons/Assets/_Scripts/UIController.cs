using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text totalCustomer;
    [SerializeField] private Text times;
    [SerializeField] private TextMeshProUGUI currentCustomer;
    [SerializeField] private TextMeshProUGUI arrivalTime;
    [SerializeField] private TextMeshProUGUI clockTime;
    [SerializeField] private TextMeshProUGUI serviceTime;
    [SerializeField] private TextMeshProUGUI waitingTime;

    [SerializeField] private ArrivalProcess arrivalProcess;
    [SerializeField] private Waypoints waypoints;

    public int hours;
    public int minutes;
    public int seconds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        arrivalProcess = GameObject.Find("CustomerArrival").GetComponent<ArrivalProcess>();
        waypoints = GameObject.Find("Waypoints").GetComponent<Waypoints>();
    }

    // Update is called once per frame
    void Update()
    {
        arrivalTime.text = "Arrival Time: " + arrivalProcess.interArrivalTime.ToString("F2") + " mins";

        int totalSeconds = Mathf.FloorToInt(arrivalProcess.startTime * 360);
        hours = totalSeconds / 3600;
        minutes = (totalSeconds % 3600) / 60;
        seconds = totalSeconds % 60;

        clockTime.text = string.Format("Clock Time: {0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

        serviceTime.text = "Service Time: " + waypoints.serviceTime.ToString("F2") + " mins";
        waitingTime.text = "Waiting Time: " + waypoints.waitTime.ToString("F2") + " mins";

        totalCustomer.text = arrivalProcess.customerCount.ToString();
        times.text = arrivalTime.text + "\n\n" + waitingTime.text + "\n\n" + serviceTime.text + "\n";

    }
}
