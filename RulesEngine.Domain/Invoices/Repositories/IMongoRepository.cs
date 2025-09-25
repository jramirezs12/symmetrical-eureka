using MongoDB.Driver;
using RulesEngine.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Domain.Invoices.Repositories
{
    public interface IMongoRepository<TDocument, TEntityId> where TDocument : Entity<TEntityId>
    {
        IQueryable<TDocument> AsQueryable();
        IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression);
        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);
        Task<IEnumerable<TDocument>> FilterBy(FilterDefinition<TDocument> filterExpression);
        Task<IEnumerable<TDocument>> FilterByAsync(FilterDefinition<TDocument> filterExpression, FindOptions<TDocument> options);
        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);
        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task<TDocument> FindOneAsync(FilterDefinition<TDocument> filterExpression);
        TDocument FindById(TEntityId id);
        Task<TDocument> FindByIdAsync(TEntityId id);
        void InsertOne(TDocument document);
        Task InsertOneAsync(TDocument document);
        void InsertMany(ICollection<TDocument> documents);
        Task InsertManyAsync(ICollection<TDocument> documents);
        void ReplaceOne(TDocument document);
        Task ReplaceOneAsync(TDocument document);
        Task UpdateOneAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, UpdateOptions options);
        Task UpdateManyAsync(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, UpdateOptions options);
        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        void DeleteById(TEntityId id);
        Task DeleteByIdAsync(TEntityId id);
        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
}
