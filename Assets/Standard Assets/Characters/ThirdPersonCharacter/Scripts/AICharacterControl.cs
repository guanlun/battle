using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        private Transform target;                                    // target to aim for
        public String team;

        private List<AICharacterControl> enemies;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            enemies = new List<AICharacterControl>();

            AICharacterControl[] soldierControls = GameObject.FindObjectsOfType(typeof(AICharacterControl)) as AICharacterControl[];
            foreach (AICharacterControl sc in soldierControls) {
                if (sc.team != this.team)
                {
                    this.enemies.Add(sc);
                }
            }

            print(this.enemies.Count);
        }

        private void FindTarget()
        {
            float closestDist = float.MaxValue;
            AICharacterControl closestAgent = null;

            foreach (AICharacterControl enemy in enemies)
            {
                Vector3 enemyPosition = enemy.transform.position;
                float dist = Vector3.Distance(enemyPosition, transform.position);

                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestAgent = enemy;
                }
            }

            target = closestAgent.transform;
        }

        private void Update()
        {
            FindTarget();

            if (target != null)
                agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
