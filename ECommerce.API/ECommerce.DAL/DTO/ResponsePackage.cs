namespace ECommerce.DAL.DTO
{
    public class ResponsePackage<T> : ResponsePackageNoData
    {
        public T TransferObject { get; set; }

        public ResponsePackage()
        {
            Status = ResponseStatus.Ok;
            Message = string.Empty;
        }

        public ResponsePackage(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public ResponsePackage(T data, int status = 200, string message = "")
        {
            TransferObject = data;
            Status = status;
            Message = message;
        }
    }

    public class ResponsePackageNoData
    {
        public string Message { get; set; }
        public int Status { get; set; }

        public ResponsePackageNoData()
        {
            Status = ResponseStatus.Ok;
            Message = string.Empty;
        }

        public ResponsePackageNoData(int status, string message)
        {
            Status = status;
            Message = message;
        }
    }

    public static class ResponseStatus
    {
        public const int Ok = 200;
        public const int Error = 400;
        public const int InternalServerError = 500;
        public const int Unautorized = 401;
    }
}
