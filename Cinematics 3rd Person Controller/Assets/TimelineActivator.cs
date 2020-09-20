using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider))]

public class TimelineActivator : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public string playerTAG;
    public Transform interectionLocation;
    public bool autoActivate = false;

    public bool interact { get; set; }

    [Header("Activation Zone Events")]
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;

    [Header("Timeline Events")]
    public UnityEvent OnTimeLineStart;
    public UnityEvent OnTimeLineEnd;

    private bool isPlaying;
    private bool playerInside;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInside && !isPlaying)
        {
            if(interact || autoActivate)
            {
                PlayTimeLine();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTAG))
        {
            playerInside = true;
            playerTransform = other.transform;
            OnPlayerEnter.Invoke();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTAG))
        {
            playerInside = false;
            playerTransform = null;
            OnPlayerExit.Invoke();
        }
    }
    private void PlayTimeLine()
    {
        if (playerTransform && interectionLocation)
            playerTransform.SetPositionAndRotation(interectionLocation.position, interectionLocation.rotation);

        if (autoActivate)
            playerInside = false;

        if (playableDirector)
            playableDirector.Play();

        isPlaying = true;
        interact = false;

        StartCoroutine(waitForTimeLineToEnd());
    }
    private IEnumerator waitForTimeLineToEnd()
    {
        OnTimeLineStart.Invoke();

        float timeLineDuration = (float)playableDirector.duration;

        while(timeLineDuration > 0)
        {
            timeLineDuration -= Time.deltaTime;
            yield return null;
        }

        isPlaying = false;

        OnTimeLineEnd.Invoke();
    }
}
