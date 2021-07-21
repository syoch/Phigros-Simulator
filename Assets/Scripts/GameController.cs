using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Line
{
  [System.NonSerialized]
  public GameObject obj;

  public Rotate[] rotates;
  public Position[] positions;
  public Note[] notes;
}
[Serializable]
public class Rotate
{
  public double[] start;
  public double[] end;
  public int val;
}
[Serializable]
public class Position
{
  public double[] start;
  public double[] end;
  public int[] from;
  public int[] to;
}

[Serializable]
public class Note
{
  public double[] timing;
  public string type;
  public double pos;
}
[Serializable]
public class SongSpeed
{
  public double[] timing;
  public double bpm;
}
[Serializable]
public class Chart
{
  public string name;
  public SongSpeed[] times;
  public Line[] lines;
}

public class GameController : MonoBehaviour
{
  public float BarTime;
  public Chart chart;
  public int idx;
  public GameObject LinePrefab;
  public GameObject LoadingObject;
  public double BarYSize;
  static public float StartTime;

  static public GameController Instance;
  public bool Started;
  static bool f;
  void Awake()
  {
    Instance = this;
  }
  // Start is called before the first frame update
  void Start()
  {
    BarTime = 60f / (200f * 8f); // *4f is 32 -> 4
    BarYSize = 100f / 8f;

    StartCoroutine("Load");
  }
  IEnumerator Load()
  {
    LoadingObject.SetActive(true);
    // load prefabs
    Debug.Log("Load - Loading prefabs...");
    LineController.LoadPrefab();
    yield return NoteController.LoadSprites();
    LinePrefab = Resources.Load<GameObject>("Line");
    yield return null;
    // load chart
    Debug.Log("Load - Loading Chart...");
    yield return LoadChart();
    // end
    Debug.Log("Load - Waiting 0.5 second");
    yield return new WaitForSeconds(0.5f);
    GameStart();
    Debug.Log("Load - Loading Process Is Done");
    LoadingObject.SetActive(false);
    Debug.Break();
  }
  IEnumerator LoadChart()
  {
    // load chart( as json )
    Debug.Log("LoadChart - loading json");
    var raw = Resources.Load<TextAsset>("test").text;
    yield return null;
    chart = JsonUtility.FromJson<Chart>(raw);
    yield return null;

    Debug.Log("LoadChart - loading lines");
    foreach (var line in chart.lines)
    {
      yield return Instantiate<GameObject>(LinePrefab)
                  .GetComponent<LineController>()
                  .Load(line);
    }
    Debug.Log("LoadChart - done");
  }
  void GameStart()
  {
    Started = true;
    StartTime = Time.time;
  }
  // Update is called once per frame
  void Update()
  {
    var barIndex = Mathf.Round(Time.time / BarTime);
    var time = Time.time % BarTime;
  }
  public double TimingToYPos(double[] timing)
  {
    return BarYSize * (timing[0] + timing[1] / timing[2]);
  }
  public double TimingToTime(double[] timing)
  {
    var ret = 2 * 60f / 200f * (timing[0] + timing[1] / timing[2]);
    return ret;
  }
}
