using UnityEngine;

public class TriggerAnim : MonoBehaviour
{
    [SerializeField] TransitionTrigger transitionTrigger;
    private void Awake()
    {
        if (transitionTrigger == null)
        {
            transitionTrigger = GetComponent<TransitionTrigger>();
        }
        
        transitionTrigger.TriggerTransition();
    }
}
