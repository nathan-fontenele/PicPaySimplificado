using System.Text;
using System.Text.Json;
using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Interfaces;
using PicPaySimplificado.DTOs;

namespace PicPaySimplificado.Application
{
    public class TransactionService
    {
        private readonly UsersService _usersService;
        private readonly ITransactionRepository<Transaction, Guid> _transactionRepository;
        private readonly NotificationService _notificationService;

        public TransactionService(
            UsersService usersService,
            ITransactionRepository<Transaction, Guid> transactionRepository)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _notificationService = new NotificationService();
        }

        public async Task<Transaction> CreateTransactionAsync(TransactionDto transaction)
        {
            var sender = await _usersService.FindUserByIdAsync(transaction.senderId)
                         ?? throw new ArgumentException($"Sender '{transaction.senderId}' not found");
            var receiver = await _usersService.FindUserByIdAsync(transaction.receiverId)
                         ?? throw new ArgumentException($"Receiver '{transaction.receiverId}' not found");
            
            _usersService.ValidateSenderForTransaction(sender, transaction.value);
            
            bool isAuthorized = this._usersService.ValidateSenderForTransaction(sender, transaction.value);
            if (!isAuthorized)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            if (!await ValidateTransactionAsync(transaction.value))
                throw new InvalidOperationException("Unauthorized transaction");

            var newTransaction = new Transaction
            {
                Amount = transaction.value,
                SenderId = sender._guid,
                ReceiverId = receiver._guid,
                Created = DateTime.UtcNow
            };

            sender._balance = sender.GetBalance() - newTransaction.Amount;
            receiver._balance = receiver.GetBalance() + newTransaction.Amount;

            await _transactionRepository.AddAsync(newTransaction);
            await _usersService.UpdateUserAsync(sender);
            await _usersService.UpdateUserAsync(receiver);

            _notificationService.SendeNotification(sender, "Transaction sent successfully");
            _notificationService.SendeNotification(receiver, "Transaction received successfully");

            return newTransaction;
        }

        private async Task<bool> ValidateTransactionAsync(decimal amount)
        {
            using var httpClient = new HttpClient();
            
            try
            {
                var requestUri = $"https://util.devi.tools/api/v2/authorize";
                var response = await httpClient.GetAsync(requestUri);
                
                if (!response.IsSuccessStatusCode)
                    return false;

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ValidationTransactionDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                
                return string.Equals(result?.Status, "success", StringComparison.OrdinalIgnoreCase)
                       && (result.Data?.Authorization ?? false);

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
