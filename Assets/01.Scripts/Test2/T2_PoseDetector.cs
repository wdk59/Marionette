using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Pose
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecongnized;
}

public class T2_PoseDetector : MonoBehaviour
{
    public bool debugMode = true;

    public float threshold = 0.05f;      // 포즈 유사도 한계점
    public OVRSkeleton skeleton;
    public List<Pose> poses;
    private List<OVRBone> fingerBones;
    private Pose previousPose;

    // Start is called before the first frame update
    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousPose = new Pose();
    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            SavePose();
        }

        Pose currentPose = Recognize();
        bool hasRecognized = !currentPose.Equals(new Pose());
        // Check if new pose
        if (hasRecognized && !currentPose.Equals(previousPose))
        {
            // New pose
            Debug.Log("New Pose Found: " + currentPose.name);
            previousPose = currentPose;
            currentPose.onRecongnized.Invoke();
        }
    }

    void SavePose()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);

        Pose newPose = new Pose();
        newPose.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();

        foreach(var bone in fingerBones)
        {
            Debug.Log("boneeeeeeeeeeee");
            // finger position relative to root
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        newPose.fingerDatas = data;
        poses.Add(newPose);
    }

    Pose Recognize()
    {
        Pose currentPose = new Pose();
        float currentMin = Mathf.Infinity;

        foreach(var pose in poses)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, pose.fingerDatas[i]);    // 벡터 간 거리를 통해 유사도 측정
                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }
            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentPose = pose;
            }
        }

        return currentPose;
    }
}
