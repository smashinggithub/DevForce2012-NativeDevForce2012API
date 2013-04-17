using System.Collections;
using ClientUIDataApp5.ModelServices;
using ClientUIDataApp5.DomainModel;
using System;

namespace ClientUIDataApp5.ViewModels
{
    public class ProductsViewModel : EditableGridViewModelBase<Product>
    {
        private IEnumerable _categories;
        private bool _isCategoryLoaded;
        private string _supplierTitle;

        public ProductsViewModel()
            : base()
        {
            this.IsPrerequisiteDataLoaded = false;

            if (!this.IsInDesignMode)
            {
                this.CategoriesSource = CategoriesRepository.Instance;
                this.LoadCategories();
            }

            this.SupplierTitle = "Please select an item to see its supplier details.";
        }

        public IEnumerable Categories
        {
            get { return this._categories; }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged("Categories");
                }
            }
        }

        public string SupplierTitle
        {
            get { return _supplierTitle; }
            set
            {
                if (_supplierTitle != value)
                {
                    _supplierTitle = value;
                    OnPropertyChanged("SupplierTitle");
                }
            }
        }

        public IDataRepository CategoriesSource { get; set; }

        protected override IDataRepository DataSource
        {
            get
            {
                return ProductsRepository.Instance;
            }
        }

        public bool IsCategoriesLoaded
        {
            get { return _isCategoryLoaded; }
            set
            {
                if (_isCategoryLoaded != value)
                {
                    _isCategoryLoaded = value;
                    OnPropertyChanged("IsCategoriesLoaded");

                    if (value)
                        this.NotifyPrerequisiteDataLoaded();
                }
            }
        }

        private void NotifyPrerequisiteDataLoaded()
        {
            if (this.IsCategoriesLoaded)
                this.IsPrerequisiteDataLoaded = true;
        }

        protected override void OnSelectedItemChanged(Product oldItem, Product newItem)
        {
            base.OnSelectedItemChanged(oldItem, newItem);

            if (this.SelectedItem != null && this.SelectedItem.Supplier != null)
                this.SupplierTitle = "Supplier Details - " + this.SelectedItem.Supplier.CompanyName;
            else
                this.SupplierTitle = "";
        }

        private async void LoadCategories()
        {
            try
            {
                var categories = await this.CategoriesSource.GetData();
                this.Categories = categories;
                this.IsCategoriesLoaded = true;
            }
            catch (Exception ex)
            {
                this.Presenter.ShowErrorMessage(
                        "An exception has occurred during data loading\n." +
                        "Message: " + ex.Message +
                        "Stack Trace: " + ex.StackTrace);
            }
        }
    }
}
