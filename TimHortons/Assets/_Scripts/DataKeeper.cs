using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DataKeeper : MonoBehaviour
{
    public static DataKeeper Instance;

    public int totalCustomerCount;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            GameObject.Find("CustomerCount").GetComponentInChildren<Text>().text = "Today's Customer: " + totalCustomerCount.ToString();
        }
    }
}
