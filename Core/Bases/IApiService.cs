﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusillade;
using Refit;
using Polly;

namespace Core
{
    public interface IApiService<T, in TKey> where T : class
    {
        //[Post("")]
        //Task<T> Create([Body] T payload);

        [Get("")]
        Task<List<T>> FindAll();

        //[Get("")]
        //Task<List<T>> FindAllPagination([AliasAs("offset")] int offet, [AliasAs("limit")] int limit);

        [Get("/{key}")]
        Task<T> FindOne(TKey key);

        //[Put("/{key}")]
        //Task Update(TKey key, [Body]T payload);

        //[Delete("/{key}")]
        //Task Delete(TKey key);
    }
}
