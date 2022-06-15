
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newspaper.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Core.Extensions
{
   public static class ModelStateExt
    {
        public static List<string> GetMessegesErrors(this ModelStateDictionary model)
        {
            var errors = new List<string>();
            foreach (var modelState in model.Values)
            {
                foreach (var error in modelState.Errors)
                {
#if DEBUG
                    errors.Add(!(string.IsNullOrWhiteSpace(error.ErrorMessage)) ? error.ErrorMessage : error.Exception.Message);
#else
                    errors.Add(!(string.IsNullOrWhiteSpace(error.ErrorMessage)) ? error.ErrorMessage : "Server error - Please contact us.");
#endif
                }
            }
            return errors;
        }
        public static AjaxResult GetMessegesErrorsSummary(this ModelStateDictionary model)
        {
            string validationErrors = "";
            var errors = model.GetMessegesErrors();
            validationErrors = String.Join("<br/>", errors);
            return validationErrors;
        }
    }
   

}
