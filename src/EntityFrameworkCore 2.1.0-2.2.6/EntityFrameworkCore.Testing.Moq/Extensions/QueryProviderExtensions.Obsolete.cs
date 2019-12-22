﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkCore.Testing.Moq.Extensions
{
    /// <summary>Extensions for query provider and mock query provider types.</summary>
    public static partial class QueryProviderExtensions
    {
        /// <summary>Creates a mocked query provider.</summary>
        /// <typeparam name="T">The query provider source item type.</typeparam>
        /// <param name="queryProviderToMock">The query provider to mock.</param>
        /// <param name="enumerable">The query provider source.</param>
        /// <returns>A mocked query provider.</returns>
        [Obsolete("This will be removed in a future version. Use QueryProviderExtensions.CreateMockedQueryProvider instead.")]
        public static IQueryProvider CreateMock<T>(this IQueryProvider queryProviderToMock, IEnumerable<T> enumerable)
            where T : class
        {
            return queryProviderToMock.CreateMockedQueryProvider(enumerable);
        }
    }
}