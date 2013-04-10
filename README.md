Send to Dropbox
=========

This is a Windows Explorer extension primarily written to add a new item "Send to Dropbox" to Explorer context menu.
This is a submenu that lists root-level folders in your default dropbox directory (c:\users\<username>\dropbox on Windows7) as such, if you click on a folder, it copies currently selected files/folders into that dropbox folder.
So it gives you a quick way to share your files via dropbox without having to open dropbox folder.

It also adds one more context menu option – Rename... – which gives you a quick way to rename a set of files by assigning a running number and/or a value as either prefix or suffix to them.

This project is tested on Windows7 only. However, it should work on prior versions as well as on Windows8 (though you may test for incompatibility). 

In order to test it, you must open Project properties > Build tab and set 'Register for COM Interop. This is purposefully not checked so that you can build it without Visual Studio kicking in to register as a side-effect of Build. Once this checkbox is checked, rebuild solution and open a new folder. Right click on a file/folder and  you should see the two menu items added.

Enjoy!

Please see [IContextMenu][1] to learn how this works.
[1]: http://msdn.microsoft.com/query/dev10.query?appId=Dev10IDEF1&l=EN-US&k=k(ICONTEXTMENU);k(TargetFrameworkMoniker-%22.NETFRAMEWORK%2cVERSION%3dV3.5%22);k(DevLang-CSHARP)&rd=true

For any query/suggestion, you can mail me at varunkho at outlook dot com
[Follow me on Twitter](http://twitter.com/varunkho)

