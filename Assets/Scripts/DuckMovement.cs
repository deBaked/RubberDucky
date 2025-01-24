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

    [SerializeField] CharacterController controller;
    [SerializeField] Transform cam;
    [SerializeField] GameObject water;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    void Start()
    {
        WiimoteManager.FindWiimotes();
        StartCoroutine(ActivateMote());


        //if (mote == null)
        //{
        //    ContiniousScanning();
        //}
        //else
        //{
        //    mote.SendPlayerLED(false, true, false, false);
        //}
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

            if (moveY > -0.7) { moveY = -0.7f; }

            Debug.Log(moveY);
        }

        Vector3 movement = new Vector3(moveX, 0, 0);
        controller.Move(-movement * Time.deltaTime * playerSpeed);
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

    void FixedUpdate()
    {
        water.transform.position += new Vector3(0, 0, moveY * 0.2f);
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
