namespace Good.Admin.Common.Primitives
{
    public class ErrorResult : AjaxResult
    {
        public ErrorResult(string msg = "操作失败!", int errorCode = 0)
        {
            msg = msg;
            success = false;
            code = errorCode;
        }
    }
}