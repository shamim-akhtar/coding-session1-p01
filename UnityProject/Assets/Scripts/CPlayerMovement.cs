using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerMovement : MonoBehaviour
{
  [SerializeField]
  CharacterController mCharacterController;
  [SerializeField]
  Animator mAnimator;

  public float mWalkSpeed = 1.5f;
  public float mRotationSpeed = 50.0f;

  private void Start()
  {
  }

  private void Update()
  {
    // here we will implement the player movement.

    float hInput = Input.GetAxis("Horizontal");
    float vInput = Input.GetAxis("Vertical");

    float speed = mWalkSpeed;

    if (Input.GetKey(KeyCode.LeftShift))
    {
      speed = mWalkSpeed * 2.0f;
    }

    if (mAnimator == null) return;

    transform.Rotate(0.0f, hInput * mRotationSpeed * Time.deltaTime, 0.0f);

    Vector3 forward =
        transform.TransformDirection(Vector3.forward).normalized;
    forward.y = 0.0f;

    mCharacterController.Move(forward * vInput * speed * Time.deltaTime);
    //mAnimator.SetFloat("PosX", 0);
    mAnimator.SetFloat("WalkSpeed", vInput * speed / (2.0f * mWalkSpeed));
  }
}
