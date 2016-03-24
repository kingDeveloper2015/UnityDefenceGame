using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class FlyingObjectMover : MonoBehaviour {

    public List<Vector3> Paths;
    public List<float> MovementSpeed;
    public float startingDelay;

    private bool isMoving = false;
    private bool isRotating = false;
    private int currentPath = 0;
    private Vector3 startingPos;
    private Vector3 startingRot;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
        if(Paths.Count > 0)
        {
            startingRot = getRotation(transform.position, Paths[0]);
        }
        else
        {
            startingRot = Vector3.zero;
        }
        ResetAnimation();
    }
	
    void StartAnim()
    {
        if (Paths.Count > 0)
        {
            isMoving = true;
        }
    }
	// Update is called once per frame
	void Update () {
	
	}

    void ResetAnimation()
    {
        isMoving = false;
        currentPath = 0;
        transform.position = startingPos;
        transform.rotation = Quaternion.Euler(startingRot.x, startingRot.y, startingRot.z);
        Invoke("StartAnim", startingDelay);
    }

    void FixedUpdate()
    {

        if (!GameController.Instance.gameOver && isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, Paths[currentPath], MovementSpeed[currentPath] * Time.fixedDeltaTime);

            if (transform.position == Paths[currentPath])
            {
                currentPath++;

                if (currentPath >= Paths.Count)
                {
                    ResetAnimation();
                }
                else
                {
                    transform.DOLocalRotate(getRotation(Paths[currentPath-1], Paths[currentPath]), 0.2f, RotateMode.Fast);
                }
            }
        }
    }

    Vector3 getRotation(Vector3 from, Vector3 to)
    {
        Vector3 rot = Vector3.zero;

        if (from.y == to.y)
        {
            if (from.x > to.x)
            {
                rot = new Vector3(0f, 0f, -90f);
            }
            else if (from.x < to.x)
            {
                rot = new Vector3(0f, 0f, 90f);
            }
        }
        else if (from.x == to.x)
        {
            if (from.y > to.y)
            {
                rot = new Vector3(0f, 0f, 0f);
            }
            else if (from.y < to.y)
            {
                rot = new Vector3(0f, 0f, 180f);
            }
        }
        return rot;
    }
}
