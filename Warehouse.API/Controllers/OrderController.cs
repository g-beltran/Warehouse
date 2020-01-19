using System;
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
    public class OrderController : ControllerBase
    {
        private readonly WarehouseDBContext _context;

        public OrderController(WarehouseDBContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Order.ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Order
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            var item = _context.Item.FirstOrDefault(x => x.Sku.ToLower() == order.Sku.ToLower());

            if (item == null)
                return new ConflictResult();

            if (item.StockQuantity < order.Quantity)
                return new ConflictResult();

            var userExists = _context.User.Any(x => x.Id == order.UserId);

            if (!userExists)
                return new ForbidResult();


            item.StockQuantity -= order.Quantity;
            order.Modified = DateTime.Now;
            order.Status = (int)Enumerations.OrderStatus.Active;

            _context.Entry(item).State = EntityState.Modified;
            _context.Order.Add(order);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var item = _context.Item.FirstOrDefault(x => x.Sku.ToLower() == order.Sku.ToLower());

            _context.Order.Remove(order);

            item.StockQuantity += order.Quantity;
            _context.Entry(item).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return order;
        }

        [HttpPut("Cancel/{id}")]
        [Authorize]
        public async Task<ActionResult<Order>> CancelOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }          

            var item = _context.Item.FirstOrDefault(x => x.Sku.ToLower() == order.Sku.ToLower());

            item.StockQuantity += order.Quantity;
            order.Status = (int)Enumerations.OrderStatus.Canceled;
            order.Quantity = 0;

            _context.Entry(order).State = EntityState.Modified;
            _context.Entry(item).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
