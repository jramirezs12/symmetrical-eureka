using MongoDB.Driver;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.Primitives;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;
using System.Linq.Expressions;

namespace RulesEngine.Persistence.Repositories
{
    public abstract class MongoRepository<TDocument, TEntityId> : IMongoRepository<TDocument, TEntityId>
        where TDocument : Entity<TEntityId>
        where TEntityId : class
    {
        private readonly GiproDbContext _context;
        private readonly BasicGiproDbContext _contextBasic;
        private readonly IMongoDatabase _database;
        protected readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(GiproDbContext context,
                               string database,
                               string collection_name)
        {
            _context = context;
            _database = _context.DbContext.GetDatabase(database);
            _collection = _database.GetCollection<TDocument>(collection_name);
        }

        public MongoRepository(BasicGiproDbContext context,
                               string database,
                               string collection_name)
        {
            _contextBasic = context;
            _database = _contextBasic._dbContext.GetDatabase(database);
            _collection = _database.GetCollection<TDocument>(collection_name);
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual async Task<IEnumerable<TDocument>> FilterByAsync(FilterDefinition<TDocument> filterExpression, FindOptions<TDocument> options)
        {
            var result = await _collection.FindAsync(filterExpression, options);
            return await result.ToListAsync();
        }
        public virtual async Task<IEnumerable<TDocument>> FilterBy(FilterDefinition<TDocument> filterExpression)
        {
            return await _collection.Find(filterExpression).ToListAsync();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        public async Task<TDocument> FindOneAsync(FilterDefinition<TDocument> filterExpression)
        {
            return await _collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        public virtual TDocument FindById(TEntityId id)
        {
            //var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TDocument> FindByIdAsync(TEntityId id)
        {
            return Task.Run(() =>
            {
                //var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual void InsertOne(TDocument document)
        {
            _collection.InsertOne(document);
        }

        public virtual async Task InsertOneAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public async Task UpdateOneAsync(FilterDefinition<TDocument> filterDefinition,
                                         UpdateDefinition<TDocument> update,
                                         UpdateOptions options)
        {
            await _collection.UpdateOneAsync(filterDefinition, update, options);
        }

        public async Task UpdateManyAsync(FilterDefinition<TDocument> filterDefinition,
                                          UpdateDefinition<TDocument> update,
                                          UpdateOptions options)
        {
            await _collection.UpdateManyAsync(filterDefinition, update, options);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(TEntityId id)
        {
            // var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(TEntityId id)
        {
            return Task.Run(() =>
            {
                // var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }
    }
}
