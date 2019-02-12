using System;
using System.Threading;
using System.Threading.Tasks;

namespace RockLib.Encryption.Async
{
    /// <summary>
    /// An implementation of <see cref="IAsyncEncryptor"/> that delegates all functionality
    /// to an instance of <see cref="IEncryptor"/>. All tasks returned by its methods are
    /// guaranteed to be already completed or faulted.
    /// </summary>
    public sealed class SynchronousAsyncEncryptor : IAsyncEncryptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronousAsyncEncryptor"/> class.
        /// </summary>
        /// <param name="encryptor">
        /// The instance of <see cref="IEncryptor"/> that performs the actual work
        /// done by the methods of this instance of <see cref="SynchronousAsyncEncryptor"/>.
        /// </param>
        public SynchronousAsyncEncryptor(IEncryptor encryptor)
        {
            Encryptor = encryptor ?? throw new ArgumentNullException(nameof(encryptor));
        }

        /// <summary>
        /// Gets the instance of <see cref="IEncryptor"/> that performs the actual work
        /// done by the methods of this instance of <see cref="SynchronousAsyncEncryptor"/>.
        /// </summary>
        public IEncryptor Encryptor { get; }

        /// <summary>
        /// Synchronously encrypts the specified plain text.
        /// <para>The <see cref="Task{TResult}"/> returned by this method is guaranteed
        /// to be either completed or faulted.</para>
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A completed task whose result represents the encrypted value as a string.</returns>
        public Task<string> EncryptAsync(string plainText, CancellationToken cancellationToken = default(CancellationToken))
        {
            var completion = new TaskCompletionSource<string>();
            try
            {
                completion.SetResult(Encryptor.Encrypt(plainText));
            }
            catch (Exception ex)
            {
                completion.SetException(ex);
            }
            return completion.Task;
        }

        /// <summary>
        /// Synchronously encrypts the specified plain text.
        /// <para>The <see cref="Task{TResult}"/> returned by this method is guaranteed
        /// to be either completed or faulted.</para>
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A completed task whose result represents the encrypted value as a byte array.</returns>
        public Task<byte[]> EncryptAsync(byte[] plainText, CancellationToken cancellationToken = default(CancellationToken))
        {
            var completion = new TaskCompletionSource<byte[]>();
            try
            {
                completion.SetResult(Encryptor.Encrypt(plainText));
            }
            catch (Exception ex)
            {
                completion.SetException(ex);
            }
            return completion.Task;
        }
    }
}
