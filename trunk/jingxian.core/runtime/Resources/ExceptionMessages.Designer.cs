﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.3053
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace jingxian.core.runtime.Resources {
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
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("jingxian.core.runtime.Resources.ExceptionMessages", typeof(ExceptionMessages).Assembly);
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
        ///   查找类似 The Assembly &apos;{0}&apos; is unavailable. This might be due to excluding it from the Platform&apos;s &lt;assemblyFileSet&gt;. 的本地化字符串。
        /// </summary>
        internal static string AssemblyUnavailable {
            get {
                return ResourceManager.GetString("AssemblyUnavailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot add same listener for context &apos;{0}&apos;. Use contains methods to verify. 的本地化字符串。
        /// </summary>
        internal static string CannotAddListenerForContext {
            get {
                return ResourceManager.GetString("CannotAddListenerForContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot remove listener for non-existent context &apos;{0}&apos;. Use contains methods to verify. 的本地化字符串。
        /// </summary>
        internal static string CannotRemoveListenerForNonExistentContext {
            get {
                return ResourceManager.GetString("CannotRemoveListenerForNonExistentContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The component with Id &apos;{0}&apos; could not be found. This might be due to removal or unloading of PlugIns. Also check your Id to make sure you do not have a typo in it. 的本地化字符串。
        /// </summary>
        internal static string ComponentNotFound {
            get {
                return ResourceManager.GetString("ComponentNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Attempt detected to instantiate service of type &apos;{0}&apos; twice. This is not supported yet. {1} 的本地化字符串。
        /// </summary>
        internal static string InstantiateServiceTwice {
            get {
                return ResourceManager.GetString("InstantiateServiceTwice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 No components for this composite filter defined 的本地化字符串。
        /// </summary>
        internal static string NoComponentsForCompositeFilterDefined {
            get {
                return ResourceManager.GetString("NoComponentsForCompositeFilterDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Other currently instantiated services are: 的本地化字符串。
        /// </summary>
        internal static string OtherCurrentlyInstantiatedServices {
            get {
                return ResourceManager.GetString("OtherCurrentlyInstantiatedServices", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The string argument &apos;{0}&apos; cannot be null or empty. 的本地化字符串。
        /// </summary>
        internal static string StringArgumentNullOrEmpty {
            get {
                return ResourceManager.GetString("StringArgumentNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Write Once semantic violated. 的本地化字符串。
        /// </summary>
        internal static string WriteOnceSemanticViolated {
            get {
                return ResourceManager.GetString("WriteOnceSemanticViolated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Write Once semantic violated: The member &apos;{0}&apos; is only writable once and was already set. 的本地化字符串。
        /// </summary>
        internal static string WriteOnceSemanticViolatedMember {
            get {
                return ResourceManager.GetString("WriteOnceSemanticViolatedMember", resourceCulture);
            }
        }
    }
}