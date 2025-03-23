using UnityEngine;

public class GuideController : MonoBehaviour
{
    public GameObject guideArrowPrefeb;
    public GameObject gameButton;
    DataKeeper dataKeeper;
    private void Start()
    {
        dataKeeper = DataKeeper.Instance;
    }
    private void Update()
    {
        if(dataKeeper.taskList.Count >= dataKeeper.totalTask)
        {
            gameButton.SetActive(true);
        }
    }
    public void InstantiateGuide(Vector3 spawnPosition)
    {
        Instantiate(guideArrowPrefeb, spawnPosition, Quaternion.identity);
    }
}

