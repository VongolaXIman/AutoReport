using Model;
using Model.DTO;
using System;

namespace Service.AllLog
{
    public interface IAllLogService
    {
        /// <summary>
        /// Insert log
        /// </summary>
        void APIlogAdd(ELKLOG logData, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "");

        void APIlogAdd(ELKLOG logData, string param, string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "");

        //void APIlogAdd<T>(ELKLOG logData, T param, string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") where T : class;

        /// <summary>
        /// 專門記錄執行「結果」(ResponseModel)，ResponseModel.Data及msg不會記錄
        /// </summary>
        /// <param name="logData">ELKLOG class</param>
        /// <param name="param">要記錄的參數</param>
        /// <param name="message">自訂要記錄的log欄位</param>
        /// <param name="response">執行結果(會記錄Success及Status)</param>
        /// <param name="memberName">方法名稱(不必輸入)</param>
        void APIlogResult(ELKLOG logData, string param, string message, ResponseModel response, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "");

        /// <summary>
        /// 專門記錄進入 catch 的非預期錯誤，param欄位要先記錄在class裡此方法預設記錄：1.level = "ERROR" 2.status = "500" 3.result = "false"
        /// </summary>
        /// <param name="logData">ELKLOG class</param>
        /// <param name="ex">此參數直接傳入catch的Exception物件,記錄ex.Message及ex.InnerException</param>
        /// <param name="memberName">方法名稱(不必輸入)</param>
        void APIlogError(ELKLOG logData, Exception ex, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "");

        /// <summary>
        /// 專門記錄進入 catch 的非預期錯誤，param欄位要先記錄在class裡此方法預設記錄：1.level = "ERROR" 2.status = "500" 3.result = "false"
        /// </summary>
        /// <param name="logData">ELKLOG class</param>
        /// <param name="strException">錯誤訊息</param>
        /// <param name="memberName">方法名稱(不必輸入)</param>
        void APIlogError(ELKLOG logData, string strException, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "");
    }
}
