using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RefitDemo
{
    public interface IAuthorApi
    {
        [Get("/api/Author")]
        Task<IEnumerable<Author>> GetAuthors();

        [Get("/api/Author/{id}")]
        Task<Author> GetAuthor(int id);

        [Post("/api/Author")]
        Task PostAuthor([Body]Author author);

        [Put("/api/Author/{id}")]
        Task PutAuthor(int id, [Body]Author author);

        [Delete("/api/Author/{id}")]
        Task DeleteAuthor(int id);
    }
}
