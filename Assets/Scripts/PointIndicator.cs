using UnityEngine;
using System.Collections;
using System;

public class PointIndicator : MonoBehaviour
{

    Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Use this for initialization
    void Start()
    {

    }

    void PlayAnim()
    {
        if (anim != null)
            anim.SetTrigger("Play");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayAnim();
        }
    }

    public void OnDisable()
    {
        PlayerController.OnPointSelected -= PointClicked;
    }

    public void OnEnable()
    {
        PlayerController.OnPointSelected += PointClicked;
    }

    private void PointClicked(Vector3 pos)
    {
        transform.localPosition = pos;
        PlayAnim();
    }
}
