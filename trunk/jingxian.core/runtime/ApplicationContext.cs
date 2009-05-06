using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{
    using jingxian.core.runtime.Filters;

    public class ApplicationContext : AbstractServiceContainer, IApplicationContext
    {
        public static string ProductName = string.Empty;
        public static string CompanyName = string.Empty;
        public static string ProductVersion = string.Empty;

        string _name;
        string _bundlePath;
        AssemblyFileSet _availableAssemblies;
        bool _scanForBundlesInSecondAppDomain;
        string _applicationLaunchableId;
        ICommandLineArguments _arguments;
        IProperties _misc = new MapProperties();


        static ApplicationContext()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            AssemblyCompanyAttribute companyAttr;
            AssemblyProductAttribute productAttr;

            object[] attributes = asm.GetCustomAttributes(true);
            for (int i = 0; i < attributes.Length; i++)
            {
                if ((companyAttr = attributes[i] as AssemblyCompanyAttribute) != null
                    && !string.IsNullOrEmpty(companyAttr.Company))
                {
                    CompanyName = companyAttr.Company;
                    continue;
                }
                else if ((productAttr = attributes[i] as AssemblyProductAttribute) != null
                    && !string.IsNullOrEmpty(productAttr.Product))
                {
                    ProductName = productAttr.Product;
                    continue;
                }
            }

            System.Diagnostics.FileVersionInfo fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(asm.Location);
            ProductVersion = fileVersionInfo.FileVersion;
        }

        static string GetRunPath()
        {
            return System.IO.Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location );
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string BundlePath
        {
            get { return string.IsNullOrEmpty(_bundlePath)?GetRunPath():_bundlePath; }
            set { _bundlePath = value; }
        }

        public AssemblyFileSet AvailableAssemblies
        {
            get 
            {
                if (null == _availableAssemblies)
                {
                    _availableAssemblies = new AssemblyFileSet();
                    _availableAssemblies.PositiveIncludeByDefault = true;
                }
                return _availableAssemblies; 
            }
        }

        public bool ScanForBundlesInSecondAppDomain
        {
            get { return _scanForBundlesInSecondAppDomain; }
            set { _scanForBundlesInSecondAppDomain = value; }
        }

        public string ApplicationLaunchableId
        {
            get { return _applicationLaunchableId; }
            set { _applicationLaunchableId = value; }
        }

        public IProperties Misc
        {
            get { return _misc; }
        }

        #region IApplicationContext 成员


        public ICommandLineArguments Arguments
        {
            get { return _arguments; }
            set { _arguments = value; }
        }

        #endregion
    }
}
