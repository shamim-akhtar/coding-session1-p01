
using UnityEngine;

namespace TPC
{
  /// <summary>
  /// This implementation of third preson camera only follows the player
  /// but does not track the player's rotation.
  /// </summary>
  public class TPCFollowTrackPosition : TPCFollow
  {
    public TPCFollowTrackPosition(Transform cameraTransform, Transform playerTransform)
        : base(cameraTransform, playerTransform)
    {
    }

    public override void Tick()
    {
      // Create the initial rotation quaternion based on the 
      // camera angle offset.
      Quaternion initialRotation =
         Quaternion.Euler(GameConstants.CameraAngleOffset);

      // Now rotate the camera to the above initial rotation offset.
      // We do it using damping/Lerp
      // You can change the damping to see the effect.
      mCameraTransform.rotation =
          Quaternion.RotateTowards(mCameraTransform.rotation,
              initialRotation,
              Time.deltaTime * GameConstants.Damping);

      // We now call the base class Update method to take care of the
      // position tracking.
      base.Tick();
    }
  }
}