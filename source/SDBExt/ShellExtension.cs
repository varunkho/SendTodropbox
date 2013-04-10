using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SDBExt
{
    [ComVisible(true), Guid("9F64E1CD-3C71-4D90-AF09-C68E99F377F7")]
    public class ShellExtension : IShellExtInit, IContextMenu
    {

        static readonly string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
        sdbWorkerPath = appPath + "\\SDBWorker.exe",
        logFile = appPath + "\\log.txt";

        #region IContextMenu
        protected IDataObject m_dataObject = null;
        IntPtr m_hDrop = IntPtr.Zero;
        string DropboxDir
        {
            get { return Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%") + @"\dropbox"; }
        }
        string[] mFolders;

        bool IsFSItem(CMF uFlags)
        {
            if ((uFlags & CMF.CMF_DEFAULTONLY) == 0)
            {
                uint nselected = Helpers.DragQueryFile(m_hDrop, 0xffffffff, null, 0);
                if (nselected >= 1)
                {
                    StringBuilder sb = new StringBuilder(1024);
                    Helpers.DragQueryFile(m_hDrop, 0, sb, sb.Capacity + 1);
                    string fsi = sb.ToString();
                    if (Directory.Exists(fsi) || File.Exists(fsi))
                        return true;
                }
            }

            return false;
        }

        // Returns: number of menu items inserted.
        // Ignore Send To menus, shortcuts, defaultonly
        int IContextMenu.QueryContextMenu(HMenu hMenu, int iMenu, int idCmdFirst, int idCmdLast, CMF uFlags)
        {
            int id = 0;
            if ((uFlags & (CMF.CMF_VERBSONLY | CMF.CMF_DEFAULTONLY | CMF.CMF_NOVERBS)) == 0 ||
                (uFlags & CMF.CMF_EXPLORE) != 0)
            //if (IsFSItem(uFlags))
            {
                if (Helpers.DragQueryFile(m_hDrop, 0xffffffff, null, 0) > 1)
                    Helpers.InsertMenu(hMenu, iMenu, MFMENU.MF_STRING | MFMENU.MF_ENABLED, new IntPtr(idCmdFirst + 1), "&Rename ..."); // 1 for rename

                mFolders = Directory.GetDirectories(DropboxDir); // look into implementing caching folder list and hooking to file system watcher
                // Create the submenu popup, add folder items to it and then finally insert it into the explorer contextmenu.
                HMenu submenu = Helpers.CreatePopupMenu();
                Helpers.AppendMenu(submenu, MFMENU.MF_STRING | MFMENU.MF_ENABLED,
                    new IntPtr(idCmdFirst + 2), "Route"); // 2 for root dropbox folder.
                id = 2;
                for (int i = 0; i < mFolders.Length; i++)
                {
                    Helpers.AppendMenu(submenu, MFMENU.MF_STRING | MFMENU.MF_ENABLED, new IntPtr(idCmdFirst + ++id), mFolders[i].Substring(mFolders[i].LastIndexOf('\\') + 1));
                }

                Helpers.InsertMenu(hMenu, 5, MFMENU.MF_BYPOSITION | MFMENU.MF_POPUP | MFMENU.MF_ENABLED, submenu.handle, "Send to &Dropbox");
            }
            return id;
        }


        void IContextMenu.GetCommandString(int idCmd, uint uFlags, int pwReserved, StringBuilder commandString, int cchMax)
        {
            // not implemented; not required in most cases.
        }

        // invoke the command on the context menu, it could be by hand
        // or using the ShellExecute!!!
        void IContextMenu.InvokeCommand(IntPtr pici)
        {
            try
            {
                //Log("invoked");
                INVOKECOMMANDINFO ici = (INVOKECOMMANDINFO)Marshal.PtrToStructure(pici, typeof(INVOKECOMMANDINFO));
                //Log("further");
                //Log(ici.lpVerb.ToString());
                switch (ici.lpVerb) // remember, the id we added to cmdFirst in queryContextMenu is contained in lpVerb
                {
                    case 1:
                        Rename();
                        break;
                    case 2: // Remember root dropbox dir?
                        Copy(DropboxDir);
                        break;
                    default:
                        Copy(mFolders[ici.lpVerb - 3]);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message.Replace("{", "{{").Replace("}", "}}"));
            }
        }

        /// <summary>
        /// Invokes SDBWorker to do the copy by passing source files/folders and destination folder.
        /// </summary>
        /// <param name="destDir"></param>
        private void Copy(string destDir)
        {
            StringBuilder sb = new StringBuilder(1024);
            uint nselected = Helpers.DragQueryFile(m_hDrop, 0xffffffff, null, 0);
            if (nselected < 1)
                return;

            //Log("count: {0}", nselected);
            string args = string.Empty;
            for (uint i = 0; i < nselected; i++)
            {
                Helpers.DragQueryFile(m_hDrop, i, sb, sb.Capacity + 1);
                args += "\"" + sb.ToString().Trim() + "\" ";
            }

            new System.Threading.Thread(() =>
                {
                    try
                    {
                        args += " \"" + destDir + "\"";
                        args = "\"-copy\" " + args;
                        Log(sdbWorkerPath);
                        Log(args);

                        if (File.Exists(sdbWorkerPath))
                            Process.Start(sdbWorkerPath, args);
                        else
                            Log("sdbWorker.exe not found.");
                    }
                    catch (Exception ex)
                    {
                        Log(ex.Message.Replace("{", "{{").Replace("}", "}}"));
                    }
                }
            ).Start();
        }

        /// <summary>
        /// Invokes SDBWorker to begin renaming multiple files/folders.
        /// </summary>
        /// <param name="destDir"></param>
        private void Rename()
        {
            StringBuilder sb = new StringBuilder(1024);
            uint nselected = Helpers.DragQueryFile(m_hDrop, 0xffffffff, null, 0);
            if (nselected < 1)
                return;

            //Log("count: {0}", nselected);
            string args = string.Empty;
            for (uint i = 0; i < nselected; i++)
            {
                Helpers.DragQueryFile(m_hDrop, i, sb, sb.Capacity + 1);
                args += "\"" + sb.ToString().Trim() + "\" ";
            }

            new System.Threading.Thread(() =>
            {
                try
                {
                    args = "\"-rename\" " + args;
                    Log(sdbWorkerPath);
                    Log(args);

                    if (File.Exists(sdbWorkerPath))
                        Process.Start(sdbWorkerPath, args);
                    else
                        Log("sdb.exe not found.");
                }
                catch (Exception ex)
                {
                    Log(ex.Message.Replace("{", "{{").Replace("}", "}}"));
                }
            }
            ).Start();
        }
        #endregion

        #region IShellExtInit
        int IShellExtInit.Initialize(IntPtr pidlFolder, IntPtr lpdobj, uint hKeyProgID)
        {
            try
            {
                // save the information about the selection
                m_dataObject = null;
                if (lpdobj != (IntPtr)0)
                {
                    m_dataObject = (IDataObject)Marshal.GetObjectForIUnknown(lpdobj);
                    FORMATETC fmt = new FORMATETC();
                    fmt.cfFormat = CLIPFORMAT.CF_HDROP;
                    fmt.ptd = IntPtr.Zero;
                    fmt.dwAspect = DVASPECT.DVASPECT_CONTENT;
                    fmt.lindex = -1;
                    fmt.tymed = TYMED.TYMED_HGLOBAL;
                    STGMEDIUM medium = new STGMEDIUM();
                    m_dataObject.GetData(ref fmt, ref medium);
                    m_hDrop = medium.data;
                    Log(m_hDrop.ToString());
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
            return 0;
        }
        #endregion

        #region Registration
        [System.Runtime.InteropServices.ComRegisterFunctionAttribute()]
        static void RegisterServer(String str1)
        {
            string keyname = "SDBExt";
            string guid = "{" + typeof(ShellExtension).GUID.ToString() + "}";

            Func<string, bool> addKey = (string regPath) =>
                {
                    try
                    {
                        RegistryKey rk, rk2;
                        rk = Registry.ClassesRoot.OpenSubKey(regPath, true);
                        rk2 = rk.OpenSubKey(keyname);
                        if (rk2 == null)
                            rk2 = rk.CreateSubKey(keyname);
                        rk2.SetValue("", guid);
                        rk2.Close();
                        rk.Close();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Log(e.ToString());
                    }
                    return false;
                };

            addKey(@"*\shellex\ContextMenuHandlers");
            addKey(@"directory\shellex\ContextMenuHandlers");
        }

        [System.Runtime.InteropServices.ComUnregisterFunctionAttribute()]
        static void UnregisterServer(String str1)
        {
            string keyname = "SDBExt";
            string guid = "{" + typeof(ShellExtension).GUID.ToString() + "}";

            Func<string, bool> RemoveKey = (string regPath) =>
                {
                    try
                    {
                        RegistryKey rk = Registry.ClassesRoot.OpenSubKey(regPath, true);
                        rk.DeleteSubKey(keyname, false);
                        rk.Close();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Log(e.ToString());
                    }
                    return false;
                };

            RemoveKey(@"*\shellex\ContextMenuHandlers");
            RemoveKey(@"directory\shellex\ContextMenuHandlers");
        }
        #endregion

        private static void Log(string msg, params object[] args)
        {
#if DEBUG
            msg = DateTime.Now.ToString("hh:mm:ss ") + msg + "\r\n";
            File.AppendAllText(logFile, args == null || args.Length == 0 ? msg : string.Format(msg, args));
#endif
        }

    }
}
