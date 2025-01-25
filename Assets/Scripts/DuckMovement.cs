using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using WiimoteApi;

public class DuckMovement : MonoBehaviour
{
    Wiimote mote;

    float moveX, moveY;

    public float offset;

    public bool flyDisabled;
    // {
    //     get { return flyDisabled; }
    //     set
    //     {
    //         flyDisabled = value;
    //         canFly(value);
    //     }
    // }

    [SerializeField] CharacterController controller;
    
    [Header("No Fly Movement Settings")]
    public float noFlyPlayerSpeed = 2.5f;
    public float noFlyPlayerForwardSpeed = 0.7f;
    public float noFlyPlayerForwardSpeed_Mult = 1f; 
    
    [Header("Flying Movement Settings")]
    public float flyingPlayerSpeed = 2.0f;
    public float flyingPlayerForwardSpeed = 0.5f;
    public float flyingPlayerForwardSpeed_Mult = 0.7f;
    
    //Movement Settings
    float playerSpeed = 2.0f;
    float playerForwardSpeed = 0.5f;
    float playerForwardSpeed_Mult = 0.7f; 
    
    Vector3 wiiMovement;
    Vector3 movement;
    
    void Start()
    {
        WiimoteManager.FindWiimotes();
        StartCoroutine(ActivateMote());
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
                playerForwardSpeed_Mult = 2f;
            }
            else
            {
                playerForwardSpeed_Mult = 1f;
            }

            if (flyDisabled)
            {
                moveY = 0f;
            }

            wiiMovement = new Vector3(moveX, moveY, playerForwardSpeed * playerForwardSpeed_Mult);
            controller.Move(-wiiMovement * Time.deltaTime * playerSpeed);
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerForwardSpeed_Mult = 2f;
            }
            else
            {
                playerForwardSpeed_Mult = 1f;
            }

            moveY = Input.GetAxis("Vertical");

            if (flyDisabled)
            {
                moveY = 0f;
            }
            
            movement = new Vector3(Input.GetAxis("Horizontal"), moveY, playerForwardSpeed * playerForwardSpeed_Mult);
            controller.Move(movement * Time.deltaTime * playerSpeed);
        }
        
        //cam.transform.position = new Vector3(0, 0, this.gameObject.transform.position.z);

        //if (movement != Vector3.zero)
        //{
        //    gameObject.transform.forward = movement;
        //}

        //// Makes the player jump
        //if (Input.getbu("Space") && groundedPlayer)
        //{
        //    playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        //}

        //playerVelocity.y += gravityValue * Time.deltaTime;
        //controller.Move(playerVelocity * Time.deltaTime);
    }

    /*
    void FixedUpdate()
    {
        if (mote != null)
        {
            water.transform.position += new Vector3(0, 0, moveY * 0.2f);
        }
        else
        {
            water.transform.position += new Vector3(0, 0, moveY * 0.2f);
        }
        
    }*/

    public void canFly(bool flying)
    {
        if (flying)
        {
            flyDisabled = false;
            
            // PLAYER IS FLYING
            playerSpeed = flyingPlayerSpeed;
            playerForwardSpeed = flyingPlayerForwardSpeed;
            playerForwardSpeed_Mult = flyingPlayerForwardSpeed_Mult;
        }
        else
        {
            flyDisabled = true;
            
            // PLAYER CANNOT FLY
            playerSpeed = noFlyPlayerSpeed;
            playerForwardSpeed = noFlyPlayerForwardSpeed;
            playerForwardSpeed_Mult = noFlyPlayerForwardSpeed_Mult;
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
