using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Sky
{
  public class Lightning : MonoBehaviour
  {
    public static Lightning instance;

    public void Awake()
    {
      if (instance == null)
      {
        instance = this;
      }
    }

    public GameObject lightning;
    public LineRenderer lightningLine;
    public GameObject lightningStart;
    private Transform lightningStartTransform;
    public GameObject lightningEnd;
    private Transform lightningEndTransform;
    private Vector3 lightningEndDefaultLoc;

    private const float defaultWidth = 2.5f;

    public void Start()
    {
      lightning.SetActive(false);
      lightningStartTransform = lightningStart.GetComponent<Transform>();
      lightningEndTransform = lightningEnd.GetComponent<Transform>();
      lightningEndDefaultLoc = lightningEndTransform.position;
      lightningEndTransform.position = lightningStartTransform.position;

      lightningLine.widthCurve = AnimationCurve.Linear(0, defaultWidth, 1f, defaultWidth);
    }

    public async void Run()
    {
      lightning.SetActive(true);
      for (int i = 1; i <= 10; i++)
      {
        lightningEndTransform.position = Vector3.Lerp(lightningEndTransform.position, lightningEndDefaultLoc, i / 10f);
        await Task.Delay(10);
      }

      await Task.Delay(500);
      for (int i = 1; i <= 10; i++)
      {
        lightningLine.widthCurve = AnimationCurve.Linear(0, defaultWidth * (10 - i) / 10f, 1f, defaultWidth * (10 - i) / 10f);
        await Task.Delay(10);
      }
    }
  }
}