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
    public class FeedbackRepository : BaseRepository<Feedback>, IFeedbackRepository
    {
        private readonly FeedbackDAO _surveyDAO;
        public FeedbackRepository(FeedbackDAO surveyDAO) : base(surveyDAO)
        {
            _surveyDAO = surveyDAO;
        }
    }
}
