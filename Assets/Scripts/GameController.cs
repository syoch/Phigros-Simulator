using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class Line
{
  [SerializeField]
  public class Rotate
  {
    public int[] timing;
    public int[] val;
  }
  [SerializeField]
  public class Position
  {
    public int[] timing;
    public int x;
    public int y;
  }

  [SerializeField]
  public class Note
  {
    public int[] timing;
    public string type;
    public double pos;
  }
  public GameObject[] noteobjects;

  public Rotate[] rotates;
  public Position[] positions;
  public Note[] notes;
}

[SerializeField]
public class Chart
{
  [SerializeField]
  public class SongSpeed
  {
    public int[] timing;
    public double bpm;
  }


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
  public GameObject LineNote;
  public GameObject[] LineObjects;
  // Start is called before the first frame update
  void Start()
  {
    BarTime = 60f / 200f;
    chart = JsonUtility.FromJson<Chart>(Resources.Load<TextAsset>("test").text);

    TapNote = Resources.Load<GameObject>("Note");
    DragNote = Resources.Load<GameObject>("Note");
    FlickNote = Resources.Load<GameObject>("Note");

    LineNote = Resources.Load<GameObject>("Line");

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
    foreach (var note in line.notes)
    {
      if (note.type == "tap")
      {
        noteobject = Instantiate(TapNote);
        LineObjects[i].GetComponent<LineController>().notes.Add(noteobject);
      }
      if (note.type == "drag")
      {
        noteobject = Instantiate(DragNote);
        LineObjects[i].GetComponent<LineController>().notes.Add(noteobject);
      }
      if (note.type == "flick")
      {
        noteobject = Instantiate(FlickNote);
        LineObjects[i].GetComponent<LineController>().notes.Add(noteobject);
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
    var barIndex = Mathf.Round(Time.time / BarTime);
    var time = Time.time % BarTime;
  }
}
