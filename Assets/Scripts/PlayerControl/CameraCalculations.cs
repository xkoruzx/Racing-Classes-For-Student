using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCalculations : MonoBehaviour
{
    private float yTerrainOffset;
    private float terrainPos;

    public float CalculateOffset(Transform target, int playerLayerNumber, float maxDistance, float distanceOffset)
    {
        int layerMask = 1 << playerLayerNumber;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(target.position, -transform.forward, out hit, maxDistance + 1.0f, layerMask))
        {
            distanceOffset = maxDistance - hit.distance + 2.0f;
            distanceOffset = Mathf.Clamp(distanceOffset, 0, maxDistance);
        }
        else
        {
            distanceOffset = 0; //No hit object found.
        }
        return distanceOffset;
    }

    public float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }
        else if (angle > 360f)
        {
            angle -= 360f;
        }

        return Mathf.Clamp(angle, min, max);
    }

    public float FindOffsetY(float minimumHeight)
    {
        if (Terrain.activeTerrain)
        {
            terrainPos = Terrain.activeTerrain.SampleHeight(transform.position);
        }
        else
        {
            terrainPos = 0;
        }
        yTerrainOffset = terrainPos + minimumHeight;

        return yTerrainOffset;
    }

    public float FindPosition(float minimumHeight)
    {
        if (Terrain.activeTerrain)
        {
            terrainPos = Terrain.activeTerrain.SampleHeight(transform.position);
        }
        else
        {
            terrainPos = 0;
        }
        yTerrainOffset = terrainPos + minimumHeight;

        return terrainPos;
    }
}
