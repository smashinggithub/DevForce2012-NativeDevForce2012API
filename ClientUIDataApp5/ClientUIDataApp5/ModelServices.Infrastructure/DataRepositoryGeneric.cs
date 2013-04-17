using System;
using System.Collections;
using System.Collections.Generic;
using IdeaBlade.EntityModel;
using Intersoft.Client.Data.ComponentModel;
using Intersoft.Client.Data.Provider.DevForce2012;
using System.Threading.Tasks;

namespace ClientUIDataApp5.ModelServices
{
    /// <summary>
    /// Represents a generic data repository that provides read-only data access.
    /// </summary>
    public class DataRepository<T> : IDataRepository where T : Entity
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DataRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="DomainContext"/> that provides access to all functionality of the data service.</param>
        public DataRepository(EntityManager context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets the <see cref="EntityManager"/> instance which provides access to all functionality of the data service.
        /// </summary>
        public EntityManager Context { get; private set; }

        /// <summary>
        /// Gets the <see cref="EntityQuery"/> that represents the entity's LINQ query.
        /// </summary>
        public virtual EntityQuery<T> EntityQuery { get; private set; }

        /// <summary>
        /// Gets the <see cref="List"/> that provides access to a collection of entities as the results of the entity query.
        /// </summary>
        public virtual List<T> EntitySet { get; private set; }

        /// <summary>
        /// Retrieves the data from the repository asynchronously.
        /// </summary>
        /// <param name="onSuccess">The callback to invoke when the operation succeeded.</param>
        /// <param name="onFail">The callback to invoke when the operation failed.</param>
        public virtual async Task<IEnumerable> GetData()
        {
            if (Intersoft.Client.Framework.ISControl.IsInDesignModeStatic)
                return null;

            IEnumerable results = await this.EntityQuery.ExecuteAsync();
            return results;
        }

        /// <summary>
        /// Retrieves the data from the repository asynchronously based on the given QueryDescriptor and other parameters.
        /// </summary>
        /// <param name="queryDescriptor">A <see cref="QueryDescriptor"/> instance that describes the query.</param>
        /// <param name="onSuccess">The callback to invoke when the operation succeeded.</param>
        /// <param name="onItemCountRetrieved">The callback to invoke when the total item count operation succeeded.</param>
        /// <param name="onFail">The callback to invoke when the operation failed.</param>
        public virtual async Task<IEnumerable> GetData(QueryDescriptor queryDescriptor)
        {
            if (Intersoft.Client.Framework.ISControl.IsInDesignModeStatic)
                return null;

            #region 6.0.9+

            //compatible with devforce 6.0.9+
            //var query = this.EntityQuery.Parse(queryDescriptor);
            //query.ExecuteAsync(
            //    op =>
            //    {
            //        if (op.CompletedSuccessfully)
            //        {
            //            if (onSuccess != null)
            //                onSuccess(op.Results);

            //            if (onItemCountRetrieved != null)
            //                onItemCountRetrieved(-1); // not applicable in DF
            //        }
            //        else
            //        {
            //            if (onFail != null)
            //            {
            //                op.MarkErrorAsHandled();
            //                onFail(op.Error);
            //            }
            //        }
            //    });

            #endregion

            #region 6.1.3.1+

            //compatible with devforce 6.1.3.1+
            try
            {
                return await this.EntityQuery.ExecuteAsync(queryDescriptor);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            #endregion
        }

        /// <summary>
        /// Retrieves the total item count asynchronously based on the specified <see cref="EntityQuery"/> and the given <see cref="QueryDescriptor"/>.
        /// </summary>
        /// <param name="queryDescriptor">A <see cref="QueryDescriptor"/> instance that describes the query.</param>
        /// <param name="onSuccess">The callback to invoke when the operation succeeded.</param>
        public virtual async Task<int> GetTotalItemCount(QueryDescriptor queryDescriptor)
        {
            var query = this.EntityQuery.Parse(queryDescriptor, false);
            int count = await query.Parse(queryDescriptor, false).AsScalarAsync().Count();

            return count;
        }
    }
}
