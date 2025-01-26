using TMPro;
using UnityEngine;
using VHierarchy.Libs;

public class EndScreen : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private GameObject duckling;
    [SerializeField] private GameObject failure;
    
    [SerializeField] string[] endMessages;
    [SerializeField] private TextMeshProUGUI endText;
    
    [SerializeField] private int ducklingCount = 4;
    
    private Animator _animator;
    private GameSingleton _gameSingleton;
    private string _endMessage;
    private int _placeInList = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        
        // Quack quacky quack
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        _gameSingleton = GameSingleton.GetInstance();

        //ducklingCount = _gameSingleton.GetDucklingCount();
        
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // Check to avoid an out of bounds error
        if (_endMessage == null || _endMessage.Length < 4)
        {
            for (int i = 0; i < 5; i++)
            {
                if (endMessages[i].IsNullOrEmpty())
                {
                    endMessages[i] = " ";
                }
            }
        }
        
        endText.text = endMessages[ducklingCount];
    }

    public void TriggerDuckMarker()
    {
        TriggerBubbleNoise();
        
        if (ducklingCount > 0)
        {
            --ducklingCount;
            Instantiate(duckling, spawnPoints[_placeInList], false);
        }
        else
        {
            Instantiate(failure, spawnPoints[_placeInList], false);
        }

        ++_placeInList;
    }
    
    /// <summary>
    /// Quacky quack quack
    /// </summary>
    public void TriggerBubbleNoise()
    {
        audioSource.Play();
    }
    
    public void TriggerAnim()
    {
        _animator.SetBool("Start", true);
    }
    
    
}
