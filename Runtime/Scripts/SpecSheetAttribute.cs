using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecSheetSys {

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class SpecSheetAttribute : Attribute {

		public const string DEFAULT_FILENAME = "SpecSheet_Noname";

		public SpecSheetFormat format;
		public string filename;

		public SpecSheetAttribute(
			string filename,
			SpecSheetFormat format = SpecSheetFormat.CSV
			) {

			this.format = format;
			this.filename = filename;
		}

		public string FileName {
			get {
				return (string.IsNullOrEmpty(filename)
					? DEFAULT_FILENAME
					: filename)
					+ $".{format.ToString().ToLower()}";
			}
		}
	}
}
