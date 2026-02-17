using BluntServe.Data;
using BluntServe.Interfaces;
using BluntServe.Models;
using Microsoft.EntityFrameworkCore;

namespace BluntServe.Services
{
    public class LogService : ILogService
    {
        private readonly PgDbContext _dbContext;
        public LogService(PgDbContext dbContext) => _dbContext = dbContext;

        public async Task SaveLogAsync(SysLog log)
        {
            _dbContext.SysLogs.Add(log);
            await _dbContext.SaveChangesAsync();
        }
    }
}
