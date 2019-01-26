using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.AI;

public class OwnerNavigation : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent mNavMeshAgent;
    public Vector3[] randomPositions;
    Vector3 lastPosition;
    float last_speed;
    public BoxCollider sightLine;
    public Transform target;
    public Transform truePos;
    public GameObject player;
    public float stopping_threshold;
    Player_Movement player_movement;
    bool chase = false;
    int layerMask = 1 << 2;
    public float listenThreshold;
    public float sightDistance;



    void Start()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        mNavMeshAgent.updateRotation = true;
        setUpSight();
        setUpTracking();
    }

    private void ListenForPlayer() {
      //TODO: get player position certainty
      if (player_movement.speed == 0) {
        return;
      }
      float a;
      Debug.Log("Locked on, advancing");
      if (player_movement.speed > 2) {
         a = Random.Range(-20f,20f) * 2 * Mathf.PI;
      } else {
          a = Random.Range(-8f,8f) * 2 * Mathf.PI;
      }
      float r =  1;

      // If you need it in Cartesian coordinates
      float x = a * Mathf.Cos(a);
      float z = a * Mathf.Sin(a);
      //FOR TESTING
      target.position = truePos.position + (new Vector3(x,0,z));
      last_speed = player_movement.speed;
    }
    void setUpTracking() {
      player_movement = player.GetComponent<Player_Movement>();
    }
    void setUpSight() {
      sightLine.size = new Vector3(1f,1f,sightDistance);
      sightLine.center = new Vector3(0,0,sightDistance/2);
    }
    void updateRotation() {
      transform.LookAt(transform.position + mNavMeshAgent.velocity);
      Debug.DrawLine(transform.position, transform.position + mNavMeshAgent.velocity, Color.black,0.0f);
    }
    void OnTriggerEnter(Collider c) {
      if (c.gameObject.tag == "Player") {
        Debug.Log("DETECTED");
        chase = true;
      }
    }
    // Update is called once per frame
    void Update()
    {
      if (chase) {
        target.position = truePos.position;
        //check sightline
        if (Physics.Linecast(transform.position, target.position,layerMask))
       {
           chase = false;
       }
      }
      mNavMeshAgent.SetDestination(target.position);
      if ((lastPosition - transform.position).sqrMagnitude > stopping_threshold) {
        if (last_speed != player_movement.speed) {
          ListenForPlayer();
        }
      }
      updateRotation();
      lastPosition = transform.position;

      if (!mNavMeshAgent.pathPending)
      {
          if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
          {
              if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f)
              {
                  Debug.Log("reached destination");
                  int i = Random.Range(0,randomPositions.Length);
                  target.position = randomPositions[i];
              }
          }
      }
  }
}
