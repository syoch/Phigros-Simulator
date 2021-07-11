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
  public int[] timing;
  public int[] val;
}
[Serializable]
public class Position
{
  public int[] timing;
  public int x;
  public int y;
}

[Serializable]
public class Note
{
  public int[] timing;
  public string type;
  public double pos;
}
[Serializable]
public class SongSpeed
{
  public int[] timing;
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
    GameObject noteobject;
    LineObjects[i] = Instantiate(LinePrefab);
    var controller = LineObjects[i].GetComponent<LineController>();
    foreach (var note in line.notes)
    {
      if (note.type == "tap")
      {
        noteobject = Instantiate(TapNote);
      }
      else if (note.type == "drag")
      {
        noteobject = Instantiate(DragNote);
      }
      else if (note.type == "flick")
      {
        noteobject = Instantiate(FlickNote);
      }
      else
      {
        noteobject = Instantiate(TapNote);
      }
      controller.notes.Add(noteobject);
    }
  }

  // Update is called once per frame
  void Update()
  {
    var barIndex = Mathf.Round(Time.time / BarTime);
    var time = Time.time % BarTime;
  }
}
