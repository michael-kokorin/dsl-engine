﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infrastructure.Notifications.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Infrastructure.Notifications.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Notification already exists. Project Id=&apos;{0}&apos;, Rule name=&apos;{1}&apos;.
        /// </summary>
        internal static string NotificationAlreadyExists {
            get {
                return ResourceManager.GetString("NotificationAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Notification has been created. Id=&apos;{0}&apos;, Name=&apos;{1}&apos;.
        /// </summary>
        internal static string NotificationHasBeenCreated {
            get {
                return ResourceManager.GetString("NotificationHasBeenCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Notification has been published. Title=&apos;{0}&apos;, Protocol=&apos;{1}&apos;, Target=&apos;{2}&apos;.
        /// </summary>
        internal static string NotificationHasBeenPublished {
            get {
                return ResourceManager.GetString("NotificationHasBeenPublished", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Notification has been read from queue to process. Title=&apos;{0}&apos;, Protocol=&apos;{1}&apos;, Targets=&apos;{2}&apos;.
        /// </summary>
        internal static string NotificationHasBeenReadFromQueue {
            get {
                return ResourceManager.GetString("NotificationHasBeenReadFromQueue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Notification has been sent. Title=&apos;{0}&apos;, Protocol=&apos;{1}&apos;, Targets=&apos;{2}&apos;.
        /// </summary>
        internal static string NotificationHasBeenSent {
            get {
                return ResourceManager.GetString("NotificationHasBeenSent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SMTP message has been sent. Title=&apos;{0}&apos;, Recipients=&apos;{1}&apos;, Is HTML=&apos;{2}&apos;.
        /// </summary>
        internal static string SmptMessageHasBeenSent {
            get {
                return ResourceManager.GetString("SmptMessageHasBeenSent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no one unprocessed notification in queue..
        /// </summary>
        internal static string ThereIsNoOneUnprocessedNotificationInQueue {
            get {
                return ResourceManager.GetString("ThereIsNoOneUnprocessedNotificationInQueue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown notification protocol type. Type=&apos;{0}&apos;.
        /// </summary>
        internal static string UnknownNotificationProtocolType {
            get {
                return ResourceManager.GetString("UnknownNotificationProtocolType", resourceCulture);
            }
        }
    }
}