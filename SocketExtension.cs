using System;
using System.Reflection;
using Oxide.Core;
using Oxide.Core.Extensions;

namespace Oxide.Ext.Socket
{
    public class SocketExtension : Extension
    {
        internal static Assembly Assembly = Assembly.GetExecutingAssembly();
        internal static AssemblyName AssemblyName = Assembly.GetName();
        internal static VersionNumber AssemblyVersion = new VersionNumber(AssemblyName.Version.Major, AssemblyName.Version.Minor, AssemblyName.Version.Build);
        internal static string AssemblyAuthors = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly, typeof(AssemblyCompanyAttribute), false)).Company;

        private readonly Socket Library;
        
        public SocketExtension(ExtensionManager manager) : base(manager)
        {
            Library = new Socket();
        }

        public override string Name => "Socket";
        public override string Author => AssemblyAuthors;
        public override VersionNumber Version => AssemblyVersion;
        
        public override void Load()
        {
            Manager.RegisterLibrary("Socket", Library);
        }

        public override void OnModLoad()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Interface.Oxide.LogException("Socket Unhandled Exception", (Exception) args.ExceptionObject);
            };
            Library.Initialize();
        }
        
        public override bool SupportsReloading => false;
    }
}