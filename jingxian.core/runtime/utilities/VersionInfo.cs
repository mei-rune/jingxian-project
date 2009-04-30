
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace jingxian.core.utilities
{
	public sealed class VersionInfo
	{
		private VersionInfo()
		{

		}

		public static string GetFileVersionInfoAsPlainTextString()
		{
			DirectoryInfo baseDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
			FileInfo[] dllFiles = baseDir.GetFiles("*.dll", SearchOption.AllDirectories);
			FileInfo[] exeFiles = baseDir.GetFiles("*.exe", SearchOption.AllDirectories);
			List<FileInfo> files = new List<FileInfo>(exeFiles);
			files.AddRange(dllFiles);
			files.Sort(delegate(FileInfo x, FileInfo y)
								{
									return StringComparer.OrdinalIgnoreCase.Compare(x.Name, y.Name);
								});

			StringBuilder builder = new StringBuilder();
            builder.AppendFormat(Error.FileVersionInfoOfFilesInDirectory, files.Count, baseDir);
			builder.AppendLine();
            builder.AppendLine(Error.FileVersionFormat);
			builder.AppendLine();

			foreach (FileInfo file in files)
			{
				try
				{
					FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(file.Name);
					Assembly asm = Assembly.ReflectionOnlyLoadFrom(file.FullName);

					PortableExecutableKinds peKinds;
					ImageFileMachine imgFileMachine;
					asm.ManifestModule.GetPEKind(out peKinds, out imgFileMachine);

					builder.AppendFormat("{0}, {1}, {2}, {3}, {4}",
						file.Name, fileVersionInfo.FileVersion, imgFileMachine, peKinds, file.CreationTimeUtc);
				}
				catch (Exception)
				{
					builder.AppendFormat("{0}, , , , {1}", file.Name, file.CreationTimeUtc);
				}
				builder.AppendLine();
			}

			return builder.ToString();
		}
	}
}
