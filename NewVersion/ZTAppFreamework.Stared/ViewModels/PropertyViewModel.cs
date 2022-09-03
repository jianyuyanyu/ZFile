using FluentValidation.Results;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Text;
using ZTAppFreamework.Stared.Validations;

namespace ZTAppFreamework.Stared.ViewModels
{
    public class PropertyViewModel : BindableBase, IDataErrorInfo
    {
  
        public PropertyViewModel()
        {
            validator = ContainerLocator.Container.Resolve<GlobalValidator>();
        }

        protected readonly GlobalValidator validator;
        public  string VerifyTostring<T>(T model,string columnName = "")
        {
          return Verify(model)?.Errors?.FirstOrDefault(x => x.PropertyName == columnName)?.ErrorMessage;
        }       
        /// <summary>
        /// 实体验证器方法
        /// </summary>
        /// <typeparam name="T">验证结果</typeparam>
        /// <param name="model">验证实体</param>
        /// <returns></returns>
        public  virtual ValidationResult Verify<T>(T model, bool ShowError = true)
        {
            var validationResult = validator.Validate<T>(model);
            if (!validationResult.IsValid && ShowError)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in validationResult.Errors)
                {
                    stringBuilder.AppendLine(item.ErrorMessage);
                }
                //AppDialogHelper.Warn(stringBuilder.ToString());
            }
            return validationResult;
        }
        public  string Error { get; set; }
        public virtual string this[string columnName] { get=>""; }

    
    
     
    }
}

