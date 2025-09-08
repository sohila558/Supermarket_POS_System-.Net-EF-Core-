using MySuperMarket.DTOs;

namespace MySuperMarket.Repository.InvoiceRepository
{
    public interface IRepoInvoices
    {
        Task AddAsync(AddInvoiceDTO invoice);
        Task<List<AddInvoiceDTO>> GetAllAsync(AddInvoiceDTO dto);
        Task<AddInvoiceDTO?> GetByIdASync(int id);
        Task<bool> RefundAsync(int id);
        Task<bool> RefundPartialAsync(RefundedInvoiceDTO dto);
    }
}
