﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour{

	public static CameraMover cTrack; //cFollow
	public float dampTime = 0.15f; //figure out what this is
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	public float midX;
	public float midY;
	public float midZ;

	public Transform player1; //target1
	public Transform player2; //target2

	public Vector3 midPoint;
	public Vector3 distance;
	public float camDist;

	public float camOffset;
	public float bounds;

	Camera camera;


	void Awake(){ //Startup of the game
	
		if (cTrack == null) {
			DontDestroyOnLoad (gameObject);
			cTrack = this;

		} else if (cTrack != this) {
			Destroy (gameObject);
		}
	}


	void Start(){
		camDist = 15.0f;
		bounds = 12.0f;
		camera = GetComponent<Camera>();
	}

	void Update(){

		distance = player1.position - player2.position;
		if (camDist >= 19.0f) {
			camDist = 19.0f;
		}
		if (camDist <= 10.0f) {
			camDist = 10.0f;
		}
		if (distance.x < 0) {
			distance.x = distance.x * -1;
		}
		if (distance.z < 0) {
			distance.z = distance.z * -1;
		}

		if (player1.position.x < (transform.position.x - bounds)) {
			Vector3 pos = player1.position;
			pos.x = transform.position.x - bounds;
			player1.position = pos;
		}

		if (player1.position.x >(transform.position.x + bounds)) {
			Vector3 pos = player1.position;
			pos.x = transform.position.x + bounds;
			player1.position = pos;
		}

		if (player2.position.x < (transform.position.x - bounds)) {
			Vector3 pos = player2.position;
			pos.x = transform.position.x - bounds;
			player2.position = pos;
		}

		if (player2.position.x > (transform.position.x + bounds)) {
			Vector3 pos = player2.position;
			pos.x = transform.position.x + bounds;
			player2.position = pos;
		}

		if (distance.x > 15.0f) {
			camOffset = distance.x * 0.3f;
			if (camOffset >= 8.5f) {
				camOffset = 8.5f;
			}
		} else if (distance.x < 14.0f) {
			camOffset = distance.x * 0.3f;
		} else if (distance.z < 14.0f) {
			camOffset = distance.x * 0.3f;
		}

		midX = (player2.position.x + player1.position.x) /2; 
		midY = (player2.position.y + player1.position.y) /2;
		midZ = (player2.position.z + player1.position.z) /2;

		midPoint = new Vector3 (midX, midY, midZ);

		if (player1) {
			Vector3 point = camera.WorldToViewportPoint(midPoint);
			Vector3 delta = midPoint - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.7f, camDist + camOffset)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}

	}

}