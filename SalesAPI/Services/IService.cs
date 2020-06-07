using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAPI.Services
{
    public interface IService<T>
    {
        Task<bool> CheckExists(int id);
        Task<List<T>> FindAllAsync();
    }
}
