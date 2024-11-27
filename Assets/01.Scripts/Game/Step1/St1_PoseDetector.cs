using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class St1_PoseDetector : MonoBehaviour
{
    [SerializeField] private List<ActiveStateSelector> poses;
    public string nowPoseName = "Idle";
    public TMPro.TMP_Text PoseName_L;
    public TMPro.TMP_Text PoseName_R;

    private ActiveStateSelector selectedPose;

    // Start is called before the first frame update
    void Update()
    {
        Debug.Log("currentPose: " + nowPoseName);
        foreach (var item in poses)
        {
            if (item.gameObject.name == "Jump" || item.gameObject.name == "Dash")
            {
                item.WhenSelected += () => SetPoseState(item.gameObject.name, true);
                item.WhenUnselected += () => SetPoseState("Idle", true);
            }
            else
            {
                item.WhenSelected += () => SetPoseState(item.gameObject.name, false);
                item.WhenUnselected += () => SetPoseState("Idle", false);
            }
        }
    }

    private void SetPoseState(string poseName, bool isRight)
    {
        //Debug.Log("D_poseName: " + poseName);
        nowPoseName = poseName;
        if (isRight)
        {
            PoseName_R.text = "Right Pose Name: " + poseName;
        } else
        {
            PoseName_L.text = "Left Pose Name: " + poseName;
        }
    }
}
