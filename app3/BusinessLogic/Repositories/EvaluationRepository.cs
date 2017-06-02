using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repositories
{
    public class EvaluationRepository : BaseRepository<Evaluation>
    {
        public List<Evaluation> Get(int startAt = 0, int pageSize = 10)
        {
            return Get(o => true, startAt, pageSize);
        }

        public Evaluation Get(int id)
        {
            return Get(o => o.Id == id);
        }

        public int Count()
        {
            return Count(o => true);
        }

        public override int Update(Evaluation model)
        {
            Evaluation entity = Get(model.Id);

            if (entity == null)
            {
                entity = model;
                context.Evaluations.Add(entity);
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
            Evaluation entity = Get(id);

            if (entity != null)
            {
                context.Evaluations.Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
