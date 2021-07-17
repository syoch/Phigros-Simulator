using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
  public float StartPos;
  private bool _initalized = false;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (!_initalized) return;

    var pos = transform.localPosition;
    pos.y -= Time.deltaTime
            / GameController.Instance.BarTime
            * (float)GameController.Instance.BarYSize;
    transform.localPosition = pos;

    if (pos.y < 0)
    {
      // TODO: play note's SE
      Destroy(gameObject, 0);
    }
  }

  public void init()
  {
    _initalized = true;
    StartPos = transform.position.y;
  }
}
