using API.App.DTO;
using System;

namespace API
{
    public sealed class Result<TValue>
        where TValue : notnull
    {
        #region Variables
        private readonly bool isSuccess;
        private readonly TValue value;
        private readonly ResultErrorDto error;
        #endregion



        #region Constructor Method
        public Result(bool IsSuccess,
                      TValue Value,
                      ResultErrorDto Error)
            : base()
        {
            this.isSuccess = IsSuccess;
            this.value = Value;
            this.error = Error;
        }
        #endregion



        #region Field
        public bool IsSuccess
        {
            get => this.isSuccess;
        }

        public TValue Value
        {
            get
            {
                if (!this.isSuccess)
                {
                    throw new Exception(message: "Operación inválida: no se puede obtener el valor de un resultado fallido.");
                }
                
                return this.value;
            }
        }

        public ResultErrorDto Error
        {
            get
            {
                if (this.isSuccess)
                {
                    throw new Exception(message: "Operación inválida: no se puede obtener el error de un resultado exitoso.");
                }
                
                return this.error;
            }
        }
        #endregion



        public static Result<TValue> Success(TValue Value)
        {
            return new Result<TValue>(IsSuccess: true, Value: Value, Error: default!);
        }

        public static Result<TValue> Failure(ResultErrorDto Error)
        {
            return new Result<TValue>(IsSuccess: false, Value: default!, Error: Error);
        }
    }
}