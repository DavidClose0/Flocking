using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoider : Kinematic
{
    ObstacleAvoidance myMoveType;
    Face myRotateType;

    public float avoidDistance;
    public float lookAhead;

    // Start is called before the first frame update
    void Start()
    {
        myMoveType = new ObstacleAvoidance();
        myMoveType.character = this;
        myMoveType.target = myTarget;
        myMoveType.avoidDistance = avoidDistance;
        myMoveType.lookAhead = lookAhead;

        myRotateType = new Face();
        myRotateType.character = this;
        myRotateType.target = myTarget;
    }

    // Update is called once per frame
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.linear = myMoveType.getSteering().linear;
        steeringUpdate.angular = myRotateType.getSteering().angular;
        base.Update();
    }
}
