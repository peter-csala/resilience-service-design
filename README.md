# Resilience Service Design

## Demo application
This is an oversimplified sample application which demonstrates the basics of the [Polly](https://github.com/App-vNext/Polly).  
It shows you how can you easily setup:
- [Timeout](https://github.com/App-vNext/Polly/wiki/Timeout): `GetTimeoutPolicy` ([Ref](https://github.com/peter-csala/resilience-service-design/blob/main/ResilienceServiceDesignDemo/ResilienceServiceDesignDemo/Program.cs#L66))
- [Retry](https://github.com/App-vNext/Polly/wiki/Retry): `GetRetryPolicy` ([Ref](https://github.com/peter-csala/resilience-service-design/blob/main/ResilienceServiceDesignDemo/ResilienceServiceDesignDemo/Program.cs#L81))
- [CircuitBreaker](https://github.com/App-vNext/Polly/wiki/Circuit-Breaker): `GetCircuitBreakerPolicy` ([Ref](https://github.com/peter-csala/resilience-service-design/blob/main/ResilienceServiceDesignDemo/ResilienceServiceDesignDemo/Program.cs#L99))
- [PolicyWrap](https://github.com/App-vNext/Polly/wiki/PolicyWrap): `Policy.WrapAsync` ([Ref](https://github.com/peter-csala/resilience-service-design/blob/main/ResilienceServiceDesignDemo/ResilienceServiceDesignDemo/Program.cs#L37))

It does that in an [asynchronous](https://github.com/App-vNext/Polly/wiki/Asynchronous-action-execution) fashion.   

The program simulates a faulty remote network call (async non-blocking I/O) in the `SampleCallAsync` ([Ref](https://github.com/peter-csala/resilience-service-design/blob/main/ResilienceServiceDesignDemo/ResilienceServiceDesignDemo/Program.cs#L53))

## Resilience Article
It lists a bunch of commonly used resilient patterns.  
It does that in the following way:
1. It categories it
2. It makes clear how does it relate to similar terms or concepts
3. It describes how does it work
4. In some cases it lists the tweaking options (parameters)
5. It brings some examples where it is being used and how
