using UnityEngine;
using System.Collections;

public class EnemyMover : MonoBehaviour {
    public enum MovementDirectionEnum { Left, Right, Stop }

    private MovementDirectionEnum MovementDirection = MovementDirectionEnum.Stop;

    public float Speed = 1.0f;
    public float HorizontalVariation = 0.0f;
    public float HorizontalSpeed = 1.0f;

    private float OriginalY = 0.0f;
    private bool goingUp = true;

    // Use this for initialization
    void Start () {
        OriginalY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (MovementDirection != MovementDirectionEnum.Stop)
        {
            float newX = transform.position.x;
            float newY = transform.position.y;

            if (MovementDirection == MovementDirectionEnum.Left)
            {
                newX -= (Speed * Time.deltaTime);
            }
            else if (MovementDirection == MovementDirectionEnum.Right)
            {
                newX += (Speed * Time.deltaTime);
            }

            float horizVar = (HorizontalVariation * HorizontalSpeed) * Time.deltaTime;

            if (goingUp)
            {
                newY += horizVar;
            }
            else
            {
                newY -= horizVar;
            }

            if (newY >= OriginalY + HorizontalVariation)
            {
                goingUp = false;
            }
            else if (newY <= OriginalY - HorizontalVariation)
            {
                goingUp = true;
            }

            transform.position = new Vector3(newX, newY, transform.position.z);
        }
	}

    public void SetMovementDirection(MovementDirectionEnum mv)
    {
        this.MovementDirection = mv;

        if (mv == MovementDirectionEnum.Right)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            var sp = this.GetComponent<SpriteRenderer>();

            if (sp != null)
            {
                
            }
        }
    }

    public void SetSpeedAndHoriz(float Speed, float Horiz, float HorizSpeed)
    {
        this.Speed = Speed;
        this.HorizontalVariation = Horiz;
    }
}
