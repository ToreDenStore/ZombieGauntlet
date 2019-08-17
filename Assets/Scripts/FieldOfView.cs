using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script uses the code from the following series of tutorial videos by Sebastian Lague
    about implementing a player field-of-view:
    EP1: https://www.youtube.com/watch?v=rQG9aUWarwE
    EP2: https://www.youtube.com/watch?reload=9&v=73Dc5JTCmKI
    EP3: https://www.youtube.com/watch?v=xkcCWqifT9M
*/
public class FieldOfView : MonoBehaviour
{
    public float meshResolution;
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask obstacleMask;
    public MeshFilter viewMeshFilter;
    public float maskCutawayDistance;
    public int edgeResolveIterations;
    public float edgeDistanceThreshold;

    private Mesh viewMesh;

    // Start is called before the first frame update
    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        int stepcount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepcount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepcount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle/2 + stepAngleSize * i;
            
            ViewCastInfo newViewCast = ViewCast(angle);
            //Debug.DrawLine(transform.position, newViewCast.point, Color.blue);

            if (i > 0)
            {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    //Debug.DrawLine(transform.position, oldViewCast.point, Color.blue);
                    //Debug.DrawLine(transform.position, newViewCast.point, Color.blue);
                    if (edge.pointA != Vector3.zero)
                    {
                        //print("Adding angle");
                        viewPoints.Add(edge.pointA);
                        //Debug.DrawLine(transform.position, edge.pointA, Color.red);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        //print("Adding angle");
                        viewPoints.Add(edge.pointB);
                        //Debug.DrawLine(transform.position, edge.pointB, Color.green);
                    }
                }
            }
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
            //print("Drawing line from " + transform.position + " to " + transform.position + DirFromAngle(angle, true) * viewRadius);
        }
        //print(viewPoints.Count);

        int vertexCount = viewPoints.Count + 1; //1 extra for origin point
        Vector3[] vericies = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3]; //the view trangles that together constructs the sight

        vericies[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            //Debug.DrawLine(transform.position, viewPoints[i], Color.yellow);
            vericies[i + 1] = transform.InverseTransformPoint(viewPoints[i])
                + transform.InverseTransformPoint(viewPoints[i]).normalized * maskCutawayDistance;

            if (i < vertexCount - 2)
            {
                //012034056...
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vericies;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
            //return new ViewCastInfo(false, transform.position, viewRadius, globalAngle);
        }
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        //print("Angle min: " + minAngle);
        //print("Angle max: " + maxAngle);

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            //print("Distance: " + Mathf.Abs(minViewCast.distance - maxViewCast.distance));
            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            } else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        //print("Angle end min: " + minAngle);
        //print("Angle end max: " + maxAngle);

        return new EdgeInfo(minPoint, maxPoint);
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
