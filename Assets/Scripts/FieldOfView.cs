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
        for (int i = 0; i <= stepcount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle/2 + stepAngleSize * i;
            Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);

            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);

            //print("Drawing line from " + transform.position + " to " + transform.position + DirFromAngle(angle, true) * viewRadius);
        }

        int vertexCount = viewPoints.Count + 1; //1 extra for origin point
        Vector3[] vericies = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3]; //the view trangles that together constructs the sight

        vericies[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vericies[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.forward * maskCutawayDistance;

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
        }
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
}
