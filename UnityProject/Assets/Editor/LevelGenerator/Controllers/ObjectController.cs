﻿using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ObjectController
{
	/**
	 * An object controller responsible for providing a Singleton
	 * instance for KeyListener and Context. The Context is used to 
	 * attach objects to it; the Context can be used as:
	 *   ObjectController.GetContext.AddComponent<*component-class-name*>();
	 * 
	 * It also provides a way to get the currently selected game object.
	 */
	private static string contextName = "Level Generator Context";
	private static KeyListener keyListener = new KeyListener();
	private static bool lightSelection = false;

	private static List<GameObject> selectedGameObjects = new List<GameObject>();

	public static GameObject GetContext()
	{
		GameObject go = GameObject.Find(contextName);
		if (go == null) 
			return new GameObject(contextName);
		
		return go;
	}

	public static void Move(float x, float y)
	{
		GameObject[] gameObjects = GetGameObjects();

		foreach (GameObject gameObject in gameObjects) 
		{
			IsometricPosition position = gameObject.GetComponent<IsometricPosition>();
			position.Set(position.x + x, position.y + y);
		}
	}

	public static void SwitchLightSelection()
	{
		lightSelection = !lightSelection;
	}

	public static bool GetLightSelection()
	{
		return lightSelection;
	}

	public static void MoveLight(float x, float y)
	{
		GameObject[] gameObjects = GetGameObjects();

		foreach (GameObject gameObject in gameObjects) 
		{
			Debug.Log (x.ToString ("0.00") + " " + y.ToString ("0.00"));
			Light lightObject = gameObject.GetComponentInChildren<Light>();

			Debug.Log (lightObject);
			if (lightObject == null) 
			{
				continue;
			}

			GameObject lightAttachedTo = lightObject.gameObject;
			Debug.Log (lightAttachedTo);

			Vector3 position = lightAttachedTo.transform.position;
			lightAttachedTo.transform.position = new Vector3(position.x + x, position.y + y, position.z);
		}
	}
		
	// There is only one static key listener on ObjectController,
	// but more can be registered.
	public static KeyListener GetKeyListener()
	{
		return keyListener;
	}

	public static IsometricPosition GetPosition()
	{
		// TEMPORARY
		return GetGameObjects()[0].GetComponent<IsometricPosition> ();
	}

	public static GameObject[] GetGameObjects()
	{
		List<GameObject> previousSelection = new List<GameObject>(selectedGameObjects);
		selectedGameObjects.Clear();

		foreach (GameObject gameObject in Selection.gameObjects) 
		{
			selectedGameObjects.Add(gameObject);
			previousSelection.Remove(gameObject);

			SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();

			if (renderer != null) 
			{
				Color color = renderer.color;
				color.a = 0.5f;

				gameObject.GetComponent<SpriteRenderer>().color = color;
			}

		}

		// Handle those not selected anymore.
		foreach (GameObject gameObject in previousSelection) 
			if (gameObject != null) 
			{
				SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
				if (renderer != null)
				{
					Color color = renderer.color;
					color.a = 1.0f;

					renderer.color = color;
				}
			}	
				
		return Selection.gameObjects;
	}

	public static bool SelectedGameObject()
	{
		return Selection.gameObjects.Length > 0;
	}
}

