using Newspaper.Core.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Newspaper.Core.Common
{
    public class AjaxResult
    {

        public static string CatchError(Exception ex)
        {
#if DEBUG

            return (new AjaxResult(ex.Message).ToJsonNS());

#else            
            ////if (ex.Message.StartsWith("<x>"))
            if (ex.GetType() == typeof(FMSUserExeption))            
                return (new AjaxResult(ex.Message)).ToJsonNS();            
            else
                return (new AjaxResult("We are sorry, something went wrong and we are working on it now").ToJsonNS());
#endif
        }

        public static AjaxResult CatchExError(Exception ex)
        {
#if DEBUG

            return new AjaxResult(ex.Message);

#else            
            ////if (ex.Message.StartsWith("<x>"))
            if (ex.GetType() == typeof(FMSUserExeption))            
                return (new AjaxResult(ex.Message));            
            else
                return (new AjaxResult("We are sorry, something went wrong and we are working on it now"));
#endif
        }

        public static string CatchError(Exception ex, string ErrorKey)
        {
#if DEBUG

            return (new AjaxResult("Unexpected error", new ServerParam(ErrorKey, ex.Message)).ToJsonNS());

#else

            //if (ex.Message.StartsWith("<x>"))
            if (ex.GetType() == typeof(FMSUserExeption))
            {
                return (new AjaxResult("Server error", new ServerParam(ErrorKey, ex.Message)).ToJsonNS());
            }
            else
                return (new AjaxResult("Server error", new ServerParam(ErrorKey, "Server error - please try later.")).ToJsonNS());
#endif
        }

        public string Status { set; get; }
        public string ErrorMessage { set; get; }
        public bool IsError { set; get; }
        public Dictionary<string, object> ServerParams { set; get; }

        public AjaxResult()
        {
            this.ServerParams = new Dictionary<string, object>();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="ServerParams">Parameters that passes to client </param>
        public AjaxResult(string ErrorMessage, params ServerParam[] ServerParams)
        {
            this.ServerParams = new Dictionary<string, object>();
            this.ErrorMessage = ErrorMessage;
            IsError = true;
            Status = "Fail";

            if (ServerParams.Length > 0)
                foreach (ServerParam par in ServerParams)
                    this.ServerParams.Add(par.Key, par.Value);
        }


        /// <summary>
        /// Use it when the result is success .
        /// it set IsError Property to false, ErrorMessage to null And Status to success 
        /// </summary>
        /// <param name="ServerParams"></param>
        public AjaxResult(params ServerParam[] ServerParams)
        {
            this.ServerParams = new Dictionary<string, object>();
            IsError = false;
            this.Status = "Success";
            if (ServerParams.Length > 0)
                foreach (ServerParam par in ServerParams)
                    this.ServerParams.Add(par.Key, par.Value);
        }

        /// <summary>
        /// used when the result is one object and success with only one server parameter with supported name. 
        /// </summary>
        /// <param name="param"></param>
        public AjaxResult(string resultKey, object resultValue)
            : this(new ServerParam(resultKey, resultValue))
        {

        }

        /// <summary>
        /// Used To Add Parameter in the AjaxResult object. 
        /// </summary>
        public void AddParameter(ServerParam param)
        {
            if (param != null && ServerParams != null)
                this.ServerParams.Add(param.Key, param.Value);
        }

        public void AddParameter(string Key, object Value)
        {
            if (Value != null)
                this.ServerParams.Add(Key, Value);
        }

        /// <summary>
        /// Used To Add Parameters in the AjaxResult object.
        /// </summary>
        public void AddParameters(IList<ServerParam> parameters)
        {
            if (parameters != null)
                foreach (ServerParam param in parameters)
                    if (param != null)
                        this.ServerParams.Add(param.Key, param.Value);
        }

        /// <summary>
        /// Convert This object to json 
        /// </summary>
        /// <param name="handleReferenceLooping"> To handle references that cause looping. </param>
        /// <returns></returns>
        public string ResultToJson(bool handleReferenceLooping = true)
        {
            return this.ToJsonNS(handleReferenceLooping);
        }

        public static implicit operator AjaxResult(string ErrorMessage)
        {
            return new AjaxResult(ErrorMessage);
        }

        public static implicit operator AjaxResult(bool result)
        {
            return result ? new AjaxResult() : new AjaxResult("Sorry, Server Error. ");
        }


        public static AjaxResult operator +(AjaxResult obj, ServerParam parameter)
        {
            obj.AddParameter(parameter);
            return obj;
        }

        public static AjaxResult operator -(AjaxResult obj, string key)
        {
            obj.ServerParams.Remove(key);
            return obj;
        }


        public static string ReturnSingleResult<T>(T pram, bool error = false, string errorMsg = "")
        {
            if (error)
            {
                var res = new AjaxResult();
                res += new ServerParam("P", pram);
                res.IsError = true;
                res.ErrorMessage = errorMsg;
                return res.ToJsonNS();
            }
            else
            {
                return (new AjaxResult(new ServerParam("P", pram))).ToJsonNS();
            }

        }

    }

    public class ServerParam
    {
        public string Key { set; get; }
        public object Value { set; get; }

        public ServerParam()
        {

        }

        public ServerParam(string key, object value)
        {
            Key = key;
            Value = value;
        }

    }

}
