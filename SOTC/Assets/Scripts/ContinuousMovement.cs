using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    // Movement variables
    public XRNode moveInputSource;
    public float speed = 1;
    public float additionalHeight = 0.2f;
    private float yVel;
    private Vector2 inputAxis;

    // Jumping Variables
    public XRNode jumpInputSource;
    public float jumpForce = 0;
    private bool jumpIsPressed;
    private bool canJump;

    // Other Variables
    public LayerMask groundLayer;
    private XROrigin xrOrigin;
    private Rigidbody rb;
    private CapsuleCollider cc;
    public Transform cam;

    void Start()
    {
        xrOrigin = GetComponent<XROrigin>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        InputDevice moveDevice = InputDevices.GetDeviceAtXRNode(moveInputSource);
        moveDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        InputDevice jumpDevice = InputDevices.GetDeviceAtXRNode(jumpInputSource);
        jumpDevice.TryGetFeatureValue(CommonUsages.primaryButton, out jumpIsPressed);

        yVel = rb.velocity.y;
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();

        if (IsGrounded())
        {
            Quaternion headYaw = Quaternion.Euler(0, cam.eulerAngles.y, 0);
            Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
            rb.velocity = new Vector3(direction.x * speed, yVel, direction.z * speed);
        }

        Jump();
    }

    void CapsuleFollowHeadset()
    {
        if (xrOrigin && xrOrigin.CameraInOriginSpaceHeight > 0)
        {
            cc.height = xrOrigin.CameraInOriginSpaceHeight + additionalHeight;
            Vector3 capsuleCenter = transform.InverseTransformPoint(cam.position);
            cc.center = new Vector3(capsuleCenter.x, cc.height / 2, capsuleCenter.z);
        }
    }

    public bool IsGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(cc.center);
        float rayLength = cc.center.y + 0.01f;
        bool hasHit = Physics.Raycast(rayStart, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        return hasHit;
    }

    void Jump()
    {
        if (IsGrounded())
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        if (jumpIsPressed && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}