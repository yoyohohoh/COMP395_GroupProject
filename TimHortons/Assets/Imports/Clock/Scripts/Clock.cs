using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour
{

    //-- set start time 00:00
    public int minutes = 0;
    public int hours = 0;
    public int seconds = 0;
    public bool realTime = true;

    public GameObject pointerSeconds;
    public GameObject pointerMinutes;
    public GameObject pointerHours;
    public SimulationParameters simulationParameters;
    [SerializeField] private ArrivalProcess arrivalProcess;
    [SerializeField] private OrderGenerator orderGenerator;

    //-- time speed factor
    public float clockSpeed = 1.0f;     // 1.0f = realtime, < 1.0f = slower, > 1.0f = faster

    //-- internal vars
    float msecs = 0;

    void Start()
    {
        if (simulationParameters == null)
        {
            realTime = true;
        }
        else
        {
            if (GameObject.Find("CustomerArrival"))
            {
                arrivalProcess = GameObject.Find("CustomerArrival").GetComponent<ArrivalProcess>();
            }
            if (GameObject.Find("OrderGenerator"))
            {
                orderGenerator = GameObject.Find("OrderGenerator").GetComponent<OrderGenerator>();
            }
        }

        //-- set real time
        if (realTime)
        {
            hours = System.DateTime.Now.Hour;
            minutes = System.DateTime.Now.Minute;
            seconds = System.DateTime.Now.Second;
        }
    }

    void Update()
    {
        if (realTime)
        {
            msecs += Time.deltaTime * clockSpeed;
            if (msecs >= 1.0f)
            {
                msecs -= 1.0f;
                seconds++;
                if (seconds >= 60)
                {
                    seconds = 0;
                    minutes++;
                    if (minutes > 60)
                    {
                        minutes = 0;
                        hours++;
                        if (hours >= 24)
                            hours = 0;
                    }
                }
            }
        }
        else
        {
            if (arrivalProcess != null)
            {
                int totalSeconds = Mathf.FloorToInt(arrivalProcess.startTime * (3600 / simulationParameters.TimeScale));
                AdjustSimulationTime(totalSeconds);
            }
            else if (orderGenerator != null)
            {
                int totalSeconds = Mathf.FloorToInt(orderGenerator.startTime * (3600 / simulationParameters.TimeScale));
                AdjustSimulationTime(totalSeconds);
            }
        }
        //-- calculate pointer angles
        float rotationSeconds = (360.0f / 60.0f) * seconds;
        float rotationMinutes = (360.0f / 60.0f) * minutes;
        float rotationHours = ((360.0f / 12.0f) * hours) + ((360.0f / (60.0f * 12.0f)) * minutes);

        //-- draw pointers
        pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationSeconds);
        pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationMinutes);
        pointerHours.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationHours);

    }

    void AdjustSimulationTime(int totalSeconds)
    {
        hours = 8 + totalSeconds / 3600;
        minutes = (totalSeconds % 3600) / 60;
        seconds = totalSeconds % 60;
    }
}
