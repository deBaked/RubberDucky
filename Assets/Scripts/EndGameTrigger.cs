using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EndGameTrigger : MonoBehaviour
{
    [SerializeField] TransitionTrigger transitionTrigger;
    
    private BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            transitionTrigger.TriggerTransition();
    }
}
