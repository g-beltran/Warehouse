﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Entities;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly WarehouseDBContext _context;

        public ItemController(WarehouseDBContext context)
        {
            _context = context;
        }

        // GET: api/Item
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Item>>> GetItem()
        {
            return await _context.Item.ToListAsync();
        }

        // GET: api/Item/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(string id)
        {
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Item/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutItem(string id, Item item)
        {
            if (id != item.Sku)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Item
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Item.Add(item);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ItemExists(item.Sku))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetItem", new { id = item.Sku }, item);
        }

        // DELETE: api/Item/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Item>> DeleteItem(string id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        [HttpGet("Available/{maxItems}/{pageNumber}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Item>>> GetAvailableItems(int maxItems, int pageNumber)
        {
            return await _context.Item
                .Where(x=>x.StockQuantity>0)
                .OrderBy(x => x.Sku)
                .Skip(maxItems * (pageNumber - 1))
                .Take(maxItems)
                .ToListAsync();            
        }

        [HttpGet("Search/{sku}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Item>>> SearchForItem(string sku)
        {
            return await _context.Item                
                .Where(x=>x.Sku.ToLower().StartsWith(sku.ToLower()))                
                .ToListAsync();
        }

        private bool ItemExists(string id)
        {
            return _context.Item.Any(e => e.Sku == id);
        }
    }
}
