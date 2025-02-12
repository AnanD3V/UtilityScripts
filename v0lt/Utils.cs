using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Utilities
{
	public static class Utils
	{
		/// <summary>
		/// Generates a random integer within a range
		/// </summary>
		/// <param name="min">Minimum range inclusive</param>
		/// <param name="max">Maximum range inclusive</param>
		/// <returns>A random interger</returns>
		public static int RandomIntInclusive(int min, int max) => Random.Range(min, max + 1);

		/// <summary>
		/// Generates a random number thats always different from the last chosen number
		/// </summary>
		/// <param name="min">Minimum range inclusive</param>
		/// <param name="max">Minimum range exclusive</param>
		/// <param name="lastChosenNumber">Reference to set the last chosen number</param>
		/// <returns>A random different number</returns>
		public static int RandomRangeNoDupe(int min, int max, ref int lastChosenNumber)
		{
			int generatedNumber = lastChosenNumber;

			while (generatedNumber == lastChosenNumber) 
				generatedNumber = Random.Range(min, max);

			lastChosenNumber = generatedNumber;

			return generatedNumber;
		}

		/// <summary>
		/// Generates a random Vector3 within a range
		/// </summary>
		/// <param name="min">Minimum range inclusive</param>
		/// <param name="max">Maximum range inclusive</param>
		/// <param name="excludeX">Don't generate X value</param>
		/// <param name="excludeY">Don't generate Y value</param>
		/// <param name="excludeZ">Don't generate Z value</param>
		/// <returns>A random Vector3</returns>
		public static Vector3 RandomVector(float min, float max, bool excludeX = false, bool excludeY = false, bool excludeZ = false) => new(excludeX ? 0f : Random.Range(min, max), excludeY ? 0f : Random.Range(min, max), excludeZ ? 0f : Random.Range(min, max));

		/// <summary>
		/// Generates a random number between the X and Y values of Vector2, inclusive
		/// </summary>
		/// <param name="vector">The Vector2 to get the range from</param>
		/// <returns>A random float</returns>
		public static float RandomFromVector2(Vector2 vector) => Random.Range(vector.x, vector.y);

		/// <summary>
		/// Generates a random point on a navmesh
		/// </summary>
		/// <param name="origin">The origin to generate the random position from</param>
		/// <param name="range">How far from the origin to generate the point</param>
		/// <param name="areaMask">Navmesh surface area mask</param>
		/// <returns>A Vector3 with the random position</returns>
		public static Vector3 RandomNavmeshPoint(Vector3 origin, float range, int areaMask)
		{
			var randomDirection = Random.insideUnitSphere * range;

			randomDirection += origin;

			NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, range, areaMask);

			return navHit.position;
		}

		/// <summary>
		/// Prints the items of a collection
		/// </summary>
		/// <param name="collection">The collection</param>
		public static void PrintCollection(ICollection collection)
		{
			foreach (var item in collection) 
				Debug.Log(item.ToString());
		}

		/// <summary>
		/// Checks a list of structs for duplicate items, the struct must implement IEquatable
		/// </summary>
		/// <typeparam name="T">The struct implementing IEquatable</typeparam>
		/// <param name="structList">The list to check</param>
		/// <param name="foundMaches">The amount of matches found</param>
		/// <returns>True if duplicates are found</returns>
		public static bool CheckStructListDuplicate<T>(List<T> structList, out int foundMaches) where T : struct, IEquatable<T>
		{
			if (structList.IsNullOrEmpty())
			{
				foundMaches = 0;
				return false;
			}

			var dupeDataList = new List<T>();

			dupeDataList = structList.FindAll((T structData) => {

				int matchCount = 0;

				foreach (var data in structList)
				{
					if (structData.Equals(data)) 
						matchCount++;
				}

				return matchCount > 1;
			});

			foundMaches = dupeDataList.Count;

			return dupeDataList.Count > 1;
		}

		/// <summary>
		/// Gets the closest number to the target from a collection of values
		/// </summary>
		/// <param name="collection">The collection to get the values from</param>
		/// <param name="target">The target to find the closest number to</param>
		/// <returns>The closest number to the target</returns>
		public static int GetClosestNumber(this int[] collection, int target)
		{
			float[] floatArray = new float[collection.Length];
			Array.Copy(collection, floatArray, collection.Length);

			return Mathf.RoundToInt(GetClosestNumber(floatArray, target));
		}

		/// <summary>
		/// Gets the closest number to the target from a collection of values
		/// </summary>
		/// <param name="collection">The collection to get the values from</param>
		/// <param name="target">The target to find the closest number to</param>
		/// <returns>The closest number to the target</returns>
		public static float GetClosestNumber(this float[] collection, float target)
		{
			float closest = collection[0];
			float closestDifference = Mathf.Abs(closest - target);

			foreach (var number in collection)
			{
				float currentDifference = Mathf.Abs(number - target);
				if (currentDifference < closestDifference)
				{
					closest = number;
					closestDifference = currentDifference;
				}
			}

			return closest;
		}

		/// <summary>
		/// Checks if the <paramref name="collection"/> is null or empty
		/// </summary>
		/// <param name="collection">The collection to check</param>
		/// <returns>True if is null or empty, false if not</returns>
		public static bool IsNullOrEmpty(this ICollection collection) => collection == null || collection.Count == 0;

		/// <summary>
		/// Gets the coordinates of a two dimensional array
		/// </summary>
		/// <typeparam name="T">Array type</typeparam>
		/// <param name="matrix">The 2D array</param>
		/// <param name="value">The array value you want to get the coordinates from</param>
		/// <returns>A tuple containing the coordinates of the array</returns>
		public static (int row, int column) CoordinatesOf<T>(this T[,] matrix, T value)
		{
			int row = matrix.GetLength(0);
			int collumn = matrix.GetLength(1);

			for (int x = 0; x < row; ++x)
			{
				for (int y = 0; y < collumn; ++y)
				{
					if (matrix[x, y].Equals(value))
						return (x, y);
				}
			}

			return (-1, -1);
		}

		/// <summary>
		/// Enables a single object from a list of objects
		/// </summary>
		/// <param name="objects">Array of objects to toggle from</param>
		/// <param name="index">Object index to toggle on</param>
		public static void ToggleObject(GameObject[] objects, int index)
		{
			foreach (var @object in objects) 
				@object.SetActive(false);

			objects[index].SetActive(true);
		}

		/// <summary>
		/// Tries to get a component from the parent of the currentObject
		/// </summary>
		/// <typeparam name="T">The type of the component</typeparam>
		/// <param name="component">Target Component</param>
		/// <param name="currentObject">The current object to check the parents from</param>
		/// <param name="parentComponent">The desired component</param>
		/// <returns>True if the component was found, False otherwise</returns>
		public static bool TryGetComponentInParent<T>(this Component component, Transform currentObject, out T parentComponent) where T : Component
		{
			var currentParent = currentObject.transform.parent;

			while (currentParent != null)
			{
				if (currentParent.TryGetComponent(out T foundComponent))
				{
					parentComponent = foundComponent;
					return true;
				}

				currentParent = currentParent.parent;
			}

			parentComponent = null;
			return false;
		}

		/// <summary>
		/// Execute logic once
		/// </summary>
		/// <param name="logic">The logic to execute</param>
		/// <param name="wasExecuted">Set the referenced bool to false to execute again</param>
		public static void DoOnce(Action logic, ref bool wasExecuted)
		{			
			if (!wasExecuted)
			{
				wasExecuted = true;
				logic?.Invoke();
			}
		}
	}
}
