using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO.Base;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class FeedBackDAO : BaseDAO<Feedback>
    {
        public FeedBackDAO(SeminarManagementDbContext context) : base(context) { }
        public async Task<IEnumerable<Feedback>> GetByEventIdAsync(Guid eventId)
        {
            return await _context.Set<Feedback>()
                .Where(f => f.EventId == eventId)
                .ToListAsync();
        }
        public async Task<bool> FeedbackExistsAsync(Guid eventId, Guid userId)
        {
            return await _context.Feedbacks
                .AnyAsync(f => f.EventId == eventId && f.UserId == userId);
        }

    }
}
