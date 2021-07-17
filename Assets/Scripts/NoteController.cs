using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
  public float StartPos;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (!GameController.Instance.Started) return;

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
    StartPos = transform.position.y;
  }
}
