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
  public int[] val;
}
[Serializable]
public class Position
{
  public double[] start;
  public double[] end;
  public int x;
  public int y;
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
  public GameObject TapNote;
  public GameObject DragNote;
  public GameObject FlickNote;
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
    TapNote = Resources.Load<GameObject>("TapNote");
    yield return null;
    DragNote = Resources.Load<GameObject>("DragNote");
    yield return null;
    FlickNote = Resources.Load<GameObject>("FlickNote");
    yield return null;
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
    int i = 0;
    foreach (var line in chart.lines)
    {
      Debug.LogFormat("LoadChart - LoadLines - loading Lines[{0}]", i);
      yield return LoadLine(i, line);
      ++i;
      yield return null;
    }
    StartTime = Time.time;
    Debug.Log("LoadChart - done");
  }
  IEnumerator LoadLine(int i, Line line)
  {
    line.obj = Instantiate(LinePrefab);
    line.obj.GetComponent<LineController>().line = line;
    int j = 0;
    foreach (var note in line.notes)
    {
      Debug.LogFormat("LoadLine - Lines[{0}]:Notes[{1}]", i, j);
      yield return LoadNote(note, line);
      j++;
    }
  }
  IEnumerator LoadNote(Note note, Line line)
  {
    var controller = line.obj.GetComponent<LineController>();

    var pos = Camera.main.ViewportToWorldPoint(new Vector2(
      (float)((note.pos + 1) / 2),
      1
    ));
    controller.MakeNode(
      GetNoteBaseObject(note.type),
      BarYSize + TimingToYPos(note.timing),
      pos.x
    );
    yield return null;
  }
  GameObject GetNoteBaseObject(string type)
  {
    if (type == "tap")
    {
      return TapNote;
    }
    else if (type == "drag")
    {
      return DragNote;
    }
    else if (type == "flick")
    {
      return FlickNote;
    }
    else
    {
      return TapNote;
    }
  }

  void GameStart()
  {
    Started = true;
  }
  // Update is called once per frame
  void Update()
  {
    var barIndex = Mathf.Round(Time.time / BarTime);
    var time = Time.time % BarTime;
  }
  double TimingToYPos(double[] timing)
  {
    return BarYSize * (timing[0] + timing[1] / timing[2]);
  }
}
