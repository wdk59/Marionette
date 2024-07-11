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

public class PoseDetector : MonoBehaviour
{
    public bool debugMode = true;

    public float threshold = 0.05f;      // ���� ���絵 �Ѱ���
    public OVRSkeleton skeleton;
    public List<Pose> poses;
    private List<OVRBone> fingerBones;
    private Pose previousPose;
    [SerializeField] private Pose currentPose;

    [SerializeField] bool isPalmUp = false;
    [SerializeField] private GameObject handVisual; 

    bool isChanging;
    private float timer = 0f;

    public TMPro.TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousPose = new Pose();
        currentPose = new Pose();

        isChanging = false;
    }

    // Update is called once per frame
    void Update()
    {
        // print name
        SetPoseState(currentPose.name);
        Debug.Log("currentPose: " + currentPose.name);

        if (handVisual.transform.rotation.x >= -0.45 && handVisual.transform.rotation.x <= 0.45)
        {
            isPalmUp = true;
            Debug.Log("up: " + handVisual.transform.rotation.x);
        } else
        {
            isPalmUp = false;
            Debug.Log("down: " + handVisual.transform.rotation.x);
        }

        // input
        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            SavePose();
        }

        if (debugMode && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Ready");
        }

        currentPose = Recognize();

        // previous: Has Pose, current: NULL
        if (!previousPose.Equals(new Pose()) && currentPose.Equals(new Pose()))
        {
            timer = Time.realtimeSinceStartup;
            isChanging = true;
        }

        bool hasRecognized = !currentPose.Equals(new Pose());   // true: currentPose is not a blank Pose structor
        if (isChanging && hasRecognized)
        {
            isChanging = false;
        }

        if (hasRecognized && !currentPose.Equals(previousPose)) // Check if new pose
        {
            // New pose
            //Debug.Log("New Pose Found: " + currentPose.name);
            previousPose = currentPose;
            //currentPose.onRecongnized.Invoke();
        }

    }

    public string currentPoseName()
    {
        return currentPose.name;
    }

    void SavePose()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);

        Pose newPose = new Pose();
        newPose.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();

        foreach (var bone in fingerBones)
        {
            // finger position relative to root
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        newPose.fingerDatas = data;
        poses.Add(newPose);
    }
    private void SetPoseState(string poseName)
    {
        text.text = "Pose Name:" + poseName;
    }

    Pose Recognize()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        Pose currentPose = new Pose();
        float currentMin = Mathf.Infinity;

        foreach (var pose in poses)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, pose.fingerDatas[i]);    // ���� �� �Ÿ��� ���� ���絵 ����
                //Debug.Log("i: " + i + " / distance: " + distance);
                if (distance > threshold)
                {
                    isDiscarded = true;
                    //Debug.Log("discarded");
                    break;
                }
                sumDistance += distance;
            }
            //Debug.Log("sumDistance: " + sumDistance);
            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentPose = pose;
            }
        }

        return currentPose;
    }
}