﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BeAsBee.Domain.Resources {
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
    public class Translations {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Translations() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BeAsBee.Domain.Resources.Translations", typeof(Translations).Assembly);
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
        ///   Looks up a localized string similar to The number of items per page cannot be null.
        /// </summary>
        public static string COUNT_CANNOT_BE_NULL {
            get {
                return ResourceManager.GetString("COUNT_CANNOT_BE_NULL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please choose correct number of images..
        /// </summary>
        public static string ENTITY_HAVE_INCORRECT_NUMBER_OF_IMAGES {
            get {
                return ResourceManager.GetString("ENTITY_HAVE_INCORRECT_NUMBER_OF_IMAGES", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} with id {1} is not found.
        /// </summary>
        public static string ENTITY_WITH_ID_NOT_FOUND {
            get {
                return ResourceManager.GetString("ENTITY_WITH_ID_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception occured while processing request: {0}.
        /// </summary>
        public static string EXCEPTION_OCCURED_WHILE_PROCESSING_REQUEST {
            get {
                return ResourceManager.GetString("EXCEPTION_OCCURED_WHILE_PROCESSING_REQUEST", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception message: {0}.
        /// </summary>
        public static string EXEPTION_MESSAGE {
            get {
                return ResourceManager.GetString("EXEPTION_MESSAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incorrect number of lists to filter vacancies..
        /// </summary>
        public static string INCORRECT_NUMBER_OF_LISTS_TO_FILTER_VACANCIES {
            get {
                return ResourceManager.GetString("INCORRECT_NUMBER_OF_LISTS_TO_FILTER_VACANCIES", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inner exception:  {0}.
        /// </summary>
        public static string INNER_EXCEPTION {
            get {
                return ResourceManager.GetString("INNER_EXCEPTION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stack trace:  {0}.
        /// </summary>
        public static string STACK_TRACE {
            get {
                return ResourceManager.GetString("STACK_TRACE", resourceCulture);
            }
        }
    }
}