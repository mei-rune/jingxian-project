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
        ///   查找类似 程序集 &apos;{0}&apos; 不可用. 它在&lt;assemblyFileSet&gt;中可能被过滤了. 的本地化字符串。
        /// </summary>
        internal static string AssemblyUnavailable {
            get {
                return ResourceManager.GetString("AssemblyUnavailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 不能为 context &apos;{0}&apos; 增加监听者. 的本地化字符串。
        /// </summary>
        internal static string CannotAddListenerForContext {
            get {
                return ResourceManager.GetString("CannotAddListenerForContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 不能为不存在的 context &apos;{0}&apos; 删除监听者.  的本地化字符串。
        /// </summary>
        internal static string CannotRemoveListenerForNonExistentContext {
            get {
                return ResourceManager.GetString("CannotRemoveListenerForNonExistentContext", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 组件[ id= &apos;{0}&apos; ] 没有找到. 可能被移除或卸载. 的本地化字符串。
        /// </summary>
        internal static string ComponentNotFound {
            get {
                return ResourceManager.GetString("ComponentNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 检测到试图实例化服务 &apos;{0}&apos; 两次. {1} 的本地化字符串。
        /// </summary>
        internal static string InstantiateServiceTwice {
            get {
                return ResourceManager.GetString("InstantiateServiceTwice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 没有为这个复合过滤器定义给件. 的本地化字符串。
        /// </summary>
        internal static string NoComponentsForCompositeFilterDefined {
            get {
                return ResourceManager.GetString("NoComponentsForCompositeFilterDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 当前其它实例化的服务: 的本地化字符串。
        /// </summary>
        internal static string OtherCurrentlyInstantiatedServices {
            get {
                return ResourceManager.GetString("OtherCurrentlyInstantiatedServices", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 字符串 &apos;{0}&apos; 不能为null或空. 的本地化字符串。
        /// </summary>
        internal static string StringArgumentNullOrEmpty {
            get {
                return ResourceManager.GetString("StringArgumentNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 违返写一次的语义. 的本地化字符串。
        /// </summary>
        internal static string WriteOnceSemanticViolated {
            get {
                return ResourceManager.GetString("WriteOnceSemanticViolated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 违返写一次的语义: 成员 &apos;{0}&apos; 只能设置一次. 的本地化字符串。
        /// </summary>
        internal static string WriteOnceSemanticViolatedMember {
            get {
                return ResourceManager.GetString("WriteOnceSemanticViolatedMember", resourceCulture);
            }
        }
    }
}
