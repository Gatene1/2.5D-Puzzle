using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform _waypointUp, _waypointDown;
    [SerializeField] private float _speed = 5.0f;

   private bool _goingDown = false;

   public void CallElevator()
    {
        _goingDown = !_goingDown;
    }    

    private void FixedUpdate()
    {
        if (_goingDown)        
            transform.position = Vector3.MoveTowards(transform.position, _waypointDown.transform.position, _speed * Time.deltaTime);
        else if (!_goingDown)
            transform.position = Vector3.MoveTowards(transform.position, _waypointUp.transform.position, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }


}
