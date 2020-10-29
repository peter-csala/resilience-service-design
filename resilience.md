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

