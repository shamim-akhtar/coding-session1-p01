using UnityEngine;

namespace TPC
{
  /// <summary>
  /// Now we can implement few concrete third-person-camera types.
  /// The below class implement a Track funcionality.
  /// </summary>
  public class TPCTrack : TPCBase
  {
    public TPCTrack(Transform cameraTransform, Transform playerTansform)
      : base(cameraTransform, playerTansform)
    {
    }

    public override void Tick()
    {
      const float playerHeight = 2.0f;
      Vector3 targetPos = mPlayerTransform.position;
      targetPos.y += playerHeight;
      mCameraTransform.LookAt(targetPos);
    }
  }
}