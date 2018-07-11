using LitJson;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RefitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            foo();
            //bar();

            Console.ReadKey();
        }

        private async static void foo()
        {
            var authorApi = RestService.For<IAuthorApi>("http://localhost:8794");

            Console.WriteLine("----------Get----------");
            var authors = await authorApi.GetAuthors();
            foreach (var item in authors)
                Console.WriteLine(JsonMapper.ToJson(item));

            Console.WriteLine("----------Get(id)----------");
            var author = await authorApi.GetAuthor(1);
            Console.WriteLine(JsonMapper.ToJson(author));

            Console.WriteLine("----------Post----------");
            await authorApi.PostAuthor(new Author()
            {
                Name = "New Author"
            });
            authors = await authorApi.GetAuthors();
            foreach (var item in authors)
                Console.WriteLine(JsonMapper.ToJson(item));

            Console.WriteLine("----------Put----------");
            author.Name += "_1";
            await authorApi.PutAuthor(1, author);
            author = await authorApi.GetAuthor(1);
            Console.WriteLine(JsonMapper.ToJson(author));

            Console.WriteLine("----------Delete----------");
            var delId = 2;
            await Policy
                .Handle<ApiException>()
                .RetryForeverAsync((ex, count) =>
                {
                    delId++;
                })
                .ExecuteAsync(() => authorApi.DeleteAuthor(delId));
            authors = await authorApi.GetAuthors();
            foreach (var item in authors)
                Console.WriteLine(JsonMapper.ToJson(item));
        }

        private static int compute()
        {
            var a = 0;
            return 1 / a;
        }

        private static void bar()
        {
            try
            {
                Policy
                    .Handle<DivideByZeroException>()
                    .Retry(3, (ex, count) =>
                    {
                        Console.WriteLine("执行失败! 重试次数 {0}", count);
                        Console.WriteLine("异常来自 {0}", ex.GetType().Name);
                    }).Execute(() => compute());
            }
            catch (DivideByZeroException ex)
            {
            }
        }
    }
}
