using UnityEngine;
using System.Collections;

public class EnemyMover : MonoBehaviour {
    public enum MovementDirectionEnum { Left, Right, Stop }

    private MovementDirectionEnum MovementDirection = MovementDirectionEnum.Stop;

    public float Speed = 1.0f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (MovementDirection != MovementDirectionEnum.Stop)
        {
            float newX = transform.position.x;

            if (MovementDirection == MovementDirectionEnum.Left)
            {
                newX -= (Speed * Time.deltaTime);
            }
            else if (MovementDirection == MovementDirectionEnum.Right)
            {
                newX += (Speed * Time.deltaTime);
            }

            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
	}

    public void SetMovementDirection(MovementDirectionEnum mv)
    {
        this.MovementDirection = mv;
    }
}
