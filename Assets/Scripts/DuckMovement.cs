using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using WiimoteApi;

public class DuckMovement : MonoBehaviour
{
    Wiimote mote;

    float moveX, moveY;

    public bool flyDisabled;
    public Slider boostSlider;
    public GameObject Bubble;

    [SerializeField] ParticleSystem QuackVFX;
    [SerializeField] AudioSource[] Quacks;
    [SerializeField] ParticleSystem FartVFX;
    [SerializeField] AudioSource[] Farts;
    [HideInInspector] public bool farted;

    private MotherDuckCounter motherDuckSC;

    [SerializeField] Rigidbody controller;
    
    [Header("No Fly Movement Settings")]
    public float noFlyPlayerSpeed = 3f;
    public float noFlyPlayerForwardSpeed = 0.07f;
    //public float noFlyPlayerForwardSpeed_Mult = 1f; 
    
    [Header("Flying Movement Settings")]
    public float flyingPlayerSpeed = 1.5f;
    public float flyingPlayerForwardSpeed = 0.05f;
    //public float flyingPlayerForwardSpeed_Mult = 0.7f;
    
    [Header("Current Movement Settings")]
    public float playerSpeed;
    public float playerForwardSpeed;
    public float SprintSpeed;
    public float rotationSpeed;
    private float playerForwardSpeed_Mult; 
    
    Vector3 wiiMovement;
    Vector3 movement;
    
    void Start()
    {
        playerSpeed = noFlyPlayerSpeed;
        playerForwardSpeed = noFlyPlayerForwardSpeed;
        motherDuckSC = gameObject.GetComponent<MotherDuckCounter>();

        WiimoteManager.FindWiimotes();
        StartCoroutine(ActivateMote());

        //playerForwardSpeed_Mult = noFlyPlayerForwardSpeed_Mult;
    }

    private void ContiniousScanning()
    {
        WiimoteManager.FindWiimotes();
        mote = WiimoteManager.Wiimotes[0];

        if (mote == null)
        {
            ContiniousScanning();
        }
        else
        {
            mote.SendPlayerLED(false, true, false, false);
        }
    }


    void Update()
    {
        // WII MOTE INPUTS
        if(mote != null)
        {
            // recalibrate the wiiMote
            if (mote.Button.home)
            {
                mote.Accel.CalibrateAccel(AccelCalibrationStep.LEFT_SIDE_UP);
            }

            //Debug.Log("getting data");
            float[] accell = mote.Accel.GetCalibratedAccelData();

            moveX = accell[1] - 0.3f;
            moveY = accell[0] - 0.1f;

            if (moveX > -0.25f && moveX < 0.25f) { moveX = 0f; }
            if (moveY > -0.25f && moveY < 0.25f) { moveY = 0f; }

            //if (moveY > -0.7) { moveY = -0.7f; }
            moveY = moveY * 0.05f;
            //Debug.Log(moveY);

            if (boostSlider.value != 0 && mote.Button.two)
            {
                if (flyDisabled)
                {
                    playerForwardSpeed_Mult = 1f * (SprintSpeed / 2);
                }
                else
                {
                    playerForwardSpeed_Mult = 1f * SprintSpeed;
                }

                if (!farted && boostSlider.value > 1f)
                {
                    farted = true;
                    TriggerFart();
                }
                boostSlider.value -= 0.2f;
            }
            else
            {
                playerForwardSpeed_Mult = 1f;

                boostSlider.value += 0.1f;
            }

            if (flyDisabled)
            {
                moveY = 0f;
            }

            wiiMovement = new Vector3(0, moveY * 0.5f , (playerForwardSpeed * playerForwardSpeed_Mult ) * Time.deltaTime);
            
            transform.Translate(wiiMovement);

            transform.RotateAround(transform.position, Vector3.up, -moveX);
            transform.forward += new Vector3(0, wiiMovement.y / 2f, 0);
            
            //wiiMovement = new Vector3(moveX, moveY, playerForwardSpeed * playerForwardSpeed_Mult);
            // controller.Move(-wiiMovement * Time.deltaTime * playerSpeed);
            //
            // if (wiiMovement != Vector3.zero)
            // {
            //     gameObject.transform.forward = wiiMovement;
            //     
            // }
        }
        // WASD INPUTS
        else
        {
            if (boostSlider.value != 0 && Input.GetKey(KeyCode.LeftShift))
            {
                if (flyDisabled)
                {
                    playerForwardSpeed_Mult = 1f * (SprintSpeed / 2);
                }
                else
                {
                    playerForwardSpeed_Mult = 1f * SprintSpeed;
                }

                if (!farted && boostSlider.value > 1f)
                {
                    farted = true;
                    TriggerFart();
                }
                boostSlider.value -= 0.2f;
            }
            else
            {
                playerForwardSpeed_Mult = 1f;

                boostSlider.value += 0.1f;
            }

            moveY = Input.GetAxis("Vertical") * 0.05f ;

            if (flyDisabled)
            {
                moveY = 0f;
            }

            movement = new Vector3(0, moveY, playerForwardSpeed * Time.deltaTime);
            
            //controller.AddForce(movement, ForceMode.Force);
            transform.Translate(movement);

            transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Horizontal"));
            transform.forward += new Vector3(0, movement.y / 2f, 0);
            
        }
        
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     Debug.Log("collision");
    //     playerForwardSpeed = 0f;
    // }
    //
    // private void OnCollisionExit(Collision other)
    // {
    //     if (flyDisabled)
    //     {
    //         // if you player isn't flying
    //         playerForwardSpeed = noFlyPlayerForwardSpeed;
    //     }
    //     else
    //     {
    //         // if play is flying
    //         playerForwardSpeed = flyingPlayerForwardSpeed;
    //     }
    // }


    public void canFly(bool flying)
    {
        if (flying)
        {
            flyDisabled = false;
            
            // PLAYER IS FLYING
            playerSpeed = flyingPlayerSpeed;
            playerForwardSpeed = flyingPlayerForwardSpeed;
            //playerForwardSpeed_Mult = flyingPlayerForwardSpeed_Mult;
        }
        else
        {
            flyDisabled = true;
            
            // PLAYER CANNOT FLY
            playerSpeed = noFlyPlayerSpeed;
            playerForwardSpeed = noFlyPlayerForwardSpeed;
            //playerForwardSpeed_Mult = noFlyPlayerForwardSpeed_Mult;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CamCollision"))
        {
            TriggerQuack();

            //Debug.Log("Force applied");
            Vector3 point = other.contacts[0].point;
            Vector3 direction = Vector3.Normalize(point - transform.position) * 6f;

            float speed;
            if (flyDisabled)
            {
                speed = noFlyPlayerForwardSpeed;
            }
            else
            {
                speed = flyingPlayerForwardSpeed;
            }
            
            playerForwardSpeed = 0f;

            controller.AddForce(-direction, ForceMode.Impulse);
            StartCoroutine(RemoveForce(speed, direction));
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("SmallBounce"))
        {
            TriggerQuack();

            //Debug.Log("Force applied");
            Vector3 point = other.contacts[0].point;
            Vector3 direction = Vector3.Normalize(point - transform.position) * 2f;

            float speed;
            if (flyDisabled)
            {
                speed = noFlyPlayerForwardSpeed;
            }
            else
            {
                speed = flyingPlayerForwardSpeed;
            }

            playerForwardSpeed = 0f;

            controller.AddForce(-direction, ForceMode.Impulse);
            StartCoroutine(RemoveForce(speed, direction));
        }
    }

    private IEnumerator RemoveForce(float defaultSpeed, Vector3 direction)
    {
        yield return new WaitForSeconds(0.4f);
        controller.velocity *= 0.8f;
        yield return new WaitForSeconds(0.2f);
        controller.velocity *= 0.5f;
        yield return new WaitForSeconds(0.1f);
        controller.velocity = Vector3.zero;
        playerForwardSpeed = defaultSpeed;
    }
    
    private void TriggerQuack()
    {
        Quacks[UnityEngine.Random.Range(0, Quacks.Length)].Play();
        QuackVFX.Play();
    }

    private void TriggerFart()
    {
        Farts[UnityEngine.Random.Range(0, Farts.Length)].Play();
        FartVFX.Play();
        StartCoroutine(enableFarting());
    }

    IEnumerator enableFarting()
    {
        yield return new WaitForSeconds(0.5f);
        farted = false;
    }

    IEnumerator ActivateMote()
    {
        yield return new WaitUntil(() => WiimoteManager.HasWiimote());

        mote = WiimoteManager.Wiimotes[0];
        mote.SendDataReportMode(InputDataType.REPORT_BUTTONS_ACCEL_EXT16);
        mote.Accel.CalibrateAccel(AccelCalibrationStep.LEFT_SIDE_UP);
        mote.SendPlayerLED(true, false, false, false);

        Debug.Log("mote connected");
    }
}
