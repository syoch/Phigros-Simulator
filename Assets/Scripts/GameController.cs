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
  public GameObject[] notes;

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
  // Start is called before the first frame update
  void Start()
  {
    BarTime = 60f / 200f;
    chart = JsonUtility.FromJson<Chart>(Resources.Load<TextAsset>("test").text);

    TapNote = Resources.Load<GameObject>("Note");
    DragNote = Resources.Load<GameObject>("Note");
    FlickNote = Resources.Load<GameObject>("Note");

    LineNote = Resources.Load<GameObject>("Line");


  }

  // Update is called once per frame
  void Update()
  {
    // Debug.LogFormat("{0} {1}", Mathf.Round(Time.time / BarTime), Time.time % BarTime);
  }
}
