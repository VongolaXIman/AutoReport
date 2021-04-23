using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Model.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Service.AllLog
{
    public class AllLogService : IAllLogService
    {
        private readonly ILoggerFactory _loggerFactory;
        public AllLogService(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void APIlogResult(ELKLOG logData, string param, string message, ResponseModel response, [CallerMemberName] string memberName = "")
        {
            logData.level = response.Success == true ? EnumLogLevel.INFO.ToString() : EnumLogLevel.WARN.ToString();
            logData.param = param;
            logData.message = message;
            logData.status = response.Status;
            logData.result = response.Success.ToString();

            APIlogAdd(logData, memberName);
        }

        public void APIlogAdd(ELKLOG logData, string param, string message, [CallerMemberName] string memberName = "")
        {
            logData.param = param;
            logData.message = message;

            APIlogAdd(logData, memberName);
        }

        public void APIlogAdd(ELKLOG logData, [CallerMemberName] string memberName = "")
        {
            var logger = _loggerFactory.CreateLogger(logData.logger);
            logger.Log(GetLevel(logData.level), "apiName: {apiName}, apiVer: {apiVer}, companyID: {companyID}, status: {status}, result: {result}, message: {message}, param: {param}, eventId: {eventId}",
                memberName,
                logData.apiVer,
                logData.companyID,
                logData.status,
                logData.result,
                logData.message,
                logData.param,
                logData.eventId);
        }

        public void APIlogError(ELKLOG logData, Exception ex, [CallerMemberName] string memberName = "")
        {
            var strException = $"error message: {ex.Message}, InnerException: {ex.InnerException}";
            APIlogError(logData, ex, strException, memberName);
        }

        public void APIlogError(ELKLOG logData, string strException, [CallerMemberName] string memberName = "")
        {
            APIlogError(logData, new Exception(), strException: strException, memberName: memberName);
        }

        private void APIlogError(ELKLOG logData, Exception ex = null, string strException = null, [CallerMemberName] string memberName = "")
        {
            logData.status = "500";
            logData.result = "false";
            logData.exception = strException;

            var logger = _loggerFactory.CreateLogger(logData.logger);
            logger.LogError(ex, "apiName: {apiName}, apiVer: {apiVer}, companyID: {companyID}, status: {status}, result: {result}, message: {message}, param: {param}, exception: {exception}, eventId: {eventId}",
                memberName,
                logData.apiVer,
                logData.companyID,
                logData.status,
                logData.result,
                logData.message,
                logData.param,
                logData.exception,
                logData.eventId);
        }

        private LogLevel GetLevel(string level)
        {
            if (!string.IsNullOrEmpty(level) && Enum.TryParse(typeof(EnumLogLevel), level, out object logLevel))
            {
                return ((EnumLogLevel)logLevel) switch
                {
                    EnumLogLevel.INFO => LogLevel.Information,
                    EnumLogLevel.WARN => LogLevel.Warning,
                    EnumLogLevel.ERROR => LogLevel.Error,
                    _ => LogLevel.Trace,
                };
            }
            else
            {
                return LogLevel.Trace;
            }
        }
    }
}
