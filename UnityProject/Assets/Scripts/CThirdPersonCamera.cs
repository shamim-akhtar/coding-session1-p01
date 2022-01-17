using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPC;

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
