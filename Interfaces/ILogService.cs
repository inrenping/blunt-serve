using BluntServe.Models;

namespace BluntServe.Interfaces
{
    /// <summary>
    /// 日志处理
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// 新增日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        Task SaveLogAsync(SysLog log);
    }
}
