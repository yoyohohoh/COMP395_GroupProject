using UnityEngine;

public class GuideController : MonoBehaviour
{
    public GameObject guideArrowPrefeb;

    public void InstantiateGuide(Vector3 spawnPosition)
    {
        Instantiate(guideArrowPrefeb, spawnPosition, Quaternion.identity);
    }
}

