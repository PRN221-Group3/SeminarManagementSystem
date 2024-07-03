using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repositories.BaseRepo;

namespace Repositories.Interfaces
{
<<<<<<<< HEAD:Repositories/Interfaces/IFeedbackRepository.cs
    public interface IFeedbackRepository : IBaseRepository<Feedback>
========
    public interface IFeedBackRepository : IBaseRepository<Feedback>
>>>>>>>> 6d7c81a336f71296ce5f66c8a9a062fbc584fccd:Repositories/Interfaces/IFeedBackRepository.cs
    {
        Task<IEnumerable<Feedback>> GetByEventIdAsync(Guid eventId);
        Task<bool> FeedbackExistsAsync(Guid eventId, Guid userId);
    }
}
