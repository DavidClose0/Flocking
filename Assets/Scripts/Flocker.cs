using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : Kinematic
{
    BlendedSteering myMoveType;
    LookWhereGoing myRotateType;
    Arrive cohesion;
    Separation separation;
    VelocityMatch velocityMatch;
    public Kinematic[] targets; // Public for inspector visibility if needed

    public float cohesionWeight = 0.1f;
    public float separationWeight = 0.6f;
    public float velocityMatchWeight = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        // Find all birds except self
        GameObject[] birdObjects = GameObject.FindGameObjectsWithTag("Bird");
        List<Kinematic> birdKinematics = new List<Kinematic>();
        foreach (GameObject birdObject in birdObjects)
        {
            Kinematic birdKinematic = birdObject.GetComponent<Kinematic>();
            if (birdKinematic != null && birdKinematic != this) // Exclude self
            {
                birdKinematics.Add(birdKinematic);
            }
        }
        targets = birdKinematics.ToArray();

        // Initialize steering behaviors
        cohesion = new Arrive();
        cohesion.character = this;
        cohesion.target = new GameObject("CohesionTarget"); // Dummy target, will be updated

        separation = new Separation();
        separation.character = this;
        separation.targets = targets;

        velocityMatch = new VelocityMatch();
        velocityMatch.character = this;
        velocityMatch.targets = targets;

        myMoveType = new BlendedSteering();
        myMoveType.behaviors = new BlendedSteering.BehaviorAndWeight[]
        {
            new BlendedSteering.BehaviorAndWeight() { behavior = cohesion, weight = cohesionWeight },
            new BlendedSteering.BehaviorAndWeight() { behavior = separation, weight = separationWeight },
            new BlendedSteering.BehaviorAndWeight() { behavior = velocityMatch, weight = velocityMatchWeight }
        };

        myRotateType = new LookWhereGoing();
        myRotateType.character = this;
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Calculate cohesion target (center of mass)
        Vector3 centerOfMass = Vector3.zero;
        if (targets.Length > 0)
        {
            foreach (Kinematic target in targets)
            {
                centerOfMass += target.transform.position;
            }
            centerOfMass /= targets.Length;
        }
        cohesion.target.transform.position = centerOfMass; // Update cohesion target position

        steeringUpdate = new SteeringOutput();
        steeringUpdate.linear = myMoveType.getSteering().linear;
        steeringUpdate.angular = myRotateType.getSteering().angular;
        base.Update();
    }
}