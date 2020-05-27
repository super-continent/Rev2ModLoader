using System;
using EasyHook;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Rev2Hook;

namespace Rev2ModLoader
{
    class ModLoader
    {
        string channelName = null;
        XrdServer server;
        public bool Inject()
        {
            var guilty_gear = Process.GetProcessesByName("GuiltyGearXrd");

            if (guilty_gear.Length > 0)
            {
                var PID = guilty_gear[0].Id;
                var library = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Rev2Hook.dll");

                RemoteHooking.IpcCreateServer<XrdServer>(ref channelName, System.Runtime.Remoting.WellKnownObjectMode.Singleton);

                try
                {
                    RemoteHooking.Inject(PID, library, library, channelName);
                    server = RemoteHooking.IpcConnectClient<XrdServer>(channelName);
                    server.SetModsPath(
                        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                        + "\\rev2_mods");
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error while injecting: {0}", e);
                    return false;
                }
            };
            return false;
        }

        public byte[] TryExtractScript(int index)
        {
            try
            {
                if (server == null)
                {
                    throw new Exception("Not hooked into game!");
                }
                return server.ExtractScript(index);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
    }
}
