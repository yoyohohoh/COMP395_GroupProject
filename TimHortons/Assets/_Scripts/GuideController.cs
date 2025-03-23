using System.Collections.Generic;
using UnityEngine;


public class GuideController : MonoBehaviour
{
    public GameObject guideArrowPrefeb;
    public GameObject gameButton;
    DataKeeper dataKeeper;

    public List<Vector3> listOfPositions;
    public List<int> listOfRotations;
    private void Start()
    {
        dataKeeper = DataKeeper.Instance;
        if(!dataKeeper.taskList.Contains("LongBlack"))
        {
            InstantiateGuide(0);
        }
        else if (!dataKeeper.taskList.Contains("CaffeLatte"))
        {
            InstantiateGuide(1);
        }
    }
    private void Update()
    {
        if (dataKeeper.isTutorialCompleted)
        {
            gameButton.SetActive(true);
        }
    }
    public void InstantiateGuide(int positionIndex)
    {
        Instantiate(guideArrowPrefeb, listOfPositions[positionIndex], Quaternion.Euler(0, listOfRotations[positionIndex], 0));
    }
}

