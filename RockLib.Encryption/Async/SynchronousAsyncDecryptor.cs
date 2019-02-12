using System;
using System.Threading;
using System.Threading.Tasks;

namespace RockLib.Encryption.Async
{
    /// <summary>
    /// An implementation of <see cref="IAsyncDecryptor"/> that delegates all functionality
    /// to an instance of <see cref="IAsyncDecryptor"/>. All tasks returned by its methods are
    /// guaranteed to be already completed or faulted.
    /// </summary>
    public sealed class SynchronousAsyncDecryptor : IAsyncDecryptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronousAsyncDecryptor"/> class.
        /// </summary>
        /// <param name="decryptor">
        /// The instance of <see cref="IDecryptor"/> that performs the actual work
        /// done by the methods of this instance of <see cref="SynchronousAsyncDecryptor"/>.
        /// </param>
        public SynchronousAsyncDecryptor(IDecryptor decryptor)
        {
            Decryptor = decryptor ?? throw new ArgumentNullException(nameof(decryptor));
        }

        /// <summary>
        /// Gets the instance of <see cref="IDecryptor"/> that performs the actual work
        /// done by the methods of this instance of <see cref="SynchronousAsyncDecryptor"/>.
        /// </summary>
        public IDecryptor Decryptor { get; }

        /// <summary>
        /// Synchronously decrypts the specified cipher text.
        /// <para>The <see cref="Task{TResult}"/> returned by this method is guaranteed
        /// to be either completed or faulted.</para>
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A completed task whose result represents the decrypted value as a string.</returns>
        public Task<string> DecryptAsync(string cipherText, CancellationToken cancellationToken = default(CancellationToken))
        {
            var completion = new TaskCompletionSource<string>();
            try
            {
                completion.SetResult(Decryptor.Decrypt(cipherText));
            }
            catch (Exception ex)
            {
                completion.SetException(ex);
            }
            return completion.Task;
        }

        /// <summary>
        /// Synchronously decrypts the specified cipher text.
        /// <para>The <see cref="Task{TResult}"/> returned by this method is guaranteed
        /// to be either completed or faulted.</para>
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A completed task whose result represents the decrypted value as a byte array.</returns>
        public Task<byte[]> DecryptAsync(byte[] cipherText, CancellationToken cancellationToken = default(CancellationToken))
        {
            var completion = new TaskCompletionSource<byte[]>();
            try
            {
                completion.SetResult(Decryptor.Decrypt(cipherText));
            }
            catch (Exception ex)
            {
                completion.SetException(ex);
            }
            return completion.Task;
        }
    }
}
