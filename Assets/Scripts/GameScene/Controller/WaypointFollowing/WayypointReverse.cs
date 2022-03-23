namespace Assets.Scripts.GameScene.Controller.WaypointFollowing
{
    public class WayypointReverse : IWaypointPathType
    {
        bool goingForward;

        public WayypointReverse()
        {
            goingForward = true;
        }

        public void GetNextWaypoint(ref int currentWaypoint, int waypointCount)
        {
            if (currentWaypoint + 1 >= waypointCount)
                goingForward = false;
            else if (currentWaypoint - 1 <= 0)
                goingForward = true;

            currentWaypoint += goingForward ? 1 : -1;
        }
    }
}
