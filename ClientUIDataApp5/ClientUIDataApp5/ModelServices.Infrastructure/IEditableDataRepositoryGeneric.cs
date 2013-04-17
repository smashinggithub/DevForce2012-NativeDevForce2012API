using System;
using System.Collections;
using System.Threading.Tasks;

namespace ClientUIDataApp5.ModelServices
{
    /// <summary>
    /// Defines the methods to support the editable data repository implementation.
    /// </summary>
    public interface IEditableDataRepository<T> : IDataRepository
    {
        /// <summary>
        /// Create a new entity based on the type of the data repository.
        /// </summary>
        /// <returns>Returns a newly created entity.</returns>
        T Create();

        /// <summary>
        /// Validate the specified entity.
        /// </summary>
        /// <param name="entity">The entity to validate.</param>
        void Validate(T entity);

        /// <summary>
        /// Insert the specified entity to the data repository.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        void Insert(T entity);

        /// <summary>
        /// Delete one or more entity in the specified collection from the data repository.
        /// </summary>
        /// <param name="entities">A collection of entity to delete.</param>
        void Delete(IList entities);

        /// <summary>
        /// Delete the specified entity from the data repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        /// Reject the changes which have been made to the entity.
        /// </summary>
        /// <param name="entity">The entity of which changes to reject.</param>
        void RejectChanges(T entity);

        /// <summary>
        /// Reject all changes made to the data repository since it is first loaded.
        /// </summary>
        void RejectChanges();

        /// <summary>
        /// Save all changes made to the data repository since it is first loaded.
        /// </summary>
        /// <param name="onSuccess">The callback to invoke when the save operation succeeded.</param>
        /// <param name="onError">The callback to invoke when the save operation failed.</param>
        Task SaveChanges();
    }
}
