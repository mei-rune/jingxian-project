﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.3082
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace jingxian.core.runtime.simpl.Resources {
    using System;
    
    
    /// <summary>
    ///   强类型资源类，用于查找本地化字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("jingxian.core.runtime.simpl.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   为使用此强类型资源类的所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 参数 &apos;{0}&apos; 不是一个有效的类. 的本地化字符串。
        /// </summary>
        internal static string ArgumentIsNotValidClassType {
            get {
                return ResourceManager.GetString("ArgumentIsNotValidClassType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 程序集 &apos;{0}&apos; 不包含 bundle 定義配置文件 (Bundle.xml). 的本地化字符串。
        /// </summary>
        internal static string AssemblyContainsNoBundleDefinition {
            get {
                return ResourceManager.GetString("AssemblyContainsNoBundleDefinition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &quot;新增一个 bundle [ name =&apos;{0}&apos;, id= &apos;{1}&apos;, assembly= &apos;{2}&apos;].&quot; 的本地化字符串。
        /// </summary>
        internal static string BundleAddedInfo {
            get {
                return ResourceManager.GetString("BundleAddedInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 跳过 bundle  [ name =&apos;{0}&apos;, id= &apos;{1}&apos;, assembly= &apos;{2}&apos;] (已有一个相同的 id 的 bundle [ name= &apos;{3}&apos;, assembly= &apos;{4}&apos;]). 的本地化字符串。
        /// </summary>
        internal static string BundleSkippedDueToDuplicateId {
            get {
                return ResourceManager.GetString("BundleSkippedDueToDuplicateId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 跳过扩展点[ name= &apos;{0}&apos;, id= &apos;{1}&apos;, bundlePath= &apos;{2}&apos;] (已有一个相同的 id 的扩展点[ bundleName=&apos;{3}&apos;,bundlePath= &apos;{4}&apos;]). 的本地化字符串。
        /// </summary>
        internal static string ExtensionPointSkippedDueToDuplicateId {
            get {
                return ResourceManager.GetString("ExtensionPointSkippedDueToDuplicateId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 跳过扩展[ name=&apos;{0}&apos;, id &apos;{1}&apos;,bundlePath=  &apos;{2}&apos;] (已有一个相同的 id 的扩展[ bundleName=&apos;{3}&apos;,bundlePath= &apos;{4}&apos;]). 的本地化字符串。
        /// </summary>
        internal static string ExtensionSkippedDueToDuplicateId {
            get {
                return ResourceManager.GetString("ExtensionSkippedDueToDuplicateId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 增加服务发生错误[ id= &apos;{0}&apos;]. 的本地化字符串。
        /// </summary>
        internal static string FailedToAddService {
            get {
                return ResourceManager.GetString("FailedToAddService", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 反序列化配置节点[ name= &apos;{0}&apos;, extensionId= &apos;{1}&apos;]发生错误. 的本地化字符串。
        /// </summary>
        internal static string FailedToDeserializeConfiguration {
            get {
                return ResourceManager.GetString("FailedToDeserializeConfiguration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 从资源[ name=&apos;{0}&apos;,assembly=&apos;{1}&apos;,assemblyPath=&apos;{2}&apos; ]中载入 bundle 失败. 的本地化字符串。
        /// </summary>
        internal static string FailedToLoadBundleFromResource {
            get {
                return ResourceManager.GetString("FailedToLoadBundleFromResource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot launch context &apos;{0}&apos; (platform already launched with context &apos;{1}&apos;). 的本地化字符串。
        /// </summary>
        internal static string InvalidOperationExceptionDueToSecondLaunchAttempt {
            get {
                return ResourceManager.GetString("InvalidOperationExceptionDueToSecondLaunchAttempt", resourceCulture);
            }
        }
    }
}
