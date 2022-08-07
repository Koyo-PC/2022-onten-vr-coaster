using System;
using UnityEngine;

namespace Sky
{
  public class GameClock : MonoBehaviour
  {
    public float time;
    private float _startTime = GetTime(13);
    public float timeSpeed = 1000;
    private const float DayLength = 60 * 60 * 24;

    public GameObject skyObject;
    private Material _skyMaterial;
    public GameObject sunObject;
    private Transform _sunTransform;
    public GameObject playerObject;
    private Transform _playerTransform;


    public void Start()
    {
      if (skyObject == null) Debug.LogError("Sky is NULL");
      _skyMaterial = skyObject.GetComponent<Renderer>().material;
      time = _startTime;
      if (sunObject == null) Debug.LogError("sunObject is NULL");
      _sunTransform = sunObject.GetComponent<Transform>();
      _playerTransform = playerObject.GetComponent<Transform>();
    }

    public void Update()
    {
      time += Time.deltaTime * timeSpeed;
      _skyMaterial.SetFloat("_Round", PosOfDay(time));
      _sunTransform.position = getSunLightVec(time) * -1000;
      // _sunTransform.Rotate(_playerTransform.position - _sunTransform.position);
      _sunTransform.LookAt(new Vector3(0,0,0));
      Debug.Log("Current time : " + PosOfDay(time) * 24);
    }

    private static float GetTime(float hour)
    {
      return 60 * 60 * hour;
    }

    private float PosOfDay(float time)
    {
      return time / DayLength % 1;
    }

    private Vector3 getSunLightVec(float t)
    {
      const float latitude = 34.76f; // Koyo
      return new Vector3(
        (float)Math.Sin(PosOfDay(t) * 2 * Math.PI),
        (float)(Math.Cos(latitude / 180 * Math.PI) * Math.Cos(PosOfDay(t) * 2 * Math.PI)),
        (float)(Math.Cos(PosOfDay(t) * 2 * Math.PI) * Math.Sin(latitude / 180 * Math.PI))
      ).normalized;
    }
  }
}