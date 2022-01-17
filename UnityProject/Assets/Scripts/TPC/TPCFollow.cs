using UnityEngine;

namespace TPC
{
  /// <summary>
  /// Lets implement a few classes that will allow the camera
  /// to follow the player.
  /// </summary>
  public abstract class TPCFollow : TPCBase
  {
    public TPCFollow(Transform cameraTransform, Transform playerTansform)
      : base(cameraTransform, playerTansform)
    {
    }
    public override void Tick()
    {
      // Now we calculate the camera transformed axes.
      // We do this because our camera's rotation might have changed
      // in the derived class Update implementations. Calculate the new 
      // forward, up and right vectors for the camera.
      Vector3 forward = mCameraTransform.rotation * Vector3.forward;
      Vector3 right = mCameraTransform.rotation * Vector3.right;
      Vector3 up = mCameraTransform.rotation * Vector3.up;

      // We then calculate the offset in the camera's coordinate frame. 
      // For this we first calculate the targetPos
      Vector3 targetPos = mPlayerTransform.position;

      // Add the camera offset to the target position.
      // Note that we cannot just add the offset.
      // You will need to take care of the direction as well.
      Vector3 desiredPosition = targetPos
          + forward * GameConstants.CameraPositionOffset.z
          + right * GameConstants.CameraPositionOffset.x
          + up * GameConstants.CameraPositionOffset.y;

      // Finally, we change the position of the camera, 
      // not directly, but by applying Lerp.
      Vector3 position = Vector3.Lerp(mCameraTransform.position,
          desiredPosition, Time.deltaTime * GameConstants.Damping);
      mCameraTransform.position = position;

    }
  }
}