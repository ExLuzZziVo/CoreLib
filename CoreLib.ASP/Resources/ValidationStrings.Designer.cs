﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoreLib.ASP.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ValidationStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ValidationStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CoreLib.ASP.Resources.ValidationStrings", typeof(ValidationStrings).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file &apos;{0}&apos; has the incorrect format.
        /// </summary>
        public static string FileFormatError {
            get {
                return ResourceManager.GetString("FileFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please complete the captcha.
        /// </summary>
        public static string ReCaptchaValidationError {
            get {
                return ResourceManager.GetString("ReCaptchaValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number of uploaded files must be no more than {0}.
        /// </summary>
        public static string UploadFileCountError {
            get {
                return ResourceManager.GetString("UploadFileCountError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The uploaded file has the incorrect format.
        /// </summary>
        public static string UploadFileFormatError {
            get {
                return ResourceManager.GetString("UploadFileFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The size of the uploaded file must not exceed {0}.
        /// </summary>
        public static string UploadFileSizeError {
            get {
                return ResourceManager.GetString("UploadFileSizeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///The size of the uploaded file &apos;{0}&apos; must not exceed {1}.
        /// </summary>
        public static string UploadFileWithDisplayNameSizeError {
            get {
                return ResourceManager.GetString("UploadFileWithDisplayNameSizeError", resourceCulture);
            }
        }
    }
}
