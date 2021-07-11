using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
  public float StartPos;
  // Start is called before the first frame update
  void Start()
  {
    StartPos = transform.position.y;
  }

  // Update is called once per frame
  void Update()
  {
    var pos = transform.position;

    pos.y = StartPos
          - (Time.time - GameController.StartTime)
            / GameController.Instance.BarTime
            * (float)GameController.Instance.a;
    transform.position = pos;
  }
}
