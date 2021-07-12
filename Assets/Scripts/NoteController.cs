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
    var pos = transform.localPosition;
    pos.y -= Time.deltaTime
            / GameController.Instance.BarTime
            * (float)GameController.Instance.a;
    transform.localPosition = pos;
  }
}
