using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO;
using Repositories.BaseRepo;
using Repositories.Interfaces;

namespace Repositories
{
    public class FeedBackRepository : BaseRepository<Feedback>, IFeedBackRepository
    {
        private readonly FeedBackDAO _feedbackDAO;
        public FeedBackRepository(FeedBackDAO feedbackDAO) : base(feedbackDAO)
        {
            _feedbackDAO = feedbackDAO;
        }
        public async Task<IEnumerable<Feedback>> GetByEventIdAsync(Guid eventId)
        {
            return await _feedbackDAO.GetByEventIdAsync(eventId);
        }

        public async Task<bool> FeedbackExistsAsync(Guid eventId, Guid userId)
        {
            return await _feedbackDAO.FeedbackExistsAsync(eventId, userId);
        }
    }
}
