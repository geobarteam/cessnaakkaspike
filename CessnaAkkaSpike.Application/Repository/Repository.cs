using System;
using System.Collections.Generic;
using System.IO;
using CessnaAkkaSpike.Application.Messages;
using Newtonsoft.Json;

namespace CessnaAkkaSpike.Application.Repository
{
    public interface IRepository
    {
        void CreateInstaller(string installerName);
        void RegisterInstallerinRelease(string messageInstallerName);
        Dictionary<string, PipelineMessage> ReadAllApprovals();
        void StoreApprovals(Dictionary<string, PipelineMessage> pipelineMessages);
    }
    public class Repository : IRepository
    {
        private const string installersFileName = @"installer.txt";
        private const string releaseFileName = @"release.txt";
        private const string approvalsFileName = @"approvals.txt";

        private readonly string _releaseFilePath;
        private readonly string _installersPath;
        private readonly string _approvalsFilePath;

        private readonly string _rootPath;

        public Repository()
        {
            _rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _installersPath = Path.Combine(_rootPath, installersFileName);
            _releaseFilePath = Path.Combine(_rootPath, releaseFileName);
            _approvalsFilePath = Path.Combine(_rootPath, approvalsFileName);
        }

        public void CreateInstaller(string installerName)
        {
            WriteToFile(installerName, _installersPath);
          
        }

        private static void WriteToFile(string installerName, string installersPath)
        {
            File.AppendAllLines(installersPath, new[] {installerName});
        }

        public void RegisterInstallerinRelease(string installerName)
        {
           WriteToFile(installerName, _releaseFilePath);
        }
        
        public void StoreApprovals(Dictionary<string, PipelineMessage> pipelineMessages)
        {
            string json = JsonConvert.SerializeObject(pipelineMessages);
            File.WriteAllText(_approvalsFilePath, json);
        }

        public Dictionary<string, PipelineMessage> ReadAllApprovals()
        {
            if (!File.Exists(_approvalsFilePath))
            {
                return new Dictionary<string, PipelineMessage>();
            }
            string json = File.ReadAllText(_approvalsFilePath);
            return JsonConvert.DeserializeObject<Dictionary<string, PipelineMessage>>(json);
        }

      
    }
}
