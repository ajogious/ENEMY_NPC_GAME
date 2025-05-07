using UnityEngine;

public class WaypointGroup : MonoBehaviour
{
    public Transform[] waypoints;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (waypoints == null || waypoints.Length < 2) return;

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}
