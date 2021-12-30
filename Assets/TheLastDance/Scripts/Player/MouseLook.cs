using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //variables
    public static float mouseSensivity = 100f;
    float xRotation = 0f;

    private float upRecoil;
    private float sideRecoil;
    private float recoilSpeed = 20f;

    // references
    public Transform playerBody;
    public Transform head;
    public Transform rightHand;
    public Transform leftHand;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = head.transform.position;

        leftHand.forward = transform.forward;
        leftHand.right = transform.right;
        leftHand.up = transform.up;
        
        rightHand.forward = transform.forward;
        rightHand.right = transform.right;
        rightHand.up = transform.up;

        float mouseX = sideRecoil + Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = upRecoil + Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        sideRecoil -= recoilSpeed * Time.deltaTime;
        upRecoil -= recoilSpeed * Time.deltaTime;

        if (sideRecoil < 0)
            sideRecoil = 0;
        if (upRecoil < 0)
            upRecoil = 0;
        
        xRotation += -mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        if (Input.GetKey(KeyCode.C))
        {
            sideRecoil /= 2;
            upRecoil /= 2;
        }
    }

    public void GetRecoil(float up, float side)
    {
        upRecoil += up;
        sideRecoil += side;
    }
}
