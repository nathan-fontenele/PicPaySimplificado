using System.Text;
using System.Text.Json;
using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Interfaces;
using PicPaySimplificado.DTOs;

namespace PicPaySimplificado.Application
{
    public class Transaction
    {
        private readonly Users _users;
        private readonly ITransactionRepository<Domain.Transaction, Guid> _transactionRepository;
        private readonly Notification _notification;

        public Transaction(
            Users users,
            ITransactionRepository<Domain.Transaction, Guid> transactionRepository)
        {
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _notification = new Notification();
        }

        public async Task<Domain.Transaction> CreateTransactionAsync(TransactionDto transaction)
        {
            var sender = await _users.FindUserByIdAsync(transaction.senderId)
                         ?? throw new ArgumentException($"Sender '{transaction.senderId}' not found");
            var receiver = await _users.FindUserByIdAsync(transaction.receiverId)
                         ?? throw new ArgumentException($"Receiver '{transaction.receiverId}' not found");
            
            _users.ValidateSenderForTransaction(sender, transaction.value);
            
            bool isAuthorized = this._users.ValidateSenderForTransaction(sender, transaction.value);
            if (!isAuthorized)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            if (!await ValidateTransactionAsync(transaction.value))
                throw new InvalidOperationException("Unauthorized transaction");

            var newTransaction = new Domain.Transaction
            {
                Amount = transaction.value,
                SenderId = sender.Guid,
                ReceiverId = receiver.Guid,
                Created = DateTime.UtcNow
            };

            sender.Balance = sender.GetBalance() - newTransaction.Amount;
            receiver.Balance = receiver.GetBalance() + newTransaction.Amount;

            await _transactionRepository.AddAsync(newTransaction);
            await _users.UpdateUserAsync(sender);
            await _users.UpdateUserAsync(receiver);

            _notification.SendeNotification(sender, "Transaction sent successfully");
            _notification.SendeNotification(receiver, "Transaction received successfully");

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
