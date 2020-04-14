using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecSheetSys.Examples {

	public static class SampleSpecSheet {

		[SpecSheet("Sample01")]
		public static IEnumerable<object[]> Table() {
			yield return new object[]{ 0, 1 };
			yield return new object[]{ 2, 2 };
		}
	}
}
