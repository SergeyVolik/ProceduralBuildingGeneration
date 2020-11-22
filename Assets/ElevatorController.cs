using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField]
    AudioSource liftSound;
    [SerializeField]
    AudioClip listOpen;
    [SerializeField]
    AudioClip LiftMoving;
    [SerializeField]
    AudioClip liftStoped;

    [SerializeField]
    public int m_floors;

    [SerializeField]
    public float m_floorHight = 2.5f;

    [SerializeField]
    public int m_currentFloor = 1;

    bool moveToUp = true;
    public void SetSettings(int floors, int currentFloor, float floorHight = 2.5f)
    {
        m_floors = floors;
        m_floorHight = floorHight;
        m_currentFloor = currentFloor;
    }

    void Awake()
    {
        liftSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ToNextFloor();
    }

    private void ToNextFloor()
    {
        float nextY;
        if (m_currentFloor == 1)
        {
            moveToUp = true;
        }
        else if (m_currentFloor == m_floors)
        {
            moveToUp = false;
        }
        if (moveToUp)
        {
            m_currentFloor++;
            nextY = transform.position.y + m_floorHight;
        }
        else {
            nextY = transform.position.y - m_floorHight;
            m_currentFloor--;

        }

        liftSound.clip = LiftMoving;
        liftSound.loop = true;
        liftSound.Play(0);
      
        transform.DOMoveY(nextY, 2f).OnComplete(() => {
            liftSound.Stop();
            liftSound.loop = false;
            liftSound.clip = liftStoped;
            liftSound.Play();
        });
    }
}
