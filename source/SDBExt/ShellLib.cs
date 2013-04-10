using System;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SDBExt
{
    [ComVisible(false)]
    public enum LVCFMT
    {
        LEFT = 0x0000,
        RIGHT = 0x0001,
        CENTER = 0x0002,
        JUSTIFYMASK = 0x0003,
        IMAGE = 0x0800,
        BITMAP_ON_RIGHT = 0x1000,
        COL_HAS_IMAGES = 0x8000
    }
    [Flags, ComVisible(false)]
    public enum SHCOLSTATE
    {
        TYPE_STR = 0x1,
        TYPE_INT = 0x2,
        TYPE_DATE = 0x3,
        TYPEMASK = 0xf,
        ONBYDEFAULT = 0x10,
        SLOW = 0x20,
        EXTENDED = 0x40,
        SECONDARYUI = 0x80,
        HIDDEN = 0x100,
        PREFER_VARCMP = 0x200
    }

    [ComVisible(false), StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class LPCSHCOLUMNINIT
    {
        public uint dwFlags; //ulong
        public uint dwReserved; //ulong
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string wszFolder; //[MAX_PATH]; wchar
    }

    [ComVisible(false), StructLayout(LayoutKind.Sequential)]
    public struct SHCOLUMNID
    {
        public Guid fmtid; //GUID
        public uint pid; //DWORD
    }

    [ComVisible(false), StructLayout(LayoutKind.Sequential)]
    public class LPCSHCOLUMNID
    {
        public Guid fmtid; //GUID
        public uint pid; //DWORD
    }

    [ComVisible(false), StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public struct SHCOLUMNINFO
    {
        public SHCOLUMNID scid; //SHCOLUMNID
        public ushort vt; //VARTYPE
        public LVCFMT fmt; //DWORD
        public uint cChars; //UINT
        public SHCOLSTATE csFlags;  //DWORD
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)] //MAX_COLUMN_NAME_LEN
        public string wszTitle; //WCHAR
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] //MAX_COLUMN_DESC_LEN
        public string wszDescription; //WCHAR
    }

    [ComVisible(false), StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class LPCSHCOLUMNDATA
    {
        public uint dwFlags; //ulong
        public uint dwFileAttributes; //dword
        public uint dwReserved; //ulong
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pwszExt; //wchar
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string wszFile; //[MAX_PATH]; wchar
    }

    [ComVisible(false), ComImport, Guid("E8025004-1C42-11d2-BE2C-00A0C9A83DA1"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IColumnProvider
    {
        [PreserveSig()]
        int Initialize(LPCSHCOLUMNINIT psci);
        [PreserveSig()]
        int GetColumnInfo(int dwIndex, out SHCOLUMNINFO psci);

        /// <summary>
        /// Note: these objects must be threadsafe!  GetItemData _will_ be called
        /// simultaneously from multiple threads.
        /// </summary>
        [PreserveSig()]
        int GetItemData(LPCSHCOLUMNID pscid, LPCSHCOLUMNDATA pscd, out object /*VARIANT */ pvarData);
    }


    [ComVisible(false)]
    public abstract class ColumnProvider : IColumnProvider
    {
        [DllImport("Shell32.dll")]
        static extern void SHChangeNotify(int wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        const int SHCNE_ASSOCCHANGED = 0x08000000;

        public abstract int Initialize(LPCSHCOLUMNINIT psci);
        public abstract int GetColumnInfo(int dwIndex, out SHCOLUMNINFO psci);
        public abstract int GetItemData(LPCSHCOLUMNID pscid, LPCSHCOLUMNDATA pscd, out object pvarData);


        [ComRegisterFunction]
        public static void Register(System.Type t)
        {
            RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"Folder\shellex\ColumnHandlers\" + t.GUID.ToString("B"));
            key.SetValue(string.Empty, t.GetType().Name);
            key.Close();

            // Tell Explorer to refresh
            SHChangeNotify(SHCNE_ASSOCCHANGED, 0, IntPtr.Zero, IntPtr.Zero);
        }

        [ComUnregisterFunction]
        public static void UnRegister(System.Type t)
        {
            try
            {
                Registry.ClassesRoot.DeleteSubKeyTree(@"Folder\shellex\ColumnHandlers\" + t.GUID.ToString("B"));
            }
            catch
            {
                // Ignore all
            }

            // Tell Explorer to refresh
            SHChangeNotify(SHCNE_ASSOCCHANGED, 0, IntPtr.Zero, IntPtr.Zero);
        }
    }

    // Note that with Flag C# accepts duplicate enum entries
    [Flags]
    public enum MFMENU : uint
    {
        MF_UNCHECKED = 0,
        MF_STRING = 0,
        MF_ENABLED = 0,
        MF_BYCOMMAND = 0,
        MF_GRAYED = 1,
        MF_DISABLED = 0x00000002,
        MF_CHECKED = 0x00000008,
        MF_POPUP = 0x00000010,
        MF_HILITE = 0x00000080,
        MF_BYPOSITION = 0x00000400,
        MF_SEPARATOR = 0x00000800,
    }

    // Make these constants 
    [Flags]
    public enum MIIM : uint
    {
        STATE = 0x00000001,
        ID = 0x00000002,
        SUBMENU = 0x00000004,
        CHECKMARKS = 0x00000008,
        TYPE = 0x00000010,
        DATA = 0x00000020,
        STRING = 0x00000040,
        BITMAP = 0x00000080,
        FTYPE = 0x00000100
    }

    [Flags]
    public enum MF : uint
    {
        INSERT = 0x00000000,
        CHANGE = 0x00000080,
        APPEND = 0x00000100,
        DELETE = 0x00000200,
        REMOVE = 0x00001000,
        BYCOMMAND = 0x00000000,
        BYPOSITION = 0x00000400,
        SEPARATOR = 0x00000800,
        ENABLED = 0x00000000,
        GRAYED = 0x00000001,
        DISABLED = 0x00000002,
        UNCHECKED = 0x00000000,
        CHECKED = 0x00000008,
        USECHECKBITMAPS = 0x00000200,
        STRING = 0x00000000,
        BITMAP = 0x00000004,
        OWNERDRAW = 0x00000100,
        POPUP = 0x00000010,
        MENUBARBREAK = 0x00000020,
        MENUBREAK = 0x00000040,
        UNHILITE = 0x00000000,
        HILITE = 0x00000080,
        DEFAULT = 0x00001000,
        SYSMENU = 0x00002000,
        HELP = 0x00004000,
        RIGHTJUSTIFY = 0x00004000,
        MOUSESELECT = 0x00008000
    }

    [Flags]
    public enum MFS : uint
    {
        GRAYED = 0x00000003,
        DISABLED = MFS.GRAYED,
        CHECKED = MF.CHECKED,
        HILITE = MF.HILITE,
        ENABLED = MF.ENABLED,
        UNCHECKED = MF.UNCHECKED,
        UNHILITE = MF.UNHILITE,
        DEFAULT = MF.DEFAULT,
        MASK = 0x0000108B,
        HOTTRACKDRAWN = 0x10000000,
        CACHEDBMP = 0x20000000,
        BOTTOMGAPDROP = 0x40000000,
        TOPGAPDROP = 0x80000000,
        GAPDROP = 0xC0000000
    }

    public enum CLIPFORMAT : short
    {
        CF_TEXT = 1,
        CF_BITMAP = 2,
        CF_METAFILEPICT = 3,
        CF_SYLK = 4,
        CF_DIF = 5,
        CF_TIFF = 6,
        CF_OEMTEXT = 7,
        CF_DIB = 8,
        CF_PALETTE = 9,
        CF_PENDATA = 10,
        CF_RIFF = 11,
        CF_WAVE = 12,
        CF_UNICODETEXT = 13,
        CF_ENHMETAFILE = 14,
        CF_HDROP = 15,
        CF_LOCALE = 16,
        CF_MAX = 17,

        CF_OWNERDISPLAY = 0x0080,
        CF_DSPTEXT = 0x0081,
        CF_DSPBITMAP = 0x0082,
        CF_DSPMETAFILEPICT = 0x0083,
        CF_DSPENHMETAFILE = 0x008E,

        CF_PRIVATEFIRST = 0x0200,
        CF_PRIVATELAST = 0x02FF,

        CF_GDIOBJFIRST = 0x0300,
        CF_GDIOBJLAST = 0x03FF
    }


    [Flags]
    public enum DVASPECT
    {
        DVASPECT_CONTENT = 1,
        DVASPECT_THUMBNAIL = 2,
        DVASPECT_ICON = 4,
        DVASPECT_DOCPRINT = 8
    }

    [Flags]
    public enum TYMED
    {
        TYMED_HGLOBAL = 1,
        TYMED_FILE = 2,
        TYMED_ISTREAM = 4,
        TYMED_ISTORAGE = 8,
        TYMED_GDI = 16,
        TYMED_MFPICT = 32,
        TYMED_ENHMF = 64,
        TYMED_NULL = 0
    }

    [Flags]
    public enum CMF : uint
    {
        CMF_NORMAL = 0x00000000,
        CMF_DEFAULTONLY = 0x00000001,
        CMF_VERBSONLY = 0x00000002,
        CMF_EXPLORE = 0x00000004,
        CMF_NOVERBS = 0x00000008,
        CMF_CANRENAME = 0x00000010,
        CMF_NODEFAULT = 0x00000020,
        CMF_INCLUDESTATIC = 0x00000040,
        CMF_RESERVED = 0xffff0000      // View specific
    }

    // GetCommandString uFlags
    [Flags]
    public enum GCS : uint
    {
        VERBA = 0x00000000,     // canonical verb
        HELPTEXTA = 0x00000001,     // help text (for status bar)
        VALIDATEA = 0x00000002,     // validate command exists
        VERBW = 0x00000004,     // canonical verb (unicode)
        HELPTEXTW = 0x00000005,     // help text (unicode version)
        VALIDATEW = 0x00000006,     // validate command exists (unicode)
        UNICODE = 0x00000004,     // for bit testing - Unicode string
        VERB = GCS.VERBA,
        HELPTEXT = GCS.HELPTEXTA,
        VALIDATE = GCS.VALIDATEA
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MENUITEMINFO
    {
        public int cbSize;
        public uint fMask;
        public uint fType;
        public uint fState;
        public int wID;
        public IntPtr	/*HMENU*/	  hSubMenu;
        public IntPtr	/*HBITMAP*/   hbmpChecked;
        public IntPtr	/*HBITMAP*/	  hbmpUnchecked;
        public IntPtr	/*ULONG_PTR*/ dwItemData;
        public String dwTypeData;
        public uint cch;
        public IntPtr /*HBITMAP*/ hbmpItem;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FORMATETC
    {
        public CLIPFORMAT cfFormat;
        public IntPtr ptd;
        [MarshalAs(UnmanagedType.U4)]
        public DVASPECT dwAspect;
        public int lindex;
        [MarshalAs(UnmanagedType.U4)]
        public TYMED tymed;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STGMEDIUM
    {
        [MarshalAs(UnmanagedType.U4)]
        public int tymed;
        public IntPtr data;
        [MarshalAs(UnmanagedType.IUnknown)]
        public object pUnkForRelease;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct INVOKECOMMANDINFO
    {
        public int cbSize;    // sizeof(CMINVOKECOMMANDINFO)
        public int fMask;     // any combination of CMIC_MASK_*
        public IntPtr hwnd;      // might be NULL (indicating no owner window)
        public int lpVerb;    // either a string or MAKEINTRESOURCE(idOffset)
        public string lpParameters;  // might be NULL (indicating no parameter)
        public string lpDirectory;   // might be NULL (indicating no specific directory)
        public int nShow;     // one of SW_ values for ShowWindow() API
        public int dwHotKey;
        public IntPtr hIcon;
    }
    //public struct INVOKECOMMANDINFO
    //{
    //    //NOTE: When SEE_MASK_HMONITOR is set, hIcon is treated as hMonitor
    //    public int cbSize;						// sizeof(CMINVOKECOMMANDINFO)
    //    public uint fMask;						// any combination of CMIC_MASK_*
    //    public uint wnd;						// might be NULL (indicating no owner window)
    //    public int verb;
    //    [MarshalAs(UnmanagedType.LPStr)]
    //    public string parameters;				// might be NULL (indicating no parameter)
    //    [MarshalAs(UnmanagedType.LPStr)]
    //    public string directory;				// might be NULL (indicating no specific directory)
    //    public int Show;						// one of SW_ values for ShowWindow() API
    //    public uint HotKey;
    //    public uint hIcon;
    //}


    [ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), GuidAttribute("0000010e-0000-0000-C000-000000000046")]
    public interface IDataObject
    {
        [PreserveSig()]
        int GetData(ref FORMATETC a, ref STGMEDIUM b);
        [PreserveSig()]
        void GetDataHere(int a, ref STGMEDIUM b);
        [PreserveSig()]
        int QueryGetData(int a);
        [PreserveSig()]
        int GetCanonicalFormatEtc(int a, ref int b);
        [PreserveSig()]
        int SetData(int a, int b, int c);
        [PreserveSig()]
        int EnumFormatEtc(uint a, ref Object b);
        [PreserveSig()]
        int DAdvise(int a, uint b, Object c, ref uint d);
        [PreserveSig()]
        int DUnadvise(uint a);
        [PreserveSig()]
        int EnumDAdvise(ref Object a);
    }

    [ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), GuidAttribute("000214e8-0000-0000-c000-000000000046")]
    public interface IShellExtInit
    {
        [PreserveSig()]
        int Initialize(IntPtr pidlFolder, IntPtr lpdobj, uint /*HKEY*/ hKeyProgID);
    }


    [ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), GuidAttribute("000214e4-0000-0000-c000-000000000046")]
    public interface IContextMenu
    {
        // IContextMenu methods
        [PreserveSig()]
        int QueryContextMenu(HMenu hmenu, int iMenu, int idCmdFirst, int idCmdLast, CMF uFlags);
        [PreserveSig()]
        void InvokeCommand(IntPtr pici);
        [PreserveSig()]
        void GetCommandString(int idcmd, uint uflags, int reserved, StringBuilder commandstring, int cch);
    }


    public struct HMenu
    {
        public HMenu(IntPtr x)
        {
            handle = x;
        }
        public IntPtr handle;
    }

    public class Helpers
    {
        #region Win32 Imports

        [DllImport("shell32")]
        internal static extern uint DragQueryFile(IntPtr hDrop, uint iFile, [Out]StringBuilder buffer, int cch);

        [DllImport("user32")]
        internal static extern HMenu CreatePopupMenu();

        [DllImport("user32")]
        internal static extern bool InsertMenuItem(HMenu hmenu, uint uposition, uint uflags, ref MENUITEMINFO mii);

        // MF_BYCOMMAND MF_BYPOSITION MF_STRING MF_POPUP
        [DllImport("user32")]
        internal static extern bool AppendMenu(HMenu hmenu, MFMENU uflags, IntPtr uIDNewItemOrSubmenu, string text);

        [DllImport("user32")]
        internal static extern bool InsertMenu(HMenu hmenu, int position, MFMENU uflags, IntPtr uIDNewItemOrSubmenu, string text);
        #endregion
    }

}
