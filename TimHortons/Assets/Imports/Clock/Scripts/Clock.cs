using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour
{

    //-- set start time 00:00
    public int minutes = 0;
    public int hour = 0;
    public int seconds = 0;
    public bool realTime = true;

    public GameObject pointerSeconds;
    public GameObject pointerMinutes;
    public GameObject pointerHours;
    public SimulationParameters simulationParameters;
    [SerializeField] UIController uiController;

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
        //-- set real time
        if (realTime)
        {
            hour = System.DateTime.Now.Hour;
            minutes = System.DateTime.Now.Minute;
            seconds = System.DateTime.Now.Second;
        }
        else
        {
            uiController = GameObject.Find("UIController").GetComponent<UIController>();
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
                        hour++;
                        if (hour >= 24)
                            hour = 0;
                    }
                }
            }
        }
        else
        {
            hour = 8 + uiController.hours;
            minutes = uiController.minutes;
            seconds = uiController.seconds;
        }
        //-- calculate pointer angles
        float rotationSeconds = (360.0f / 60.0f) * seconds;
        float rotationMinutes = (360.0f / 60.0f) * minutes;
        float rotationHours = ((360.0f / 12.0f) * hour) + ((360.0f / (60.0f * 12.0f)) * minutes);

        //-- draw pointers
        pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationSeconds);
        pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationMinutes);
        pointerHours.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationHours);

    }
}
