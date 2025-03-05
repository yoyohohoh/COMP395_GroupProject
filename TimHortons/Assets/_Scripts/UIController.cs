using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private InputSystem_Actions _inputs;
    [SerializeField] private Text totalCustomerNo;
    [SerializeField] private Text times;
    [SerializeField] private TextMeshProUGUI totalCustomer;
    [SerializeField] private TextMeshProUGUI arrivalTime;
    [SerializeField] private TextMeshProUGUI clockTime;
    [SerializeField] private TextMeshProUGUI serviceTime;
    [SerializeField] private TextMeshProUGUI waitingTime;
    [SerializeField] private TextMeshProUGUI restart;

    [SerializeField] private ArrivalProcess arrivalProcess;
    [SerializeField] private Waypoints waypoints;

    public int hours;
    public int minutes;
    public int seconds;

    public bool isShowing;
    public bool isShowingTag;
    public bool isPausingGame;

    private void Awake()
    {
        _inputs = new InputSystem_Actions();

        _inputs.Player.Jump.performed += context => PauseGame();
    }

    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();

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

        //find list of game object with tag "DataTag"
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("DataTag");

        foreach (GameObject obj in objectsWithTag)
        {
            obj.SetActive(isShowingTag);
        }

        Time.timeScale = isPausingGame ? 0f : 1f;
        
    }

    public void ShowingData()
    { 
        if (isShowing) { isShowing = false; }
        else { isShowing = true; }
    }

    public void ShowingTag()
    {
        if (isShowingTag) { isShowingTag = false; }
        else { isShowingTag = true; }
    }

    public void PauseGame()
    {
        if (isPausingGame) { isPausingGame = false; }
        else { isPausingGame = true; }
    }

    public void ResumeGame()
    {
        SceneManager.LoadScene(0);
    }
}
