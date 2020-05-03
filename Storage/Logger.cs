using Notorious;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Core;

namespace QoL.Storage
{
    public static class Logger
    {
        public static void LogToFiles(string name, string LogDetail) => File.WriteAllText($"QoL\\Logs\\{name}.txt", LogDetail);

        public static string BuildLogDetail(this APIUser user)
        {
            var PlayerManager = Wrappers.GetPlayerManager();
            var Player = PlayerManager.GetPlayer(user.id);
            string Information = $"{user.displayName}'s Information\n\n";
            Console.WriteLine(Information);

            //Information += $"Username: {user.username}\n";
            //Information += $"Display Name: {user.displayName}\n";
            //Information += $"UserID: {user.id}\n";
            ////Information += $"SteamID: {(SteamID == 0 ? "Hidden." : SteamID.ToString())}\n";
            //Information += $"AvatarID: {user.avatarId}\n";
            //Information += $"Avatar Asset URL: {user.currentAvatarAssetUrl}\n";
            //Information += $"Date Logged: {DateTime.Now.ToShortDateString()} @ {DateTime.Now.ToShortTimeString()}\n";

            return Information.ToString();
        }
    }
}
