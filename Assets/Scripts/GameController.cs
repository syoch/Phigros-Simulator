using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class NoteBase
{
  public int[] timing;
  public int line;
  public string type;
}
[SerializeField]
public class Note : NoteBase
{
  public double pos; // -120 ~ 120
}

[SerializeField]
public class NewLine : NoteBase
{
  public double[] pos;
  double rot;
}
[SerializeField]
public class MoveLine : NoteBase
{
  public double[] pos;
  public double rot;
}

[SerializeField]
public class Speed : NoteBase
{
  public double rot;
}
[SerializeField]
public class Chart
{
  public string name;
  public int line_count;
  public NoteBase[] notes;
}

public class GameController : MonoBehaviour
{
  public float BarTime;
  public Chart chart;
  public int idx;
  // Start is called before the first frame update
  void Start()
  {
    BarTime = 60f / 200f;
    chart = JsonUtility.FromJson<Chart>(Resources.Load<TextAsset>("test").text);
  }

  // Update is called once per frame
  void Update()
  {
    // Debug.LogFormat("{0} {1}", Mathf.Round(Time.time / BarTime), Time.time % BarTime);
  }
}
