﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace CessnaAkkaSpike.Repository
{
    public interface IRepository
    {
        void CreateInstaller(string installerName);
        void RegisterInstallerinRelease(string messageInstallerName);
    }
    public class Repository : IRepository
    {
        public void CreateInstaller(string installerName)
        {
            var installersPath = @".\installer.txt";
            WriteToFile(installerName, installersPath);
        }

        private static void WriteToFile(string installerName, string installersPath)
        {
            File.AppendAllLines(installersPath, new[] {installerName});
        }

        public void RegisterInstallerinRelease(string installerName)
        {
            var installersPath = @".\release.txt";
            WriteToFile(installerName, installersPath);
        }
    }
}