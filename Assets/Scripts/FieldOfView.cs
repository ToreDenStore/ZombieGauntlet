using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float meshResolution;
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        int stepcount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepcount;

        for (int i = 0; i <= stepcount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle/2 + stepAngleSize * i;
            Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);

            print("Drawing line from " + transform.position + " to " + transform.position + DirFromAngle(angle, true) * viewRadius);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
