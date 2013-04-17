using System.Collections.Generic;
using System.Linq;
using IdeaBlade.EntityModel;
using IdeaBlade.Validation;
using ClientUIDataApp5.DomainModel;

namespace ClientUIDataApp5.ModelServices
{
    /// <summary>
    /// Represents the products repository class for sample purpose.
    /// </summary>
    public class ProductsRepository : EditableDataRepository<Product>
    {
        private static IDataRepository _repository;

        /// <summary>
        /// Initializes a new instance of <see cref="ProductsRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="EntityManager"/> instance, providing access to all functionality of the data service.</param>
        public ProductsRepository(EntityManager context)
            : base(context)
        {
        }

        private static bool IsReusable
        {
            get { return true; }
        }

        private NorthwindEntities EntityContext
        {
            get
            {
                return ((NorthwindEntities)this.Context);
            }
        }

        /// <summary>
        /// Returns an instance of <see cref="ProductsRepository"/>. 
        /// If the <see cref="IsReusable"/> is true, the property will return an existing cached copy of the instance.
        /// </summary>
        public static IDataRepository Instance
        {
            get
            {
                if (_repository == null || !IsReusable)
                    _repository = new ProductsRepository(RepositoryManager.Create());

                return _repository;
            }
        }

        /// <summary>
        /// Gets a <see cref="List"/> that provides access to a collection of entities as the results of the entity query.
        /// </summary>
        public override List<Product> EntitySet
        {
            get
            {
                return this.EntityQuery.ToList();
            }
        }

        /// <summary>
        /// Gets the <see cref="EntityQuery"/> that represents the entity's LINQ query.
        /// </summary>
        public override EntityQuery<Product> EntityQuery
        {
            get
            {
                return this.EntityContext.Products;
            }
        }

        /// <summary>
        /// Validate the specified entity.
        /// </summary>
        /// <param name="entity">The entity to validate.</param>
        public override void Validate(Product entity)
        {
            base.Validate(entity);

            entity.EntityAspect.ValidationErrors.Clear();
            entity.EntityAspect.VerifierEngine.Execute(entity);

            if (entity.CategoryID < 1 || entity.CategoryID > 8)
                entity.EntityAspect.ValidationErrors.Add(new VerifierResult(VerifierResultCode.Error, "Specified CategoryID does not exist", new string[] { "CategoryID" }));

            if (entity.UnitPrice < 0)
                entity.EntityAspect.ValidationErrors.Add(new VerifierResult(VerifierResultCode.Error, "Unit Price can not be less than 0", new string[] { "UnitPrice" }));

            if (entity.UnitsInStock < 0)
                entity.EntityAspect.ValidationErrors.Add(new VerifierResult(VerifierResultCode.Error, "Units in Stock can not be less than 0", new string[] { "UnitsInStock" }));

            if (entity.UnitsOnOrder < 0)
                entity.EntityAspect.ValidationErrors.Add(new VerifierResult(VerifierResultCode.Error, "Units on Order can not be less than 0", new string[] { "UnitsOnOrder" }));
        }
    }
}
