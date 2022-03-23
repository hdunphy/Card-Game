using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameScene.Controller.WaypointFollowing
{
    public enum PathType { Loop, Reverse }

    public enum WaypointState { Moving, Waiting }

    public class WaypointFollower : MonoBehaviour
    {
        [SerializeField] private List<Vector2> Waypoints;
        [SerializeField] private PathType PathType;
        [SerializeField] private float SecondsAtWaypoint;

        private IMovement Movement;
        private IWaypointPathType WaypointPathType;
        private int CurrentWaypoint;

        private WaypointState State;

        private void Start()
        {
            CurrentWaypoint = 0;
            Movement = GetComponent<IMovement>();

            Movement.SetCanMove(true);
            State = WaypointState.Moving;

            WaypointPathType = PathType switch
            {
                PathType.Reverse => new WayypointReverse(),
                _ => new WayypointLoop(),
            };
        }

        private void Update()
        {
            switch (State)
            {
                case WaypointState.Moving:
                    Moving();
                    break;
            }
        }

        private void Moving()
        {
            if (Vector2.Distance(Waypoints[CurrentWaypoint], transform.position) <= 0.1f)
            {
                WaypointPathType.GetNextWaypoint(ref CurrentWaypoint, Waypoints.Count);
                
                Movement.SetMoveDirection(Vector2.zero);
                State = WaypointState.Waiting;
                
                StartCoroutine(GetNextState());
            }
            else
            {
                Vector2 direction = (Waypoints[CurrentWaypoint] - (Vector2)transform.position).normalized;
                Movement.SetMoveDirection(direction);
            }
        }

        private IEnumerator GetNextState()
        {
            yield return new WaitForSeconds(SecondsAtWaypoint);

            State = WaypointState.Moving;
        }

        private void OnDrawGizmosSelected()
        {
            if (Waypoints == null) return;
            
            Gizmos.color = Color.blue;
            foreach(var point in Waypoints)
            {
                Gizmos.DrawSphere(point, .25f);
            }
        }
    }
}
