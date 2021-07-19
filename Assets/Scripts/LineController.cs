using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LineRotate
{
  public double Start;
  public double End;
  public double Deg;
  override public string ToString()
  {
    return string.Format("{0}->{1}:{2}", Start, End, Deg);
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
  static public GameObject NotePrefab;
  static public GameObject LinePrefab;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
  public IEnumerator Load(Line line)
  {
    Debug.Log("loading... - loading...");
    line.obj = gameObject;
    this.line = line;

    rotates = new LineRotate[line.rotates.Length];
    LineRotate data;
    int i = 0;
    foreach (var rotate in line.rotates)
    {
      data = new LineRotate();
      data.Start = rotate.start[0] + rotate.start[1] / rotate.start[2];
      data.End = rotate.end[0] + rotate.end[1] / rotate.end[2];
      data.Deg = rotate.val;
      rotates[i] = data;
      yield return null;
      i++;
    }
    Array.Sort(rotates, (a, b) => { return a.Start.CompareTo(b.Start); });

    Debug.Log("loading... - loading notes...");
    foreach (var note in line.notes)
    {
      Debug.Log("loading... - loading notes... - loading...");
      var noteobj = MakeNode(note, line);
    }
  }
  public IEnumerable MakeNode(Note note, Line line)
  {
    var obj = Instantiate(NotePrefab, transform);
    yield return null;

    var pos = Camera.main.ViewportToWorldPoint(new Vector2(
      (float)((note.pos + 1) / 2),
      1
    ));
    yield return null;

    obj.transform.position = new Vector2(
      (float)(GameController.Instance.BarYSize + GameController.Instance.TimingToYPos(note.timing)),
      pos.x
    );
    yield return null;

    var controller = obj.GetComponent<NoteController>();
    controller.init();
    yield return controller.Load(note, line);
  }
  static public void LoadPrefab()
  {
    NotePrefab = Resources.Load<GameObject>("Note");
    LinePrefab = Resources.Load<GameObject>("Line");
  }
  static public IEnumerable Make(Line line)
  {
    return Instantiate<GameObject>(LinePrefab)
                  .GetComponent<LineController>()
                  .Load(line);
  }
}
