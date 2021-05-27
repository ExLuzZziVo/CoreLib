﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoreLib.CORE.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CoreLib.CORE.Resources.ValidationStrings", typeof(ValidationStrings).Assembly);
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
        ///   Looks up a localized string similar to The sequence &apos;{0}&apos; must contain no more than {1}.
        /// </summary>
        public static string CollectionMaxLengthError {
            get {
                return ResourceManager.GetString("CollectionMaxLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The sequence &apos;{0}&apos; must contain at least {1} elements.
        /// </summary>
        public static string CollectionMinLengthError {
            get {
                return ResourceManager.GetString("CollectionMinLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The sequence &apos;{0}&apos; must contain at least {1} and no more than {2} elements.
        /// </summary>
        public static string CollectionRangeLengthError {
            get {
                return ResourceManager.GetString("CollectionRangeLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fields &apos;{0}&apos; and &apos;{1}&apos; must match.
        /// </summary>
        public static string CompareError {
            get {
                return ResourceManager.GetString("CompareError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value of the field &apos;{0}&apos; must be greater than the value of the field &apos;{1}&apos;.
        /// </summary>
        public static string CompareToGreaterThanError {
            get {
                return ResourceManager.GetString("CompareToGreaterThanError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value of the field &apos;{0}&apos; must be greater than or equal to the value of the field &apos;{1}&apos;.
        /// </summary>
        public static string CompareToGreaterThanOrEqualError {
            get {
                return ResourceManager.GetString("CompareToGreaterThanOrEqualError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value of the field &apos;{0}&apos; must be less than the value of the field &apos;{1}&apos;.
        /// </summary>
        public static string CompareToSmallerThanError {
            get {
                return ResourceManager.GetString("CompareToSmallerThanError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value of the field &apos;{0}&apos; must be less than or equal to the value of the field &apos;{1}&apos;.
        /// </summary>
        public static string CompareToSmallerThanOrEqualError {
            get {
                return ResourceManager.GetString("CompareToSmallerThanOrEqualError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The sequence &apos;{0}&apos; contains elements with following errors.
        /// </summary>
        public static string ComplexObjectCollectionValidationError {
            get {
                return ResourceManager.GetString("ComplexObjectCollectionValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t be null.
        /// </summary>
        public static string ComplexObjectCollectionValidationError_ContainsNull {
            get {
                return ResourceManager.GetString("ComplexObjectCollectionValidationError_ContainsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Element {0}: .
        /// </summary>
        public static string ComplexObjectCollectionValidationError_Index {
            get {
                return ResourceManager.GetString("ComplexObjectCollectionValidationError_Index", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field &apos;{0}&apos; has the following errors.
        /// </summary>
        public static string ComplexObjectValidationError {
            get {
                return ResourceManager.GetString("ComplexObjectValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Date in field &apos;{0}&apos; must be in the range {1} - {2}.
        /// </summary>
        public static string DateTimeError {
            get {
                return ResourceManager.GetString("DateTimeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field &apos;{0}&apos; must have a value of at least {1} and no more than {2}.
        /// </summary>
        public static string DigitRangeValuesError {
            get {
                return ResourceManager.GetString("DigitRangeValuesError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An object with the same name already exists.
        /// </summary>
        public static string DuplicateNameError {
            get {
                return ResourceManager.GetString("DuplicateNameError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more files not found.
        /// </summary>
        public static string FileNotFoundError {
            get {
                return ResourceManager.GetString("FileNotFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while processing the file at line: {0}.
        /// </summary>
        public static string FileParseByLineError {
            get {
                return ResourceManager.GetString("FileParseByLineError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid e-mail address.
        /// </summary>
        public static string InvalidEmailAddressFormatError {
            get {
                return ResourceManager.GetString("InvalidEmailAddressFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more files have an invalid save path.
        /// </summary>
        public static string InvalidFilePathError {
            get {
                return ResourceManager.GetString("InvalidFilePathError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid phone number.
        /// </summary>
        public static string InvalidPhoneFormatError {
            get {
                return ResourceManager.GetString("InvalidPhoneFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Object ID not found.
        /// </summary>
        public static string NoIdError {
            get {
                return ResourceManager.GetString("NoIdError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Object not found.
        /// </summary>
        public static string NotFoundError {
            get {
                return ResourceManager.GetString("NotFoundError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field &apos;{0}&apos; is required.
        /// </summary>
        public static string RequiredError {
            get {
                return ResourceManager.GetString("RequiredError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Something went wrong:(.
        /// </summary>
        public static string SomethingWentWrong {
            get {
                return ResourceManager.GetString("SomethingWentWrong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field &apos;{0}&apos; has wrong format.
        /// </summary>
        public static string StringFormatError {
            get {
                return ResourceManager.GetString("StringFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field &apos;{0}&apos; must contain no more than {1} characters.
        /// </summary>
        public static string StringMaxLengthError {
            get {
                return ResourceManager.GetString("StringMaxLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field &apos;{0}&apos; must contain at least {1} characters.
        /// </summary>
        public static string StringMinLengthError {
            get {
                return ResourceManager.GetString("StringMinLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field &apos;{0}&apos; must contain at least {1} and no more than {2} characters.
        /// </summary>
        public static string StringRangeLengthError {
            get {
                return ResourceManager.GetString("StringRangeLengthError", resourceCulture);
            }
        }
    }
}
