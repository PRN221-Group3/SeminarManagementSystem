using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repositories.BaseRepo;

namespace Repositories.Interfaces
{
    public interface IFeedbackRepository : IBaseRepository<Feedback>
    {
    }
}
