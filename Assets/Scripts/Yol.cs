using UnityEngine;

public class Yol : MonoBehaviour
{

    public Transform yol;

    private void OnDrawGizmos()
    {
        Transform startPos = yol.GetChild(0);

        foreach (Transform durak in yol)
        {
            Gizmos.DrawLine(startPos.position, durak.position);
            startPos = durak;
        }

        Gizmos.DrawLine(startPos.position, yol.GetChild(0).position);
    }
}
