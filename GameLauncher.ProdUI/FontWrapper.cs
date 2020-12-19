using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GameLauncher.ProdUI
{
	internal class FontWrapper
	{
		internal static class UnsafeNativeMethods
		{
			[DllImport("gdi32.dll")]
			public static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
		}

		private PrivateFontCollection mPrivateFontCollection;

		private List<string> mFonts;

		private static FontWrapper _instance = new FontWrapper();

		public static FontWrapper Instance => _instance;

		private FontWrapper()
		{
			mPrivateFontCollection = new PrivateFontCollection();
			mFonts = new List<string>();
		}

		public FontFamily GetFontFamily(string fontName)
		{
			if (!mFonts.Contains(fontName))
			{
				LoadEmbeddedFont(fontName);
			}
			int num = mFonts.IndexOf(fontName);
			return mPrivateFontCollection.Families[num];
		}

		private void LoadEmbeddedFont(string fontName)
		{
			Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GameLauncher.ProdUI.Fonts." + fontName);
			IntPtr intPtr = Marshal.AllocCoTaskMem((int)manifestResourceStream.Length);
			byte[] array = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
			Marshal.Copy(array, 0, intPtr, (int)manifestResourceStream.Length);
			uint pcFonts = 0u;
			UnsafeNativeMethods.AddFontMemResourceEx(intPtr, (uint)array.Length, IntPtr.Zero, ref pcFonts);
			mPrivateFontCollection.AddMemoryFont(intPtr, (int)manifestResourceStream.Length);
			manifestResourceStream.Close();
			Marshal.FreeCoTaskMem(intPtr);
			mFonts.Add(fontName);
			mFonts.Sort();
		}
	}
}
