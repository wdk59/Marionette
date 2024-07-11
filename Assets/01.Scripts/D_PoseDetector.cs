using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_PoseDetector : MonoBehaviour
{
    [SerializeField] private List<ActiveStateSelector> poses;
    public string nowPoseName = "Idle";
    public TMPro.TextMeshPro text;

    private ActiveStateSelector selectedPose;

    // Start is called before the first frame update
    void Update()
    {
        Debug.Log("D_currentPose: " + nowPoseName);
        foreach (var item in poses)
        {
            item.WhenSelected += () => SetPoseState(item.gameObject.name);
            item.WhenUnselected += () => SetPoseState("Idle");
        }
    }

    private void SetPoseState(string poseName)
    {
        //Debug.Log("D_poseName: " + poseName);
        nowPoseName = poseName;
        text.text = "D: " + poseName;
    }
}
