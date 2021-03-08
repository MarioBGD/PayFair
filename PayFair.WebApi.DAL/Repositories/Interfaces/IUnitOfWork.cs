using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PayFair.WebApi.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Guid Id { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void Commit();
        void Rollback();
    }
}
