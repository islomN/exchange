namespace ExchangeApi.Logics
{
    public class TBaseResult<T>
    {
        public bool Success { get; set; } = true;
        
        public T Result { get; set; }
        
        public string Message { get; set; }

        public TBaseResult()
        {
        }

        public TBaseResult(T result)
        {
            Result = result;
        }

        public TBaseResult(string message)
        {
            Message = message;
            Success = false;
        }

        public TBaseResult(string message, bool success)
        {
            Message = message;
            Success = success;
        }
    }
}