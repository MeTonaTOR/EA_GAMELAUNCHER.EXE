using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GameLauncher.ProdUI
{
	public class ExtendedWebBrowser : WebBrowser
	{
		[ComImport]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface DWebBrowserEvents2
		{
			[DispId(105)]
			void CommandStateChange([In] long command, [In] bool enable);

			[DispId(259)]
			void DocumentComplete([In] [MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);

			[DispId(251)]
			void NewWindow2([In] [Out] [MarshalAs(UnmanagedType.IDispatch)] ref object pDisp, [In] [Out] ref bool cancel);
		}

		public class WebBrowserExtendedEvents : StandardOleMarshalObject, DWebBrowserEvents2
		{
			private ExtendedWebBrowser mBrowser;

			public WebBrowserExtendedEvents(ExtendedWebBrowser browser)
			{
				mBrowser = browser;
			}

			public void NewWindow2(ref object pDisp, ref bool cancel)
			{
				mBrowser.OnNewWindow2(ref pDisp, ref cancel);
			}

			public void DocumentComplete(object pDisp, ref object url)
			{
				mBrowser.OnDocumentComplete(pDisp, url);
			}

			public void CommandStateChange(long command, bool enable)
			{
				mBrowser.OnCommandStateChange(command, ref enable);
			}
		}

		[ComImport]
		[TypeLibType(TypeLibTypeFlags.FHidden | TypeLibTypeFlags.FDual | TypeLibTypeFlags.FOleAutomation)]
		[Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E")]
		public interface IWebBrowser2
		{
			[DispId(200)]
			object Application
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			[DispId(201)]
			object Parent
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			[DispId(202)]
			object Container
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			[DispId(203)]
			object Document
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			[DispId(204)]
			bool TopLevelContainer
			{
				get;
			}

			[DispId(205)]
			string Type
			{
				get;
			}

			[DispId(206)]
			int Left
			{
				get;
				set;
			}

			[DispId(207)]
			int Top
			{
				get;
				set;
			}

			[DispId(208)]
			int Width
			{
				get;
				set;
			}

			[DispId(209)]
			int Height
			{
				get;
				set;
			}

			[DispId(210)]
			string LocationName
			{
				get;
			}

			[DispId(211)]
			string LocationURL
			{
				get;
			}

			[DispId(212)]
			bool Busy
			{
				get;
			}

			[DispId(0)]
			string Name
			{
				get;
			}

			[DispId(-515)]
			int HWND
			{
				get;
			}

			[DispId(400)]
			string FullName
			{
				get;
			}

			[DispId(401)]
			string Path
			{
				get;
			}

			[DispId(402)]
			bool Visible
			{
				get;
				set;
			}

			[DispId(403)]
			bool StatusBar
			{
				get;
				set;
			}

			[DispId(404)]
			string StatusText
			{
				get;
				set;
			}

			[DispId(405)]
			int ToolBar
			{
				get;
				set;
			}

			[DispId(406)]
			bool MenuBar
			{
				get;
				set;
			}

			[DispId(407)]
			bool FullScreen
			{
				get;
				set;
			}

			[DispId(-525)]
			WebBrowserReadyState ReadyState
			{
				get;
			}

			[DispId(550)]
			bool Offline
			{
				get;
				set;
			}

			[DispId(551)]
			bool Silent
			{
				get;
				set;
			}

			[DispId(552)]
			bool RegisterAsBrowser
			{
				get;
				set;
			}

			[DispId(553)]
			bool RegisterAsDropTarget
			{
				get;
				set;
			}

			[DispId(554)]
			bool TheaterMode
			{
				get;
				set;
			}

			[DispId(555)]
			bool AddressBar
			{
				get;
				set;
			}

			[DispId(556)]
			bool Resizable
			{
				get;
				set;
			}

			[DispId(100)]
			void GoBack();

			[DispId(101)]
			void GoForward();

			[DispId(102)]
			void GoHome();

			[DispId(103)]
			void GoSearch();

			[DispId(104)]
			void Navigate([In] string Url, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);

			[DispId(-550)]
			void Refresh();

			[DispId(105)]
			void Refresh2([In] ref object level);

			[DispId(106)]
			void Stop();

			[DispId(300)]
			void Quit();

			[DispId(301)]
			void ClientToWindow(out int pcx, out int pcy);

			[DispId(302)]
			void PutProperty([In] string property, [In] object vtValue);

			[DispId(303)]
			object GetProperty([In] string property);

			[DispId(500)]
			void Navigate2([In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);

			[DispId(503)]
			void ShowBrowserBar([In] ref object pvaClsid, [In] ref object pvarShow, [In] ref object pvarSize);
		}

		private AxHost.ConnectionPointCookie mCookie;

		private WebBrowserExtendedEvents mEvents;

		public object Application => (base.ActiveXInstance as IWebBrowser2)?.Application;

		public event EventHandler<NewWindow2EventArgs> NewWindow2;

		public event EventHandler<DocumentCompleteEventArgs> DocumentComplete;

		public event EventHandler<CommandStateChangeEventArgs> CommandStateChange;

		protected override void CreateSink()
		{
			base.CreateSink();
			mEvents = new WebBrowserExtendedEvents(this);
			mCookie = new AxHost.ConnectionPointCookie(base.ActiveXInstance, mEvents, typeof(DWebBrowserEvents2));
		}

		protected override void DetachSink()
		{
			if (mCookie != null)
			{
				mCookie.Disconnect();
				mCookie = null;
			}
			base.DetachSink();
		}

		protected void OnNewWindow2(ref object ppDisp, ref bool cancel)
		{
			EventHandler<NewWindow2EventArgs> newWindow = this.NewWindow2;
			NewWindow2EventArgs newWindow2EventArgs = new NewWindow2EventArgs(ref ppDisp, ref cancel);
			newWindow?.Invoke(this, newWindow2EventArgs);
			cancel = newWindow2EventArgs.Cancel;
			ppDisp = newWindow2EventArgs.PPDisp;
		}

		protected void OnDocumentComplete(object ppDisp, object url)
		{
			EventHandler<DocumentCompleteEventArgs> documentComplete = this.DocumentComplete;
			DocumentCompleteEventArgs documentCompleteEventArgs = new DocumentCompleteEventArgs(ppDisp, url);
			documentComplete?.Invoke(this, documentCompleteEventArgs);
			ppDisp = documentCompleteEventArgs.PPDisp;
		}

		protected void OnCommandStateChange(long command, ref bool enable)
		{
			EventHandler<CommandStateChangeEventArgs> commandStateChange = this.CommandStateChange;
			CommandStateChangeEventArgs e = new CommandStateChangeEventArgs(command, ref enable);
			commandStateChange?.Invoke(this, e);
		}
	}
}
