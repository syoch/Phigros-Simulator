using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public float BarTime;
  // Start is called before the first frame update
  void Start()
  {
    BarTime = 60f / 200f;
  }

  // Update is called once per frame
  void Update()
  {
    Debug.LogFormat("{0} {1}", Mathf.Round(Time.time / BarTime), Time.time % BarTime);
  }
}
