# Resiliency: Implement Retry on HTTP Requests

Polly:
 - .Net Library help to implement fault-tolerance and resilience strategies in the applications.
 - Provides rich set of features to define the policies to handle various types of transient faults, such as network interruptions, timeouts and resource unavailability.
 - This policies enable applications to recover gracefully from unexcepted errors, ensuring stability, reliability and an enhanced user experience.

**Policies**:

1. Simple Retry With retry Count Policy:
     - Automatically retries a Http request call for a specified number of time(s), in cause of failure. 

2. Retry Forever Policy:
    - Continuously retry operation indefinitely when exception occur.

3. Wait and Retry Policy:
    - Retry n times with a delay wait between each retry.

4. Wait Retry Forever Policy:
    - Infinite retries with a waiting period between attempts.
