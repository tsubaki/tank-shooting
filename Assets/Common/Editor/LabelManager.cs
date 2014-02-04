/*
The MIT License (MIT)

Copyright (c) 2013 yamamura tatsuhiko

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TW
{
	public class LabelManager
	{
#region CallByEditor
		[MenuItem("Edit/Label/UpdateAllSceneLabel")]
		static void AddAllLabel ()
		{
			using (var progress = new EditorProgress("UpdateAllSceneLabel")) {
				progress.UpdateProgress ("get all file", 0);
				var all = FileUtilExtra.allAssets;

				progress.UpdateProgress ("collect scene file", 0.5f);
				var sceneList = GetAllSceneFile (all);

				int currentSceneCount = 1;
				progress.progressMax = sceneList.Count;

				foreach (var asset in sceneList) {
					var scene = new Object[] {asset};
					string label = CreateLabelName (asset.name);
					var sceneReferenceList = new List<Object> (EditorUtility.CollectDependencies (scene));

					progress.UpdateProgress (
					string.Format (" update label fileName:{0} items count:{1}", asset.name, sceneReferenceList.Count),
					currentSceneCount ++);
			
					foreach (var obj in all) { 
						if (sceneReferenceList.Contains (obj)) {
							AddLabel (obj, label);
						} else {
							RemoveLabel (obj, label);
						}
					}
					AddLabel (asset, label);
				}
			} 
		}

		[MenuItem("Edit/Label/RemoveAllSceneLabel")]
		static void RemoveAllLabel ()
		{
			using (var progress = new EditorProgress("RemoveAllLabel")) {
				progress.UpdateProgress ("get all file", 0);
				var all = FileUtilExtra.allAssets;

				progress.UpdateProgress ("collect scene file", 0.5f);
				var sceneList = GetAllSceneFile (all);

				int currentSceneCount = 1;
				progress.progressMax = sceneList.Count;

				foreach (var asset in sceneList) {

					string label = CreateLabelName (asset.name);
					var sceneReferenceList = new List<Object> (EditorUtility.CollectDependencies (new Object[] {asset}));
			
					progress.UpdateProgress (
					string.Format (" update label fileName:{0} items count:{1}", asset.name, sceneReferenceList.Count),
					currentSceneCount ++);

					foreach (var obj in all) {
						RemoveLabel (obj, label);
					}
					RemoveLabel (asset, label);
				}
			}
		}

		[MenuItem("Assets/Label/AddDirectoryLabel")]
		static void AddLbelDir ()
		{
			using (var progress = new EditorProgress("AddDirectoryLabel")) {
				progress.UpdateProgress ("add labels", 0);
				string label = CreateLabelName (Selection.activeObject.name);

				var current = FileUtilExtra.currentAssets;
				progress.progressMax = current.Count;

				foreach (var obj in current) {
					AddLabel (obj, label);
				}
			}
		}

		[MenuItem("Assets/Label/RemoveDirectoryLabel")]
		static void RemoveDirLabel ()
		{
			using (var progress = new EditorProgress("RemoveDirecotryLabel")) {
				progress.UpdateProgress ("remove labels", 0);
				string label = CreateLabelName (Selection.activeObject.name);
		
				var current = FileUtilExtra.currentAssets;
				progress.progressMax = current.Count;

				foreach (var obj in current) {
					RemoveLabel (obj, label);
				}
			}
		}

		[MenuItem("Assets/Label/RemoveSceneLabel")]
		static void RemoveSceneLabel ()
		{
			using (var progress = new EditorProgress("RemoveSceneLabel")) {

				progress.UpdateProgress ("collect scene file", 0.5f);
				var sceneList = GetAllSceneFile (Selection.objects);

				int currentSceneCount = 1;
				progress.progressMax = sceneList.Count;


				foreach (var asset in sceneList) {

					progress.UpdateProgress (string.Format (" update label {0}", asset.name), currentSceneCount ++);
					Debug.Log (string.Format (" update label {0}", asset.name));

					var scene = new Object[] {asset};
					string label = CreateLabelName (asset.name);

					var sceneReferenceList = new List<Object> (EditorUtility.CollectDependencies (scene));
					foreach (var obj in sceneReferenceList) {
						RemoveLabel (obj, label);
					}
					RemoveLabel (asset, label);
				}
			}
		}

		[MenuItem("Assets/Label/UpdateSceneLabel")]
		static void AddSceneLabel ()
		{
			using (var progress = new EditorProgress("UpdateSceneLabel")) {
			
				progress.UpdateProgress ("collect scene file", 0.5f);

				var sceneList = GetAllSceneFile (Selection.objects);
				int currentSceneCount = 1;
				progress.progressMax = sceneList.Count;

				var all = FileUtilExtra.allAssets;
			
				foreach (var asset in sceneList) {

					var scene = new Object[] {asset};
					var sceneReferenceList = new List<Object> (EditorUtility.CollectDependencies (scene));

					string label = CreateLabelName (asset.name);

					progress.UpdateProgress (
					string.Format (" update label fileName:{0} items count:{1}", asset.name, sceneReferenceList.Count),
					currentSceneCount ++);
				
					foreach (var obj in all) { 
						if (sceneReferenceList.Contains (obj)) {
							AddLabel (obj, label);
						} else {
							RemoveLabel (obj, label);
						}
					}
					AddLabel (asset, label);
				}
			}
		}

	#endregion

	#region Util



		private static List<Object> GetAllSceneFile (IList<Object> all)
		{		
			List<Object> sceneList = new List<Object> ();
			foreach (var asset in all) {
				if (Path.GetExtension (AssetDatabase.GetAssetPath (asset)).Equals (".unity")) {
					sceneList.Add (asset);
				}
			}
			return sceneList;
		}

		public static string CreateLabelName (string label)
		{
			return label.Replace (" ", "");
		}

		public static void RemoveLabel (Object obj, string label)
		{
			var labels = AssetDatabase.GetLabels (obj);
			List<string> labelList = new List<string> (labels);
			if (labelList.Contains (label)) {
				labelList.Remove (label);
			}
			AssetDatabase.SetLabels (obj, labelList.ToArray ());
		}
	
		public static void AddLabel (Object obj, string label)
		{
			var labels = AssetDatabase.GetLabels (obj);
			List<string> labelList = new List<string> (labels);
			if (!labelList.Contains (label)) {
				labelList.Add (label);
			}
			AssetDatabase.SetLabels (obj, labelList.ToArray ());
		}
	
	#endregion
	}

	class EditorProgress : System.IDisposable
	{
		public EditorProgress (string methodName)
		{
			this.methodName = methodName;
		}

		public string methodName = string.Empty;
		public float progressMax = 0;

		public void UpdateProgress (string text, float current)
		{
			EditorUtility.DisplayProgressBar (methodName,
		                                 text,
		                                  current / progressMax);
		}

		public void Dispose ()
		{
			EditorUtility.ClearProgressBar ();
		}
	}
	
	/// <summary>
	/// File util extra.
	/// 	// code copy from http://wiki.unity3d.com/index.php?title=UnityAssetXrefs
	/// </summary>
	class FileUtilExtra
	{
	
		public static List<Object> allAssets {
			get {
				List<FileInfo> files = DirSearch (new DirectoryInfo (Application.dataPath), "*.*");
				List<Object> assetRefs = new List<Object> ();
				foreach (FileInfo fi in files) {
					if (fi.Name.StartsWith ("."))
						continue; // Unity ignores dotfiles.
					assetRefs.Add (AssetDatabase.LoadMainAssetAtPath (getRelativeAssetPath (fi.FullName)));
				}
				return assetRefs;
			}
		}

		public static List<Object> currentAssets {
			get {
				string path = AssetDatabase.GetAssetPath (Selection.activeObject);
				List<Object> assetRefs = new List<Object> ();

				if (!Directory.Exists (path)) {
					Debug.LogWarning ("file selected");
					return assetRefs;
				}

				List<FileInfo> files = DirSearch (new DirectoryInfo (path), "*.*");
				foreach (FileInfo fi in files) {
					if (fi.Name.StartsWith ("."))
						continue; // Unity ignores dotfiles.
					assetRefs.Add (AssetDatabase.LoadMainAssetAtPath (getRelativeAssetPath (fi.FullName)));
				}
				return assetRefs;
			}
		}

		public static string getRelativeAssetPath (string pathName)
		{
			return fixSlashes (pathName).Replace (Application.dataPath, "Assets");
		}
	
		public static string fixSlashes (string s)
		{
			const string forwardSlash = "/" , backSlash= "\\";
		
			return s.Replace (backSlash, forwardSlash);
		}
	
		public static List<FileInfo> DirSearch (DirectoryInfo d, string searchFor)
		{
			List<FileInfo> founditems = d.GetFiles (searchFor).ToList ();
			foreach (DirectoryInfo di in d.GetDirectories())
				founditems.AddRange (DirSearch (di, searchFor));
			return(founditems);
		}
	}
	
}

class LavelRemover: EditorWindow
{
	[MenuItem("Window/Label/RemoveLabels")]
	static void Init ()
	{
		var window = EditorWindow.GetWindow<LavelRemover> ();
		window.title = "Remove Labels";
	}

	string text;

	void OnGUI ()
	{
		text = EditorGUILayout.TextField ("remove label name", text);

		if (GUILayout.Button ("remove labels")) {
			
			var all = TW.FileUtilExtra.allAssets;
			using (var progress = new TW.EditorProgress("RemoveLabels")) {
				progress.progressMax = all.Count;
				int currentAsset = 1;

				foreach (var asset in all) {
					progress.UpdateProgress ("clean up ", currentAsset ++);
					TW.LabelManager.RemoveLabel (asset, text);
				}
			}
		}
	}
}

class AssetLabels: EditorWindow
{
	[MenuItem("Window/Label/Asset Labels")]
	static void Init ()
	{
		var window = EditorWindow.GetWindow<AssetLabels> ();
		window.title = "Asset Labels";
	}

	Vector2 scroll;

	void OnInspectorUpdate ()
	{
		Repaint ();
	}
	
	void OnGUI ()
	{
		var obj = Selection.activeObject;
		if (obj == null) {
			GUILayout.Label ("not selected");
			return;
		}

		var labels = AssetDatabase.GetLabels (obj);

		scroll = EditorGUILayout.BeginScrollView (scroll);
		foreach (var label in labels) {
			EditorGUILayout.SelectableLabel (label, GUILayout.Height (20));
		}
		EditorGUILayout.EndScrollView ();
	}
}