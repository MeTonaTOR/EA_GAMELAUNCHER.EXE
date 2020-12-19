using Microsoft.Win32;
using System;
using System.IO;

namespace GameLauncher.ProdUI
{
	public class Utils
	{
		public static string[] RetrieveWebApplicationData()
		{
			string[] array = new string[2];
			string arg = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\http\\UserChoice", "Progid", string.Empty);
			string text = (string)Registry.GetValue($"HKEY_CURRENT_USER\\Software\\Classes\\{arg}\\shell\\open\\command", "", string.Empty);
			if (string.IsNullOrEmpty(text))
			{
				text = (string)Registry.GetValue($"HKEY_CLASSES_ROOT\\{arg}\\shell\\open\\command", "", string.Empty);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Classes\\http\\shell\\open\\command", "", string.Empty);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = (string)Registry.GetValue("HKEY_CLASSES_ROOT\\http\\shell\\open\\command", "", string.Empty);
			}
			GameLauncherUI.Logger.Info("HTTP: " + text);
			if (string.IsNullOrEmpty(text))
			{
				text = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Classes\\Applications\\iexplore.exe\\shell\\open\\command", "", string.Empty);
			}
			if (string.IsNullOrEmpty(text))
			{
				string environmentVariable = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
				if (string.IsNullOrEmpty(environmentVariable))
				{
					environmentVariable = Environment.GetEnvironmentVariable("ProgramFiles");
				}
				string text2 = Path.Combine(environmentVariable, "Internet Explorer\\IEXPLORE.EXE");
				if (!File.Exists(text2))
				{
					return array;
				}
				array[0] = text2;
			}
			string[] array2 = text.Split('"');
			if (array2.Length > 1)
			{
				array[0] = array2[1];
			}
			if (array2.Length > 2)
			{
				array[1] = string.Join("\"", array2, 2, array2.Length - 2);
			}
			if (!array[1].Contains("%1"))
			{
				array[1] += " \"%1\"";
			}
			return array;
		}

		public static float Gauss(float x, float a, float b, float c, float floor, bool inverted)
		{
			if (c == 0f)
			{
				c = 1f;
			}
			a = 0.3989f / c;
			float num = a * (float)Math.Pow(Math.E, (0.0 - Math.Pow(x - b, 2.0)) / 2.0 * (double)c * (double)c);
			if (!inverted)
			{
				return num + floor;
			}
			return 0f - num + floor;
		}
	}
}
