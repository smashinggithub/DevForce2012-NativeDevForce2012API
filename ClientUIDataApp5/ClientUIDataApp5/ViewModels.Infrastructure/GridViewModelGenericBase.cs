using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using IdeaBlade.EntityModel;
using Intersoft.Client.Data.ComponentModel;
using Intersoft.Client.Framework;
using ClientUIDataApp5.ModelServices;

namespace ClientUIDataApp5.ViewModels
{
    /// <summary>
    /// Represents a generic ViewModel base class that supports server-side data access to DevForce using QueryDescriptor.
    /// </summary>
    /// <remarks>
    /// The <see cref="GridViewModelBase"/> class exposes four properties that you can bind to the View elements.
    /// For the automatic query to work, bind the QueryDescriptor property to the View element that supports QueryDescriptor
    /// such as UXGridView, UXDataFilter and UXDataPager.
    /// </remarks>
    public abstract class GridViewModelBase<T> : ValidationViewModelBase where T : Entity
    {
        #region Fields

        private bool _isBusy;
        private bool _isPrerequisiteDataLoaded;
        private IEnumerable _items;
        private T _selectedItem;
        private IEnumerable _selectedItems;
        private QueryDescriptor _queryDescriptor;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GridViewModelBase"/> class.
        /// </summary>
        public GridViewModelBase()
        {
            this._isPrerequisiteDataLoaded = true;

            this.QueryDescriptor = new QueryDescriptor();
            this.Presenter = new MessagePresenter();
            this.IsBusy = true;
            this.QueryLatency = 0.6;
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_selectedItem != null)
                _selectedItem.PropertyChanged -= new PropertyChangedEventHandler(OnSelectedItemPropertyChanged);

            if (_queryDescriptor != null)
                _queryDescriptor.QueryChanged -= new EventHandler(OnQueryChanged);
        }

        #endregion

        #region Properties

        private double QueryLatency { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the application is currently in design-time mode.
        /// </summary>
        protected bool IsInDesignMode
        {
            get
            {
                return Intersoft.Client.Framework.ISControl.IsInDesignModeStatic;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the prerequisite data is loaded.
        /// </summary>        
        protected virtual bool IsPrerequisiteDataLoaded
        {
            get { return _isPrerequisiteDataLoaded; }
            set
            {
                if (_isPrerequisiteDataLoaded != value)
                {
                    _isPrerequisiteDataLoaded = value;
                    OnPropertyChanged("IsPrerequisiteDataLoaded");

                    if (value)
                        this.QueryDescriptor.RaiseQueryChanged();
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="MessagePresenter"/> instance.
        /// </summary>
        protected virtual MessagePresenter Presenter { get; private set; }

        /// <summary>
        /// Gets the data repository instance that implements <see cref="IDataRepository"/>.
        /// </summary>
        protected virtual IDataRepository DataSource { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating the currently selected item.
        /// </summary>
        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    if (_selectedItem != null)
                        _selectedItem.PropertyChanged -= new PropertyChangedEventHandler(OnSelectedItemPropertyChanged);

                    T _oldItem = _selectedItem;
                    _selectedItem = value;
                    OnSelectedItemChanged(_oldItem, value);

                    if (_selectedItem != null)
                        _selectedItem.PropertyChanged += new PropertyChangedEventHandler(OnSelectedItemPropertyChanged);

                    OnPropertyChanged("SelectedItem");
                }
            }
        }

        /// <summary>
        /// Gets or sets a collection of multiple selected items.
        /// </summary>
        public IEnumerable SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                if (_selectedItems != value)
                {
                    _selectedItems = value;
                    OnPropertyChanged("SelectedItems");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value representing the collection of data.
        /// </summary>
        public IEnumerable Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged("Items");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the data repository is busy processing a query based on the given <see cref="QueryDescriptor"/>.
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="QueryDescriptor"/> instance which provides the required information for data query purpose.
        /// </summary>
        public QueryDescriptor QueryDescriptor
        {
            get
            {
                return _queryDescriptor;
            }
            set
            {
                if (_queryDescriptor != value)
                {
                    if (_queryDescriptor != null)
                        _queryDescriptor.QueryChanged -= new EventHandler(OnQueryChanged);

                    _queryDescriptor = value;
                    _queryDescriptor.QueryChanged += new EventHandler(OnQueryChanged);

                    OnPropertyChanged("QueryDescriptor");
                }
            }
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Called when the data repository needs to fetch data from the repository, either during initial load, or due to query change.
        /// </summary>
        /// <param name="dataSource">An instance of the data repository.</param>
        protected virtual void LoadData(IDataRepository dataSource)
        {
            if (!this.IsInDesignMode && dataSource == null)
                throw new ArgumentNullException("dataSource should not be null.");

            this.DataSource = dataSource;
            this.LoadData();
        }

        /// <summary>
        /// Called when the data repository needs to fetch data from the repository, either during initial load, or due to query change.
        /// </summary>
        protected virtual async void LoadData()
        {
            this.IsBusy = true;

            try
            {
                var items = await this.DataSource.GetData(this.QueryDescriptor);
                this.IsBusy = false;
                this.Items = new PagedCollectionView(items);   
            }
            catch (Exception ex)
            {
                this.IsBusy = false;

                this.Presenter.ShowErrorMessage(
                    "An exception has occurred during data loading.\n" +
                    "Message: " + ex.Message + "\n" +
                    "Stack Trace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Called when the data repository needs to get total item count for data paging purpose.
        /// </summary>
        protected virtual async void GetTotalItemCount()
        {
            int totalItemCount = await this.DataSource.GetTotalItemCount(this.QueryDescriptor);
            if (totalItemCount != -1)
                this.QueryDescriptor.PageDescriptor.TotalItemCount = totalItemCount;
        }

        /// <summary>
        /// Called when one of the properties in the <see cref="SelectedItem"/> object is changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The data related to the event.</param>
        protected virtual void OnSelectedItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called when one of the properties in the <see cref="QueryDescriptor"/> object is changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The data related to the event.</param>
        protected virtual void OnQueryChanged(object sender, EventArgs e)
        {
            // Perform data retrieval in 0.6ms latency to ensure smooth transition. By default, transition took ~0.3ms.
            // If transition is not applied in the View, it's not necessary to use ExecuteTimeOut method.
            Utility.ExecuteTimeOut(this.QueryLatency,
                () =>
                {
                    if (this.IsPrerequisiteDataLoaded)
                    {
                        this.GetTotalItemCount();
                        this.LoadData();
                        this.QueryLatency = 0;
                    }
                });
        }

        /// <summary>
        /// Called when the <see cref="SelectedItem"/> property changes.
        /// </summary>
        /// <param name="oldItem">The old selected item.</param>
        /// <param name="newItem">The new selected item.</param>
        protected virtual void OnSelectedItemChanged(T oldItem, T newItem)
        {
        }

        #endregion
    }
}
