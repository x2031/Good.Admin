﻿namespace Good.Admin.Common
{
    public class ErrorResult : AjaxResult
    {
        public ErrorResult(string msg = "操作失败!", int errorCode = 0)
        {
            base.msg = msg;
            success = false;
            code = errorCode;
        }
    }
}