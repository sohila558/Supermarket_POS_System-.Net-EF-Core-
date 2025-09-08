using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MySuperMarket.Data;
using MySuperMarket.DTOs;
using MySuperMarket.Models;

namespace MySuperMarket.Repository.InvoiceRepository
{
    public class RepoInvoices : IRepoInvoices
    {
        protected readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public RepoInvoices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(AddInvoiceDTO invoice)
        {

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var inv = _mapper.Map<Invoice>(invoice);


                var addInvoice = await _context.Invoices.AddAsync(inv);
                await _context.SaveChangesAsync();

                foreach (var item in inv.InvoiceItems)
                {
                    var product = await _context
                        .Products
                        .Where(p => p.ProductId == item.ProductId)
                        .SingleOrDefaultAsync();

                    if (product == null)
                    {
                        break;
                    }

                    product.Stock -= item.Quantity;
                }


                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();

            }
        }

        public async Task<List<AddInvoiceDTO>> GetAllAsync(AddInvoiceDTO dto)
        {
            var inv = await _context.Invoices.Include(i => i.InvoiceItems).ToListAsync();
            return _mapper.Map<List<AddInvoiceDTO>>(inv);
        }

        public async Task<AddInvoiceDTO?> GetByIdASync(int id)
        {
            var inv = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .SingleOrDefaultAsync(i => i.InvoiceId == id);

            if (inv == null)
            {
                return null;
            }

            return _mapper.Map<AddInvoiceDTO>(inv);
        }

        public async Task<bool> RefundAsync(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                //High level: Copy invoice to a new invoice with minus total and unit prices.

                //Low level:


                //3. Save invoice into database.
                //4. Add old items into stock.

                //1. Get invoice by invoice id, including invoice items
                var invoiceToBeRefunded = await _context.Invoices
                    .Include(i => i.InvoiceItems)
                    .Where(i => i.InvoiceId == id)
                    .FirstOrDefaultAsync();

                if (invoiceToBeRefunded == null)
                {
                    return false;
                }

                //2. Create a new invoice with the same data of old invoice
                var refundedInvoice = _mapper.Map<Invoice>(invoiceToBeRefunded);
                refundedInvoice.IsRefunded = true;

                await _context.SaveChangesAsync();
                refundedInvoice.InvoiceId = 0;

                await _context.AddAsync(refundedInvoice);
                await _context.SaveChangesAsync();
                //3. Make new invoice with minus values
                foreach (var item in refundedInvoice.InvoiceItems)
                {
                    item.UnitPrice = item.UnitPrice * -1;
                    item.TotalPrice = item.TotalPrice * -1;
                    item.InvoiceId = refundedInvoice.InvoiceId;

                    var itemProduct = await _context.Products.FindAsync(item.ProductId);

                    if (itemProduct is null)
                        break;


                    itemProduct.Stock += item.Quantity;
                }

                await _context.SaveChangesAsync();

                transaction.Commit();

                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();

                return false;
            }
        }

        public async Task<bool> RefundPartialAsync(RefundedInvoiceDTO dto)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var invoiceToBeRefunded = await _context.Invoices
                    .Include(i => i.InvoiceItems)
                    .Where(i => i.InvoiceId == dto.InvoiceId)
                    .FirstOrDefaultAsync();

                if (invoiceToBeRefunded is null)
                    return false;


                var refundedInvoice = _mapper.Map<Invoice>(invoiceToBeRefunded);

                refundedInvoice.InvoiceId = 0;
                refundedInvoice.IsRefunded = true;

                await _context.SaveChangesAsync();


                await _context.AddAsync(refundedInvoice);
                await _context.SaveChangesAsync();

                foreach (var item in refundedInvoice.InvoiceItems)
                {
                    item.UnitPrice = item.UnitPrice * -1;
                    item.TotalPrice = item.TotalPrice * -1;
                    item.InvoiceId = refundedInvoice.InvoiceId;

                    var itemProduct = await _context.Products.FindAsync(item.ProductId);

                    if (itemProduct == null)
                    {
                        break;
                    }

                    itemProduct.Stock += item.Quantity;
                }

                await _context.SaveChangesAsync();

                transaction.Commit();

                return true;
            }
            catch
            {
                transaction.Rollback();

                return false;
            }
        }
    }
}
