using FluentValidation.Results;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Text;
using ZTAppFreamework.Stared.Validations;

namespace ZTAppFreamework.Stared.ViewModels
{
    public class ViewModelBase : BindableBase
    {

        public ViewModelBase()
        {
            validator = Prism.Ioc.ContainerLocator.Container.Resolve<GlobalValidator>();
        }
        private bool isBusy;

        private readonly GlobalValidator validator;

        public bool IsNotBusy => !IsBusy;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsNotBusy));
            }
        }

        public virtual async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
        {
            IsBusy = true;
            try
            {
                await func();
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 实体验证器方法
        /// </summary>
        /// <typeparam name="T">验证结果</typeparam>
        /// <param name="model">验证实体</param>
        /// <returns></returns>
        public virtual ValidationResult Verify<T>(T model, bool ShowError = true)
        {
            var validationResult = validator.Validate(model);

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
    }
}
