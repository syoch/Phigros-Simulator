using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
  public float StartPos;
  static public Sprite TapNoteSprite;
  static public Sprite DragNoteSprite;
  static public Sprite FlickNoteSprite;
  static public GameObject note;
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
  static public IEnumerator LoadSprites()
  {
    var sprites = Resources.LoadAll<Sprite>("Sprites/NormalNotes");
    FlickNoteSprite = sprites[0];
    yield return null;
    DragNoteSprite = sprites[1];
    yield return null;
    TapNoteSprite = sprites[2];
    yield return null;

    note = Resources.Load<GameObject>("Note");
    // TODO: Load Multi
  }
  static Sprite GetNoteBaseObject(string type)
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
  static public IEnumerator Make(Note note, Line line)
  {
    var obj = Instantiate(NoteController.note, line.obj.transform);
    yield return null;

    var pos = Camera.main.ViewportToWorldPoint(new Vector2(
      (float)((note.pos + 1) / 2),
      1
    ));

    var y = GameController.Instance.BarYSize + GameController.Instance.TimingToYPos(note.timing);
    obj.transform.position = new Vector2(
      pos.x,
      (float)y
    );

    var controller = obj.GetComponent<NoteController>();
    controller.StartPos = obj.transform.position.y;

    obj.GetComponent<SpriteRenderer>().sprite = GetNoteBaseObject(note.type);
  }
}
