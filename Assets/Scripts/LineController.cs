using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LineRotate
{
  public LineRotate(Rotate rotate)
  {
    Start = GameController.Instance.TimingToTime(rotate.start);
    End = GameController.Instance.TimingToTime(rotate.end);
    Deg = rotate.val;
    During = End - Start;
  }
  public double Start;
  public double End;
  public double During;
  public double Deg;
  override public string ToString()
  {
    return string.Format("{0}->{1}:{2}", Start, End, Deg);
  }
}
public class LinePosition
{
  public LinePosition(Position position)
  {
    Start = GameController.Instance.TimingToTime(position.start);
    End = GameController.Instance.TimingToTime(position.end);
    During = Start - End;
    x = position.x;
    y = position.y;
  }
  public double Start;
  public double End;
  public double During;
  public int x;
  public int y;
  override public string ToString()
  {
    return string.Format("{0}->{1}:({2},{3})", Start, End, x, y);
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

  // Update is called once per frame
  float RotateBegin = 0;
  float BeforeRotate = 0;
  void Update()
  {
    if (rotates[0].Start > Time.time) return;
    var current = (Time.deltaTime) / rotates[0].During * rotates[0].Deg;
    transform.Rotate(0f, 0f, (float)current);
    if (rotates[0].End < Time.time)
    {

    }
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
  }
  IEnumerator LoadPositions()
  {
    positions = new List<LinePosition>();
    int i = 0;
    foreach (var pos in _line.positions)
    {
      positions.Add(new LinePosition(pos));
      yield return null;
      i++;
    }
    positions.Sort((a, b) => { return a.Start.CompareTo(b.Start); });
  }
  public IEnumerator Load(Line line)
  {
    line.obj = gameObject;
    this.line = line;

    LoadRotates();
    LoadPositions();

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
