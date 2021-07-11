using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
  public List<GameObject> notes;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
  public void MakeNode(GameObject baseObject, double y)
  {
    var obj = Instantiate(baseObject, transform);
    var pos = obj.transform.position;
    pos.y = (float)y;
    obj.transform.position = pos;
    notes.Add(obj);
  }
}
