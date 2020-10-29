using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
    [SerializeField] private Material _buttonMat;

    private Material _originalButtonMat;

    [SerializeField] private int _coinsToCollect = 8;

    private Player _player;
    private MeshRenderer _mr;
    private Elevator _elevator;

    private bool _insideElevatorPanel = false;

    private int _numCoins;

    private void Start()
    {
        _mr = GameObject.Find("Call_Button").GetComponent<MeshRenderer>();
        _elevator = GameObject.Find("Elevator").GetComponent<Elevator>();

        _originalButtonMat = _mr.material;

        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        _numCoins = _player.GetCoins();

        if (Input.GetKeyDown(KeyCode.E) && _numCoins >= _coinsToCollect && _insideElevatorPanel)
        {
            if (_mr.material == _originalButtonMat)
                _mr.material = _buttonMat;
            else
                _mr.material = _originalButtonMat;

            _elevator.CallElevator();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")       
           _insideElevatorPanel = true;        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            _insideElevatorPanel = false;
    }
}
