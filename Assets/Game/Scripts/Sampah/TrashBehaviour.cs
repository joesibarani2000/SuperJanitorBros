using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;

[RequireComponent(typeof(TrashData))]
public class TrashBehaviour : MonoBehaviour
{
    [SerializeField] private TrashData trashData;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private ParticleSystem spark;

    [SerializeField] private TrashPlacement trashPlacement;

    private Vector2 firstPosition; 

    // Start is called before the first frame update
    void Start()
    {
        firstPosition = transform.position;
    }

    public void DropActionTrash(Vector2 playerPos, bool facingRight)
    {
        if (trashData.Type == TrashData.TrashType.BRING)
        {
            DropTrash(playerPos, facingRight);
        }
    }

    public void PickActionTrash(Transform parent)
    {
        if (trashData.Type == TrashData.TrashType.BRING)
        {
            PickTrash(parent);
        }
    }

    public void SwipeActionTrash()
    {
        StartCoroutine(WaitSwipe());
        GameData.instance.AddScore(trashData.TrashScore);
        
    }

    IEnumerator WaitSwipe()
    {
        spark.Play();
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    public void PushActionTrash(Vector2 direction, float pushTime)
    {
        if (trashData.Type == TrashData.TrashType.PUSH)
        {
            Vector2 offsetDirection = Vector2.zero;

            if (direction == Vector2.right || direction == Vector2.left) offsetDirection = new Vector2(0, trashData.RectTrash.y / 2);
            else if (direction == Vector2.up || direction == Vector2.down) offsetDirection = new Vector2(trashData.RectTrash.x / 2, 0);

            RaycastHit2D hit1 = Physics2D.Raycast((Vector2)transform.position + offsetDirection, direction, Mathf.Infinity, trashData.LayerMask);
            RaycastHit2D hit2 = Physics2D.Raycast((Vector2)transform.position - offsetDirection, direction, Mathf.Infinity, trashData.LayerMask);

            if (hit1.collider && hit2.collider)
            {
                if (GetDistance(firstPosition, hit1.point - offsetDirection) > GetDistance(firstPosition, hit2.point))
                {
                    PushTo(direction, hit2.point + offsetDirection, pushTime);
                }
                else
                {
                    PushTo(direction, hit1.point - offsetDirection, pushTime);
                }
            }
            else if (hit1.collider)
            {
                PushTo(direction, hit1.point - offsetDirection, pushTime);
            }
            else if (hit2.collider)
            {
                PushTo(direction, hit2.point + offsetDirection, pushTime);
            }
        }
    }

    public TrashData.TrashType GetTrashType()
    {
        return trashData.Type;
    }

    public TrashData GetData()
    {
        return trashData;
    }

    private void PushTo(Vector2 direction, Vector2 hitPoint, float pushTime)
    {
        if (trashPlacement) trashPlacement.UnPlaced(this);

        Vector2 targetMove = hitPoint;
        Vector2 firstPosition = transform.position;
        if (direction == Vector2.left) targetMove = hitPoint + (Vector2.right * trashData.RectTrash.x);
        else if (direction == Vector2.right) targetMove = hitPoint + (Vector2.left * trashData.RectTrash.x);
        else if (direction == Vector2.up) targetMove = hitPoint + (Vector2.down * trashData.RectTrash.y);
        else if (direction == Vector2.down) targetMove = hitPoint + (Vector2.up * trashData.RectTrash.y);

        transform.DOMove(targetMove, pushTime).OnComplete(() =>
        {
            if (GetDistance(firstPosition, targetMove) > 0.5f)
            {
                GameData.instance.GetCamera().DOShakeRotation(0.2f, 1, 30, 10);
                TWAudioController.PlaySFX("trashimpact");
            }

            if (trashPlacement)
            {
                spark.Play();
                trashPlacement.Placed(this);
            }
        });
    }

    private void PickTrash(Transform parent)
    {
        if (trashPlacement) trashPlacement.UnPlaced(this);
        transform.parent = parent;
        transform.localPosition = Vector2.zero;
        bodyCollider.enabled = false;
    }

    private void DropTrash(Vector2 playerPos, bool facingRight)
    {
        bodyCollider.enabled = true;
        transform.parent = null;
        transform.position = playerPos + new Vector2(facingRight ? 0.8f : -0.8f, 0);
        CheckingAround();
        if (trashPlacement)
        {
            StartCoroutine(Drop());
        } 
            
    }

    IEnumerator Drop()
    {
        spark.Play();
        trashPlacement.Placed(this);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    private void CheckingAround()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("TrashPlacement"));

        if (collider && collider.CompareTag("TrashPlacement"))
        {
            trashPlacement = collider.GetComponent<TrashPlacement>();
        }
    }

    private float GetDistance(Vector2 posA, Vector2 posB)
    {
        return Mathf.Sqrt((posB - posA).sqrMagnitude);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (trashData.Type == TrashData.TrashType.SWIPE)
        {
            if (collision.tag == "Player" && GameData.instance.GetPlayer().PlayerDashing())
            {
                SwipeActionTrash();
            }
        }

        if (trashData.Type == TrashData.TrashType.PUSH)
        {
            if (collision.tag == "TrashPlacement")
            {
                trashPlacement = collision.GetComponent<TrashPlacement>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (trashData.Type == TrashData.TrashType.PUSH)
        {
            if (collision.tag == "TrashPlacement")
            {
                trashPlacement = null;
            }
        }
    }
}
