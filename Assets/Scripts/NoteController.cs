using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
  public float StartPos;
  static public Sprite TapNoteSprite;
  static public Sprite DragNoteSprite;
  static public Sprite FlickNoteSprite;
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
  static public IEnumerable LoadSprites()
  {
    var sprites = Resources.LoadAll<Sprite>("Sprites/NormalNotes");
    FlickNoteSprite = sprites[0];
    yield return null;
    DragNoteSprite = sprites[1];
    yield return null;
    TapNoteSprite = sprites[2];
    yield return null;
    // TODO: Load Multi
  }
  Sprite GetNoteBaseObject(string type)
  {
    if (type == "tap")
    {
      return TapNoteSprite;
    }
    else if (type == "drag")
    {
      return DragNoteSprite;
    }
    else if (type == "flick")
    {
      return FlickNoteSprite;
    }
    else
    {
      return TapNoteSprite;
    }
  }
}
