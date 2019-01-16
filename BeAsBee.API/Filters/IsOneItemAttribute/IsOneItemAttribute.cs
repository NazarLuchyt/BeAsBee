using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Castle.Core.Internal;

namespace BeAsBee.API.Filters.IsOneItemAttribute {
    public class IsOneItemAttribute : ValidationAttribute {
        private readonly int[] _imageTypes;
        private readonly string _propertyName;

        public IsOneItemAttribute ( string propertyName, params int[] imageTypes ) {
            _propertyName = propertyName;
            _imageTypes = imageTypes;
        }

        public override bool IsValid ( object value ) {
            if ( value is IEnumerable<object> currentValue ) {
                var currentList = currentValue.ToList();
                if ( currentList.IsNullOrEmpty() ) {
                    return false;
                }

                var firstItem = currentList.FirstOrDefault();
                if ( firstItem == null ) {
                    return false;
                }

                foreach ( var imageType in _imageTypes ) {
                    var propertyToCheck = firstItem.GetType().GetProperty( _propertyName );
                    if ( currentList.Count( item => ( int ) propertyToCheck.GetValue( item, null ) == imageType ) != 1 ) {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}