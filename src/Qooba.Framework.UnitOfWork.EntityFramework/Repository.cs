using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Qooba.Framework.UnitOfWork.Abstractions;
using Qooba.Framework.Specification.Abstractions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Qooba.Framework.UnitOfWork.EntityFramework
{
    public class Repository<TEntity, TContext> : IRepository<TEntity>
        where TContext : DbContext
        where TEntity : class
    {
        private DbSet<TEntity> entitySet;

        private readonly DbContext _context;

        public Repository(UnitOfWork<TContext> unitOfWork)
            : this(unitOfWork, typeof(TEntity).Name)
        {
            this.entitySet = _context.Set<TEntity>();
        }

        protected Repository(UnitOfWork<TContext> unitOfWork, string entitySetName)
        {
            if (string.IsNullOrEmpty(entitySetName))
            {
                throw new ArgumentNullException("Invalid entity set name", "entitySetName");
            }

            EntitySetName = entitySetName;
            _context = unitOfWork.Context;
            //TODO:
            //_context.Configuration.ProxyCreationEnabled = false;
            UnitOfWork = unitOfWork;
        }

        public IQueryable<TEntity> All()
        {
            return EntitySet;
        }

        public bool Any()
        {
            return EntitySet.Any();
        }

        public Task<bool> AnyAsync()
        {
            return EntitySet.AnyAsync();
        }

        public int Count()
        {
            return EntitySet.Count();
        }

        public Task<int> CountAsync()
        {
            return EntitySet.CountAsync();
        }

        public void Add(TEntity entity)
        {
            if (entity != null)
            {
                throw new ArgumentNullException("entity");
            }
            EntitySet.Add(entity);
        }

        public void AddOrUpdate(TEntity entity)
        {
            if (entity != null)
            {
                throw new ArgumentNullException("entity");
            }

            //TODO:
            //EntitySet.AddOrUpdate(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity != null)
            {
                throw new ArgumentNullException("entity");
            }
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void TrackItem(TEntity entity)
        {
            if (entity != null)
            {
                throw new ArgumentNullException("entity");
            }
            EntitySet.Attach(entity);
        }

        public void Remove(TEntity entity)
        {
            if (entity != null)
            {
                throw new ArgumentNullException("entity");
            }
            EntitySet.Remove(entity);
        }

        public void Merge(TEntity persisted, TEntity currents)
        {
            //TODO:
            //Context.Entry(persisted).CurrentValues.SetValues(currents);
        }

        public void Clear()
        {
            foreach (var entity in EntitySet)
            {
                Remove(entity);
            }
        }

        public IQueryable<TEntity> Filter(ISpecification<TEntity> specification)
        {
            if (specification == null)
            {
                return All();
            }
            return GetQuery(specification.FetchStrategy).Where(specification.IsSatisfiedBy());
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                return All();
            }
            return All().Where(condition);
        }

        public TEntity Single(ISpecification<TEntity> specification)
        {
            if (specification == null)
            {
                return All().Single();
            }
            return GetQuery(specification.FetchStrategy).Single(specification.IsSatisfiedBy());
        }

        public async Task<TEntity> SingleAsync(ISpecification<TEntity> specification)
        {
            if (specification == null)
            {
                return await All().SingleAsync();
            }
            return await GetQuery(specification.FetchStrategy).SingleAsync(specification.IsSatisfiedBy());
        }

        public TEntity Single(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                return All().Single();
            }
            return All().Single(condition);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                return await All().SingleAsync();
            }
            return await All().SingleAsync(condition);
        }

        public TEntity SingleOrDefault(ISpecification<TEntity> specification)
        {
            if (specification == null)
            {
                return All().SingleOrDefault();
            }
            return GetQuery(specification.FetchStrategy).SingleOrDefault(specification.IsSatisfiedBy());
        }

        public async Task<TEntity> SingleOrDefaultAsync(ISpecification<TEntity> specification)
        {
            if (specification == null)
            {
                return await All().SingleOrDefaultAsync();
            }
            return await GetQuery(specification.FetchStrategy).SingleOrDefaultAsync(specification.IsSatisfiedBy());
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                return All().SingleOrDefault();
            }
            return All().SingleOrDefault(condition);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                return await All().SingleOrDefaultAsync();
            }
            return await All().SingleOrDefaultAsync(condition);
        }

        public bool Any(ISpecification<TEntity> specification)
        {
            if (specification == null)
            {
                return All().Any();
            }
            return GetQuery(specification.FetchStrategy).Any(specification.IsSatisfiedBy());
        }

        public async Task<bool> AnyAsync(ISpecification<TEntity> specification)
        {
            if (specification == null)
            {
                return await All().AnyAsync();
            }
            return await GetQuery(specification.FetchStrategy).AnyAsync(specification.IsSatisfiedBy());
        }

        public bool Any(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                return All().Any();
            }
            return All().Any(condition);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                return await All().AnyAsync();
            }
            return await All().AnyAsync(condition);
        }

        public int Count(ISpecification<TEntity> specification)
        {
            if (specification == null)
            {
                return All().Count();
            }
            return GetQuery(specification.FetchStrategy).Count(specification.IsSatisfiedBy());
        }

        public async Task<int> CountAsync(ISpecification<TEntity> specification)
        {
            if (specification == null)
            {
                return await All().CountAsync();
            }
            return await GetQuery(specification.FetchStrategy).CountAsync(specification.IsSatisfiedBy());
        }

        public int Count(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                return All().Count();
            }
            return All().Count(condition);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                return await All().CountAsync();
            }
            return await All().CountAsync(condition);
        }

        public IUnitOfWork UnitOfWork
        {
            get;
            private set;
        }

        protected IQueryable<TEntity> GetQuery(IFetchStrategy<TEntity> fetchStrategy)
        {
            if (fetchStrategy == null)
            {
                return EntitySet;
            }

            return fetchStrategy.IncludedNavigationPaths.Aggregate(this.All(), (current, path) => path(current));
        }

        protected DbContext Context
        {
            get { return _context; }
        }

        protected string EntitySetName
        {
            get;
            private set;
        }

        protected DbSet<TEntity> EntitySet
        {
            get { return this.entitySet ?? (this.entitySet = Context.Set<TEntity>()); }
        }

        protected string[] GetEntityKeyNames()
        {
            return null;
            //TODO:
            //var set = ((IObjectContextAdapter)Context).ObjectContext.CreateObjectSet<TEntity>();
            //var entitySet = set.EntitySet;
            //return entitySet.ElementType.KeyMembers.Select(x => x.Name).ToArray();
        }

        protected object[] GetEntityKeyValues(TEntity entity)
        {
            var names = GetEntityKeyNames();
            return entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).
                Where(x => names.Contains(x.Name)).Select(x => x.GetValue(entity, null)).ToArray();
        }
    }
}
