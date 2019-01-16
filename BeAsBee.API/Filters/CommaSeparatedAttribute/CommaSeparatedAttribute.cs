using System;

namespace BeAsBee.API.Filters.CommaSeparatedAttribute {
    [AttributeUsage( AttributeTargets.Parameter )]
    public class CommaSeparatedAttribute : Attribute {
    }
}