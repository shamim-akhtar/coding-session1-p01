using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
  public static Vector3 CameraAngleOffset { get; set; }
  public static Vector3 CameraPositionOffset { get; set; }
  public static float Damping { get; set; }
  //public static float RotationSpeed { get; set; }
  //public static float MinPitch { get; set; }
  //public static float MaxPitch { get; set; }
}


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

public class CThirdPersonCamera : MonoBehaviour
{
  public Transform mCameraTransform;
  public Transform mPlayerTransform;

  public Vector3 CameraAngleOffset = new Vector3(0.0f, 0.0f, 0.0f);
  public Vector3 CameraPositionOffset = new Vector3(0.0f, 2.0f, -3.0f);
  public float Damping = 1.0f;

  public enum CameraTypes
  {
    TRACK,
    TRACK_POS,
    TRACK_POS_ROT,
  }
  public CameraTypes myCameraType;
  Dictionary<CameraTypes, TPCBase> myCameras = new Dictionary<CameraTypes, TPCBase>();

  //private TPCBase myCamera;

  // Start is called before the first frame update
  void Start()
  {
    GameConstants.CameraAngleOffset = CameraAngleOffset;
    GameConstants.CameraPositionOffset = CameraPositionOffset;
    GameConstants.Damping = Damping;
    // instantiate the actual TPC camera.
    //myCamera = new TPCTrack(mCameraTransform, mPlayerTransform);
    //myCamera = new TPCFollowTrackPosition(mCameraTransform, mPlayerTransform);

    //Now I add all my camera types to the dictionary.
    // First one is Track
    myCameras.Add(
      CameraTypes.TRACK,
      new TPCTrack(mCameraTransform, mPlayerTransform)
      );

    // the second one is TPCFollowTrackPosition
    myCameras.Add(
      CameraTypes.TRACK_POS,
        new TPCFollowTrackPosition(mCameraTransform, mPlayerTransform)
        );

    // the third one is TPCFollowTrackPositionAndRotation
    myCameras.Add(
      CameraTypes.TRACK_POS_ROT,
        new TPCFollowTrackPositionAndRotation(mCameraTransform, mPlayerTransform)
        );

    myCameraType = CameraTypes.TRACK_POS_ROT;
  }

  private void Update()
  {
    GameConstants.CameraAngleOffset = CameraAngleOffset;
    GameConstants.CameraPositionOffset = CameraPositionOffset;
    GameConstants.Damping = Damping;
  }

  // Update is called once per frame
  void LateUpdate()
  {
    // call the TCP implementation update.
    //myCamera.Tick();
    myCameras[myCameraType].Tick();
  }


}
