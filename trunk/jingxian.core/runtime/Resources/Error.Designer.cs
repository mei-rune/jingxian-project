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
    internal class Error {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Error() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("jingxian.core.runtime.Resources.Error", typeof(Error).Assembly);
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
        ///   查找类似 不能在 {1} 处添加参数 &apos;{0}&apos; (参数只能出现一次，便已经存在了 {2}). 的本地化字符串。
        /// </summary>
        internal static string ArgumentIsAllowedOnlyOnceAndWasAlreadyGiven {
            get {
                return ResourceManager.GetString("ArgumentIsAllowedOnlyOnceAndWasAlreadyGiven", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Arguments 的本地化字符串。
        /// </summary>
        internal static string Arguments {
            get {
                return ResourceManager.GetString("Arguments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 不能处理参数[ &apos;{0}&apos; , position = {1} ](参数至少有2个字节). 的本地化字符串。
        /// </summary>
        internal static string ArgumentsMustHaveMinimumLength {
            get {
                return ResourceManager.GetString("ArgumentsMustHaveMinimumLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 不能处理参数[ &apos;{0}&apos; , position = {1} ] (参数必须有一个前缀 &apos;{2}&apos;, &apos;{3}&apos; 或 &apos;{4}&apos;). 的本地化字符串。
        /// </summary>
        internal static string ArgumentsMustHavePrefix {
            get {
                return ResourceManager.GetString("ArgumentsMustHavePrefix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 参数{1} 的 {0} 成功解析. 的本地化字符串。
        /// </summary>
        internal static string ArgumentSuccessfullyParsed {
            get {
                return ResourceManager.GetString("ArgumentSuccessfullyParsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 属性 &apos;{0}&apos; 是必须的 的本地化字符串。
        /// </summary>
        internal static string AttribteIsRequired {
            get {
                return ResourceManager.GetString("AttribteIsRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 命令行分析结果 的本地化字符串。
        /// </summary>
        internal static string CommandLineParseResults {
            get {
                return ResourceManager.GetString("CommandLineParseResults", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 缺省命令 的本地化字符串。
        /// </summary>
        internal static string DefaultCommands {
            get {
                return ResourceManager.GetString("DefaultCommands", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 不能处理[ &apos;{0}&apos;, position= {1}] (没有匹配任何已知的参数). 的本地化字符串。
        /// </summary>
        internal static string DoNotMatchAnyArgument {
            get {
                return ResourceManager.GetString("DoNotMatchAnyArgument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 载入 schema 发生错误:  的本地化字符串。
        /// </summary>
        internal static string ErrorLoadingSchema {
            get {
                return ResourceManager.GetString("ErrorLoadingSchema", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 错误 的本地化字符串。
        /// </summary>
        internal static string Errors {
            get {
                return ResourceManager.GetString("Errors", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 分析参数[ position = &apos;{0}&apos;]发生错误.(参数为空) 的本地化字符串。
        /// </summary>
        internal static string FailedToParseArgument {
            get {
                return ResourceManager.GetString("FailedToParseArgument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 读命名空问 {1} 下的 {0} 必选节点时发生错误. 的本地化字符串。
        /// </summary>
        internal static string FailedToReadNonOptionalElement {
            get {
                return ResourceManager.GetString("FailedToReadNonOptionalElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 # 格式: 文件 (dll|exe), 版本, 机器, PEKinds, 创建 的本地化字符串。
        /// </summary>
        internal static string FileVersionFormat {
            get {
                return ResourceManager.GetString("FileVersionFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 # 目录 {1} 中的文件 {0} 的版本信息 {1} 的本地化字符串。
        /// </summary>
        internal static string FileVersionInfoOfFilesInDirectory {
            get {
                return ResourceManager.GetString("FileVersionInfoOfFilesInDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 忽略参数[ &apos;{0}&apos; , position = {1}] (参数不允许出现多次，它已经在 {2} 出现). 的本地化字符串。
        /// </summary>
        internal static string IgnoringArgument {
            get {
                return ResourceManager.GetString("IgnoringArgument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Key {0} 不存在 的本地化字符串。
        /// </summary>
        internal static string KeyNotExistInObjectCache {
            get {
                return ResourceManager.GetString("KeyNotExistInObjectCache", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 只能有一个参数 (找到多个 &apos;{0}&apos; 参数). 的本地化字符串。
        /// </summary>
        internal static string MultipleArgumentsWithIdentifierFound {
            get {
                return ResourceManager.GetString("MultipleArgumentsWithIdentifierFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 没有参数. 的本地化字符串。
        /// </summary>
        internal static string NoArgumentsGiven {
            get {
                return ResourceManager.GetString("NoArgumentsGiven", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0} 个解析错误. 的本地化字符串。
        /// </summary>
        internal static string ParseErrorDetected {
            get {
                return ResourceManager.GetString("ParseErrorDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 必选属性 &apos;{0}&apos; 错误或为空. 的本地化字符串。
        /// </summary>
        internal static string RequiredAttributeIsMissingOrEmpty {
            get {
                return ResourceManager.GetString("RequiredAttributeIsMissingOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法取得参数 (&apos;{0}&apos; 不是一个有效的Identifier). 的本地化字符串。
        /// </summary>
        internal static string UknownIdentifier {
            get {
                return ResourceManager.GetString("UknownIdentifier", resourceCulture);
            }
        }
    }
}