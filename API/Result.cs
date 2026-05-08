namespace API
{
    public class Result<T>
    {
        #region Variables
        private bool isSuccess;
        private T value;
        private string error;
        #endregion



        #region Constructor Method
        public Result(bool IsSuccess,
                      T Value,
                      string Error)
            : base()
        {
            this.isSuccess = IsSuccess;
            this.value = Value;
            this.error = Error;
        }
        #endregion



        #region Field
        public T Value
        {
            get => this.value;
        }

        public bool IsSuccess
        {
            get => this.isSuccess;
        }

        public string Error
        {
            get => this.error;
        }
        #endregion



        public static Result<T> Success(T Value)
        {
            return new Result<T>(true, Value, null!);
        }

        public static Result<T> Failure(string Error)
        {
            return new Result<T>(false, default!, Error);
        }
    }
}