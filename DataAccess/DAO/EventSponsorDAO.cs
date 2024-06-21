using BusinessObject.Models;
using DataAccess.DAO.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.DAO;

public class EventSponsorDAO : BaseDAO<EventSponsor>
{
    public EventSponsorDAO(SeminarManagementDbContext context) : base(context)
    {
        
    }

    public async Task<EventSponsor> GetFirstOrDefaultAsync(Expression<Func<EventSponsor, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }
}