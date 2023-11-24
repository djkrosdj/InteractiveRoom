using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс управления игрой
/// </summary>
public class GameController : MonoBehaviour
{
    private const int MAX_ITEMS_IN_HOLDER = 1;

    [SerializeField] private RayFromCamera _rayFromCamera;
    [SerializeField] private float _rayLength;
    [SerializeField] private Transform _playerHands;
    [SerializeField] private float _throwForce;

    private InteractableItem _lastHighlightedObject;
    private Door _door;
    private bool _handsIsFree;

    private void Awake()
    {
        _handsIsFree = true;
    }

    private void Update()
    {
        var playerCameraRay = _rayFromCamera.Ray(_rayLength);

        if (playerCameraRay.HasValue)
        {
            SwitchDoor(playerCameraRay);
            ActionsWithInteractableObjects(playerCameraRay);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_handsIsFree==false)
                ThrowItemForward();
        }
    }

    private void SwitchDoor(RaycastHit? ray)
    {
        if (ray.Value.transform.CompareTag("Door"))
        {
            _door = ray.Value.transform.GetComponent<Door>();

            if (Input.GetKeyDown(KeyCode.E))
            {
                _door.SwitchDoorState();
            }
        }
    }

    private void ActionsWithInteractableObjects(RaycastHit? ray)
    {
        if (ray.Value.transform.CompareTag("InteractableObject"))
        {
            _lastHighlightedObject = ray.Value.transform.GetComponent<InteractableItem>();
            _lastHighlightedObject.SetFocus();
            TakeItem(ray);
        }

        else
        {
            if (_lastHighlightedObject != null)
            {
                _lastHighlightedObject.RemoveFocus();
            }
        }
    }

    private void TakeItem(RaycastHit? ray)
    {
        InteractableItem item = ray.Value.transform.GetComponent<InteractableItem>();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_playerHands.childCount == 0)
            {
                item.transform.SetParent(_playerHands);
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.transform.position = _playerHands.position;
            }

            else if (_playerHands.childCount == 1)
            {
                _playerHands.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                _playerHands.transform.GetChild(0).transform.SetParent(null, true);
                item.transform.SetParent(_playerHands);
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.transform.position = _playerHands.position;
            }

            _handsIsFree = false;
        }
    }

    private void ThrowItemForward()
    {
        _playerHands.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
        _playerHands.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(transform.forward * _throwForce);
        _playerHands.transform.GetChild(0).transform.SetParent(null, true);
        _handsIsFree = true;
    }
}