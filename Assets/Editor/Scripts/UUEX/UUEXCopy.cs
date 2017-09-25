using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace UUEX
{
	public class UUEXCopy
	{
		[MenuItem("UUEX/Copy/Copy UUEX To Main")]
		private static void CopyUUEXToMain()
		{
			string srcPath = Application.dataPath + "/Plugins/UUEX";
			string destPath = "/Users/deja/UUEX/Assets/Plugins/UUEX";

			DirectoryCopy (srcPath, destPath);

			srcPath = Application.dataPath + "/Editor/Scripts/UUEX";
			destPath = "/Users/deja/UUEX/Assets/Editor/Scripts/UUEX";

			DirectoryCopy (srcPath, destPath);

			Debug.Log ("DONE COPYING FILES");
		}

		private static void DirectoryCopy(string srcPath, string destPath)
		{
			DirectoryInfo srcDirInfo = new DirectoryInfo (srcPath);
			if(!srcDirInfo.Exists)
			{
				Debug.LogError("SOURCE DIRECTORY NOT FOUND");
				return;
			}
			
			if (!Directory.Exists (destPath))
				Directory.CreateDirectory (destPath);
			
			FileInfo[] files = srcDirInfo.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destPath, file.Name);
				file.CopyTo(temppath, true);
			}

			DirectoryInfo[] dirs = srcDirInfo.GetDirectories();
			foreach (DirectoryInfo subdir in dirs)
			{
				string temppath = Path.Combine(destPath, subdir.Name);
				DirectoryCopy(subdir.FullName, temppath);
			}
		}
	}
}
