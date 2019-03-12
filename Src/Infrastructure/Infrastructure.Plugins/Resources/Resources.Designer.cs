﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infrastructure.Plugins.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Infrastructure.Plugins.Resources.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to Plugin activated. Plugin Id=&apos;{0}&apos;, Plugin Type=&apos;{1}&apos;.
        /// </summary>
        internal static string PluginActivator_Activate_PluginActivated {
            get {
                return ResourceManager.GetString("PluginActivator_Activate_PluginActivated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin already registered in container. PluginType=&apos;{0}&apos;.
        /// </summary>
        internal static string PluginContainerManger_Register_ {
            get {
                return ResourceManager.GetString("PluginContainerManger_Register_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin already registered in container. PluginType=&apos;{0}&apos;.
        /// </summary>
        internal static string PluginContainerManger_Register_PluginAlreadyRegistered {
            get {
                return ResourceManager.GetString("PluginContainerManger_Register_PluginAlreadyRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin already registered in container. PluginType=&apos;{0}&apos;.
        /// </summary>
        internal static string PluginContainerManger_Register_PluginRegistered {
            get {
                return ResourceManager.GetString("PluginContainerManger_Register_PluginRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin resolved from container. PluginType=&apos;{0}&apos;.
        /// </summary>
        internal static string PluginContainerManger_Resolve_PluginResolved {
            get {
                return ResourceManager.GetString("PluginContainerManger_Resolve_PluginResolved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin prepared for usage. PluginId=&apos;{0}&apos;, ProjectId=&apos;{1}&apos;, UserId=&apos;{2}&apos;.
        /// </summary>
        internal static string PluginFactory_Prepare_PluginPrepared {
            get {
                return ResourceManager.GetString("PluginFactory_Prepare_PluginPrepared", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugins initialization finished. InitializedPluginTypesCount=&apos;{0}&apos;.
        /// </summary>
        internal static string PluginInitializer_InitializePlugins_Plugins_Initialization_finished {
            get {
                return ResourceManager.GetString("PluginInitializer_InitializePlugins_Plugins_Initialization_finished", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugins initialization started..
        /// </summary>
        internal static string PluginInitializer_InitializePlugins_Plugins_initialization_started {
            get {
                return ResourceManager.GetString("PluginInitializer_InitializePlugins_Plugins_initialization_started", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin already registered. PluginTypeFullName=&apos;{0}&apos;, PluginAssemblyName=&apos;{1}&apos;, Type=&apos;{2}&apos;.
        /// </summary>
        internal static string PluginProvider_Initialize_PluginAlreadyRegistered {
            get {
                return ResourceManager.GetString("PluginProvider_Initialize_PluginAlreadyRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin not implements any plugin interfaces except base IPlugin. PluginTypeFullName=&apos;{0}&apos;, PluginAssemblyName=&apos;{1}&apos;.
        /// </summary>
        internal static string PluginProvider_Initialize_PluginNotImplementsInterfaces {
            get {
                return ResourceManager.GetString("PluginProvider_Initialize_PluginNotImplementsInterfaces", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin initialized. AssemblyName=&apos;{0}&apos;, PluginTypeName=&apos;{1}&apos;, PluginType=&apos;{2}&apos;, PluginId=&apos;{3}&apos;.
        /// </summary>
        internal static string PluginProvider_LogInitializedPlugin_PluginInitialized {
            get {
                return ResourceManager.GetString("PluginProvider_LogInitializedPlugin_PluginInitialized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin setting initialized. PluginId=&apos;{0}&apos;, SettingKey=&apos;{1}&apos;.
        /// </summary>
        internal static string PluginSettingProvider_Initialize_SettingInitialized {
            get {
                return ResourceManager.GetString("PluginSettingProvider_Initialize_SettingInitialized", resourceCulture);
            }
        }
    }
}
