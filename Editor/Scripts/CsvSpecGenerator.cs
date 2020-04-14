using SpecSheetSys;
using SpecSheetSys.Examples;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Compilation;
using UnityEngine;

namespace SpecSheetGenSys.Editor {

	public class CsvSpecGenerator : IPostprocessBuildWithReport {
		public const string AssemblyString = "Assembly-CSharp";

		public int callbackOrder => 0;

		public void OnPostprocessBuild(BuildReport report) {

			//var assemblies = CompilationPipeline.GetAssemblies(AssembliesType.Player)
			//	.Select(a => System.Reflection.Assembly.Load(a.name));
			//var assemblies = new System.Reflection.Assembly[] {
			//	System.Reflection.Assembly.Load(AssemblyString),
			//	GetType().Assembly
			//};
			var assemblies = GetType().Assembly.GetReferencedAssemblies()
				.Select(a => System.Reflection.Assembly.Load(a.Name));

			var iter = assemblies
				.SelectMany(a => a.GetTypes())
				.SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
				.Where(m => m.GetCustomAttribute<SpecSheetAttribute>() != null)
				.Where(m => typeof(IEnumerable<IList<object>>).IsAssignableFrom(m.ReturnType))
				.Where(m => m.GetParameters().Length == 0)
				;

			var folder = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "/Docs/");
			foreach (var m in iter) {
				var attr = m.GetCustomAttribute<SpecSheetAttribute>();
				var path = Path.Combine(folder, attr.FileName);
				Debug.Log($"path:{path}");
				Directory.GetParent(path).Create();

				using (var w = File.CreateText(path)) {
					foreach (var r in (IEnumerable<IList<object>>) m.Invoke(null, null)) {
						foreach (var c in r)
							w.Write($"{c},\t");
						w.WriteLine();
					}
				}
			}
		}
	}
}
