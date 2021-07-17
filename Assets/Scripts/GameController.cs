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
  public double[] timing;
  public int[] val;
}
[Serializable]
public class Position
{
  public double[] timing;
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
  public double a;
  static public float StartTime;

  static public GameController Instance;
  static bool f;
  void Awake()
  {
    Instance = this;
  }
  // Start is called before the first frame update
  void Start()
  {
    BarTime = 60f / (200f * 8f); // *4f is 32 -> 4
    a = 100f / 8f;

    StartCoroutine("LoadChart");
  }
  IEnumerator LoadChart()
  {
    LoadingObject.SetActive(true);
    // load some prefabs
    TapNote = Resources.Load<GameObject>("TapNote");
    yield return null;
    DragNote = Resources.Load<GameObject>("DragNote");
    yield return null;
    FlickNote = Resources.Load<GameObject>("FlickNote");
    yield return null;
    LinePrefab = Resources.Load<GameObject>("Line");
    yield return null;

    // load chart( as json )
    var raw = Resources.Load<TextAsset>("test").text;
    yield return null;
    chart = JsonUtility.FromJson<Chart>(raw);
    yield return null;

    int i = 0;
    foreach (var line in chart.lines)
    {
      yield return LoadLine(i, line);
      ++i;
      yield return null;
    }
    StartTime = Time.time;
    yield return new WaitForSeconds(0.5f);
    LoadingObject.SetActive(false);
  }
  IEnumerator LoadLine(int i, Line line)
  {
    line.obj = Instantiate(LinePrefab);

    foreach (var note in line.notes)
    {
      yield return LoadNote(note, line);
    }
  }
  IEnumerator LoadNote(Note note, Line line)
  {
    GameObject baseObject;
    var controller = line.obj.GetComponent<LineController>();

    if (note.type == "tap")
    {
      baseObject = TapNote;
    }
    else if (note.type == "drag")
    {
      baseObject = DragNote;
    }
    else if (note.type == "flick")
    {
      baseObject = FlickNote;
    }
    else
    {
      baseObject = TapNote;
    }
    var pos = Camera.main.ViewportToWorldPoint(new Vector2(
      (float)((note.pos + 1) / 2),
      1
    ));
    controller.MakeNode(
      baseObject,
      a + TimingToYPos(note.timing),
      pos.x
    );
    yield return null;
  }

  // Update is called once per frame
  void Update()
  {
    var barIndex = Mathf.Round(Time.time / BarTime);
    var time = Time.time % BarTime;
  }
  double TimingToYPos(double[] timing)
  {
    return a * (timing[0] + timing[1] / timing[2]);
  }
}
