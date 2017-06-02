using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repositories
{
    public class UserProfileRepository : BaseRepository<UserProfile>
    {
        public List<UserProfile> Get(int startAt = 0, int pageSize = 10, bool admin = false)
        {
            return Get(o => (admin || o.Status > 0) && o.Id > 1, startAt, pageSize);
        }

        public UserProfile Get(int id, bool admin = false)
        {
            return Get(o => o.Id == id && (admin || o.Status > 0));
        }

        public int Count(bool admin = false)
        {
            return Count(o => admin || o.Status > 0);
        }

        public override int Update(UserProfile model)
        {
            UserProfile entity = Get(model.Id, true);

            if (entity == null)
            {
                entity = model;
                entity.CreationDate = DateTime.UtcNow;
                context.UserProfiles.Add(entity);
            }
            else
            {
                context.Entry(entity).CurrentValues.SetValues(model);
            }

            context.SaveChanges();

            return entity.Id;
        }

        public override void Delete(int id, bool delete)
        {
            UserProfile entity = Get(id, true);

            if (entity != null)
            {
                if (delete)
                {
                    context.UserProfiles.Remove(entity);
                }
                else
                {
                    entity.Status = 0;
                }

                context.SaveChanges();
            }
        }

        public UserProfile GetByUserName(string username)
        {
            return context.UserProfiles.FirstOrDefault(o => o.UserName.Equals(username));
        }
    }
}
