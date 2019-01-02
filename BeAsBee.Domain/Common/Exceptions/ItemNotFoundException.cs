using System;

namespace BeAsBee.Domain.Common.Exceptions {
    public class ItemNotFoundException : Exception {
        public ItemNotFoundException () {
        }

        public ItemNotFoundException ( string errorMessage )
            : base( errorMessage ) {
        }
    }
}