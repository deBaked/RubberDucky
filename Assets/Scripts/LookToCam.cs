using UnityEngine;

public class LookToCam : MonoBehaviour
{
    
    
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
