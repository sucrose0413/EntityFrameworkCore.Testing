﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkCore.Testing.Common;
using EntityFrameworkCore.Testing.Common.Extensions;
using EntityFrameworkCore.Testing.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;

namespace EntityFrameworkCore.Testing.NSubstitute.Extensions
{
    /// <summary>Extensions for the db context type.</summary>
    public static class DbContextExtensions
    {
        private static readonly ILogger Logger = LoggerHelper.CreateLogger(typeof(DbContextExtensions));

        /// <summary>Creates and sets up a substitute db context.</summary>
        /// <typeparam name="TDbContext">The db context type.</typeparam>
        /// <param name="dbContextToMock">The db context to mock/proxy.</param>
        /// <returns>A substitute db context.</returns>
        /// <remarks>dbContextToMock would typically be an in-memory database instance.</remarks>
        public static TDbContext CreateSubstituteDbContext<TDbContext>(this TDbContext dbContextToMock)
            where TDbContext : DbContext
        {
            EnsureArgument.IsNotNull(dbContextToMock, nameof(dbContextToMock));

            var substituteDbContext = (TDbContext)
                Substitute.For(new[] {
                        typeof(TDbContext),
                        typeof(IEnumerable<object>),
                        typeof(IDbContextDependencies),
                        typeof(IDbQueryCache),
                        typeof(IDbSetCache),
                        typeof(IInfrastructure<IServiceProvider>),
                        typeof(IDbContextPoolable),
                        typeof(IDbContextPoolable)
                    },
                    new object[] { }
                );

            substituteDbContext.Add(Arg.Any<object>()).Returns(callInfo => dbContextToMock.Add(callInfo.Arg<object>()));
            substituteDbContext.AddAsync(Arg.Any<object>(), Arg.Any<CancellationToken>()).Returns(callInfo => dbContextToMock.AddAsync(callInfo.Arg<object>(), callInfo.Arg<CancellationToken>()));
            substituteDbContext.When(x => x.AddRange(Arg.Any<object[]>())).Do(callInfo => dbContextToMock.AddRange(callInfo.Arg<object[]>()));
            substituteDbContext.When(x => x.AddRange(Arg.Any<IEnumerable<object>>())).Do(callInfo => dbContextToMock.AddRange(callInfo.Arg<IEnumerable<object>>()));
            substituteDbContext.AddRangeAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>()).Returns(callInfo => dbContextToMock.AddRangeAsync(callInfo.Arg<object[]>(), callInfo.Arg<CancellationToken>()));
            substituteDbContext.AddRangeAsync(Arg.Any<IEnumerable<object>>(), Arg.Any<CancellationToken>()).Returns(callInfo => dbContextToMock.AddRangeAsync(callInfo.Arg<IEnumerable<object>>(), callInfo.Arg<CancellationToken>()));

            substituteDbContext.Attach(Arg.Any<object>()).Returns(callInfo => dbContextToMock.Attach(callInfo.Arg<object>()));
            substituteDbContext.When(x => x.AttachRange(Arg.Any<object[]>())).Do(callInfo => dbContextToMock.AttachRange(callInfo.Arg<object[]>()));
            substituteDbContext.When(x => x.AttachRange(Arg.Any<IEnumerable<object>>())).Do(callInfo => dbContextToMock.AttachRange(callInfo.Arg<IEnumerable<object>>()));

            ((IDbContextDependencies) substituteDbContext).ChangeDetector.Returns(((IDbContextDependencies) dbContextToMock).ChangeDetector);
            substituteDbContext.ChangeTracker.Returns(dbContextToMock.ChangeTracker);
            substituteDbContext.Database.Returns(dbContextToMock.Database);
            substituteDbContext.When(x => x.Dispose()).Do(callInfo => dbContextToMock.Dispose());
            ((IDbContextDependencies) substituteDbContext).EntityFinderFactory.Returns(((IDbContextDependencies) dbContextToMock).EntityFinderFactory);
            ((IDbContextDependencies) substituteDbContext).EntityGraphAttacher.Returns(((IDbContextDependencies) dbContextToMock).EntityGraphAttacher);
            substituteDbContext.Entry(Arg.Any<object>()).Returns(callInfo => dbContextToMock.Entry(callInfo.Arg<object>()));

            substituteDbContext.FindAsync(Arg.Any<Type>(), Arg.Any<object[]>()).Returns(callInfo => dbContextToMock.FindAsync(callInfo.Arg<Type>(), callInfo.Arg<object[]>()));
            substituteDbContext.FindAsync(Arg.Any<Type>(), Arg.Any<object[]>(), Arg.Any<CancellationToken>()).Returns(callInfo => dbContextToMock.FindAsync(callInfo.Arg<Type>(), callInfo.Arg<object[]>(), callInfo.Arg<CancellationToken>()));

            ((IDbQueryCache) substituteDbContext).GetOrAddQuery(Arg.Any<IDbQuerySource>(), Arg.Any<Type>()).Returns(callInfo => ((IDbQueryCache) dbContextToMock).GetOrAddQuery(callInfo.Arg<IDbQuerySource>(), callInfo.Arg<Type>()));
            ((IDbSetCache) substituteDbContext).GetOrAddSet(Arg.Any<IDbSetSource>(), Arg.Any<Type>()).Returns(callInfo => ((IDbSetCache) dbContextToMock).GetOrAddSet(callInfo.Arg<IDbSetSource>(), callInfo.Arg<Type>()));
            ((IDbContextDependencies) substituteDbContext).InfrastructureLogger.Returns(((IDbContextDependencies) dbContextToMock).InfrastructureLogger);
            ((IInfrastructure<IServiceProvider>) substituteDbContext).Instance.Returns(((IInfrastructure<IServiceProvider>) dbContextToMock).Instance);
            ((IDbContextDependencies) substituteDbContext).Model.Returns(((IDbContextDependencies) dbContextToMock).Model);
            ((IDbContextDependencies) substituteDbContext).QueryProvider.Returns(((IDbContextDependencies) dbContextToMock).QueryProvider);
            ((IDbContextDependencies) substituteDbContext).QuerySource.Returns(((IDbContextDependencies) dbContextToMock).QuerySource);

            substituteDbContext.Remove(Arg.Any<object>()).Returns(callInfo => dbContextToMock.Remove(callInfo.Arg<object>()));
            substituteDbContext.When(x => x.RemoveRange(Arg.Any<object[]>())).Do(callInfo => dbContextToMock.RemoveRange(callInfo.Arg<object[]>()));
            substituteDbContext.When(x => x.RemoveRange(Arg.Any<IEnumerable<object>>())).Do(callInfo => dbContextToMock.RemoveRange(callInfo.Arg<IEnumerable<object>>()));

            ((IDbContextPoolable) substituteDbContext).When(x => x.ResetState()).Do(callInfo => ((IDbContextPoolable) dbContextToMock).ResetState());
            ((IDbContextPoolable) substituteDbContext).When(x => x.Resurrect(Arg.Any<DbContextPoolConfigurationSnapshot>())).Do(callInfo => ((IDbContextPoolable) dbContextToMock).Resurrect(callInfo.Arg<DbContextPoolConfigurationSnapshot>()));

            substituteDbContext.SaveChanges().Returns(callInfo => dbContextToMock.SaveChanges());
            substituteDbContext.SaveChanges(Arg.Any<bool>()).Returns(callInfo => dbContextToMock.SaveChanges(callInfo.Arg<bool>()));
            substituteDbContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(callInfo => dbContextToMock.SaveChangesAsync(callInfo.Arg<CancellationToken>()));
            substituteDbContext.SaveChangesAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>()).Returns(callInfo => dbContextToMock.SaveChangesAsync(callInfo.Arg<bool>(), callInfo.Arg<CancellationToken>()));

            ((IDbContextPoolable) substituteDbContext).When(x => x.SetPool(Arg.Any<IDbContextPool>())).Do(callInfo => ((IDbContextPoolable) dbContextToMock).SetPool(callInfo.Arg<IDbContextPool>()));
            ((IDbContextDependencies) substituteDbContext).SetSource.Returns(((IDbContextDependencies) dbContextToMock).SetSource);
            ((IDbContextPoolable) substituteDbContext).SnapshotConfiguration().Returns(callInfo => ((IDbContextPoolable) dbContextToMock).SnapshotConfiguration());
            ((IDbContextDependencies) substituteDbContext).StateManager.Returns(((IDbContextDependencies) dbContextToMock).StateManager);

            substituteDbContext.Update(Arg.Any<object>()).Returns(callInfo => dbContextToMock.Update(callInfo.Arg<object>()));

            ((IDbContextDependencies) substituteDbContext).UpdateLogger.Returns(((IDbContextDependencies) dbContextToMock).UpdateLogger);

            substituteDbContext.When(x => x.UpdateRange(Arg.Any<object[]>())).Do(callInfo => dbContextToMock.UpdateRange(callInfo.Arg<object[]>()));
            substituteDbContext.When(x => x.UpdateRange(Arg.Any<IEnumerable<object>>())).Do(callInfo => dbContextToMock.UpdateRange(callInfo.Arg<IEnumerable<object>>()));

            foreach (var entity in dbContextToMock.Model.GetEntityTypes().Where(x => !x.IsQueryType))
            {
                typeof(DbContextExtensions)
                    .GetMethod(nameof(CreateAndAttachSubstituteDbSetTo), BindingFlags.NonPublic | BindingFlags.Static)
                    .MakeGenericMethod(typeof(TDbContext), entity.ClrType).Invoke(null, new object[] {substituteDbContext, dbContextToMock});
            }

            foreach (var entity in dbContextToMock.Model.GetEntityTypes().Where(x => x.IsQueryType))
            {
                typeof(DbContextExtensions)
                    .GetMethod(nameof(CreateAndAttachSubstituteDbQueryTo), BindingFlags.NonPublic | BindingFlags.Static)
                    .MakeGenericMethod(typeof(TDbContext), entity.ClrType).Invoke(null, new object[] {substituteDbContext, dbContextToMock});
            }

            return substituteDbContext;
        }

        /// <summary>Creates and sets up a substitute db context.</summary>
        /// <typeparam name="TDbContext">The db context type.</typeparam>
        /// <param name="dbContextToMock">The db context to mock/proxy.</param>
        /// <returns>A substitute db context.</returns>
        /// <remarks>dbContextToMock would typically be an in-memory database instance.</remarks>
        [Obsolete("This will be removed in a future version. Use DbContextExtensions.CreateDbContextSubstitute instead.")]
        public static TDbContext CreateMock<TDbContext>(this TDbContext dbContextToMock)
            where TDbContext : DbContext
        {
            return dbContextToMock.CreateSubstituteDbContext();
        }

        /// <summary>Creates and attaches a substitute db set to a substitute db context.</summary>
        /// <typeparam name="TDbContext">The db context type.</typeparam>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="substituteDbContext">The substitute db context.</param>
        /// <param name="dbContextToMock">The db context to mock/proxy.</param>
        private static void CreateAndAttachSubstituteDbSetTo<TDbContext, TEntity>(this TDbContext substituteDbContext, TDbContext dbContextToMock)
            where TDbContext : DbContext
            where TEntity : class
        {
            EnsureArgument.IsNotNull(substituteDbContext, nameof(substituteDbContext));
            EnsureArgument.IsNotNull(dbContextToMock, nameof(dbContextToMock));

            var substituteDbSet = dbContextToMock.Set<TEntity>().CreateSubstituteDbSet();

            var property = typeof(TDbContext).GetProperties().SingleOrDefault(p => p.PropertyType == typeof(DbSet<TEntity>));

            if (property != null)
            {
                property.GetValue(substituteDbContext.Configure()).Returns(substituteDbSet);
            }
            else
            {
                Logger.LogDebug($"Could not find a DbContext property for type '{typeof(TEntity)}'");
            }

            substituteDbContext.Configure().Set<TEntity>().Returns(callInfo => substituteDbSet);

            substituteDbContext.Add(Arg.Any<TEntity>()).Returns(callInfo => dbContextToMock.Add(callInfo.Arg<TEntity>()));
            substituteDbContext.AddAsync(Arg.Any<TEntity>(), Arg.Any<CancellationToken>()).Returns(callInfo => dbContextToMock.AddAsync(callInfo.Arg<TEntity>(), callInfo.Arg<CancellationToken>()));

            substituteDbContext.Attach(Arg.Any<TEntity>()).Returns(callInfo => dbContextToMock.Attach(callInfo.Arg<TEntity>()));
            substituteDbContext.When(x => x.AttachRange(Arg.Any<object[]>())).Do(callInfo => dbContextToMock.AttachRange(callInfo.Arg<object[]>()));
            substituteDbContext.When(x => x.AttachRange(Arg.Any<IEnumerable<object>>())).Do(callInfo => dbContextToMock.AttachRange(callInfo.Arg<IEnumerable<object>>()));

            substituteDbContext.Entry(Arg.Any<TEntity>()).Returns(callInfo => dbContextToMock.Entry(callInfo.Arg<TEntity>()));

            substituteDbContext.Find<TEntity>(Arg.Any<object[]>()).Returns(callInfo => dbContextToMock.Find<TEntity>(callInfo.Arg<object[]>()));
            substituteDbContext.Find(typeof(TEntity), Arg.Any<object[]>()).Returns(callInfo => dbContextToMock.Find(callInfo.Arg<Type>(), callInfo.Arg<object[]>()));
            substituteDbContext.FindAsync<TEntity>(Arg.Any<object[]>()).Returns(callInfo => dbContextToMock.FindAsync<TEntity>(callInfo.Arg<object[]>()));
            substituteDbContext.FindAsync<TEntity>(Arg.Any<object[]>(), Arg.Any<CancellationToken>()).Returns(callInfo => dbContextToMock.FindAsync<TEntity>(callInfo.Arg<object[]>(), callInfo.Arg<CancellationToken>()));

            substituteDbContext.Remove(Arg.Any<TEntity>()).Returns(callInfo => dbContextToMock.Remove(callInfo.Arg<TEntity>()));

            substituteDbContext.Update(Arg.Any<TEntity>()).Returns(callInfo => dbContextToMock.Update(callInfo.Arg<TEntity>()));
        }

        /// <summary>Creates and attaches a substitute db query to a substitute db context.</summary>
        /// <typeparam name="TDbContext">The db context type.</typeparam>
        /// <typeparam name="TQuery">The query type.</typeparam>
        /// <param name="substituteDbContext">The substitute db context.</param>
        /// <param name="dbContextToMock">The db context to mock/proxy.</param>
        private static void CreateAndAttachSubstituteDbQueryTo<TDbContext, TQuery>(this TDbContext substituteDbContext, TDbContext dbContextToMock)
            where TDbContext : DbContext
            where TQuery : class
        {
            EnsureArgument.IsNotNull(substituteDbContext, nameof(substituteDbContext));
            EnsureArgument.IsNotNull(dbContextToMock, nameof(dbContextToMock));

            var substituteDbQuery = dbContextToMock.Query<TQuery>().CreateSubstituteDbQuery();

            var property = typeof(TDbContext).GetProperties().SingleOrDefault(p => p.PropertyType == typeof(DbQuery<TQuery>));

            if (property != null)
            {
                property.GetValue(substituteDbContext.Configure()).Returns(substituteDbQuery);
            }
            else
            {
                Logger.LogDebug($"Could not find a DbContext property for type '{typeof(TQuery)}'");
            }

            substituteDbContext.Configure().Query<TQuery>().Returns(callInfo => substituteDbQuery);
        }

        /// <summary>Sets up ExecuteSqlCommand invocations to return a specified result.</summary>
        /// <typeparam name="TDbContext">The db context type.</typeparam>
        /// <param name="substituteDbContext">The substitute db context.</param>
        /// <param name="executeSqlCommandResult">The integer to return when ExecuteSqlCommand is invoked.</param>
        /// <param name="callback">Operations to perform after ExecuteSqlCommand is invoked.</param>
        /// <returns>The substitute db context.</returns>
        public static TDbContext AddExecuteSqlCommandResult<TDbContext>(this TDbContext substituteDbContext, int executeSqlCommandResult, Action callback = null)
            where TDbContext : DbContext
        {
            EnsureArgument.IsNotNull(substituteDbContext, nameof(substituteDbContext));

            return substituteDbContext.AddExecuteSqlCommandResult(string.Empty, new List<object>(), executeSqlCommandResult, callback);
        }

        /// <summary>Sets up ExecuteSqlCommand invocations containing a specified sql string to return a specified result.</summary>
        /// <typeparam name="TDbContext">The db context type.</typeparam>
        /// <param name="substituteDbContext">The substitute db context.</param>
        /// <param name="sql">The ExecuteSqlCommand sql string. Set up supports case insensitive partial matches.</param>
        /// <param name="executeSqlCommandResult">The integer to return when ExecuteSqlCommand is invoked.</param>
        /// <param name="callback">Operations to perform after ExecuteSqlCommand is invoked.</param>
        /// <returns>The substitute db context.</returns>
        public static TDbContext AddExecuteSqlCommandResult<TDbContext>(this TDbContext substituteDbContext, string sql, int executeSqlCommandResult, Action callback = null)
            where TDbContext : DbContext
        {
            EnsureArgument.IsNotNull(substituteDbContext, nameof(substituteDbContext));
            EnsureArgument.IsNotNull(sql, nameof(sql));

            return substituteDbContext.AddExecuteSqlCommandResult(sql, new List<object>(), executeSqlCommandResult, callback);
        }

        /// <summary>Sets up ExecuteSqlCommand invocations containing a specified sql string and parameters to return a specified result.</summary>
        /// <typeparam name="TDbContext">The db context type.</typeparam>
        /// <param name="substituteDbContext">The substitute db context.</param>
        /// <param name="sql">The ExecuteSqlCommand sql string. Set up supports case insensitive partial matches.</param>
        /// <param name="parameters">The ExecuteSqlCommand parameters. Set up supports case insensitive partial parameter sequence matching.</param>
        /// <param name="executeSqlCommandResult">The integer to return when ExecuteSqlCommand is invoked.</param>
        /// <param name="callback">Operations to perform after ExecuteSqlCommand is invoked.</param>
        /// <returns>The substitute db context.</returns>
        public static TDbContext AddExecuteSqlCommandResult<TDbContext>(this TDbContext substituteDbContext, string sql, IEnumerable<object> parameters, int executeSqlCommandResult, Action callback = null)
            where TDbContext : DbContext
        {
            EnsureArgument.IsNotNull(substituteDbContext, nameof(substituteDbContext));
            EnsureArgument.IsNotNull(sql, nameof(sql));
            EnsureArgument.IsNotNull(parameters, nameof(parameters));

            var relationalCommand = Substitute.For<IRelationalCommand>();
            relationalCommand
                .ExecuteNonQuery(Arg.Any<IRelationalConnection>(), Arg.Any<IReadOnlyDictionary<string, object>>())
                .Returns(callInfo => executeSqlCommandResult)
                .AndDoes(callInfo => { callback?.Invoke(); });

            relationalCommand
                .ExecuteNonQueryAsync(Arg.Any<IRelationalConnection>(), Arg.Any<IReadOnlyDictionary<string, object>>(), Arg.Any<CancellationToken>())
                .Returns(callInfo => Task.FromResult(executeSqlCommandResult))
                .AndDoes(callInfo => { callback?.Invoke(); });

            var rawSqlCommand = Substitute.For<RawSqlCommand>(relationalCommand, new Dictionary<string, object>());
            rawSqlCommand.RelationalCommand.Returns(relationalCommand);
            rawSqlCommand.ParameterValues.Returns(new Dictionary<string, object>());

            var rawSqlCommandBuilder = Substitute.For<IRawSqlCommandBuilder>();

            rawSqlCommandBuilder.Build(Arg.Any<string>(), Arg.Any<IEnumerable<object>>()).Throws(callInfo =>
            {
                Logger.LogDebug("Catch all exception invoked");
                return new InvalidOperationException();
            });

            rawSqlCommandBuilder
                .Build(
                    Arg.Is<string>(s => s.Contains(sql, StringComparison.CurrentCultureIgnoreCase)),
                    Arg.Is<IEnumerable<object>>(p => ParameterMatchingHelper.DoInvocationParametersMatchSetUpParameters(parameters, p))
                )
                .Returns(callInfo => rawSqlCommand)
                .AndDoes(callInfo =>
                {
                    var providedSql = callInfo.Arg<string>();
                    var providedParameters = callInfo.Arg<IEnumerable<object>>();

                    var parts = new List<string>();
                    parts.Add($"Invocation sql: {providedSql}");
                    parts.Add("Invocation Parameters:");
                    parts.Add(ParameterMatchingHelper.StringifyParameters(providedParameters));
                    Logger.LogDebug(string.Join(Environment.NewLine, parts));
                });

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(Arg.Is<Type>(t => t == typeof(IConcurrencyDetector))).Returns(callInfo => Substitute.For<IConcurrencyDetector>());
            serviceProvider.GetService(Arg.Is<Type>(t => t == typeof(IRawSqlCommandBuilder))).Returns(callInfo => rawSqlCommandBuilder);
            serviceProvider.GetService(Arg.Is<Type>(t => t == typeof(IRelationalConnection))).Returns(callInfo => Substitute.For<IRelationalConnection>());

            var databaseFacade = Substitute.For<DatabaseFacade>(substituteDbContext);
            ((IInfrastructure<IServiceProvider>) databaseFacade).Instance.Returns(serviceProvider);

            substituteDbContext.Database.Returns(databaseFacade);

            return substituteDbContext;
        }
    }
}