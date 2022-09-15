﻿using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Repositories
{
    public class EfRepositoryBase<TEntity, TContext> : IRepository<TEntity>, IAsyncRepository<TEntity>
        where TEntity : CommonEntity
        where TContext : DbContext
    {
        protected TContext Context { get; }

        public DbSet<TEntity> Table => Context.Set<TEntity>();

        public EfRepositoryBase(TContext context)
        {
            Context = context;
        }

        public TEntity? GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public TEntity? GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public IPaginate<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool enableTracking = true)
        {
            IQueryable<TEntity> queryable = Query();

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (include != null)
                queryable = include(queryable);

            if (predicate != null)
                queryable = queryable.Where(predicate);

            if (orderBy != null)
                return orderBy(queryable).ToPaginate(index, size);

            return queryable.ToPaginate(index, size);
        }

        public IPaginate<TEntity> GetListByDynamic(Dynamic.Dynamic dynamic, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool enableTracking = true)
        {
            IQueryable<TEntity> queryable = Query().AsQueryable().ToDynanic(dynamic);

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (include != null)
                queryable = include(queryable);

            return queryable.ToPaginate(index, size);
        }

        public TEntity Add(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Added;
            Context.SaveChanges();
            return entity;
        }

        public TEntity Delete(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            Context.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
            return entity;
        }

        public IQueryable<TEntity> Query() => Context.Set<TEntity>();

        public async Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public async Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IPaginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query();

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (include != null)
                queryable = include(queryable);

            if (predicate != null)
                queryable = queryable.Where(predicate);

            if (orderBy != null)
                return await orderBy(queryable).ToPaginateAsync(index, size, 0, cancellationToken);

            return await queryable.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public async Task<IPaginate<TEntity>> GetListByDynamicAsync(Dynamic.Dynamic dynamic, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query().AsQueryable().ToDynanic(dynamic);

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (include != null)
                queryable = include(queryable);

            return await queryable.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Added;
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}
