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
    public class SellerService: SalesWebService, IService<Seller>
    {
        public SellerService(SalesWebContext context) : base(context) { }

        public async Task<bool> CheckExists(int id)
        {
            bool hasAny = await _context.Seller.AnyAsync(item => item.Id == id);
            return hasAny;
        }
        /*
        private async Task<bool> _checkDuplicate(Seller Seller)
        {
            bool hasAny;
            if (Seller.Id == 0)
            {
                hasAny = await _context.Seller.AnyAsync(item => item.Name == Seller.Name);
            } else
            {
                hasAny = await _context.Seller.AnyAsync(item => item.Name == Seller.Name && item.Id != Seller.Id);
            }
            return hasAny;
        }
        */

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller
                            .Include(item => item.Department)
                            .OrderBy(item => item.Name)
                            .ToListAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller
                            .Include(item => item.Department)
                            .FirstOrDefaultAsync(item => item.Id == id);
                            
        }

        public async Task InsertAsync(Seller Seller)
        {
            _context.Add(Seller);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller Seller)
        {
            bool hasAny = await this.CheckExists(Seller.Id);
            if (!hasAny) { throw new NotFoundException("Seller not Found"); }
            try
            {
                _context.Entry(Seller).State = EntityState.Modified;
                _context.Update(Seller);
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
        public async Task RemoveAsync(int id)
        {
            var Seller = await this.FindByIdAsync(id);
            if (Seller != null)
            {
                try
                {
                    _context.Remove(Seller);
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
                throw new NotFoundException("Seller not found");
            }

        }
        
    }
}
