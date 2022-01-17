using UnityEngine;

namespace TPC
{
  public class TPCFollowTrackPositionAndRotation : TPCFollow
  {
    public TPCFollowTrackPositionAndRotation(Transform cameraTransform, Transform playerTransform)
        : base(cameraTransform, playerTransform)
    {
    }

    public override void Tick()
    {
      // We apply the initial rotation to the camera.
      Quaternion initialRotation =
          Quaternion.Euler(GameConstants.CameraAngleOffset);

      // Allow rotation tracking of the player
      // so that our camera rotates when the Player rotates and at the same
      // time maintain the initial rotation offset.
      mCameraTransform.rotation = Quaternion.Lerp(
          mCameraTransform.rotation,
          mPlayerTransform.rotation * initialRotation,
          Time.deltaTime * GameConstants.Damping);

      base.Tick();
    }
  }
}