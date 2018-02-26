using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Diagnostics;

public class Build  {
	public static void ChmodExe(string file, string param) {
		UnityEngine.Debug.Log("chmod u+x :" + file);

		Process compiler = new Process();
		compiler.StartInfo.FileName = "chmod";
		compiler.StartInfo.Arguments = param + " " + file;
		compiler.StartInfo.UseShellExecute = true;
        compiler.Start();
	}

	public static void RevertIOS() {
		string file = "ios_revert_sa.sh";
		string path = Application.dataPath + "/../";

		ChmodExe(path + file, "u+x"); //chmod u+x first

		UnityEngine.Debug.Log("RevertIOS..");

		ProcessStartInfo proc = new ProcessStartInfo();
        proc.FileName = file;
        proc.WorkingDirectory = path;
		proc.WindowStyle = ProcessWindowStyle.Minimized;
        proc.CreateNoWindow = true;
		proc.UseShellExecute = false;
        proc.RedirectStandardInput = true;
        proc.RedirectStandardOutput = true;

        Process.Start(proc);
	}


	[MenuItem("Tools/Clean Build")]
	public static void CleanBuild() {
		RevertIOS();
	}

	[MenuItem("Tools/Clean Cache")]
	public static void CleanCache() {
		PlayerPrefs.DeleteAll();
		UnityEngine.Debug.Log("Cache is cleaned!");
	}

	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
		if (target != BuildTarget.iOS) {
			return;
		}

		RevertIOS();
	}
}
