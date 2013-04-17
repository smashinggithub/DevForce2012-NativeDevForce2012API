using System.Collections;
using IdeaBlade.EntityModel;
using Intersoft.Client.Framework.Input;
using ClientUIDataApp5.ModelServices;
using System;
using System.Threading.Tasks;

namespace ClientUIDataApp5.ViewModels
{
    /// <summary>
    /// Represents a ViewModel base class that supports server-side data access, data validation 
    /// and data editing in WCF RIA Services using QueryDescriptor.
    /// </summary>
    /// <remarks>
    /// The <see cref="EditableGridViewModelBase"/> class encapsulates the common editing properties
    /// and exposes the required coommand properties that you can bind to the View elements 
    /// that support MVVM data editing such as UXGridView.
    /// For the automatic query to work, bind the QueryDescriptor property to the View element 
    /// that supports QueryDescriptor such as UXGridView, UXDataFilter and UXDataPager.
    /// </remarks>
    public abstract class EditableGridViewModelBase : GridViewModelBase
    {
        #region Fields

        private Entity _newItem;
        private bool _isBatchUpdate;
        private bool _isRefreshed;
        private bool _hasChanges;
        private bool _autoEditOperation;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableGridViewModelBase"/> class.
        /// </summary>
        public EditableGridViewModelBase()
            : base()
        {
            // Batch Update and Auto Edit Operation are not supported in WCF RIA Services.
            this.IsBatchUpdate = false;
            this.AutoEditOperation = false;

            this.DeleteRowCommand = new DelegateCommand(ExecuteDeleteRow);
            this.InsertRowCommand = new DelegateCommand(ExecuteInsertRow);
            this.PrepareNewRowCommand = new DelegateCommand(ExecutePrepareNewRow);
            this.UpdateCellCommand = new DelegateCommand(ExecuteUpdateCell);
            this.UpdateRowCommand = new DelegateCommand(ExecuteUpdateRow);
            this.RejectRowCommand = new DelegateCommand(ExecuteRejectRow);
            this.RejectChangesCommand = new DelegateCommand(ExecuteRejectChanges);
            this.SaveChangesCommand = new DelegateCommand(ExecuteSaveChanges);
            this.ValidateRowCommand = new DelegateCommand(ExecuteValidateRow);
        }

        #endregion

        #region EditableProductsSource

        private IEditableDataRepository EditableProductsSource
        {
            get
            {
                return this.DataSource as IEditableDataRepository;
            }
        }

        #endregion

        #region Editing Properties

        /// <summary>
        /// Gets or sets a value indicating whether the actions after an editing operation 
        /// should be automatically processed by the View element.
        /// </summary>
        public bool AutoEditOperation
        {
            get { return this._autoEditOperation; }
            set
            {
                if (this._autoEditOperation != value)
                {
                    this._autoEditOperation = value;
                    this.OnPropertyChanged("AutoEditOperation");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the data changes should be saved in batch.
        /// </summary>
        public bool IsBatchUpdate
        {
            get { return this._isBatchUpdate; }
            set
            {
                if (this._isBatchUpdate != value)
                {
                    this._isBatchUpdate = value;
                    this.OnPropertyChanged("IsBatchUpdate");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that represents a new entity for data adding operation.
        /// </summary>
        public Entity NewItem
        {
            get { return this._newItem; }
            set
            {
                if (this._newItem != value)
                {
                    this._newItem = value;
                    this.OnPropertyChanged("NewItem");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the data source is refreshed
        /// due to submit changes operation.
        /// </summary>
        public bool IsRefreshed
        {
            get { return this._isRefreshed; }
            set
            {
                if (this._isRefreshed != value)
                {
                    this._isRefreshed = value;
                    this.OnPropertyChanged("IsRefreshed");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether one or more changes have been made 
        /// to the current data source.
        /// </summary>
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged("HasChanges");
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets a <see cref="DelegateCommand"/> to handle delete row operation.
        /// </summary>
        public DelegateCommand DeleteRowCommand { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DelegateCommand"/> to handle insert row operation.
        /// </summary>
        public DelegateCommand InsertRowCommand { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DelegateCommand"/> to handle new row preparation.
        /// </summary>
        public DelegateCommand PrepareNewRowCommand { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DelegateCommand"/> to handle cell updating operation.
        /// </summary>
        public DelegateCommand UpdateCellCommand { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DelegateCommand"/> to handle row updating operation.
        /// </summary>
        public DelegateCommand UpdateRowCommand { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DelegateCommand"/> to handle changes cancellation at the row level.
        /// </summary>
        public DelegateCommand RejectRowCommand { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DelegateCommand"/> to handle cancellation for all the changes
        /// made to the data source.
        /// </summary>
        public DelegateCommand RejectChangesCommand { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DelegateCommand"/> to handle save changes operation.
        /// </summary>
        public DelegateCommand SaveChangesCommand { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DelegateCommand"/> to handle row validation.
        /// </summary>
        public DelegateCommand ValidateRowCommand { get; set; }

        #endregion

        #region Methods

        private async Task SaveChanges()
        {
            this.IsBusy = true;

            try
            {
                await this.EditableProductsSource.SaveChanges();
                this.IsRefreshed = true;
                this.LoadData(); // refresh
                this.HasChanges = false;
                this.IsBusy = false;            
            }
            catch (Exception ex)
            {
                this.IsBusy = false;

                this.Presenter.ShowErrorMessage(
                    "An exception has occurred during save changes.\n" +
                    "Message: " + ex.Message + "\n" +
                    "Stack Trace: " + ex.StackTrace);             
            }
        }

        private async void ExecuteDeleteRow(object parameter)
        {
            this.EditableProductsSource.Delete(parameter as IList);

            if (!this.IsBatchUpdate)
                await this.SaveChanges();
            else
                this.HasChanges = true;
        }

        private async void ExecuteInsertRow(object parameter)
        {
            this.NewItem = null;

            if (!this.IsBatchUpdate)
                await this.SaveChanges();
            else
                this.HasChanges = true;
        }

        private void ExecutePrepareNewRow(object parameter)
        {

            // It's possible to initialize the new row with default values
            // Example:
            // product.ProductName = "New Product";

            this.NewItem = this.EditableProductsSource.Create() as Entity;
            this.EditableProductsSource.Insert(this.NewItem);
        }

        private void ExecuteUpdateCell(object parameter)
        {
            object[] updateCellParameters = (object[])parameter;
            object product = updateCellParameters.GetValue(0);
            string property = updateCellParameters.GetValue(1).ToString();

            // perform cell-level validation if required
        }

        private void ExecuteValidateRow(object parameter)
        {
            this.EditableProductsSource.Validate(parameter);
        }

        private async void ExecuteUpdateRow(object parameter)
        {
            if (!this.IsBatchUpdate)
                await this.SaveChanges();
            else
                this.HasChanges = true;
        }

        private void ExecuteRejectRow(object parameter)
        {
            if (parameter != null)
                this.EditableProductsSource.RejectChanges(parameter);

            this.NewItem = null;
        }

        private void ExecuteRejectChanges(object parameter)
        {
            this.EditableProductsSource.RejectChanges();
            this.HasChanges = false;
        }

        private async void ExecuteSaveChanges(object parameter)
        {
            // if users click on the save changes while the new row is being edited,
            // presume the new row isn't intended
            if (this.NewItem != null)
                this.EditableProductsSource.RejectChanges(this.NewItem);

            try
            {
                await this.SaveChanges();
            }
            catch (Exception ex)
            {
                this.Presenter.ShowErrorMessage(ex.Message);
            }
        }

        #endregion
    }
}
