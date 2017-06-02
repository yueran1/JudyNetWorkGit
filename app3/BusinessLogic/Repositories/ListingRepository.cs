using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repositories
{
    public class ListingRepository : BaseRepository<Listing>
    {
        public List<Listing> Get(int startAt = 0, int pageSize = 10, int? status = null)
        {
            return Get(o => status == null || o.Status == status, startAt, pageSize);
        }

        public Listing Get(int id, bool admin = false)
        {
            return Get(o => o.Id == id && (admin || o.Status > 0));
        }

        public int Count(bool admin = false)
        {
            return Count(o => admin || o.Status > 0);
        }

        public override int Update(Listing model)
        {
            Listing entity = Get(model.Id, true);

            if (entity == null)
            {
                entity = model;
                entity.Id = context.Listings.Count() + 1;
                context.Listings.Add(entity);
            }
            else
            {
                context.Entry(entity).CurrentValues.SetValues(model);
            }

            context.SaveChanges();

            return entity.Id;
        }

        public override void Delete(int id, bool delete = false)
        {
            Listing entity = Get(id, true);

            if (entity != null)
            {
                if (delete)
                {
                    context.Listings.Remove(entity);
                }
                else
                {
                    entity.Status = 0;
                }

                context.SaveChanges();
            }
        }

        public int UpdateCN(ListingCN model)
        {
            ListingCN entity = context.ListingCNs.FirstOrDefault(o => o.ListingId == model.ListingId);

            if (entity == null)
            {
                entity = model;
                context.ListingCNs.Add(entity);
            }
            else
            {
                context.Entry(entity).CurrentValues.SetValues(model);
            }

            context.SaveChanges();

            return entity.ListingId;
        }
    }
}
