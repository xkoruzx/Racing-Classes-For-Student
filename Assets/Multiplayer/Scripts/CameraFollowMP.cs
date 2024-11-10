using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMP : MonoBehaviour
{
    CameraCalculations cameraCalc;

    public static Transform target;

    public int playerLayerNumber = 9;

    public float maxDistance = 4.0f;
    public float minDistance = 2.0f;
    public float minimumHeight = 1.0f;

    public float distanceOffset;
    public float finalDistance;

    public Vector2 cameraSpeed = new Vector2(3f, 1f);
    public Vector2 cameraYLimits = new Vector2(5f, 60f);

    public bool resetCamera = true;

    private float x = 0f;
    private float y = 0f;
    private float yOffset;
    private float newPos;

    private Quaternion rotation;
    private Vector3 position;

    void Start()
    {
        cameraCalc = FindObjectOfType<CameraCalculations>();
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;
        Cursor.visible = false;
    }

    private void Update()
    {
        distanceOffset = cameraCalc.CalculateOffset(target, playerLayerNumber, maxDistance, distanceOffset);
    }

    void LateUpdate()
    {
        PhotonView photonView = GameObject.Find("Player(Clone)").GetPhotonView();
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }
        x += Input.GetAxis("Mouse X") * cameraSpeed.x;
        y -= Input.GetAxis("Mouse Y") * cameraSpeed.y;

        y = cameraCalc.ClampAngle(y, cameraYLimits.x, cameraYLimits.y);

        yOffset = cameraCalc.FindOffsetY(minimumHeight);
        newPos = cameraCalc.FindPosition(minimumHeight);

        if (resetCamera == true)
        {
            x = target.transform.eulerAngles.y;
            y = 0;

            rotation = Quaternion.Euler(y, x, 0f);
        }
        else
        {
            finalDistance = Mathf.Min(-minDistance, -maxDistance + distanceOffset);
            rotation = Quaternion.Euler(y, x, 0f);
            position = rotation * new Vector3(0.0f, 0.0f, finalDistance) + (target.position - new Vector3(0.0f, newPos, 0.0f));
            position.y += yOffset;
        }

        transform.rotation = rotation;
        transform.position = position;
        resetCamera = false;
    }
}
