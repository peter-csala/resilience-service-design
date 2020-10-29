using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace ResilienceServiceDesignDemo
{
    class Program
    {
        static async Task Main()
        {
            PrintMessage("Started");
            
            try
            {
                //Step 0 - Without any resilience policy
                //var result = await SampleCallAsync();

                //Step 1/A - With timeout policy first try (without passing cancellation token)
                //var strategy = GetTimeoutPolicy;
                //var result = await strategy.ExecuteAsync(async () => await SampleCallAsync());

                //Step 1/B - With timeout policy first try (without passing cancellation token)
                //var strategy = GetTimeoutPolicy;
                //var result = await strategy.ExecuteAsync(async (ct) => await SampleCallAsync(ct), CancellationToken.None);

                //Step 2 - With retry and timeout policies
                //var strategy = Policy.WrapAsync(GetRetryPolicy, GetTimeoutPolicy);
                //var result = await strategy.ExecuteAsync(async (ct) => await SampleCallAsync(ct), CancellationToken.None);

                //Step 3/A - With retry, circuit breaker and timeout policies
                var strategy = Policy.WrapAsync(GetRetryPolicy, GetCircuitBreakerPolicy, GetTimeoutPolicy);
                var result = await strategy.ExecuteAsync(async (ct) => await SampleCallAsync(ct), CancellationToken.None);

                //Step 3/B - Change `RetryCount` const value from 2 to 6

                PrintMessage($"Finished successfully with {result}");
            }
            catch (Exception e)
            {
                PrintMessage($"Failed with {e.GetType().Name}");
            }
            
            Console.ReadKey();
        }

        private static int _times = 1;
        private static async Task<string> SampleCallAsync(CancellationToken ct = default)
        {
            PrintMessage($"{nameof(SampleCallAsync)} has been called {_times++}th times.");
            await Task.Delay(15000, ct);
            throw new CustomException($"{nameof(SampleCallAsync)} failed.");
        }

        private static readonly Lazy<Stopwatch> Timer = new Lazy<Stopwatch>(Stopwatch.StartNew);
        private static void PrintMessage(string message) => Console.WriteLine($"{Timer.Value.Elapsed:ss\\.fff}: {message}");

        #region Timeout
        private const int TimeoutInMilliseconds = 1000;
        
        private static AsyncTimeoutPolicy GetTimeoutPolicy
            => Policy
                .TimeoutAsync(
                    timeout: TimeSpan.FromMilliseconds(TimeoutInMilliseconds),
                    onTimeoutAsync: (context, timeout, _, exception) =>
                    {
                        PrintMessage($"{"Timeout",-10}{timeout,-10:ss\\.fff}: {exception.GetType().Name}");
                        return Task.CompletedTask;
                    });
        #endregion

        #region Retry
        private const int RetryCount = 2;
        private const int RetrySleepDurationInMilliseconds = 200;
        
        private static AsyncRetryPolicy GetRetryPolicy
            => Policy
                .Handle<CustomException>()
                .Or<BrokenCircuitException>()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(
                    retryCount: RetryCount,
                    sleepDurationProvider: _ => TimeSpan.FromMilliseconds(RetrySleepDurationInMilliseconds),
                    onRetry: (exception, delay, context) =>
                    {
                        PrintMessage($"{"Retry",-10}{delay,-10:ss\\.fff}: {exception.GetType().Name}");
                    });
        #endregion

        #region Circuit Breaker
        private const int CircuitBreakerFailCountInCloseState = 2;
        private const int CircuitBreakerDelayInMillisecondsBetweenOpenAndHalfOpenStates = 1000;
        
        private static AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy
            => Policy
                .Handle<CustomException>()
                .Or<TimeoutRejectedException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: CircuitBreakerFailCountInCloseState,
                    durationOfBreak: TimeSpan.FromMilliseconds(CircuitBreakerDelayInMillisecondsBetweenOpenAndHalfOpenStates),
                    onBreak: (ex, span) => PrintMessage($"{"Break",-10}{span,-10:ss\\.fff}: {ex.GetType().Name}"),
                    onReset: () => PrintMessage($"{"Reset",-10}"),
                    onHalfOpen: () => PrintMessage($"{"HalfOpen",-10}"));
        #endregion
    }

    public class CustomException : Exception
    {
        public CustomException(string message) : base(message) { }
    }
}
