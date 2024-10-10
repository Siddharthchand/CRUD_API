using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace User_API.ExceptionHandlerMiddleware
{
    public enum ErrorCode
    {
        [Description("Success")]
        Success = 0,
        [Description("Not found")]
        NotFound = 1,
        [Description("User Terminated")]
        TerminatedUser = 2,
        [Description("Invalid credentials")]
        Unauthorised = 3,
        [Description("Invalid token")]
        InvalidToken = 4,
        [Description("User already exists")]
        AlreadyExists=5,
        [Description("Credentials format wrong")]
        FormatWrong=6
    }

    public class CommonResponse(ErrorCode ErrorCode, object? Data=null)
    {
        public ErrorCode ErrorCode { get; set; } = ErrorCode;
        public string ErrorMessage {  get; set; }=ErrorCode.GetMessage();
        public object? Data { get; set; } = Data;
    }

    public static class ErrorMessage
    {
        public static string GetMessage(this System.Enum value)
        {
            FieldInfo? fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo != null)
            {
                DescriptionAttribute? attribute = (DescriptionAttribute?)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }
            return value.ToString();
        }
    }
    
}
