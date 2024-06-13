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
    public class SurveyRepository : BaseRepository<Survey>, ISurveyRepository
    {
        private readonly SurveyDAO _surveyDAO;
        public SurveyRepository(SurveyDAO surveyDAO) : base(surveyDAO)
        {
            _surveyDAO = surveyDAO;
        }
    }
}
