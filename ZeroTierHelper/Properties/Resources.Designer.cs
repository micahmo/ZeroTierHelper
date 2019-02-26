﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZeroTierHelper.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ZeroTierHelper.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Access to your API token is needed to retrieve network and member data. Click on Settings to enter your API token..
        /// </summary>
        internal static string APITokenHelp {
            get {
                return ResourceManager.GetString("APITokenHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This data is static, so at any time you may press Refresh to retrieve the latest data..
        /// </summary>
        internal static string DataHelp {
            get {
                return ResourceManager.GetString("DataHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Description.
        /// </summary>
        internal static string Description {
            get {
                return ResourceManager.GetString("Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.
        /// </summary>
        internal static string Error {
            get {
                return ResourceManager.GetString("Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File Share.
        /// </summary>
        internal static string FileShare {
            get {
                return ResourceManager.GetString("FileShare", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to —————————— ZeroTier Helper Client ——————————.
        /// </summary>
        internal static string HelpWindowHeader {
            get {
                return ResourceManager.GetString("HelpWindowHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ID.
        /// </summary>
        internal static string ID {
            get {
                return ResourceManager.GetString("ID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IP Assignments.
        /// </summary>
        internal static string IPAssignments {
            get {
                return ResourceManager.GetString("IPAssignments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to API key is needed to access ZeroTier data. Please click on Settings and enter your API key..
        /// </summary>
        internal static string MissingAPITokenError {
            get {
                return ResourceManager.GetString("MissingAPITokenError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are multiple IP addressed assigned to this member, which is currently unsupported..
        /// </summary>
        internal static string MultipleIPsAssignedToMember {
            get {
                return ResourceManager.GetString("MultipleIPsAssignedToMember", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name.
        /// </summary>
        internal static string Name {
            get {
                return ResourceManager.GetString("Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Once your data is retrieved, each network will be shown on a new tab. Within that tab, a list of members will be displayed in a grid..
        /// </summary>
        internal static string NetworkHelp {
            get {
                return ResourceManager.GetString("NetworkHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are no IP addresses assigned to this member..
        /// </summary>
        internal static string NoIPsAssignedToMember {
            get {
                return ResourceManager.GetString("NoIPsAssignedToMember", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Online.
        /// </summary>
        internal static string Online {
            get {
                return ResourceManager.GetString("Online", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ping.
        /// </summary>
        internal static string Ping {
            get {
                return ResourceManager.GetString("Ping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RDP.
        /// </summary>
        internal static string RDP {
            get {
                return ResourceManager.GetString("RDP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Version {0}.
        /// </summary>
        internal static string VersionNumber {
            get {
                return ResourceManager.GetString("VersionNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error retrieving web request: &quot;{0}&quot;.
        /// </summary>
        internal static string WebRequsetError {
            get {
                return ResourceManager.GetString("WebRequsetError", resourceCulture);
            }
        }
    }
}
