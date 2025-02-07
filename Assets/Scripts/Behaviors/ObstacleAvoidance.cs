using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : Seek
{
    public float avoidDistance = 10f;
    public float lookAhead = 10f;

    public float raycastCooldown = 1.0f; // Cooldown duration after hitting an obstacle
    private float lastHitTime = 0f;
    private Vector3 lastTargetPosition;
    private bool isLastTargetPositionInitialized = false;

    protected override Vector3 getTargetPosition()
    {
        // Initialize lastTargetPosition
        if (!isLastTargetPositionInitialized)
        {
            lastTargetPosition = base.getTargetPosition();
            isLastTargetPositionInitialized = true;
        }

        // Check if cooldown is active
        if (Time.time - lastHitTime < raycastCooldown)
        {
            return lastTargetPosition;
        }

        RaycastHit hit;
        if (Physics.Raycast(character.transform.position, character.linearVelocity, out hit, lookAhead))
        {
            Debug.DrawRay(character.transform.position, character.linearVelocity.normalized * hit.distance, Color.red, 0.5f);
            lastTargetPosition = hit.point + (hit.normal * avoidDistance);
            lastHitTime = Time.time;
            return lastTargetPosition;
        }
        else
        {
            Debug.DrawRay(character.transform.position, character.linearVelocity.normalized * lookAhead, Color.green, 0.5f);
            lastTargetPosition = base.getTargetPosition();
            return lastTargetPosition;
        }
    }
}