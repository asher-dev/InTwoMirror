﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerID { P1, P2, Both };

public class CharacterControl : MonoBehaviour
{

    public float movementSpeed = 30f;
    // max distance player must be to interact with object
    public float maxActionDistance = 10f;

    public readonly PlayerID player;

    // the closest actionable object to the player
    private GameObject actionable;

    // Secondary audio for this player
    private AudioSource audioSource;
    // Currently selected Camelot tone to play
    private int currCamelot;
    private List<AudioClip> camelotList;
    private PlayerID player;
    private Collider proximity = null;
    private GameObject otherPlayer;


    public int HealthPoints;

    // Current game board region of the player
    private GameObject currentRegion;

    // Use this for initialization
    void Start()
    {
        actionable = null;
        audioSource = GetComponent<AudioSource>();
        currCamelot = 2;

        // Add tones to the list
        camelotList = new List<AudioClip>();
        camelotList.Add(CamelotTone0);
        audioSource.clip = camelotList[0];
        // identify this and other player
        if (name == "Player1")
        {
            player = PlayerID.P1;
            otherPlayer = GameObject.Find("Player2");
        }
        else if(name == "Player2")
        {
            player = PlayerID.P2;
            otherPlayer = GameObject.Find("Player1");
        } else
        {
            player = PlayerID.Both;
        }
        //player = name == "Player1" ? PlayerID.P1 : name == "Player2" ? PlayerID.P2 : PlayerID.Both;
        if (player == PlayerID.Both) throw new System.Exception("Invalid player name for control script");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis(player.ToString() + "Horizontal"), 0f, Input.GetAxis(player.ToString() + "Vertical"));
        if (movement.magnitude > 0) {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(movement * movementSpeed);
        }
    }

    void Update() {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger && other.CompareTag("Interactive")) {
            GetComponentInChildren<SoundControlScriptPd>().Interactive = other;
            actionable = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == actionable) {
            actionable = null;
            GetComponentInChildren<SoundControlScriptPd>().Interactive = null;
        }
    }

    // Find the closest interactive object
    // From https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html
    // Not currently in use
    GameObject FindClosestInteractive()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Interactive");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < maxActionDistance && curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    // Checks distance between this and other player
    // If within distance and both players make noise, will heal 1HP
    private void CoopHeal()
    {
        Vector3 distance = otherPlayer.transform.position - transform.position;
        if(distance.sqrMagnitude < maxActionDistance)
        {
            // If players are within MaxActionDistance...
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger && other.CompareTag("Interactive")) {
            GetComponentInChildren<SoundControlScriptPd>().Interactive = other;
            proximity = other;
        }
        if (other.CompareTag("Volatile")) // Take damage from vola(tiles)^2
        {
            HealthPoints--;
            Debug.Log(name + " HP = " + HealthPoints);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == proximity) {
            proximity = null;
            GetComponentInChildren<SoundControlScriptPd>().Interactive = null;
        }
    }
}
