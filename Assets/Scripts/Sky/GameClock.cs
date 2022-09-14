using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.IO.Ports;

namespace Sky
{
  public class GameClock : MonoBehaviour
  {
    public float time;

    // private float _startTime = GetTime(13);
    private float _startTime = GetTime(10);
    public float timeSpeed = 1300;
    private const float DayLength = 60 * 60 * 24;

    public GameObject nightSkyObject;
    private Material _skyMaterial;
    public GameObject sunObject;
    private Transform _sunTransform;
    public GameObject playerObject;
    private Transform _playerTransform;
    public GameObject horizon;
    private Material _horizonMaterial;

    // private Dictionary<float, Action> timeEvents = new();

    public void Start()
    {
      if (nightSkyObject == null) Debug.LogError("Sky is NULL");
      _skyMaterial = nightSkyObject.GetComponent<Renderer>().material;
      time = _startTime;
      if (sunObject == null) Debug.LogError("sunObject is NULL");
      _sunTransform = sunObject.GetComponent<Transform>();
      _playerTransform = playerObject.GetComponent<Transform>();
      if (horizon == null) Debug.LogError("horizon is NULL");
      _horizonMaterial = horizon.GetComponent<Renderer>().material;

      // timeEvents.Add(DayLength + GetTime(1), () => Lightning.instance.Run());

      Time.timeScale = 0;
    }

    public void Update()
    {
      
      if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
      {
        startGame();
      }
      
      time += Time.deltaTime * timeSpeed;
      _skyMaterial.SetFloat("_Round", PosOfDay(time));
      _sunTransform.position = getSunLightVec(time) * -1000;
      // _sunTransform.Rotate(_playerTransform.position - _sunTransform.position);
      _sunTransform.LookAt(new Vector3(0, 0, 0));
      // Debug.Log("Current time : " + PosOfDay(time) * 24);

      _horizonMaterial.SetVector("_Level1Color", getSkyColor(time));
      _horizonMaterial.SetVector("_Level0Color", getSkySubColor(time));

      // List<float> finishedEventID = new List<float>();
      // foreach (KeyValuePair<float,Action> data in timeEvents)
      // {
      //   if (data.Key < time)
      //   {
      //     finishedEventID.Add(data.Key);
      //     data.Value.Invoke();
      //   }
      // }
      // foreach (var id in finishedEventID)
      // {
      //   timeEvents.Remove(id);
      // }
    }

    private static float GetTime(float hour)
    {
      return 60 * 60 * hour;
    }

    private float PosOfDay(float t)
    {
      return t / DayLength % 1;
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

    /**
     * 0 | black | 5 | black - orange | 6 | orange - blue | 7 | blue - white | 12 | (same)
     */
    private Vector4 getSkyColor(float t)
    {
      float currentTime = t % DayLength;
      Vector4 black = new Vector4(0, 0, 0, 0.1f);
      Vector4 orange = new Vector4(0f, 0.1f, 0.8f, 0.3f);
      Vector4 blue = new Vector4(0, 0.17f, 0.6f, 0.8f);
      Vector4 white = new Vector4(0.2f, 0.33f, 0.66f, 0.7f);
      if (currentTime < GetTime(5)) return black;
      if (currentTime < GetTime(6)) return Vector4.Lerp(black, blue, (currentTime - GetTime(5)) / GetTime(1));
      if (currentTime < GetTime(7)) return blue;
      if (currentTime < GetTime(12)) return Vector4.Lerp(blue, white, (currentTime - GetTime(7)) / GetTime(5));
      if (currentTime < GetTime(17)) return Vector4.Lerp(white, blue, (currentTime - GetTime(12)) / GetTime(5));
      if (currentTime < GetTime(18)) return Vector4.Lerp(blue, orange, (currentTime - GetTime(17)) / GetTime(1));
      if (currentTime < GetTime(19)) return Vector4.Lerp(orange, black, (currentTime - GetTime(18)) / GetTime(1));
      return black;
    }

    private Vector4 getSkySubColor(float t)
    {
      float currentTime = t % DayLength;
      Vector4 black = new Vector4(0, 0, 0.005f, 0.5f);
      Vector4 orange = new Vector4(0.9f, 0.7f, 0.3f, 0.2f);
      Vector4 blue = new Vector4(0, 0.1647059f, 0.6f, 0.7f);
      Vector4 white = new Vector4(0.854902f, 0.8901961f, 0.9294118f, 0.3f);
      if (currentTime < GetTime(5)) return black;
      if (currentTime < GetTime(6)) return Vector4.Lerp(black, white, (currentTime - GetTime(5)) / GetTime(1));
      if (currentTime < GetTime(17)) return white;
      if (currentTime < GetTime(18)) return Vector4.Lerp(white, orange, (currentTime - GetTime(17)) / GetTime(1));
      if (currentTime < GetTime(19)) return Vector4.Lerp(orange, black, (currentTime - GetTime(18)) / GetTime(1));
      return black;
    }

    public void startGame()
    {
      Time.timeScale = 1;
    } 
  }
}