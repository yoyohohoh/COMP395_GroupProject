using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GuideController : MonoBehaviour
{
    public GameObject guideArrowPrefeb;
    public TextMeshPro dialogue;
    public TextMeshPro guide;
    public GameObject gameButton;
    DataKeeper dataKeeper;

    public List<Vector3> listOfPositions;
    public List<int> listOfRotations;
    private void Start()
    {
        dataKeeper = DataKeeper.Instance;
        if (!dataKeeper.taskList.Contains("LongBlack"))
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
            guide.text = "You are Ready to Work!";
            
        }
    }
    public void InstantiateGuide(int positionIndex)
    {
        dialogue = guideArrowPrefeb.GetComponentInChildren<TextMeshPro>();
        dialogue.text = "Tutorial " + (positionIndex + 1);
        Instantiate(guideArrowPrefeb, listOfPositions[positionIndex], Quaternion.Euler(0, listOfRotations[positionIndex], 0));
    }
}

