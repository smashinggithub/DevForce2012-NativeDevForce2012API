using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using IdeaBlade.EntityModel;

namespace ClientUIDataApp5.ViewModels
{
    public class ValidationViewModelBase : INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        #region Properties

        private Dictionary<string, string> _errors = new Dictionary<string, string>();

        public virtual bool HasErrors
        {
            get
            {
                return _errors.Count > 0;
            }
        }

        #endregion

        #region Public

        public void SetError(string propertyName, string errorMessage)
        {
            _errors[propertyName] = errorMessage;
            this.OnPropertyChanged(propertyName);
        }

        public bool HasError(string propertyName)
        {
            return this._errors.ContainsKey(propertyName);
        }

        public ValidationResult GetFirstError()
        {
            if (_errors.Count > 0)
            {
                KeyValuePair<string, string> firstError = _errors.First();
                return new ValidationResult(firstError.Value, new string[] { firstError.Key });
            }

            return null;
        }

        #endregion

        #region Protected

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void ClearError(string propertyName)
        {
            this._errors.Remove(propertyName);
        }

        protected void ClearAllErrors()
        {
            List<string> properties = new List<string>();

            foreach (KeyValuePair<string, string> error in this._errors)
                properties.Add(error.Key);

            this._errors.Clear();

            foreach (string property in properties)
                this.OnPropertyChanged(property);
        }

        protected void ValidateProperty(string propertyName, object value)
        {
            this.ClearError(propertyName);
            List<ValidationResult> results = new List<ValidationResult>();
            Validator.TryValidateProperty(value, new ValidationContext(this, null, null) { MemberName = propertyName }, results);

            if (results.Count > 0)
            {
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < results.Count; i++)
                {
                    builder.Append(results[i]);

                    if (i < results.Count - 1)
                        builder.AppendLine();
                }

                this.SetError(propertyName, builder.ToString());
            }
        }

        protected void ValidateEntity(Entity entity)
        {
            entity.EntityAspect.ValidationErrors.Clear();
            entity.EntityAspect.VerifierEngine.Execute(entity);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IDataErrorInfo Members

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                if (this._errors.ContainsKey(columnName))
                {
                    return this._errors[columnName];
                }
                return string.Empty;
            }
        }

        #endregion

        #region IDisposable

        public virtual void Dispose()
        {
        }

        #endregion
    }
}
