using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Line
{
  [System.NonSerialized]
  public GameObject[] noteobjects;

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
  public GameObject[] LineObjects;
  public double a;
  static public float StartTime;

  static public GameController Instance;
  void Awake()
  {
    Instance = this;
  }
  // Start is called before the first frame update
  void Start()
  {
    BarTime = 60f / 200f;
    chart = JsonUtility.FromJson<Chart>(Resources.Load<TextAsset>("test").text);

    TapNote = Resources.Load<GameObject>("Note");
    DragNote = Resources.Load<GameObject>("Note");
    FlickNote = Resources.Load<GameObject>("Note");

    LinePrefab = Resources.Load<GameObject>("Line");

    loadChart();
    StartTime = Time.time;
  }
  void loadChart()
  {
    LineObjects = new GameObject[chart.lines.Length];
    int i = 0;
    foreach (var line in chart.lines)
    {
      LoadLine(i, line);
      ++i;
    }
  }
  void LoadLine(int i, Line line)
  {
    var lineObject = Instantiate(LinePrefab);
    var controller = lineObject.GetComponent<LineController>();

    LineObjects[i] = lineObject;

    GameObject baseObject;
    foreach (var note in line.notes)
    {
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
      controller.MakeNode(baseObject, a + TimingToYPos(note.timing), pos.x);
    }
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
