using UnityEngine;
public class Billboard : MonoBehaviour
{
        Transform camTransform;
        void Start () => camTransform = Camera.main.transform;
        void LateUpdate () => transform.LookAt(camTransform.rotation * Vector3.forward, Vector3.up);
}