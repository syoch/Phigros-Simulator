using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LineRotate
{
  public LineRotate(Rotate rotate)
  {
    Start = 60f / 200f * 2f + GameController.Instance.TimingToTime(rotate.start);
    End = 60f / 200f * 2f + GameController.Instance.TimingToTime(rotate.end);
    Deg = rotate.val;
    During = End - Start;
    EndDeg = 0;
  }
  public double Start;
  public double End;
  public double During;
  public double Deg;
  public double EndDeg;
  public bool IsInitalized;
  override public string ToString()
  {
    return string.Format("{0:f2}->{1:f2}:{2:f2}", Start, End, Deg);
  }
}
public class LinePosition
{
  public LinePosition(Position position)
  {
    Start = 60f / 200f * 2f + GameController.Instance.TimingToTime(position.start);
    End = 60f / 200f * 2f + GameController.Instance.TimingToTime(position.end);
    During = Start - End;
    x = position.x;
    y = position.y;
  }
  public double Start;
  public double End;
  public double During;
  public int x;
  public int y;
  public int EndX;
  public int EndY;

  public bool IsInitalized;
  override public string ToString()
  {
    return string.Format("{0:f2}->{1:f2}:({2},{3})", Start, End, x, y);
  }
}

public class LineController : MonoBehaviour
{
  private Line _line;
  public Line line
  {
    set
    {
      _line = value;
    }
  }
  public List<LineRotate> rotates;
  public List<LinePosition> positions;
  static public GameObject NotePrefab;
  static public GameObject LinePrefab;
  // Start is called before the first frame update
  void Start()
  {

  }

  void UpdateLineRotate(float time)
  {
    if (rotates.Count <= 0) return;

    var currentRotation = rotates[0];

    if (currentRotation.Start > time) return;
    if (!currentRotation.IsInitalized)
    {
      currentRotation.EndDeg = transform.rotation.eulerAngles.z + currentRotation.Deg;
      currentRotation.IsInitalized = true;
    }
    transform.Rotate(
      0,
      0,
      (float)(
        (Time.deltaTime) *
        (currentRotation.Deg / currentRotation.During)
      )
    );

    if (currentRotation.End < time)
    {
      var angle = transform.eulerAngles;
      angle.z = (float)currentRotation.EndDeg;
      transform.eulerAngles = angle;
      rotates.RemoveAt(0);
      return;
    }

  }
  // Update is called once per frame
  void Update()
  {
    if (!GameController.Instance.Started) return;
    var time = Time.time - GameController.StartTime;
    UpdateLineRotate(time);


  }
  IEnumerator LoadRotates()
  {
    rotates = new List<LineRotate>();
    int i = 0;
    foreach (var rotate in _line.rotates)
    {
      rotates.Add(new LineRotate(rotate));
      yield return null;
      i++;
    }
    rotates.Sort((a, b) => { return a.Start.CompareTo(b.Start); });
    Debug.Log(string.Join(", ", rotates));
  }
  IEnumerator LoadPositions()
  {
    positions = new List<LinePosition>();
    yield return null;
    /*
    int i = 0;
    foreach (var pos in _line.positions)
    {
      positions.Add(new LinePosition(pos));
      yield return null;
      i++;
    }
    positions.Sort((a, b) => { return a.Start.CompareTo(b.Start); });
    Debug.Log(string.Join(", ", positions));
    */
  }
  public IEnumerator Load(Line line)
  {
    line.obj = gameObject;
    this.line = line;

    yield return LoadRotates();
    yield return LoadPositions();
    foreach (var note in line.notes)
    {
      yield return NoteController.Make(note, line);
    }
  }

  static public void LoadPrefab()
  {
    NotePrefab = Resources.Load<GameObject>("Note");
    LinePrefab = Resources.Load<GameObject>("Line");
  }
}
