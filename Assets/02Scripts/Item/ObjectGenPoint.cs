using UnityEngine;

public class ObjectGenPoint : MonoBehaviour
{
    public GameObject objPrefab;
    public void ObjGen()
    {
        Vector3 pos = transform.position;
        pos.z = -1f;

        Instantiate(objPrefab, pos, Quaternion.identity);
    }
}
