using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace ZeroFramework.EventBus.RabbitMQ
{
    public class DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory, ILogger<DefaultRabbitMQPersistentConnection> logger, int retryCount = 5) : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));

        private readonly ILogger<DefaultRabbitMQPersistentConnection> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        private readonly int _retryCount = retryCount;

        private IConnection? _connection = null;

        readonly object _syncRoot = new();

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel()
        {
            if (!IsConnected || _connection is null)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");

            lock (_syncRoot)
            {
                for (int retryAttempt = 1; retryAttempt <= _retryCount; retryAttempt++)
                {
                    var time = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));

                    try
                    {
                        _connection = _connectionFactory.CreateConnection();
                        break;
                    }
                    catch (SystemException ex) when (ex is BrokerUnreachableException || ex is SocketException)
                    {
                        _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                    }

                    Task.Delay(time).Wait();
                }

                if (IsConnected && _connection is not null)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown!;
                    _connection.CallbackException += OnCallbackException!;
                    _connection.ConnectionBlocked += OnConnectionBlocked!;

                    _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);

                    return true;
                }
                else
                {
                    _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

                    return false;
                }
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }

        private bool _disposed;

        // Protected implementation of Dispose pattern.
        //https://docs.microsoft.com/zh-cn/dotnet/standard/garbage-collection/implementing-dispose
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            // Release any managed resources here.
            if (disposing)
            {
                // dispose managed state (managed objects).
                _connection?.Dispose();
            }

            // free unmanaged resources (unmanaged objects) and override a finalizer below.
            // set large fields to null.

            _disposed = true;

            // Call the base class implementation.
            //base.Dispose(disposing);
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DefaultRabbitMQPersistentConnection() => Dispose(false);
    }
}
