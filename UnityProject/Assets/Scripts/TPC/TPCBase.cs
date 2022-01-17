using UnityEngine;

namespace TPC
{

  /// <summary>
  /// We implement a base class that only holds the
  /// references to the two transforms.
  /// TPC base DOES NOR implement any third-person
  /// camera functioalities.
  /// </summary>
  public abstract class TPCBase
  {
    protected Transform mCameraTransform;
    protected Transform mPlayerTransform;

    public TPCBase(Transform cameraTransform, Transform playerTansform)
    {
      mCameraTransform = cameraTransform;
      mPlayerTransform = playerTansform;
    }

    public abstract void Tick();
  }
}