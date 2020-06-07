using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;
using SalesAPI.Models.Entities;
using SalesAPI.Services;
using SalesAPI.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SalesAPI.Services.Sales
{
    public class DepartmentService: SalesWebService, IService<Department>
    {
        public DepartmentService(SalesWebContext context) : base(context) { }

        public async Task<bool> CheckExists(int id)
        {
            bool hasAny = await _context.Department.AnyAsync(item => item.Id == id);
            return hasAny;
        }
        private async Task<bool> _checkDuplicate(Department department)
        {
            bool hasAny;
            if (department.Id == 0)
            {
                hasAny = await _context.Department.AnyAsync(item => item.Name == department.Name);
            } else
            {
                hasAny = await _context.Department.AnyAsync(item => item.Name == department.Name && item.Id != department.Id);
            }
            return hasAny;
        }

        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department
                            .OrderBy(item => item.Name)
                            .ToListAsync();
        }

        public async Task<Department> FindByIdAsync(int id)
        {
            return await _context.Department
                            .FirstOrDefaultAsync(item => item.Id == id);
                            
        }

        public async Task InsertAsync(Department department)
        {
            if (await this._checkDuplicate(department)) { throw new DuplicateException<Department>(_context.Department); }
            _context.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            bool hasAny = await this.CheckExists(department.Id);
            if (!hasAny) { throw new NotFoundException("Department not Found"); }
            if (await this._checkDuplicate(department)) { throw new DuplicateException<Department>(_context.Department); }
            try
            {
                _context.Entry(department).State = EntityState.Modified;
                _context.Update(department);
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
        public async Task RemoveAsync(int id)
        {
            var department = await this.FindByIdAsync(id);
            if (department != null)
            {
                try
                {
                    _context.Remove(department);
                    await _context.SaveChangesAsync();
                    return;
                }
                catch (DbUpdateException e)
                {
                    throw new IntegrityException(e.Message);
                }
            }
            else
            {
                throw new NotFoundException("Department not found");
            }

        }
        
    }
}
