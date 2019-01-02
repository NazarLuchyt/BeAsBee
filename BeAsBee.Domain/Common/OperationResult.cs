using System;

namespace BeAsBee.Domain.Common {
    public class OperationResult {
        public OperationResult () {
        }

        public OperationResult ( object value ) {
            Value = value;
            IsSuccess = true;
        }

        public OperationResult ( Exception ex ) {
            Exception = ex;
            IsSuccess = false;
        }

        public object Value { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
    }

    public class OperationResult<T> where T : class {
        public OperationResult () {
        }

        public OperationResult ( T value ) {
            Value = value;
            IsSuccess = true;
        }

        public OperationResult ( Exception ex ) {
            Exception = ex;
            IsSuccess = false;
        }

        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
    }
}