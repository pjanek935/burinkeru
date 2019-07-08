using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashTrail : MonoBehaviour
{
    [SerializeField] GameObject singleTrail;
    [SerializeField] int length = 20;
    [SerializeField] float segmentWidth = 0.2f;
    [SerializeField] float maxTime = 0.5f;
    [SerializeField] float minTime = 0.1f;

    public void Start()
    {
        createSegments();
    }

    void createSegments ()
    {
        for (int i = 0; i < length; i ++)
        {
            GameObject newGameObject = Instantiate(singleTrail);
            newGameObject.SetActive(true);
            newGameObject.transform.SetParent(this.transform, false);
            TrailRenderer trailRenderer = newGameObject.GetComponent<TrailRenderer>();
            trailRenderer.startWidth = segmentWidth;
            trailRenderer.endWidth = segmentWidth;
            trailRenderer.time = maxTime - i * ((maxTime - minTime) / length);
            newGameObject.transform.localPosition = new Vector3(0, -segmentWidth * i / this.transform.lossyScale.y, 0);

        }
    }
}
