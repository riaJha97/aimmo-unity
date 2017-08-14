﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace UnityTest
{
	[TestFixture]
	internal class IsometricPositionTests
	{
		[Test]
		public void ZeroPositionDoesntChange()
		{
			GameObject empty = new GameObject();
			IsometricPosition position = empty.AddComponent<IsometricPosition>();
		
			position.Set(0.0f, 0.0f, 0.0f);

			Assert.AreEqual(position.x, 0.0f);
			Assert.AreEqual(position.y, 0.0f);
		}
	}
}