using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LineRotate
{
  public LineRotate(Rotate rotate)
  {
    Start = GameController.Instance.TimingToYPos(rotate.start);
    End = GameController.Instance.TimingToYPos(rotate.end);
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
    Start = GameController.Instance.TimingToYPos(position.start);
    End = GameController.Instance.TimingToYPos(position.end);
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
  public LineRotate[] rotates;
  public LinePosition[] positions;
  static public GameObject NotePrefab;
  static public GameObject LinePrefab;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (rotates[0].Start > Time.time) return;


  }
  IEnumerator LoadRotates()
  {
    rotates = new LineRotate[_line.rotates.Length];
    int i = 0;
    foreach (var rotate in _line.rotates)
    {
      rotates[i] = new LineRotate(rotate);
      yield return null;
      i++;
    }
    Array.Sort(rotates, (a, b) => { return a.Start.CompareTo(b.Start); });
  }
  IEnumerator LoadPositions()
  {
    positions = new LinePosition[_line.positions.Length];
    int i = 0;
    foreach (var pos in _line.positions)
    {
      positions[i] = new LinePosition(pos);
      yield return null;
      i++;
    }
    Array.Sort(positions, (a, b) => { return a.Start.CompareTo(b.Start); });
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
