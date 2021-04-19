﻿/*
MIT License

Copyright (c) 2018 hecomi

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace uDllExporter
{

public class DllExporter : ScriptableWizard
{
	const string BuildErrorMessage = "Build Error (see Consele output).";
	const string UnknownErrorMessage =  "Export Error (Please report the error message to uDllExporter developer).";

	public enum TargetDirectory
	{
		All,
		UnityEngine,
		UnityEditor,
	}

	[SerializeField]
	TargetDirectory targetDirectory = TargetDirectory.All;

	[SerializeField]
	string outputDirectoryPath;

	[SerializeField]
	string outputDllName;

	struct DllInfo
	{
		public string name;
		public string path;
	}
	List<DllInfo> unityEngineDllList;
	List<DllInfo> unityEditorDllList;
	List<DllInfo> otherDllList;

	int unityEngineDllListFlags = 1;
	int unityEditorDllListFlags = 1;
	int otherDllListFlags = 0;

	List<string> files = null;
	Vector2 filesScrollPos = Vector2.zero;
	string outputInfo;

	static string unityEditorPath
	{
		get
		{
			var appPath = System.Environment.GetCommandLineArgs()[0];
			var appDir = Path.GetDirectoryName(appPath);
#if UNITY_EDITOR_WIN
			return appDir + "/Data/";
#else
			return appDir + "/../";
#endif
		}
	}

	static string unityEngineDllPath
	{
		get	{ return unityEditorPath + "Managed/UnityEngine.dll"; }
	}

	static string unityEditorDllPath
	{
		get	{ return unityEditorPath + "Managed/UnityEditor.dll"; }
	}

	static string unityExtensionsDirPath
	{
		get	{ return unityEditorPath + "UnityExtensions"; }
	}

	static string smcsPath
	{
#if UNITY_EDITOR_WIN
		get	{ return unityEditorPath + "Mono/bin/smcs.bat"; }
#else
		get	{ return unityEditorPath + "Mono/bin/smcs"; }
#endif
	}

    [MenuItem("Window/uDllExporter")]
    static void Open()
    {
        var window = DisplayWizard<DllExporter>("uDllExporter", "Close", "Build DLL");        
		window.InitOutputParameters();
		window.InitDllList();
	}

	void InitOutputParameters()
	{
		var projectDir = Directory.GetParent(Application.dataPath);
		outputDirectoryPath = projectDir.FullName + "/Build";
		outputDllName = projectDir.Name + ".dll";
	}

	void InitDllList()
	{
		unityEngineDllList = new List<DllInfo>();
		unityEditorDllList = new List<DllInfo>();
		otherDllList = new List<DllInfo>();

		unityEngineDllList.Add(new DllInfo { name = "UnityEngine.dll", path = unityEngineDllPath });
		unityEditorDllList.Add(new DllInfo { name = "UnityEditor.dll", path = unityEditorDllPath });

		foreach (var path in GetUnityExtensionDllPaths())
		{
			var name = Path.GetFileName(path);
			var info = new DllInfo { name = Path.GetFileName(path), path = path };
			if (name.IndexOf("UnityEngine") != -1)
			{
				unityEngineDllList.Add(info);
			}
			else if (name.IndexOf("UnityEditor") != -1)
			{
				unityEditorDllList.Add(info);
			}
			else
			{
				otherDllList.Add(info);
			}
		}
	}

	protected override bool DrawWizardGUI()
	{
		EditorGUILayout.LabelField("Export Settings", EditorStyles.boldLabel);

		++EditorGUI.indentLevel;
		{
			base.DrawWizardGUI();

			if (unityEditorDllList == null || unityEditorDllList.Count == 0 ||
				unityEditorDllList == null || unityEngineDllList.Count == 0 ||
				otherDllList == null || otherDllList.Count == 0)
			{
				InitDllList();
			}

			unityEngineDllListFlags = EditorGUILayout.MaskField(
				"Unity Engine DLLs", 
				unityEngineDllListFlags, 
				unityEngineDllList.Select(x => x.name).ToArray());

			unityEditorDllListFlags = EditorGUILayout.MaskField(
				"Unity Editor DLLs", 
				unityEditorDllListFlags, 
				unityEditorDllList.Select(x => x.name).ToArray());

			if (otherDllList.Count > 0)
			{
				otherDllListFlags = EditorGUILayout.MaskField(
					"Other DLLs", 
					otherDllListFlags, 
					otherDllList.Select(x => x.name).ToArray());
			}
		}
		--EditorGUI.indentLevel;

		EditorGUILayout.Space();

		if (files != null)
		{
			EditorGUILayout.LabelField("Exported Files", EditorStyles.boldLabel);
			++EditorGUI.indentLevel;
			{
				filesScrollPos = EditorGUILayout.BeginScrollView(filesScrollPos);
				var assetsPathLength = Application.dataPath.Length + 1;
				foreach (var file in files)
				{
					EditorGUILayout.LabelField(file.Substring(assetsPathLength));
				}
				EditorGUILayout.EndScrollView();
			}
			--EditorGUI.indentLevel;
		}

		if (!string.IsNullOrEmpty(outputInfo))
		{
			EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
			++EditorGUI.indentLevel;
			EditorGUILayout.LabelField(outputInfo);
			--EditorGUI.indentLevel;
		}
		
		EditorGUILayout.Space();

		return true;
	}

    void OnWizardCreate()
    {
    }

	void OnWizardOtherButton()
	{
		CreateOutputDirectory();
		ExportDll();
	}
	
	void CreateOutputDirectory()
	{
        if (!Directory.Exists(outputDirectoryPath))
        {
            Directory.CreateDirectory(outputDirectoryPath);
        }
	}

	void ExportDll()
	{
		errorString = "";

		try
		{
			files = GetSelectedFilePaths();
			var dlls = GetSelectedDllPaths();

			var process = new System.Diagnostics.Process();
			process.StartInfo.FileName = smcsPath;
			process.StartInfo.Arguments = GetArguments(files, dlls);
			process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;         
			process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false; 
			process.Start();

			string stdout = process.StandardOutput.ReadToEnd();         
            string stderr = process.StandardError.ReadToEnd();             
			process.WaitForExit();

			if (!string.IsNullOrEmpty(stdout))
			{
				stdout = stdout.Trim();
				Debug.LogWarning(stdout);
			}

			if (!string.IsNullOrEmpty(stderr))
			{
				stderr = stderr.Trim();
				if (stderr.IndexOf(": error") != -1 || stderr.IndexOf("Exception:") != -1)
				{
					Debug.LogError(stderr);
					errorString = BuildErrorMessage;
				}
				else
				{
					Debug.LogWarning(stderr);
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError(e.Message);
			Debug.LogError(e.StackTrace);
			errorString = UnknownErrorMessage;
		}

		if (string.IsNullOrEmpty(errorString))
		{
			var path = string.Format("{0}/{1}", outputDirectoryPath, outputDllName);
			if (!File.Exists(path))
			{
				errorString = UnknownErrorMessage;
			}
			else
			{
				var outputDllSize = new FileInfo(path).Length;
				outputInfo = string.Format("{0}/{1} ({2} byte)", 
					outputDirectoryPath, 
					outputDllName,
					outputDllSize);
			}
		}
	}

	void GetFilesRecursively(string path, ref List<string> list)
	{
		if (Directory.Exists(path))
		{
			foreach (var dir in Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories))
			{
				GetFilesRecursively(Path.GetFullPath(dir), ref list);
			}
		}
		else if (File.Exists(path))
		{
			if (path.EndsWith(".cs")) list.Add(Path.GetFullPath(path));
		}
	}

	bool IsEditorScript(string path)
	{
		return 
            path.IndexOf("/Editor/") != -1 ||
            path.IndexOf("\\Editor\\") != -1;
	}
	
	List<string> GetSelectedFilePaths()
	{
		var list = new List<string>();

        foreach (var obj in Selection.objects)
        {   
			var path = AssetDatabase.GetAssetPath(obj);
			GetFilesRecursively(path, ref list);
        }

		switch (targetDirectory)
		{
			case TargetDirectory.All:
				break;
			case TargetDirectory.UnityEngine:
				list = list.Where(x => !IsEditorScript(x)).ToList();
				break;
			case TargetDirectory.UnityEditor:
				list = list.Where(x => IsEditorScript(x)).ToList();
				break;
		}

		return list;
	}

	List<string> GetUnityExtensionDllPaths()
	{
		var list = new List<string>();

/*
		foreach (var path in Directory.GetFiles(unityExtensionsDirPath, "*.dll", SearchOption.AllDirectories))
		{
			list.Add(path);
		}
*/
		return list;
	}

	List<string> GetSelectedDllPath(List<DllInfo> list, int flags)
	{
		var selectedList = new List<string>();

		for (int i = 0; i < list.Count; ++i)
		{
			if ((flags & 1 << i) != 0)
			{
				selectedList.Add(list[i].path);
			}
		}

		return selectedList;
	}

	List<string> GetSelectedDllPaths()
	{
		var list = new List<string>();
		list.AddRange(GetSelectedDllPath(unityEngineDllList, unityEngineDllListFlags));
		list.AddRange(GetSelectedDllPath(unityEditorDllList, unityEditorDllListFlags));
		list.AddRange(GetSelectedDllPath(otherDllList, otherDllListFlags));
		return list;
	}

	string GetArguments(List<string> files, List<string> dlls)
	{
		var arguments = "";
		arguments += string.Join(" ", dlls.Select(x => string.Format("-r:\"{0}\" ", x)).ToArray());
		arguments += "-target:library ";
		arguments += string.Format("-out:\"{0}/{1}\"", outputDirectoryPath, outputDllName);
		arguments += " " + string.Join(" ", files.Select(x => string.Format("\"{0}\"", x)).ToArray());

		Debug.Log(arguments);

		return arguments;
	}
}

}