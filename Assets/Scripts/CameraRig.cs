using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] float lerpSpeed = 20f;

    float cameraDistance = 50f;
    Vector3 targetPos;
    Vector3 currPos;

    public enum CameraMovement
    {
        SnapToPoint,
        MoveAlongRail
    }

    BaseMovement[] players;

    CameraMovement moveType = CameraMovement.SnapToPoint;
    Vector3 startPoint;
    Vector3 endPoint;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<BaseMovement>();
        FindNearestRail();
    }

    // Update is called once per frame
    void Update()
    {
        MoveAlongRail();
        SetCameraPosition();
    }

    Vector3 GetPointBetweenPlayers()
    {
        Vector3 p1Pos = players[0].transform.position;
        Vector3 p2Pos = players[1].transform.position;

        return Vector3.Lerp(p1Pos, p2Pos, 0.5f);
    }

    public void AttachToRail(CameraRail rail)
    {
        moveType = rail.cameraMoveType;
        cameraDistance = rail.cameraDistance;

        switch (moveType)
        {
            case CameraMovement.SnapToPoint:
                targetPos = rail.transform.position;
                break;
            case CameraMovement.MoveAlongRail:
                startPoint = rail.railPoints[0];
                endPoint = rail.railPoints[1];
                break;
        }
    }

    void MoveAlongRail()
    {
        if (moveType != CameraMovement.MoveAlongRail) return;

        Vector3 pointBetweenPlayers = GetPointBetweenPlayers();

        Vector3 startToPlayer = pointBetweenPlayers - startPoint;
        Vector3 endToPlayer = pointBetweenPlayers - endPoint;
        Vector3 startToEnd = endPoint - startPoint;
        Vector3 endToStart = startPoint - endPoint;

        float dotToStart = Vector3.Dot(startToPlayer.normalized, startToEnd.normalized);
        float dotToEnd = Vector3.Dot(endToPlayer.normalized, endToStart.normalized);

        float totalDist = Mathf.Abs(endPoint.x - startPoint.x);
        float currDist = 0;

        if (dotToStart > 0f && dotToEnd > 0f)
        {
            currDist = Mathf.Abs(pointBetweenPlayers.x - startPoint.x);
        }
        else if (dotToEnd < 0f) currDist = totalDist;

        float alpha = Mathf.Clamp01(currDist / totalDist);

        targetPos = Vector3.Lerp(startPoint, endPoint, alpha);
    }

    void SetCameraPosition()
    {
        Vector3 offset = transform.GetChild(0).transform.forward * cameraDistance;

        transform.position += offset;

        transform.position = Vector3.Slerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);

        transform.position -= offset;
    }

    void FindNearestRail()
    {
        CameraRail[] allRails = FindObjectsOfType<CameraRail>();
        if (allRails.Length == 0) return;

        int closest = 0;
        float closestDist = Mathf.Infinity;

        Vector3 cameraPos = transform.position;

        for (int i = 0; i < allRails.Length; i++)
        {
            float dist = (cameraPos - allRails[i].transform.position).sqrMagnitude;
            if (dist < closestDist)
            {
                closest = i;
                closestDist = dist;
            }
        }

        AttachToRail(allRails[closest]);
    }    
}
