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
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
  public IEnumerator Load()
  {
    rotates = new LineRotate[_line.rotates.Length];
    LineRotate data;
    int i = 0;
    foreach (var rotate in _line.rotates)
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
  }
  public GameObject MakeNode(double y, float x)
  {
    var obj = Instantiate(NotePrefab, transform);
    var pos = obj.transform.position;
    pos.y = (float)y;
    pos.x = x;
    obj.transform.position = pos;

    obj.GetComponent<NoteController>().init();

    return obj;
  }
}
