using System;

namespace JSPWebsite.WebAPI.Filters.CommaSeparatedAttribute {
    [AttributeUsage( AttributeTargets.Parameter )]
    public class CommaSeparatedAttribute : Attribute {
    }
}