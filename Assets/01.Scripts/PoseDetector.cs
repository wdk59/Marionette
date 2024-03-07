using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseDetector : MonoBehaviour
{
    public List<ActiveStateSelector> poses;
    public string nowPoseName;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var item in poses)
        {
            item.WhenSelected += () => SetPoseState(item.gameObject.name);
            item.WhenUnselected += () => SetPoseState("");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetPoseState(string poseName)
    {
        nowPoseName = poseName;
    }
}
