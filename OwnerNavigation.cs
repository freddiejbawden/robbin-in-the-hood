using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.AI;

public class OwnerNavigation : MonoBehaviour
{
    // Start is called before the first frame update
     NavMeshAgent mNavMeshAgent;
    public Transform target;
    public Transform truePos;
    public Rigidbody player;
    public float listenThreshold;
    
    void Start()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void ListenForPlayer() {
      //TODO: get player position certainty
      float a = Random.Range(-1f,1f) * 2 * Mathf.PI;
      if (player.velocity.sqrMagnitude > listenThreshold) {
        float r =  1/(player.velocity.sqrMagnitude + 0.01f);

        // If you need it in Cartesian coordinates
        float x = r * Mathf.Cos(a);
        float z = r * Mathf.Sin(a);
        //FOR TESTING
        target.position = truePos.position + (new Vector3(x,0f,z));
      }
    }
    // Update is called once per frame
    void Update()
    {
      mNavMeshAgent.SetDestination(target.position);
      ListenForPlayer();
      if (!mNavMeshAgent.pathPending)
      {
          if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
          {
              if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f)
              {
                  Debug.Log("reached destination");
              }
          }
      }
  }
}
