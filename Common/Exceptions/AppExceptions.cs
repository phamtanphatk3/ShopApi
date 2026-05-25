namespace ShopApi.Common.Exceptions
{
    // Exception dung cho truong hop request khong hop le.
    public class AppBadRequestException : Exception
    {
        public AppBadRequestException(string message) : base(message) { }
    }

    // Exception dung cho truong hop khong tim thay du lieu.
    public class AppNotFoundException : Exception
    {
        public AppNotFoundException(string message) : base(message) { }
    }

    // Exception dung cho truong hop conflict du lieu.
    public class AppConflictException : Exception
    {
        public AppConflictException(string message) : base(message) { }
    }

    // Exception dung cho truong hop chua dang nhap/khong du quyen.
    public class AppUnauthorizedException : Exception
    {
        public AppUnauthorizedException(string message) : base(message) { }
    }

    // Exception dung cho truong hop co dang nhap nhung khong du quyen.
    public class AppForbiddenException : Exception
    {
        public AppForbiddenException(string message) : base(message) { }
    }
}
