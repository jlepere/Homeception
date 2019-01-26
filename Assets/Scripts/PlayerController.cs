﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
  [SerializeField] private float _playerAccSpeed = 5f;
  [SerializeField] private float _playerDecSpeed = 0.8f;
  [SerializeField] private float _playerMaxSpeed = 10f;
  [SerializeField] private Camera _camera;

  private Animator[] _animator;
  private Rigidbody _playerRigidBody;
  private Vector3 _playerMovement;
  private Dictionary<string, GameObject> _pickupObjects;
  private NavMeshAgent _navMeshAgent;

  private void Awake() {
    _animator = GetComponentsInChildren<Animator>();
    _playerRigidBody = GetComponent<Rigidbody>();
    _navMeshAgent = GetComponent<NavMeshAgent>();
    _playerMovement = Vector3.zero;
    _pickupObjects = new Dictionary<string, GameObject>();
  }

  private void Update()
  {
    _playerMovement = new Vector3(
      (Input.GetAxis("Horizontal") > 0.2f || Input.GetAxis("Horizontal") < -0.2f ? Input.GetAxis("Horizontal") : 0),
      0,
      (Input.GetAxis("Vertical") > 0.2f || Input.GetAxis("Vertical") < -0.2f ? Input.GetAxis("Vertical") : 0)
    );

    var velocity = _playerRigidBody.velocity;
    var position = transform.position;
    Debug.DrawLine(position, position - new Vector3(
                               velocity.x,
                               0,
                               velocity.z
                             ),
      Color.blue);
    _camera.transform.position = position;

    foreach (Animator animator in _animator) {
      animator.SetBool("isWalking", velocity.sqrMagnitude > 0.1f);
    }
  }

  private void FixedUpdate()
  {
    if (Math.Abs(_playerMovement.x) > 0.02f || Math.Abs(_playerMovement.z) > 0.02f)
      _playerRigidBody.velocity = _playerMovement * _playerAccSpeed;
    else
      _playerRigidBody.velocity *= _playerDecSpeed;
    _playerRigidBody.velocity = Vector3.ClampMagnitude(_playerRigidBody.velocity, _playerMaxSpeed);
  }

<<<<<<< HEAD
    void    OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Door")) {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Door door = other.gameObject.GetComponent<Door>();
                door.setIsOpen(true);
            }
        }
    }

    public Dictionary<string, GameObject> GetPickUpObjects()
=======
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Pick Up"))
>>>>>>> 722bf1dfbab7687abe7065be8b0b8cd70daaa88a
    {
      GameObject objectToPickUp = other.gameObject;
      objectToPickUp.SetActive(false);
      objectToPickUp.transform.SetParent(transform.parent.Find("Inventory"));
      _pickupObjects.Add(objectToPickUp.name[0].ToString(), objectToPickUp);
    }
    else if (other.gameObject.CompareTag("Bed")) {
      Transform nightLight = other.gameObject.transform.Find("Night Light");
      if (nightLight)
        _navMeshAgent.SetDestination(nightLight.gameObject.transform.position);
    }
  }

  public Dictionary<string, GameObject> GetPickUpObjects()
  {
    return _pickupObjects;
  }
}
