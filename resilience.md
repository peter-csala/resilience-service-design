# Resilience
## Definitions
### Primitives
First let’s define the primitives. They are also known as threats.  
-	**Error**: Incorrect result (a.k.a Mistake)  
-	**Fault**: Incorrect state/code (a.k.a. Defect)  
-	**Failure**: Observable incorrect behaviour  

So, an Error can cause a Fault which might end up in a Failure.

Why do we care about these?  
 *„Everything fails all the time”* - Werner Vogels

### Quality Attributes
Now we know the basics, let’s see what sort of concepts can we derive from them.   
-	**Availability**: Probability of a system to operate in a given uptime  
-	**Durability**: Ability of a system to operate for a given (long) period without interruption  
-	**Robustness**: Ability of a system to handle erroneous inputs and errors during execution  
-	**Fault Tolerance**: Property of a system to continue operating in case of failure  
-	**Disaster Recovery**: Set of tools, procedures to recover after failure  

As you can see the whole spectrum / process is covered: from healthy operation through undesired failures till full / partial recovery.

### Compound Quality Attributes
Whenever we talk about things, like reliable systems, then we actually think about a group of attributes.  
- 	**Reliability**: Ability to operate all the time  
    -	This is the desired outcome  
-	**Robustness**: Ability to respond for all input  
    - 	Whatever happens it sends a response to the requester  
-	**Resilience**: Ability to respond for environment changes  
    -	Withstand certain types of failure and remain functional 
### Expect the unexpected  
When the system has to face with unexpected behaviours we can distinguish several strategies.  
- 	**Fault Tolerance**: Ability to survive misbehaving environment  
-	**Fault Resilience**: Ability to recover from misbehaving   environment
-	**Antifragility**: Ability to resist misbehaving environment and then get better    

*“It not about how to make money.
It is all about how not to lose.”* - Uwe Friedrichsen

## Taxonomy
It is really hard to categorize mechanisms, because each classification is done by using a single aspect. For example:
-	How is it triggered? 
     -	Proactively or Reactively
-	When is it triggered?
    -	Before or After the fact
-	How does it achieves its promise? 
    -	via isolation, via supervision, via what so ever
-	etc.

One of the most well-know classification uses the following model:
-	**Prevention**: Something bad is going to happen and we can cease it before it occurs
-	**Deterrence**: If Something bad is going to happen and we will impose a penalty
-	**Detection**: Something bad is happening and we are able to notice it
-	**Mitigation**: Something bad is happening and we are able to reduce the impact

As you can see the first two are: proactive and before the fact. The second half are: reactive and after the fact. None of them tell us anything about the how.

Here is a pretty awesome infographic about the different resilience strategies (it uses a slightly different categorization):
 
TODO: Add image here

The best thing about these strategies is that they are composable. In other words, your strategy can be combined by using some of these. If an inner mechanism does not provide sufficient solution then it can propagate the problem to the outer one. With this escalation you can create a really robust & resilient solution. In the next section we will explore a handful of well-known mechanisms. These are wide-spread and battle-hardened solutions. 
## Basic mechanisms
### Fail Fast
**Categories**: proactive, before the fact  
> Please bear in mind that fail fast is not equivalent with fail safe! The later one is not a robust solution because it [conceals the defect](https://javapapers.com/core-java/fail-fast-vs-fail-safe/).

Have you heard the following wisdom: *“Bring the pain forward“*? It is a well-known and widely used principle (from programming till devops everywhere). This can be translated into the following: If you know that [we ain’t gonna make it](https://www.youtube.com/watch?v=IquUgtA_n04&feature=youtu.be&t=29) then … just kidding :smiley: So, if we can determine that some of the preconditions are not met then we do not proceed. In other words, if we know that it will fail because some of the circumstances aren’t met then we should exit early.

**Application**: This principle can be used on almost every level:
-	A function should check its parameters and if they are not in the desired state then it should early exit
-	An endpoint should check the requester’s rights and if they are not sufficient then the request must be rejected
-	A service instance should check its dependencies' availability and if they are not reachable then the load balancer should not direct traffic to this instance
### Escalation
**Categories**: reactive, after the fact
> *Escalation* and *elevation* are sometimes used [interchangeably](https://en.wikipedia.org/wiki/Privilege_escalation) even though they mean absolutely different things. The synonym for the former is increase where the synonym for the latter is promote. 
> -	Former increases intensity/extent, which might need to involve others from the hierarchy who have broader rights to solve the problem
> - Whereas the latter promotes itself to gain broader rights to be able to solve the problem 

Whenever we try to deal with a given problem we are doing it in a given context, which might not be sufficient. The context determines our possibilities and scope. Others *might* live in different context, which *might* open up other opportunities. I used the verb *might*, because escalation does not necessary solve the problem. It is based on an assumption, that if I have right A and B then my supervisor would have more privileges than I do. 

In order to be able to delegate my problem to others there should be some sort of supervision / lineage or hierarchy. As you can image there could be fairly long escalation chains, but from an individual perspective it should only know about its supervised descendants and its supervisor.

**Application**: Even though this principle is really general it is not applied everywhere:
-	On a function level, we can rarely find things like hierarchy, except the call stack. There are languages (like golang) where there are [defer statements](https://blog.golang.org/defer-panic-and-recover) and they were designed for clean-up / recovery. (It is a more general concept than the finally block).
-	On application level there are several concurrency models (like [actor model](https://en.wikipedia.org/wiki/Actor_model), [communicating sequential processes](https://en.wikipedia.org/wiki/Communicating_sequential_processes)) that are heavily relying on [supervision](https://getakka.net/articles/concepts/supervision.html). 

### Fallback
**Categories**: reactive, after the fact
> Please note that *fallback* and *fail back *are not the same. 
The former one is more general and it means using a surrogate option. 
The latter one is used during **disaster recovery**: 
> -	when we switch from the unhealthy machine to the backup it is called *fail over*
> -	when we switch back from the backup to the healthy machine it is called *fail back*

This is the simplest pattern among all. If a given component is unable to processed with its normal flow (for whatever reason) it can return with a predefined (rarely dynamically constructed) “basic“ response in order to improve robustness. Remember robustness means, whatever happens the requester will receive some sort of answer. This answer is usually a reference to a value which is outside of the normal flow. Most of the time this pattern is applied as a last chance. If none of the ancestors in the supervision hierarchy are able to handle the problem then we might end up in a situation when we should fall back to our last resort. In other words, this pattern is rarely used alone, it is mostly the last item in the chain of the resilience strategies. A common pattern is the combination of *caching* & fallback: *If I can’t use fresh data then I will fall back to use stale one*.

**Application**: This simple pattern appears on each and every level
-	On programming language level we can use things like [default value](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/default) expressions, [conditional operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator) (`?:`) and [null coalescing operators](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator) (`??` | `??=`)
-	On BCL level you can find a lot of methods that accept fallback value (a.k.a default), for example [GetValueOrDefault](https://referencesource.microsoft.com/#mscorlib/system/nullable.cs,61), [FirstOrDefault](http://www.technicaloverload.com/linq-single-vs-singleordefault-vs-first-vs-firstordefault/) 
-	On function level we can define a response variable with an extremal value. If everything works fine then we will overwrite that, otherwise we will use the initialization value
-	In case of inheritance we rely on this simple principle as well. If the derived type does not override a given function then it will fall back to base class’s implementation
-	…
-	On infrastructure level this mechanism is heavily used to provide high availability, for example using replication in order to overcome on a single instance malfunctioning / failure. 
    -	If a given instance is not reachable then we can fall back to the other instance
