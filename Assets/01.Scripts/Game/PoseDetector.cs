using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Pose
{
    public string name;
    public List<Vector3> fingerDatas;
    public bool isPalmUp;

    public UnityEvent onRecongnized;
}

public class PoseDetector : MonoBehaviour
{
    public bool debugMode = true;

    public float threshold = 0.05f;      // ���� ���絵 �Ѱ���
    public OVRSkeleton skeleton_L;
    public OVRSkeleton skeleton_R;

    public List<Pose> poses_L;
    public List<Pose> poses_R;

    private List<OVRBone> fingerBones;

    //private Pose previousPose_L;
    //private Pose previousPose_R;

    [SerializeField] private Pose currentPose_L;
    [SerializeField] private Pose currentPose_R;

    [SerializeField] bool isPalmUp_L = false;
    [SerializeField] bool isPalmUp_R = false;
    
    [SerializeField] public GameObject handVisual_L;
    [SerializeField] public GameObject handVisual_R;

    //bool isChanging_L;
    //bool isChanging_R;

    public TMPro.TMP_Text PoseName_L;
    public TMPro.TMP_Text PoseName_R;

    // Start is called before the first frame update
    void Start()
    {
        //fingerBones_L = new List<OVRBone>(skeleton_L.Bones);
        fingerBones = null;

        //previousPose_L = new Pose();
        //previousPose_R = new Pose();
        currentPose_L = new Pose();
        currentPose_R = new Pose();

        //isChanging_L = false;
        //isChanging_R = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Print Pose Name
        SetPoseState(currentPose_L.name, currentPose_R.name);
        Debug.Log("Left CurrentPose: " + currentPose_L.name);
        Debug.Log("Right CurrentPose: " + currentPose_R.name);

        // Palm Face
        if (handVisual_L.transform.rotation.x >= -0.45 && handVisual_L.transform.rotation.x <= 0.45)
        {
            isPalmUp_L = true;
            Debug.Log("up: " + handVisual_L.transform.rotation.x);
        } else
        {
            isPalmUp_L = false;
            Debug.Log("down: " + handVisual_L.transform.rotation.x);
        }
        if (handVisual_R.transform.rotation.x >= -0.45 && handVisual_R.transform.rotation.x <= 0.45)
        {
            isPalmUp_R = false;
            Debug.Log("up: " + handVisual_R.transform.rotation.x);
        }
        else
        {
            isPalmUp_R = true;
            Debug.Log("down: " + handVisual_R.transform.rotation.x);
        }

        // Debug Mode - Save Left Hand Poses
        if (debugMode && Input.GetKeyDown(KeyCode.J))
        {
            SavePose('L');
        }
        // Debug Mode - Save Left Hand Poses
        if (debugMode && Input.GetKeyDown(KeyCode.K))
        {
            SavePose('R');
        }
        if (debugMode && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Ready");
        }

        //currentPose_L = Recognize('L');
        currentPose_R = Recognize('R');

        /*
        // previous: Has Pose, current: NULL
        if (!previousPose_L.Equals(new Pose()) && currentPose_L.Equals(new Pose()))
        {
            isChanging_L = true;
        }
        if (!previousPose_R.Equals(new Pose()) && currentPose_R.Equals(new Pose()))
        {
            isChanging_R = true;
        }

        bool hasRecognized = !currentPose_L.Equals(new Pose());     // true: currentPose is not a blank Pose structor
        if (isChanging_L && hasRecognized)
        {
            isChanging_L = false;
        }
        if (hasRecognized && !currentPose_L.Equals(previousPose_L)) // Check if new pose
        {
            // New pose
            //Debug.Log("New Pose Found: " + currentPose.name);
            previousPose_L = currentPose_L;
            //currentPose.onRecongnized.Invoke();
        }

        hasRecognized = !currentPose_R.Equals(new Pose());          // true: currentPose is not a blank Pose structor
        if (isChanging_R && hasRecognized)
        {
            isChanging_R = false;
        }
        if (hasRecognized && !currentPose_R.Equals(previousPose_R)) // Check if new pose
        {
            // New pose
            //Debug.Log("New Pose Found: " + currentPose.name);
            previousPose_R = currentPose_R;
            //currentPose.onRecongnized.Invoke();
        }
        */
    }

    public string currentLeftPoseName()
    {
        return currentPose_L.name;
    }
    public string currentRightPoseName()
    {
        return currentPose_R.name;
    }

    void SavePose(int LorR)
    {
        Pose newPose = new Pose();
        newPose.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();

        if (LorR == 'L')
        {
            fingerBones = new List<OVRBone>(skeleton_L.Bones);

            foreach (var bone in fingerBones)
            {
                // finger position relative to root
                data.Add(skeleton_L.transform.InverseTransformPoint(bone.Transform.position));
            }

            newPose.fingerDatas = data;
            newPose.isPalmUp = isPalmUp_L;

            poses_L.Add(newPose);
        } else if (LorR == 'R')
        {
            fingerBones = new List<OVRBone>(skeleton_R.Bones);


            foreach (var bone in fingerBones)
            {
                // finger position relative to root
                data.Add(skeleton_R.transform.InverseTransformPoint(bone.Transform.position));
            }

            newPose.fingerDatas = data;
            newPose.isPalmUp = isPalmUp_R;

            poses_R.Add(newPose);
        }
    }
    private void SetPoseState(string poseName_L, string poseName_R)
    {
        PoseName_L.text = "Left Pose Name: " + poseName_L;
        PoseName_R.text = "Right Pose Name: " + poseName_R;
        if (poseName_L == null)
        {
            PoseName_L.text = "Left Pose Name: Idle";
        }
        if (poseName_R == null)
        {
            PoseName_R.text = "Right Pose Name: Idle";
        }
    }

    Pose Recognize(int LorR)
    {
        Pose currentPose = new Pose();
        currentPose.name = "Idle";
        float currentMin = Mathf.Infinity;

        if (LorR == 'L')
        {
            fingerBones = new List<OVRBone>(skeleton_L.Bones);

            foreach (var pose in poses_L)
            {
                float sumDistance = 0;
                bool isDiscarded = false;

                // Check Shape of Hand based on FingerBones
                for (int i = 0; i < fingerBones.Count; i++)
                {
                    Vector3 currentData = skeleton_L.transform.InverseTransformPoint(fingerBones[i].Transform.position);
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

                // Determine the Pose of Hand based on Shape and Palm Direction
                if (!isDiscarded && sumDistance < currentMin && isPalmUp_L == pose.isPalmUp)
                {
                    currentMin = sumDistance;
                    currentPose = pose;
                }
            }

        } else if (LorR == 'R')
        {
            fingerBones = new List<OVRBone>(skeleton_R.Bones);

            foreach (var pose in poses_R)
            {
                float sumDistance = 0;
                bool isDiscarded = false;

                // Check Shape of Hand based on FingerBones
                for (int i = 0; i < fingerBones.Count; i++)
                {
                    Vector3 currentData = skeleton_R.transform.InverseTransformPoint(fingerBones[i].Transform.position);
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

                // Determine the Pose of Hand based on Shape and Palm Direction
                if (!isDiscarded && sumDistance < currentMin && isPalmUp_R == pose.isPalmUp)
                {
                    currentMin = sumDistance;
                    currentPose = pose;
                }
            }
        }

        return currentPose;
    }
}
