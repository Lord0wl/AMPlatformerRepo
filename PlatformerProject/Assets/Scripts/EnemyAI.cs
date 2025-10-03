using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    GameObject player;
    //the speed at which the enemy will move
    public float chaseSpeed = 5.0f;
    //how close the player needs to be for the enemy to start chasing
    public float chaseTriggerDistance = 10f;
    // Start is called before the first frame update
    //do we want the enemy to return home when the player gets away?
    public bool returnHome = true;
    //my home position = my starting position
    Vector3 home;
    //should we patrol or not?
    public bool patrol = true;
    //what direction do we want to patrol in?
    public Vector3 patrolDirection = Vector3.zero;
    //how far do we want to patrol
    public float patrolDistance = 3f;
    bool isHome = true;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //home is my starting position when the game is played
        home = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //figure out where the player is, how far away
        //how far away is the player from me, the enemy
        //destination (player position) - starting position (enemy position)
        //gives the vector from the enemy to the player
        Vector3 chaseDir = player.transform.position - transform.position;
        //if the player is "close" chase the player
        //if they player gets within chaseTriggerDistance units away, chase the player
        if (chaseDir.magnitude < chaseTriggerDistance)
        {
            //chase the player!
            chaseDir.Normalize();
            GetComponent<Rigidbody2D>().velocity = chaseDir * chaseSpeed;
        }
        //if the returnHome variable is true, try to get home
        else if (returnHome && !isHome)
        {
            //return home
            Vector3 homeDir = home - transform.position;
            //if I am far enough away from home, try to go home
            if (homeDir.magnitude > 0.2f)
            {
                homeDir.Normalize();
                GetComponent<Rigidbody2D>().velocity = homeDir * chaseSpeed;
            }
            //if we're close to home
            else
            {
                //stop moving, so we don't twitch like crazy
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                isHome = true;
            }
        }
        //if patrol is true, we want to patrol
        else if (patrol)
        {
            //have the enemy patrol in a given direction
            //if they get too far away from their starting position, flip their direction
            Vector3 displacement = transform.position - home;
            if (displacement.magnitude >= patrolDistance)
            {
                //we have gone too far, we need to turn back
                patrolDirection = -displacement;
            }
            //push the enemy RB in the direction of the patrol
            patrolDirection.Normalize();
            GetComponent<Rigidbody2D>().velocity = patrolDirection * chaseSpeed;
        }
        //otherwise, stop moving
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }
}
