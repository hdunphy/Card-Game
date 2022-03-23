namespace Assets.Scripts.GameScene.Controller.WaypointFollowing
{
    public class WayypointLoop : IWaypointPathType
    {
        public void GetNextWaypoint(ref int currentWaypoint, int waypointCount)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypointCount)
            {
                currentWaypoint = 0;
            }
        }
    }
}
