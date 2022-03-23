namespace Assets.Scripts.GameScene.Controller.WaypointFollowing
{
    public interface IWaypointPathType
    {
        void GetNextWaypoint(ref int currentWaypoint, int waypointCount);
    }
}
