﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Peng;

namespace Scott
{
    public class GravityGun : MonoBehaviour
    {
        private bool pickupActive;
        RaycastResult rayHit;
        private GameObject grabTarget;
        private bool fireDown;
        public float gravDistance;
        public float grabbingDistance;
        public GameObject cam;
        public GameObject gravPoint;
        public float maxSnapDistance;
        public GameObject firePart;

        void Update()
        {
            switch (GetComponent<Player>().Mode) {
                case GameStates.TITLE:
                    break;
                case GameStates.PLAY:
                    UpdateGameplay();
                    break;
            }
        }

        public void autoDrop()
        {
            pickupActive = false;
            grabTarget.GetComponent<InteractiveObject>().drop();
            grabTarget = null;
        }

        public void grabbedDestroyed()
        {
            pickupActive = false;
            grabTarget = null;
        }

        private void UpdateGameplay() {
            gravPoint.transform.position = cam.transform.position + cam.transform.forward * gravDistance;
            if (Input.GetAxis("Fire1") < 1 && fireDown) {
                fireDown = false;
            }
            if (Input.GetAxis("Fire1") > 0 && !pickupActive && !fireDown) {
                if (firePart.activeInHierarchy) {
                    firePart.SetActive(false);
                }
                firePart.SetActive(true);
                fireDown = true;
                Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, grabbingDistance)) {
                    if (hit.collider.gameObject.GetComponent<InteractiveObject>() != null) {
                        pickupActive = true;
                        grabTarget = hit.collider.gameObject;
                        grabTarget.GetComponent<InteractiveObject>().pickup(gravPoint, this);
                    }
                }
            } else if (Input.GetAxis("Fire1") > 0 && pickupActive && !fireDown) {
                fireDown = true;
                pickupActive = false;
                grabTarget.GetComponent<InteractiveObject>().drop();
                grabTarget = null;
            }
        }
    }
}