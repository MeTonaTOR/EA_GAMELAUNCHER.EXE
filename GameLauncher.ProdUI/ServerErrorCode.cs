using System.Runtime.InteropServices;

namespace GameLauncher.ProdUI
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ServerErrorCode
	{
		public const int UnknownRemoteAccountManagementError = 6;

		public const int LoginFailureLimitReached = -520;

		public const int UsernameTooShort = -706;

		public const int UsernameNotAllowed = -708;

		public const int EmailSyntax = -710;

		public const int MissEmail = -712;

		public const int EmailDuplicate = -713;

		public const int MissPwd = -714;

		public const int EmailTooShort = -745;

		public const int NoUserWithEmail = -746;

		public const int WrongPwd = -747;

		public const int RemoteUserIsBanned = -748;

		public const int RemoteUserIsGameBanned = -750;

		public const int UsernameDuplicate = -773;

		public const int NoUserWithId = -774;

		public const int UserNotUnique = -775;

		public const int NoUserWithUsername = -777;

		public const int MissUsername = -778;

		public const int EmailDuplicateGlobal = -1807;

		public const int MaximumUsersLoggedInHardCapReached = -521;

		public const int MaximumUsersLoggedInSoftCapReached = -522;

		public const int MaximumUsersLoggedInUnspecified = -523;

		public const int MissingRequiredEntitlements = -1612;

		public const int UserHasNoEntitlements = -1730;

		public const int BannedEntitlements = -1613;
	}
}
