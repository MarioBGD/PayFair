using PayFair.WebApi.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PayFair.WebApi.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly Guid _id;
        protected readonly IDbConnection _connection;

        protected IDbTransaction _transaction;
        public UnitOfWork(IDbConnection connection)
        {
            _id = new Guid();
            _connection = connection;

            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public UnitOfWork()
        {
            _id = new Guid();
            _connection = new SqlConnection(Config.DbConnectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public virtual void Commit()
        {
            _transaction.Commit();
            _transaction.Dispose();
        }


        /// <summary>
        /// Discard all changes by transaction
        /// </summary>
        public virtual void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        public virtual void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
            _transaction = null;

            _connection.Dispose();
        }

        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        public IDbConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return _transaction;
            }
        }
        //public ITestRepository Tests { get; }
    }
}
