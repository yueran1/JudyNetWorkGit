using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        protected DBEntities context;

        public BaseRepository()
        {
            context = new DBEntities();
        }

        protected IQueryable<T> DbSet(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate);
        }

        protected List<T> Get(Expression<Func<T, bool>> predicate, int startAt, int pageSize)
        {
            return context.Set<T>().Where(predicate).OrderBy(o => 1).Skip(startAt).Take(pageSize).ToList();
        }

        protected T Get(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().SingleOrDefault(predicate);
        }

        protected int Count(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate).Count();
        }

        public abstract int Update(T model);
        public abstract void Delete(int id, bool delete);

        // generic functions
        public SiteSetting GetSeting(string setting)
        {
            return context.SiteSettings.FirstOrDefault(o => o.Name.Equals(setting));
        }

        public void SetSeting(SiteSetting model)
        {
            SiteSetting entity = context.SiteSettings.SingleOrDefault(o => o.Id == model.Id);
            context.Entry(entity).CurrentValues.SetValues(model);
            context.SaveChanges();
        }
    }
}
