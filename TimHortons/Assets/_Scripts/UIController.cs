using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text totalCustomerNo;
    [SerializeField] private Text times;
    [SerializeField] private TextMeshProUGUI totalCustomer;
    [SerializeField] private TextMeshProUGUI arrivalTime;
    [SerializeField] private TextMeshProUGUI clockTime;
    [SerializeField] private TextMeshProUGUI serviceTime;
    [SerializeField] private TextMeshProUGUI waitingTime;

    [SerializeField] private ArrivalProcess arrivalProcess;
    [SerializeField] private Waypoints waypoints;

    public int hours;
    public int minutes;
    public int seconds;

    public bool isShowing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        arrivalProcess = GameObject.Find("CustomerArrival").GetComponent<ArrivalProcess>();
        waypoints = GameObject.Find("Waypoints").GetComponent<Waypoints>();


    }

    // Update is called once per frame
    void Update()
    {
        totalCustomer.text = "Today's Customer: " + arrivalProcess.customerCount.ToString();
        arrivalTime.text = "Arrival Time: " + arrivalProcess.interArrivalTime.ToString("F2") + " mins";

        int totalSeconds = Mathf.FloorToInt(arrivalProcess.startTime * 360);
        hours = totalSeconds / 3600;
        minutes = (totalSeconds % 3600) / 60;
        seconds = totalSeconds % 60;

        clockTime.text = string.Format("Clock Time: {0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

        serviceTime.text = "Service Time: " + waypoints.serviceTime.ToString("F2") + " mins";
        waitingTime.text = "Waiting Time: " + waypoints.waitTime.ToString("F2") + " mins";

        totalCustomerNo.text = arrivalProcess.customerCount.ToString();
        times.text = arrivalTime.text + "\n\n" + waitingTime.text + "\n\n" + serviceTime.text + "\n";

        totalCustomer.enabled = isShowing;
        arrivalTime.enabled = isShowing;
        clockTime.enabled = isShowing;
        serviceTime.enabled = isShowing;
        waitingTime.enabled = isShowing;

    }

    public void ShowingData()
    { 
        if (isShowing) { isShowing = false; }
        else { isShowing = true; }
    }
}
