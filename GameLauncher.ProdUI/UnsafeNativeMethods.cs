using System.Runtime.InteropServices;

namespace GameLauncher.ProdUI
{
	internal static class UnsafeNativeMethods
	{
		[DllImport("kernel32", CharSet = CharSet.Auto)]
		public static extern int GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);
	}
}
