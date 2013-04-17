using System.Collections;
using IdeaBlade.EntityModel;
using Intersoft.Client.Data.ComponentModel;
using System.Threading.Tasks;

namespace ClientUIDataApp5.ModelServices
{
    /// <summary>
    /// Defines the methods to support a data repository implementation.
    /// </summary>
    public interface IDataRepository
    {
        // <summary>
        /// Gets the <see cref="EntityManager"/> instance which provides access to all functionality of the data service.
        /// </summary>
        EntityManager Context { get; }

        /// <summary>
        /// Retrieves the data from the repository asynchronously.
        /// </summary>
        /// <param name="onSuccess">The callback to invoke when the operation succeeded.</param>
        /// <param name="onFail">The callback to invoke when the operation failed.</param>
        Task<IEnumerable> GetData();

        /// <summary>
        /// Retrieves the data from the repository asynchronously based on the given QueryDescriptor and other parameters.
        /// </summary>
        /// <param name="queryDescriptor">A <see cref="QueryDescriptor"/> instance that describes the query.</param>
        /// <param name="onSuccess">The callback to invoke when the operation succeeded.</param>
        /// <param name="onItemCountRetrieved">The callback to invoke when the total item count operation succeeded.</param>
        /// <param name="onFail">The callback to invoke when the operation failed.</param>
        Task<IEnumerable> GetData(QueryDescriptor queryDescriptor);

        /// <summary>
        /// Retrieves the total item count asynchronously based on the specified <see cref="EntityQuery"/> and the given <see cref="QueryDescriptor"/>.
        /// </summary>
        /// <param name="queryDescriptor">A <see cref="QueryDescriptor"/> instance that describes the query.</param>
        /// <param name="onSuccess">The callback to invoke when the operation succeeded.</param>
        Task<int> GetTotalItemCount(QueryDescriptor queryDescriptor);
    }
}
