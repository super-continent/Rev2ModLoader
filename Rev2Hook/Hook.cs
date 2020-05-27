using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using EasyHook;

namespace Rev2Hook
{
    interface IGame
    {
        byte[] ExtractScript(ScriptType whichScript);
        void SetModsPath(string path);
        void UpdateCharacterEnabled(string characterShortname, bool isEnabled);
    }

    public enum ScriptType
    {
        Player1,
        Player1EF,
        Player2,
        Player2EF,
        Cmn,
        CmnEF,
    }

    public class Hook : IEntryPoint
    {
        XrdServer server;

        IntPtr baseAddr;
        int PID;

        Queue<string> messageQueue = new Queue<string>();



        LocalHook loadScriptHook;

        public Hook(RemoteHooking.IContext context, string channelName)
        {
            server = RemoteHooking.IpcConnectClient<XrdServer>(channelName);

            var guilty_gear = Process.GetProcessesByName("GuiltyGearXrd");

            PID = guilty_gear[0].Id;
            baseAddr = guilty_gear[0].Modules[0].BaseAddress;

            server.Ping();
        }

        public void Run(RemoteHooking.IContext context, string channelName)
        {
            server.IsInjected(RemoteHooking.GetCurrentProcessId());

            server.ReportMessage("Setting up hook...");
            loadScriptHook = LocalHook.Create(
                IntPtr.Add(baseAddr, 0xBBF000),
                new LoadScript_delegate(LoadScript_hook),
                this
            );

            loadScriptHook.ThreadACL.SetExclusiveACL(new int[] { 0 });
            var addr = IntPtr.Add(baseAddr, 0xBBF000);
            messageQueue.Enqueue(string.Format("Added hook to {0:X}!", (int)addr));

            try
            {


                while (true)
                {
                    System.Threading.Thread.Sleep(500);
                    string[] queue;

                    lock (messageQueue)
                    {
                        queue = messageQueue.ToArray();
                        messageQueue.Clear();
                    }

                    foreach (string message in queue)
                    {
                        server.ReportMessage(message);
                    }
                    server.Ping();
                }
            }
            catch
            {
                loadScriptHook.Dispose();
            }

            loadScriptHook.Dispose();
        }


        #region LoadScript_hook
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate void LoadScript_delegate(IntPtr putScriptPointerHere, IntPtr scriptLocation, int size);


        int counter = 0;
        string lastCharacter;

        void LoadScript_hook(IntPtr putScriptPointerHere, IntPtr scriptLocation, int size)
        {
            ScriptType script = ScriptType.Player1;
            try
            {
                this.messageQueue.Enqueue($"LOADING SCRIPT, ARGUMENTS ARE:\n" +
                    $"POINTER DESTINATION: 0x{(int)putScriptPointerHere:X8}\n" +
                    $"SCRIPT POINTER: 0x{(int)scriptLocation:X8}\n" +
                    $"SIZE: {size}");

                switch (counter)
                {
                    case 0:
                        server.ClearScriptEntries();
                        script = ScriptType.Player1;
                        lastCharacter = server.CheckForCharacter(scriptLocation);
                        counter++;
                        break;
                    case 1:
                        script = ScriptType.Player1EF;
                        counter++;
                        break;
                    case 2:
                        script = ScriptType.Player2;
                        lastCharacter = server.CheckForCharacter(scriptLocation);
                        counter++;
                        break;
                    case 3:
                        script = ScriptType.Player2EF;
                        counter++;
                        break;
                    case 4:
                        script = ScriptType.Cmn;
                        counter++;
                        break;
                    case 5:
                        script = ScriptType.CmnEF;
                        counter = 0;
                        break;
                }

                (IntPtr mod_location, int mod_size) = this.server.TryLoadScript(script, lastCharacter);

                server.ReportMessage($"Got location: {mod_location}\nGot size: {mod_size}");
                if (mod_location != IntPtr.Zero && mod_size != -1)
                {
                    scriptLocation = mod_location;
                    size = mod_size;
                }

                LoadScript_delegate loadscript_orig = (LoadScript_delegate)Marshal.GetDelegateForFunctionPointer(
                    this.loadScriptHook.HookBypassAddress, typeof(LoadScript_delegate));

                server.AddScriptEntry(scriptLocation, size);
                loadscript_orig(putScriptPointerHere, scriptLocation, size);
            }
            catch (Exception e)
            {
                server.ReportException(e);
            }
            return;
        }
        #endregion
    }
}