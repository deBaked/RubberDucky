using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using WiimoteApi;

public class DuckMovement : MonoBehaviour
{
    Wiimote mote;

    float moveX, moveY;

    public float offset;

    public bool flyDisabled;
    
    [SerializeField] CharacterController controller;
    
    [Header("No Fly Movement Settings")]
    public float noFlyPlayerSpeed = 3f;
    public float noFlyPlayerForwardSpeed = 0.7f;
    //public float noFlyPlayerForwardSpeed_Mult = 1f; 
    
    [Header("Flying Movement Settings")]
    public float flyingPlayerSpeed = 1.5f;
    public float flyingPlayerForwardSpeed = 0.5f;
    //public float flyingPlayerForwardSpeed_Mult = 0.7f;
    
    //Movement Settings
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

            Debug.Log(moveY);

            if (mote.Button.two)
            {
                if (flyDisabled)
                {
                    playerForwardSpeed_Mult = 1f * (SprintSpeed / 2);
                }
                else
                {
                    playerForwardSpeed_Mult = 1f * SprintSpeed;
                }
               
            }
            else
            {
                playerForwardSpeed_Mult = 1f;
            }

            if (flyDisabled)
            {
                moveY = 0f;
            }

            wiiMovement = new Vector3(0, moveY, 0.05f);
            
            transform.Translate(wiiMovement);

            transform.RotateAround(transform.position, Vector3.up, moveX);
            transform.forward += new Vector3(0, wiiMovement.y, 0); ;
            
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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (flyDisabled)
                {
                    playerForwardSpeed_Mult = 1f * (SprintSpeed / 2);
                }
                else
                {
                    playerForwardSpeed_Mult = 1f * SprintSpeed;
                }
            }
            else
            {
                playerForwardSpeed_Mult = 1f;
            }

            moveY = Input.GetAxis("Vertical") * 0.05f ;

            if (flyDisabled)
            {
                moveY = 0f;
            }

            movement = new Vector3(0, moveY, 0.05f);
            
            transform.Translate(movement);

            transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Horizontal"));
            transform.forward += new Vector3(0, movement.y, 0);
            
        }
        
    }


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
