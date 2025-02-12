using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Linq;

namespace Utilities
{
	public static class Utils
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="customMessage"></param>
		public static void PrintHi(string customMessage)
		{
			Debug.Log("Hi!" + customMessage);
		}
		
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
		public static void PrintCollection(this ICollection collection)
		{
			foreach (var item in collection) 
				Debug.Log(item.ToString());
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
        /// Clamps an angle between a minimum and maximum value, handling wrap-around at 360 degrees.
        /// </summary>
        /// <param name="angle">The angle to clamp.</param>
        /// <param name="min">The minimum angle.</param>
        /// <param name="max">The maximum angle.</param>
        /// <returns>The clamped angle.</returns>
        public static float ClampAngle(float angle, float min, float max)
        {
            angle = angle % 360;
            if (angle < -360) angle += 360;
            if (angle > 360) angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }


        /// <summary>
        /// Randomly shuffles the elements of a list in place using the Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        public static void ShuffleList<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        /// <summary>
        /// Remaps a value from one range to another.
        /// </summary>
        /// <param name="value">The value to remap.</param>
        /// <param name="fromMin">The minimum of the original range.</param>
        /// <param name="fromMax">The maximum of the original range.</param>
        /// <param name="toMin">The minimum of the target range.</param>
        /// <param name="toMax">The maximum of the target range.</param>
        /// <returns>The remapped value.</returns>
        public static float RemapValue(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        }



    }
}
