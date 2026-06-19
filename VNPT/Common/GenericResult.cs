namespace VNPT.Common
{
    public class GenericResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public GenericResult() { }
        public GenericResult(bool Success, string Message)
        {
            this.Success = Success;
            this.Message = Message;
        }
        public GenericResult(bool Success, string Message, object Data)
        {
            this.Success = Success;
            this.Message = Message;
            this.Data = Data;
        }
    }
}
