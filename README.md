# advanced-cqrs-ddd-workshop

This sample was created during Greg Young's Advanced CQRS &amp; DDD Workshop in London Nov 7-10 of 2017.

## What is this?

The course started out given a really simple scenario. We're basically running a restaurant. So we have 4 basic actors in our system. The `Waiter` which takes orders from customers, the `Cook` which takes an order and prepares the food. Then an `Assistant Manager` which determines the price for individual line items and finally a `Cashier` which will handle actual payment for an order.

Given these four basic actors, we started implementing with composition of these actors as a pipeline through `IHandleOrder` interface which exposes one `Handle(Order)` method. This way we can send messages through a pipeline from the Waiter, to the Cook, to the Assistant Manager and finally to the Cashier. Later in the workshop we've re-implemented some of this logic in the form of a topic based Pub/Sub bus.

Conceptually you can think of each of these 4 actors as being implemented as a micro service, where messages passed between them are sent via an actual broker.

## Concepts in this sample

- Composition of logic
- Process Manager(s)
- Chaos (Seemingly random dropping messages vs. delivering messages twice)
- Idempotent message handling
- Sending messages to future self
- Weak schema between services (don't lose message elements when serializing/deserializing)
