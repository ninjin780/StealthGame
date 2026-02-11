using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (Target != null)
        {
            transform.position = Target.position + Offset;
        }
    }
}
