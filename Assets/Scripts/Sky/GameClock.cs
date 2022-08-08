using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sky
{
  public class GameClock : MonoBehaviour
  {
    public float time;

    // private float _startTime = GetTime(13);
    private float _startTime = GetTime(3);
    public float timeSpeed = 1000;
    private const float DayLength = 60 * 60 * 24;

    public GameObject nightSkyObject;
    private Material _skyMaterial;
    public GameObject sunObject;
    private Transform _sunTransform;
    public GameObject playerObject;
    private Transform _playerTransform;
    public GameObject horizon;
    public GameObject horizon_2;
    private Material _horizonMaterial;
    private Material _horizon2Material;


    public void Start()
    {
      if (nightSkyObject == null) Debug.LogError("Sky is NULL");
      _skyMaterial = nightSkyObject.GetComponent<Renderer>().material;
      time = _startTime;
      if (sunObject == null) Debug.LogError("sunObject is NULL");
      _sunTransform = sunObject.GetComponent<Transform>();
      _playerTransform = playerObject.GetComponent<Transform>();
      if (horizon == null) Debug.LogError("horizon is NULL");
      if (horizon_2 == null) Debug.LogError("horizon_2 is NULL");
      _horizonMaterial = horizon.GetComponent<Renderer>().material;
      _horizon2Material = horizon_2.GetComponent<Renderer>().material;
    }

    public void Update()
    {
      time += Time.deltaTime * timeSpeed;
      _skyMaterial.SetFloat("_Round", PosOfDay(time));
      _sunTransform.position = getSunLightVec(time) * -1000;
      // _sunTransform.Rotate(_playerTransform.position - _sunTransform.position);
      _sunTransform.LookAt(new Vector3(0, 0, 0));
      Debug.Log("Current time : " + PosOfDay(time) * 24);

      _horizonMaterial.SetVector("_Level1Color", getSkyColor(time));
      _horizonMaterial.SetVector("_Level0Color", getSkySubColor(time));
      _horizon2Material.SetVector("_Level1Color", getSkySubColor(time));
      _horizon2Material.SetVector("_Level0Color", getSkySubColor(time));
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
      Vector4 black = new Vector4(0, 0, 0, 0.1f);
      Vector4 orange = new Vector4(0f, 0.1f, 0.8f, 0.3f);
      Vector4 blue = new Vector4(0, 0.17f, 0.6f, 0.8f);
      Vector4 white = new Vector4(0.2f, 0.33f, 0.66f, 0.7f);
      if (t < GetTime(5)) return black;
      if (t < GetTime(6)) return Vector4.Lerp(black, blue, (t - GetTime(5)) / GetTime(1));
      if (t < GetTime(7)) return blue;
      if (t < GetTime(12)) return Vector4.Lerp(blue, white, (t - GetTime(7)) / GetTime(5));
      if (t < GetTime(17)) return Vector4.Lerp(white, blue, (t - GetTime(12)) / GetTime(5));
      if (t < GetTime(18)) return Vector4.Lerp(blue, orange, (t - GetTime(17)) / GetTime(1));
      if (t < GetTime(19)) return Vector4.Lerp(orange, black, (t - GetTime(18)) / GetTime(1));
      return black;
    }

    private Vector4 getSkySubColor(float t)
    {
      Vector4 black = new Vector4(0, 0, 0.005f, 0.5f);
      Vector4 orange = new Vector4(0.9f, 0.7f, 0.3f, 0.2f);
      Vector4 blue = new Vector4(0, 0.1647059f, 0.6f, 0.7f);
      Vector4 white = new Vector4(0.854902f, 0.8901961f, 0.9294118f, 0.3f);
      if (t < GetTime(5)) return black;
      if (t < GetTime(6)) return Vector4.Lerp(black, orange, (t - GetTime(5)) / GetTime(1));
      if (t < GetTime(7)) return Vector4.Lerp(orange, white, (t - GetTime(6)) / GetTime(1));
      if (t < GetTime(17)) return white;
      if (t < GetTime(18)) return Vector4.Lerp(white, orange, (t - GetTime(17)) / GetTime(1));
      if (t < GetTime(19)) return Vector4.Lerp(orange, black, (t - GetTime(18)) / GetTime(1));
      return black;
    }
  }
}